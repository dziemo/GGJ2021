using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CustomerController : MonoBehaviour
{
    public float maxDistance = 20f;
    public float pushForce = 1f;
    public Vector2 speedLimits = new Vector2(4f, 12f);

    NavMeshAgent agent;

    bool isDisabled = false;

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

    public void OnPlayerCollision (Vector3 playerPos)
    {
        if (!isDisabled)
        {
            agent.isStopped = true;
            StartCoroutine(CollisionWalkDelay());
            isDisabled = true;

            var customerPos = transform.position + (Vector3.up * (agent.height / 2));
            var pushDir = (transform.position - playerPos).normalized;

            pushDir.y = 0;

            transform.forward = -pushDir;

            RaycastHit castHit;


            if (Physics.CapsuleCast(transform.position, transform.position + (transform.up * agent.height), agent.radius, pushDir, out castHit, pushForce))
            {
                transform.DOMove(castHit.point, 0.5f);
            }
            else
            {
                transform.DOMove(transform.position + (pushDir * pushForce), 1f);
            }
        }
    }

    IEnumerator CollisionWalkDelay ()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        agent.isStopped = false;
        isDisabled = false;
    }
}
