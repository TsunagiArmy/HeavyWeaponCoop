using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

[MaterialProperty("_SpriteUV")]
public struct TestMatUV : IComponentData
{
    public float4 Value;
}
