using Unity.Entities;

public struct PlayerMoveComponent : IComponentData
{
    public float moveSpeed;
    public float rotateSpeed;
}
