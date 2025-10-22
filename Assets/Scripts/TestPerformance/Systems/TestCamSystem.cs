using System.Data.Common;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnInCameraViewSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<TestCamSpawn>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var spawnData = SystemAPI.GetSingleton<TestCamSpawn>();

        var cam = Camera.main;
        if (cam == null) return;

        var instances = state.EntityManager.Instantiate(spawnData.Prefab, spawnData.Count, Allocator.Temp);

        foreach (var entity in instances)
        {
            float vx = UnityEngine.Random.value;
            float vy = UnityEngine.Random.value;
            float distance = UnityEngine.Random.Range(spawnData.MinDistance, spawnData.MaxDistance);

            var entityPos = SystemAPI.GetComponentRW<LocalTransform>(entity);

            Vector3 pos = cam.ViewportToWorldPoint(new Vector3(vx, vy, distance));

            entityPos.ValueRW.Position = pos;
        }

        state.Enabled = false;
    }
}
