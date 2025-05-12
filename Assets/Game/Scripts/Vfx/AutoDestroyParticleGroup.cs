using UnityEngine;
using System.Collections;

public class AutoDestroyParticleGroup : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyWhenAllParticlesDone());
    }

    private IEnumerator DestroyWhenAllParticlesDone()
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>(true);

        yield return new WaitUntil(() => AllParticlesFinished(particleSystems));

        Destroy(gameObject);
    }

    private bool AllParticlesFinished(ParticleSystem[] systems)
    {
        foreach (var ps in systems)
        {
            if (ps != null && ps.IsAlive(true)) return false;
        }
        return true;
    }
}
