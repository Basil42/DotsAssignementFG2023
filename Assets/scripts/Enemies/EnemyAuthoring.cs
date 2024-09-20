using Player;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class EnemyAuthoring : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float lifetime = 5.0f;
        [SerializeField] private float fireRate = 0.5f;

        private class EnemyAuthoringBaker : Baker<EnemyAuthoring>//inbuilt component have "auto baked components" there should be a way to generate more
        {
            
            public override void Bake(EnemyAuthoring authoring)
            {
                Entity enemyEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<EnemyTag>(enemyEntity);
                AddComponent(enemyEntity,new EnemyMovement
                {
                    Speed = authoring.speed
                });
                AddComponent(enemyEntity, new Shooter
                {
                    Projectile = GetEntity(authoring.projectilePrefab,TransformUsageFlags.Dynamic),
                    FireRate = authoring.fireRate,
                    LastProjectileFired = 0
                });
                SetComponentEnabled<Shooter>(enemyEntity,true);
                AddComponent(enemyEntity, new Lifetime
                {
                    lifespan = authoring.lifetime,
                    elapsedLife = 0,
                    entity = enemyEntity
                });
                
            }
        }
    }
}