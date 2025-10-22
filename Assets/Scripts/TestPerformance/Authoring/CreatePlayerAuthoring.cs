using UnityEngine;
using Unity.Entities;

public class CreatePlayerAuthoring : MonoBehaviour
{
    public GameObject prefab;
    public int count = 10;
    public float minDistance = 5f;
    public float maxDistance = 20f;

    class Baker : Baker<CreatePlayerAuthoring>
    {
        public override void Bake(CreatePlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new TestCamSpawn
            {
                Prefab = GetEntity(authoring.prefab, TransformUsageFlags.Renderable),
                Count = authoring.count,
                MinDistance = authoring.minDistance,
                MaxDistance = authoring.maxDistance
            });
        }
    }
}
