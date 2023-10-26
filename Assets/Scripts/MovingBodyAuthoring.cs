using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public struct MovingBody : IComponentData
{
    public float Velocity;
}

public class MovingBodyAuthoring : MonoBehaviour
{
    public float Velocity;
}

class MovingBodyAuthoringBaker : Baker<MovingBodyAuthoring>
{
    public override void Bake(MovingBodyAuthoring authoring)
    {
        var component = new MovingBody
        {
            Velocity = authoring.Velocity
        };
        AddComponent(component);
    }
}

public partial struct MovingBodySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (moving, velocity) in SystemAPI.Query<RefRW<MovingBody>, RefRW<PhysicsVelocity>>().WithAll<MovingBody>())
        {
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.W))
                velocity.ValueRW.Linear += moving.ValueRO.Velocity * new float3(0, 0, 1);
            else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.S))
                velocity.ValueRW.Linear += moving.ValueRO.Velocity * new float3(0, 0, -1);
            else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.D))
                velocity.ValueRW.Linear += moving.ValueRO.Velocity * new float3(1, 0, 0);
            else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.A))
                velocity.ValueRW.Linear += moving.ValueRO.Velocity * new float3(-1, 0, 0);
        }
    }
}