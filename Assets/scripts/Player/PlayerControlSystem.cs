using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace Player
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial class PlayerControlSystem : SystemBase
    {
        private PlayerControls _playerControls;
        private Entity _player;
        protected override void OnCreate()
        {
            _playerControls = new();
        }
        
        protected override void OnStartRunning()
        {
            _playerControls.Enable();
            _player = SystemAPI.GetSingletonEntity<PlayerMove>();//this as good as a tag in this ase but might be confusing
        }
        
        protected override void OnUpdate()
        {
            float2 moveInput = _playerControls.Gameplay.Move.ReadValue<Vector2>();//acceptable here,it will refresh most frames on analog input
            foreach (var playerTransform in SystemAPI.Query<RefRW<LocalTransform>,RefRO<PlayerMove>>().WithAll<PlayerMove>())
            {
                playerTransform.Item1.ValueRW.Position.xy = playerTransform.Item1.ValueRO.Position.xy + (moveInput * playerTransform.Item2.ValueRO.MaxSpeed * SystemAPI.Time.DeltaTime);
            }

            bool isShooting = _playerControls.Gameplay.Shoot.ReadValue<float>() > 0f;
            if(SystemAPI.IsComponentEnabled<Shooter>(_player) != isShooting)
                SystemAPI.SetComponentEnabled<Shooter>(_player,isShooting);
            
        }

        protected override void OnStopRunning()
        {
            _playerControls.Disable();
            _player = Entity.Null;
        }
    }
}