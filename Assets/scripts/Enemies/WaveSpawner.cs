using Unity.Entities;

namespace Enemies
{
    public struct WaveSpawner : IComponentData, IEnableableComponent
    {
        public float Duration;
        public float CoolDownTime;
        public float WaveTimer;
        public Entity SpawnedEntity;
        public float LastEntitySpawnTime;
        public float SpawnFrequency;
    }
}