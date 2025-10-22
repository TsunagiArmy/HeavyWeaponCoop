using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;

public partial struct TestAnimSysrem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        bool spacePressed = Input.GetKey(KeyCode.Space);

        float4 uv;

        foreach (var (animData, atlas, materialProperty, entity) in
            SystemAPI.Query<RefRW<SpriteAnimationData>, RefRO<SpriteAnimationAtlas>, RefRW<TestMatUV>>()
                     .WithEntityAccess())
        {
            ref var data = ref animData.ValueRW;

            data.FrameTimer -= deltaTime;
            if (data.FrameTimer > 0f) continue;

            data.FrameTimer = data.TargetFrameRate;

            //Debug.Log($"アニメーション更新");

            if (spacePressed)
            {
                if (!data.IsRunning)
                {
                    data.IsRunning = true;
                    data.CurrentFrame = 0;
                }
                data.CurrentFrame = (data.CurrentFrame + 1) % atlas.ValueRO.Blob.Value.RunFrames.Length;
                uv = atlas.ValueRO.Blob.Value.RunFrames[data.CurrentFrame];
                materialProperty.ValueRW.Value = uv;
            }
            else
            {
                if (data.IsRunning)
                {
                    data.IsRunning = false;
                    data.CurrentFrame = 0;
                }
                data.CurrentFrame = (data.CurrentFrame + 1) % atlas.ValueRO.Blob.Value.IdleFrames.Length;
                uv = atlas.ValueRO.Blob.Value.IdleFrames[data.CurrentFrame];
                materialProperty.ValueRW.Value = uv;
            }
        }
    }
}
