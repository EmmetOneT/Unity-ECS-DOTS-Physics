using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;

public struct SpawnPhysics : IComponentData
{
    public Entity Prefab;
    public int Count;
}

public class SpawnPhysicsAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public int Count;
}

public class SpawnPhysicsBaker : Baker<SpawnPhysicsAuthoring>
{
    public override void Bake(SpawnPhysicsAuthoring authoring)
    {
        AddComponent(new SpawnPhysics
        {
            Prefab = GetEntity(authoring.Prefab),
            Count = authoring.Count,
        });
    }
}

[BurstCompile]
public partial struct SphereSpawningSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var SpawnPhysics = SystemAPI.GetSingleton<SpawnPhysics>();
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        for (int i = 0; i < SpawnPhysics.Count; i++)
        {
            var entity = ecb.Instantiate(SpawnPhysics.Prefab);
            ecb.SetComponent(entity, new LocalTransform
            {
                Position = new float3(0, 2 + i * 2, 0),
                Rotation = quaternion.identity,
                Scale = 1
            });
        }

        state.Enabled = false;
    }
}