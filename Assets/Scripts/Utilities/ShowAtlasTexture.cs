using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways] // 実行中・エディタどちらでも動く
public class ShowAtlasTexture : MonoBehaviour
{
    [Tooltip("Atlas テクスチャを確認したい Renderer を指定してください")]
    public Renderer targetRenderer;

    [ContextMenu("Show Atlas Texture")]
    public void ShowAtlas()
    {
        if (targetRenderer == null)
        {
            Debug.LogWarning("[ShowAtlasTexture] targetRenderer が設定されていません");
            return;
        }

        var mat = targetRenderer.sharedMaterial;
        if (mat == null)
        {
            Debug.LogWarning("[ShowAtlasTexture] Renderer にマテリアルがありません");
            return;
        }

        var tex = mat.mainTexture;
        if (tex == null)
        {
            Debug.LogWarning("[ShowAtlasTexture] マテリアルに mainTexture が割り当てられていません");
            return;
        }

        Debug.Log($"[ShowAtlasTexture] Using texture: {tex.name} ({tex.width}x{tex.height})");

#if UNITY_EDITOR
        // エディタで自動的に Atlas テクスチャを選択状態にする
        Selection.activeObject = tex;
        EditorGUIUtility.PingObject(tex);
#endif
    }

    // 起動時に自動表示したい場合は Awake/Start で呼び出し
    void Start()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return; // 再生中のみ自動で確認したいならこの行を残す

        ShowAtlas();
#endif
    }
}
