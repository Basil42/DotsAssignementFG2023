using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Enemies
{
    public partial struct WaveSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer.ParallelWriter ecb = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            new WaveSpawnerJob//there has to be a way to inline these like in SystemBase
            {
                ECB = ecb,
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }

    public partial struct WaveSpawnerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public float DeltaTime;

        private void Execute([ChunkIndexInQuery] int chunkIndex, ref WaveSpawner spawner,in LocalTransform SpawnTransform)
        {
            
            if((spawner.WaveTimer += DeltaTime) < 0f)return;
            if ((spawner.LastEntitySpawnTime += DeltaTime) > 1f / spawner.SpawnFrequency)
            {
                var newEntity = ECB.Instantiate(chunkIndex, spawner.SpawnedEntity);
                ECB.SetComponent(chunkIndex,newEntity,LocalTransform.FromPosition(SpawnTransform.Position));
                spawner.LastEntitySpawnTime = 0f;
            }

            if (spawner.WaveTimer > spawner.Duration) spawner.WaveTimer = -spawner.CoolDownTime;//will return instantly until cool down has passed

        }
    }
}