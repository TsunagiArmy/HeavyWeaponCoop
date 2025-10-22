using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject prefab;            // 生成するプレハブ
    public int count = 10;               // 生成個数
    public Transform parent;             // 生成したオブジェクトの親（省略可）

    [Header("Distance (Perspective only)")]
    public float minDistance = 5f;       // カメラからの最小距離（透視）
    public float maxDistance = 20f;      // カメラからの最大距離（透視）

    [Header("Orthographic depth")]
    public float orthoDepth = 10f;       // 直交カメラ時のワールドZ（カメラからの前方方向へのオフセット）

    [Header("Overlap avoidance")]
    public bool avoidOverlap = true;     // true なら近接で重ならないよう試行
    public float avoidRadius = 0.5f;     // 重なり判定の半径
    public int maxAttemptsPerSpawn = 20; // 1つ生成するのに試行する最大回数

    private Camera cam;

    void Start()
    {
        if (prefab == null)
        {
            Debug.LogError("SpawnInCameraView: prefab が未設定です。");
            return;
        }

        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("SpawnInCameraView: Main Camera が見つかりません。Tag 'MainCamera' を設定してください。");
            return;
        }

        // 初期デフォルト調整（必要なら）
        if (!cam.orthographic)
        {
            // ensure sensible defaults relative to camera clipping planes
            minDistance = Mathf.Max(minDistance, cam.nearClipPlane + 0.1f);
            if (maxDistance <= minDistance) maxDistance = Mathf.Clamp(minDistance + 10f, minDistance + 0.1f, cam.farClipPlane);
            maxDistance = Mathf.Min(maxDistance, cam.farClipPlane - 0.1f);
        }

        SpawnMany();
    }

    void SpawnMany()
    {
        for (int i = 0; i < count; i++)
        {
            bool spawned = TrySpawnSingle();
            if (!spawned)
            {
                Debug.LogWarning($"SpawnInCameraView: {i + 1} 個目の生成に失敗しました（試行回数上限）。");
            }

            Debug.Log($"SpawnInCameraView: {i + 1} 個目の生成に成功しました。");
        }
    }

    bool TrySpawnSingle()
    {
        for (int attempt = 0; attempt < maxAttemptsPerSpawn; attempt++)
        {
            Vector3 worldPos = GetRandomPointInCameraView();

            // オーバーラップ回避
            if (avoidOverlap)
            {
                // 屏風的に Physics.OverlapSphere を使う（コライダーがある場合）
                Collider[] hits = Physics.OverlapSphere(worldPos, avoidRadius);
                if (hits != null && hits.Length > 0)
                {
                    // 衝突あり -> 再試行
                    continue;
                }
            }

            // Instantiate
            GameObject go = Instantiate(prefab, worldPos, Quaternion.identity, parent);
            // 必要ならカメラ面に向けるなどの処理をここに入れる
            return true;
        }
        return false; // すべての試行で失敗
    }

    Vector3 GetRandomPointInCameraView()
    {
        // ランダムなビューポート座標（x:0-1, y:0-1）
        float vx = Random.value;
        float vy = Random.value;

        if (cam.orthographic)
        {
            // 直交カメラ：ViewportToWorldPoint の z はカメラからの距離（ワールド空間での深さ）
            // orthoDepth をカメラ前方に対する距離として使う
            Vector3 viewportPoint = new Vector3(vx, vy, orthoDepth);
            // 注意: ViewportToWorldPoint は z をカメラ座標系での距離として扱う（カメラの forward 方向）
            return cam.ViewportToWorldPoint(viewportPoint);
        }
        else
        {
            // 透視カメラ：z にカメラからの距離を指定
            float distance = Random.Range(minDistance, maxDistance);
            Vector3 viewportPoint = new Vector3(vx, vy, distance);
            return cam.ViewportToWorldPoint(viewportPoint);
        }
    }

    // デバッグ用: シーンビュー上で重なり判定球を可視化
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        if (!avoidOverlap) return;

        Gizmos.color = Color.yellow;
        // 生成予定点をいくつか描く（表示目的）
        for (int i = 0; i < Mathf.Min(10, count); i++)
        {
            Vector3 p = GetRandomPointInCameraView();
            Gizmos.DrawWireSphere(p, avoidRadius);
        }
    }
}
