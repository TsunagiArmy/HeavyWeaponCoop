using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class TestAnimAuthoring : MonoBehaviour
{
    public Sprite[] idleSprites;
    public Sprite[] runSprites;
    public float targetFrameRate = 0.1f;

    class Baker : Baker<TestAnimAuthoring>
    {
        public override void Bake(TestAnimAuthoring authoring)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var root = ref builder.ConstructRoot<SpriteUVBlob>();

            var idleArray = builder.Allocate(ref root.IdleFrames, authoring.idleSprites.Length);
            for (int i = 0; i < authoring.idleSprites.Length; i++)
            {
                //Vector2[] uvs = SpriteUtility.GetSpriteUVs(authoring.idleSprites[i], true);
                Vector2[] uvs = authoring.idleSprites[i].uv;

                idleArray[i] = new float4(uvs[3].x, uvs[3].y, uvs[0].x, uvs[0].y);
            }

            var runArray = builder.Allocate(ref root.RunFrames, authoring.runSprites.Length);
            for (int i = 0; i < authoring.runSprites.Length; i++)
            {
                //Vector2[] uvs = SpriteUtility.GetSpriteUVs(authoring.runSprites[i], true);
                Vector2[] uvs = authoring.runSprites[i].uv;

                runArray[i] = new float4(uvs[3].x, uvs[3].y, uvs[0].x, uvs[0].y);
            }

            if (authoring.idleSprites != null && authoring.idleSprites.Length > 0)
            {
                //var atlasTex = SpriteUtility.GetSpriteTexture(authoring.idleSprites[0], true);
                var atlasTex = authoring.idleSprites[0].texture;

                // Renderer を取得してマテリアルに割り当て（エディタ表示用）
                var renderer = authoring.GetComponent<MeshRenderer>();
                if (renderer != null && renderer.sharedMaterial != null)
                {
                    renderer.sharedMaterial.SetTexture("_BaseMap", atlasTex);
                }
            }

            var blob = builder.CreateBlobAssetReference<SpriteUVBlob>(Allocator.Persistent);
            builder.Dispose();

            var entity = GetEntity(TransformUsageFlags.Renderable);
            AddComponent(entity, new SpriteAnimationData
            {
                FrameTimer = authoring.targetFrameRate,
                TargetFrameRate = authoring.targetFrameRate,
                CurrentFrame = 0,
                IsRunning = false
            });

            AddComponent(entity, new SpriteAnimationAtlas { Blob = blob });

            // 初期UVを設定（Idleの最初のフレーム）
            if (authoring.idleSprites != null && authoring.idleSprites.Length > 0)
            {
                //Vector2[] uvs = SpriteUtility.GetSpriteUVs(authoring.idleSprites[0], true);
                Vector2[] uvs = authoring.idleSprites[0].uv;

                float4 uvRect = new float4(uvs[3].x, uvs[3].y, uvs[0].x, uvs[0].y);
                AddComponent(entity, new TestMatUV { Value = uvRect });
            }
            else
            {
                AddComponent(entity, new TestMatUV { Value = float4.zero });
            }
        }
    }
}
