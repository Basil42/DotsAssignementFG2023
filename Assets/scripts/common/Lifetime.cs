using Unity.Entities;

namespace Player
{
    public struct Lifetime : IComponentData
    {
        public float lifespan;
        public float elapsedLife;
        public Entity entity;
    }
}