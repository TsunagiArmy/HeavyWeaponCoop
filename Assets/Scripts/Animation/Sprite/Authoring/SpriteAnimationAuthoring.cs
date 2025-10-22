using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// 
/// </summary>
[DisallowMultipleComponent]
class SpriteAnimationAuthoring : MonoBehaviour
{
    [Tooltip("Frames in this animation (drag sub-sprites produced by Sprite Editor, or individual sprite files).")]
    public Sprite[] frames;

    public bool isSplitAnimation = false;

    [Tooltip("Default duration per frame (seconds). Individual frames can override by setting 'duration' in the authoring editor, but this authoring doesn't expose per-frame durations—keep simple).")]
    public float defaultFrameDuration = 1f / 12f;

    [Tooltip("Loop by default?")]
    public bool loop = true;

    [Tooltip("Play on start?")]
    public bool playOnStart = true;

    class Baker : Baker<SpriteAnimationAuthoring>
    {
        public override void Bake(SpriteAnimationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            int n = authoring.frames != null ? authoring.frames.Length : 0;

            if (n == 0)
            {
                return;
            }

            using (var bb = new BlobBuilder(Allocator.Temp))
            {
                ref var root = ref bb.ConstructRoot<SpriteAnimationBlob>();
                var arr = bb.Allocate(ref root.frames, n);

                for(int i = 0; i < n; ++i)
                {
                    var s = authoring.frames[i];

                    if (s == null)
                    {
                        arr[i] = new SpriteFrame
                        {
                            uvMin = float2.zero,
                            uvMax = float2.zero,
                            duration = authoring.defaultFrameDuration
                        };

                        continue;
                    }

                    var rect = s.textureRect;
                    var tex = s.texture;

                    float2 uvMin = new float2(rect.xMin / tex.width, rect.yMin / tex.height);
                    float2 uvMax = new float2(rect.xMax / tex.width, rect.yMax / tex.height);

                    arr[i] = new SpriteFrame
                    {
                        uvMin = uvMin,
                        uvMax = uvMax,
                        duration = authoring.defaultFrameDuration
                    };
                }

                root.defaultFrameDuration = authoring.defaultFrameDuration;
                root.loop = authoring.loop;
                root.defaultPlaying = authoring.playOnStart;

                var blobRef = bb.CreateBlobAssetReference<SpriteAnimationListBlob>(Allocator.Temp);

                /*AddComponent(entity, new SpriteAnimationComponent
                {
                    animationsBlob = blobRef,
                    currentFrame = 0,
                    elapsedTime = 0f,
                    isPlaying = blobRef.Value.defaultPlaying
                });*/
            }
        }
    }
}
