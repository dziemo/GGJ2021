using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    public float maxDistance = 20f;
    public Vector2 speedLimits = new Vector2(4f, 12f);

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(speedLimits.x, speedLimits.y);
        PickNewPoint();
    }

    private void Update()
    {
        if (agent.remainingDistance < 0.025f)
        {
            StartCoroutine(OnPointReached());
        }
    }

    private void PickNewPoint ()
    {
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + transform.position;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

        agent.SetDestination(hit.position);
    }

    IEnumerator OnPointReached ()
    {
        PickNewPoint();
        agent.isStopped = true;
        agent.speed = Random.Range(speedLimits.x, speedLimits.y);
        yield return new WaitForSeconds(Random.Range(3f, 10f));
        agent.isStopped = false;
    }
}
