using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;


/// <summary>
/// シェーダーへ渡すデータ
/// [MaterialPropety("")] でシェーダー側の変数名を指定することでDOTSのマテリアルプロパティとして扱えるようになる
/// </summary>

[MaterialProperty("_SpriteUV")]
public struct SpriteUVProperties : IComponentData
{
    public float4 spriteUV;
}

[MaterialProperty("_TintColor")]
public struct SpriteTintProperty : IComponentData
{
    public float4 tintColor;
}
