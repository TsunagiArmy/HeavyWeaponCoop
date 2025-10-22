using Unity.Entities;

public struct TestCamSpawn : IComponentData
{
    public Entity Prefab;
    public int Count;
    public float MinDistance;
    public float MaxDistance;
}
