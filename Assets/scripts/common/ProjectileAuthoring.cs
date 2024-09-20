using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class ProjectileAuthoring : MonoBehaviour
    {
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private bool isPlayerProjectile = false;
        [SerializeField] private float lifeSpan = 1.0f;
        public float Duration;

        private class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
        {
            public override void Bake(ProjectileAuthoring authoring)
            {
                Entity projectile = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(projectile, new Projectile
                {
                    Speed = authoring.speed,
                });
                AddComponent(projectile,new Lifetime
                {
                    lifespan = authoring.lifeSpan,
                    elapsedLife = 0f,
                    entity = projectile
                });
                if(authoring.isPlayerProjectile)AddComponent<PlayerProjectile>(projectile);
                //could also pool the component
            }
        }
    }
}