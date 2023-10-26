using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Unity.Physics;

public struct ShootBullet : IComponentData
{
    public Entity Prefab;
    public int Force;
}

public class ShootBulletAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public int Force;
}

public class ShootBulletBaker : Baker<ShootBulletAuthoring>
{
    public override void Bake(ShootBulletAuthoring authoring)
    {
        AddComponent(new ShootBullet
        {
            Prefab = GetEntity(authoring.Prefab),
            Force = authoring.Force,
        });
    }
}

[BurstCompile]
public partial struct ShootBulletSystem : ISystem
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
        var ShootBullet = SystemAPI.GetSingleton<ShootBullet>();
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space))
        {
            var entity = ecb.Instantiate(ShootBullet.Prefab);

            foreach (var transform in SystemAPI.Query<TransformAspect>().WithAll<ShootBullet>())
            {

                ecb.SetComponent(entity, new LocalTransform
                {
                    Position = transform.LocalPosition,
                    Rotation = transform.LocalRotation,
                    Scale = 1
                });
            }

            ecb.SetComponent(entity, new PhysicsVelocity
            {
                Linear = new float3(0, 0, ShootBullet.Force)
            });
        }

    }
}