using Unity.Entities;
using Unity.Mathematics;

//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////
///
/// このファイル内の構造体メンバは複数Entityで共有される可能性があります！！！
/// Entityごとに個別の値を持たせたい場合は SpriteAnimationComponent を使用してください！！！
///
//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////

/// <summary>
/// 各スプライトのUV座標とフレーム時間を定義する構造体
/// </summary>
public struct SpriteFrame
{
    public float2 uvMin;
    public float2 uvMax;
    public float duration; // フレームの表示時間（秒）、0以下の場合はデフォルトのフレーム時間を使用
}

/// <summary>
/// 1アニメーションごとのデータを格納する構造体
/// </summary>
public struct SpriteAnimationBlob
{
    public BlobArray<SpriteFrame> frames; // アニメーションのフレーム配列
    public BlobArray<char> animationName; // アニメーションの名前
    public float defaultFrameDuration;　  // フレームのデフォルト表示時間（秒）frames[i].duration が 0以下の場合はこちらを適用
    public bool loop;　                   // ループ再生するかどうか
    public bool defaultPlaying;　         // デフォルトで再生状態にするかどうか
}

/// <summary>
/// このEntityが持つ複数のアニメーションデータへの参照を格納する構造体
/// </summary>
public struct SpriteAnimationListBlob
{
    public BlobArray<BlobAssetReference<SpriteAnimationBlob>> animations; // 複数のアニメーションデータへの参照配列
}