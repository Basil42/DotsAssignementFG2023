using Unity.Entities;
using Unity.Mathematics;

namespace Player
{
    public struct PlayerMove : IComponentData
    {
        public float MaxSpeed;
        public float2 Speed;
    }
    
}