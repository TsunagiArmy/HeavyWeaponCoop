using Unity.Entities;
using Unity.Mathematics;

public struct SpriteAnimationData : IComponentData
{
    public float FrameTimer;
    public float TargetFrameRate;
    public int CurrentFrame;
    public bool IsRunning;
}

public struct SpriteAnimationAtlas : IComponentData
{
    public BlobAssetReference<SpriteUVBlob> Blob; // UV座標データ
}

public struct SpriteUVBlob
{
    public BlobArray<float4> IdleFrames;
    public BlobArray<float4> RunFrames;
}
