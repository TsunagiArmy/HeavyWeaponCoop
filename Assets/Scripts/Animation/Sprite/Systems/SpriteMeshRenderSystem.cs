using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(PresentationSystemGroup))] 
[UpdateAfter(typeof(AnimationControlSystem))]
partial struct SpriteMeshRenderSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new SpriteMeshRenderJob { };
        job.ScheduleParallel();
    }

    [BurstCompile]
    partial struct SpriteMeshRenderJob : IJobEntity
    {
        private void Execute(ref SpriteUVProperties uvProp, ref SpriteTintProperty tintProp, in SpriteAnimationComponent animComp)
        {
            /*if (!animComp.animationBlob.IsCreated)
            {
                return;
            }
            
            ref var blob = ref animComp.animationBlob.Value;
            int idx = math.clamp(animComp.currentFrame, 0, blob.frames.Length - 1);
            var frame = blob.frames[idx];
            
            uvProp.spriteUV = new float4(frame.uvMin.x, frame.uvMin.y, frame.uvMax.x, frame.uvMax.y);*/
        }
    }
}
