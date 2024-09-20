using Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Enemies
{
    public partial struct EnemyMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerTag>());
            foreach (var enemy in SystemAPI.Query<RefRO<EnemyMovement>,RefRW<LocalTransform>>())
            {
                var enemytransformRO = enemy.Item2.ValueRO;
                var playerDir = playerTransform.ValueRO.Position - enemytransformRO.Position;
                var moveDir = math.normalizesafe(math.cross(playerDir,
                    new float3(0f, 0f, -1f)));
                enemy.Item2.ValueRW.Position += moveDir * enemy.Item1.ValueRO.Speed * SystemAPI.Time.DeltaTime;
                enemy.Item2.ValueRW.Rotation = quaternion.LookRotationSafe(new float3(0f, 0f, -1f),playerDir);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}