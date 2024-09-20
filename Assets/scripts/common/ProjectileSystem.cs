using Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Player
{
    public partial struct ProjectileSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Projectile>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ProjectileJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}

public partial struct ProjectileJob : IJobEntity
{
    public float DeltaTime;

    private void Execute([ChunkIndexInQuery] int ChunkIndex, ref Projectile projectile, ref LocalTransform transform)
    {
        transform.Position += transform.Up() * projectile.Speed * DeltaTime;
    }
}