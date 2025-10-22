using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        // Initialization logic if needed
    }

    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = UnityEngine.Time.deltaTime;
        foreach (var (player, transform) in SystemAPI.Query<RefRO<PlayerMoveComponent>, RefRW<LocalTransform>>())
        {
            // Handle player movement
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            transform.ValueRW.Position.x += moveDirection.x * player.ValueRO.moveSpeed * deltaTime;
            transform.ValueRW.Position.z += moveDirection.z * player.ValueRO.moveSpeed * deltaTime;

            // Handle player rotation
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.ValueRW.Rotation = Quaternion.RotateTowards(transform.ValueRO.Rotation, targetRotation, player.ValueRO.rotateSpeed * deltaTime);
            }
        }
    }
}