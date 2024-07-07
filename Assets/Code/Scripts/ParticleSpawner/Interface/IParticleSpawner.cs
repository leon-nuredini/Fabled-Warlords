using UnityEngine;

public interface IParticleSpawner
{
    public GameObject ParticleToSpawn { get; }
    void SpawnParticle(GameObject particleToSpawn, Transform[] spawnPositionArray);
}