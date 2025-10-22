// Assets/Scripts/CheckAtlas.cs
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;

[DisallowMultipleComponent]
public class CheckAtlas : MonoBehaviour
{
    [Tooltip("確認したいスプライトを複数入れてください（例: Idle, Run）")]
    public Sprite[] sprites;

    [Tooltip("実行時にこのマテリアルの mainTexture を差し替えて確認します（任意）")]
    public Material targetMaterial;

    [Tooltip("差し替え後に targetMaterial を元に戻したい場合は true にする")]
    public bool restoreMaterialOnStop = true;

    Material originalMaterialInstance;
    Texture originalMainTex;

    void Start()
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("[CheckAtlas] No sprites assigned.");
            return;
        }

        // ログヘッダ
        Debug.Log("[CheckAtlas] Start - checking sprites and loaded SpriteAtlases...");

        // 1) 各スプライトの texture を表示
        Texture firstTex = null;
        for (int i = 0; i < sprites.Length; i++)
        {
            var s = sprites[i];
            if (s == null)
            {
                Debug.LogWarning($"[CheckAtlas] sprites[{i}] is null");
                continue;
            }

            var tex = s.texture;
            Debug.Log($"[CheckAtlas] sprites[{i}] name='{s.name}' textureName='{(tex != null ? tex.name : "null")}' instanceId={(tex != null ? tex.GetInstanceID() : 0)} rect={s.textureRect}");
            if (i == 0) firstTex = tex;
        }

        // 2) sprites が同一の texture を参照しているか（一致していればAtlas化されている可能性）
        bool allSameTex = sprites.All(s => s != null && s.texture == firstTex);
        Debug.Log($"[CheckAtlas] All sprites share same texture? {allSameTex}");

        // 3) Editor/Runtime にロードされている SpriteAtlas を列挙して、どの Atlas が各 Sprite を含むか確認
        var loadedAtlases = Resources.FindObjectsOfTypeAll<SpriteAtlas>();
        Debug.Log($"[CheckAtlas] Found {loadedAtlases.Length} loaded SpriteAtlas (Resources.FindObjectsOfTypeAll).");
        for (int a = 0; a < loadedAtlases.Length; a++)
        {
            var at = loadedAtlases[a];
            Debug.Log($"[CheckAtlas] Atlas[{a}] name='{at.name}'");
            for (int i = 0; i < sprites.Length; i++)
            {
                var s = sprites[i];
                if (s == null) continue;
                bool can = at.CanBindTo(s);
                if (can)
                    Debug.Log($"  -> Atlas '{at.name}' CAN bind to sprite '{s.name}'");
                else
                    Debug.Log($"  -> Atlas '{at.name}' does NOT bind to sprite '{s.name}'");
            }
        }

        // 4) targetMaterial の mainTexture を最初の sprite.texture に差し替えて動作確認（Play時のテスト用）
        if (targetMaterial != null)
        {
            // 作業用にインスタンス化して差し替える（共有マテリアル書き換えを避ける）
            originalMaterialInstance = new Material(targetMaterial);
            originalMainTex = targetMaterial.mainTexture;

            var texToAssign = firstTex;
            if (texToAssign == null)
            {
                Debug.LogWarning("[CheckAtlas] first sprite.texture is null; cannot assign to material.");
            }
            else
            {
                // 注意: ここで割り当てる texture が Atlas のテクスチャであるかは sprite.texture の時点で決まります
                originalMaterialInstance.mainTexture = texToAssign;
                // 実際に Scene の MeshRenderer/Entity が参照するマテリアルを差し替える場合は対象 Renderer を指定する必要があります。
                // ここでは targetMaterial を直接差し替えるのではなく、ログで確認するために上書きしているだけです。
                //もし targetMaterial を直接上書きしたい場合は targetMaterial.mainTexture = texToAssign; を使う（共有マテリアル変更のため注意）。
                Debug.Log($"[CheckAtlas] Created material instance and set mainTexture = '{texToAssign.name}' (instanceId {texToAssign.GetInstanceID()}).");

                // Optionally apply instance back to scene objects that use targetMaterial:
                // WARNING: This finds all renderers using the exact same Material object reference and replaces them with the instance.
                var applied = 0;
                var renderers = FindObjectsOfType<Renderer>();
                foreach (var r in renderers)
                {
                    // cheap check: if renderer.sharedMaterial equals targetMaterial (the original), replace it
                    if (r.sharedMaterial == targetMaterial)
                    {
                        r.material = originalMaterialInstance; // this creates instance for that renderer
                        applied++;
                    }
                }
                Debug.Log($"[CheckAtlas] Applied material instance to {applied} Renderer(s) that referenced the original targetMaterial.");
            }
        }
        else
        {
            Debug.Log("[CheckAtlas] No targetMaterial assigned; skipping material assignment.");
        }

        Debug.Log("[CheckAtlas] Check complete.");
    }

    void OnDestroy()
    {
        // restore not strictly necessary - cleaning up created instance
        if (originalMaterialInstance != null)
        {
            Destroy(originalMaterialInstance);
            originalMaterialInstance = null;
        }
    }
}
