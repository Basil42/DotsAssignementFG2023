using System.Collections.Generic;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class WaveSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField] private float duration = 2f;
        [SerializeField] private float cooldownTime = 5f;
        [SerializeField] private float delayBeforeFirstWave = 7f;
        [SerializeField] private float frequency = 5f;
        [SerializeField] private GameObject prefab;
        private class WaveSpawnerAuthoringBaker : Baker<WaveSpawnerAuthoring>
        {
            public override void Bake(WaveSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);//not necessary here but could be used dynamically
                AddComponent(entity, new WaveSpawner
                {
                    Duration = authoring.duration,
                    CoolDownTime = authoring.cooldownTime,
                    WaveTimer = - authoring.delayBeforeFirstWave, //this can be negative without issues
                    SpawnedEntity = GetEntity(authoring.prefab,
                        TransformUsageFlags.Dynamic),
                    LastEntitySpawnTime = authoring.duration,//should ensure something spawns immediately, technically bound to frame rate but it's a weird edge case
                    SpawnFrequency = authoring.frequency
                });
                SetComponentEnabled<WaveSpawner>(entity,true);//for this example
            }
        }
    }
}