using Unity.Entities;

namespace Player
{
    public struct Shooter : IComponentData,IEnableableComponent
    {
        public Entity Projectile;
        public float FireRate;//projectile per second
        public double LastProjectileFired;
    }
}