using UnityEngine;

public class ZombieFast : ZombieNormal
{
    protected override void Start()
    {
        health = 1;
        base.Start();
        GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 4.5f;
    }

    protected override void Die()
    {
        base.Die(); 
    }
}
