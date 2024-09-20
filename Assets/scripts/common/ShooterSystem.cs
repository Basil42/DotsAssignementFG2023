using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Player
{
    public partial struct ShooterSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<Shooter>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            new FireProjectileJob
            {
                ecb = ecb,
                ElapsedTime = SystemAPI.Time.ElapsedTime
            }.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }

    public partial struct FireProjectileJob : IJobEntity//essentially the same thing as the spawner
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public double ElapsedTime;

        private void Execute([ChunkIndexInQuery] int chunkIndex, ref Shooter shooter, in LocalTransform localTransform)
        {
            if (ElapsedTime - shooter.LastProjectileFired <= 1.0f/shooter.FireRate) return;
            Entity projectile = ecb.Instantiate(chunkIndex, shooter.Projectile);
            ecb.SetComponent(chunkIndex,projectile,LocalTransform.FromPosition(localTransform.Position).WithRotation(localTransform.Rotation));
            shooter.LastProjectileFired = ElapsedTime;
        }
    }
}