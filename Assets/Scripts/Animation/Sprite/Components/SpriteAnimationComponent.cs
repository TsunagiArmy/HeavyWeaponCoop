using Unity.Entities;

/// <summary>
/// Systemで使用するスプライトアニメーションの状態を管理するコンポーネント
/// 1 Entityにつき１つだけ持ち、そのEntityが持つアニメーションの全ての状態を管理する
/// Entityごとに個別の値を持たせたい場合はこのコンポーネント内に値を持たせること
/// </summary>
public struct SpriteAnimationComponent : IComponentData
{
    public BlobAssetReference<SpriteAnimationListBlob> animationsBlob; // アニメーションデータへの参照
    public float elapsedTime; // フレーム切り替え用のタイマー
    public bool isPlaying;    // アニメーションが再生中かどうか
    public int currentFrame;  // 現在のフレームインデックス
    public int currentAnim;   // 現在のアニメーションインデックス
}
