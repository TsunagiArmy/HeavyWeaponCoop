using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

[DisallowMultipleComponent]
class SpriteMeshAuthoring : MonoBehaviour
{
    class Baker : Baker<SpriteMeshAuthoring>
    {
        public override void Bake(SpriteMeshAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Renderable);

            AddComponent(entity,
                         new SpriteUVProperties
                         {
                             spriteUV = new Unity.Mathematics.float4(0, 0, 1, 1)
                         });

            AddComponent(entity,
                         new SpriteTintProperty
                         {
                             tintColor = new Unity.Mathematics.float4(1, 1, 1, 1)
                         });
        }
    }
}