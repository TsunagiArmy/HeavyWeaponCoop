using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float _moveSpeed = 5f;
    public float _rotateSpeed = 360f;

    class Baker : Baker<PlayerAuthoring>
    {

        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PlayerMoveComponent
            {
                moveSpeed = authoring._moveSpeed,
                rotateSpeed = authoring._rotateSpeed
            });
        }
    }
}
