using Player;
using Unity.Burst;
using Unity.Entities;

namespace Player
{
    public partial struct LifetimeSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            new Lifetimejob()
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
                ECB = ecb
            }.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}

public partial struct Lifetimejob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public float DeltaTime;

    private void Execute([ChunkIndexInQuery] int chunkIndex, ref Lifetime lifetime)
    {
        lifetime.elapsedLife += DeltaTime;
        if (lifetime.elapsedLife < lifetime.lifespan) return;
        ECB.DestroyEntity(chunkIndex, lifetime.entity);
    }
}