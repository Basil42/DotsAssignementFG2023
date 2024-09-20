using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerAuthoring : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float fireRate = 2f;
        [SerializeField] private GameObject projectilePrefab;
        
        private class PlayerAuthoringBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                Entity playerEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerTag>(playerEntity);
                AddComponent(playerEntity, new PlayerMove
                {
                    MaxSpeed = authoring.moveSpeed,
                    Speed = default
                });
                AddComponent(playerEntity,new Shooter
                {
                    Projectile = GetEntity(authoring.projectilePrefab,TransformUsageFlags.Dynamic),
                    FireRate = authoring.fireRate,
                    LastProjectileFired = 0d
                });
                SetComponentEnabled<Shooter>(playerEntity, false);
                
            }
        }
    }
}