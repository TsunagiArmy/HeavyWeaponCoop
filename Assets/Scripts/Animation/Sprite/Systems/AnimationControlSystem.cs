using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct AnimationControlSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        var job = new AnimationControlJob
        {
            deltaTime = deltaTime
        };

        job.ScheduleParallel();
    }

    [BurstCompile]
    partial struct AnimationControlJob : IJobEntity
    {
        public float deltaTime;

        
        void Execute(ref SpriteAnimationComponent anim)
        {
            /*
            if (!anim.isPlaying || !anim.animationBlob.IsCreated)
            {
                return;
            }

            ref var blob = ref anim.animationBlob.Value;
            int frameCount = blob.frames.Length;

            if (frameCount <= 1)
            {
                return;
            }

            anim.elapsedTime += deltaTime;
            float frameDuration = blob.frames[anim.currentFrame].duration;

            if (frameDuration <= 0f)
            {
                frameDuration = blob.defaultFrameDuration;
            }

            while (anim.elapsedTime >= frameDuration)
            {
                anim.elapsedTime -= frameDuration;
                anim.currentFrame++;

                if(anim.currentFrame >= frameCount)
                {
                    if (blob.loop)
                    {
                        anim.currentFrame = 0;
                    }
                    else
                    {
                        anim.currentFrame = frameCount - 1;
                        anim.isPlaying = false;
                        break;
                    }
                }

                frameDuration = blob.frames[anim.currentFrame].duration;

                if (frameDuration <= 0f)
                {
                    frameDuration = blob.defaultFrameDuration;
                }
            }*/
        }
    }
}