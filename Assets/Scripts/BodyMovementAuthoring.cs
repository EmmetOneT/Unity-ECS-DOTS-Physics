using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;


public class BodyMovementAuthoring : MonoBehaviour
{
    class BodyBaker : Baker<BodyMovementAuthoring>
    {
        public override void Bake(BodyMovementAuthoring authoring)
        {
            AddComponent<Body>();
        }
    }
}
 
struct Body : IComponentData { }

partial class BodyMovementSystem : SystemBase
{
    
    float speed = 5;
    protected override void OnUpdate()
    {

        var dt = (float)SystemAPI.Time.DeltaTime;
        

        Entities
            .WithAll<Body>()
            .WithoutBurst()
            .ForEach((TransformAspect transform) =>
            {
                
                if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.W))
                {
                    transform.LocalPosition += new float3(0, 0, 1) * dt * speed;
                }
                else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.S))
                {
                    transform.LocalPosition += new float3(0, 0, -1) * dt * speed;
                }
                else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.D))
                {
                    transform.LocalPosition += new float3(1, 0, 0) * dt * speed;
                }
                else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.A))
                {
                    transform.LocalPosition += new float3(-1, 0, 0) * dt * speed;
                }

            }).Run();
    }
}