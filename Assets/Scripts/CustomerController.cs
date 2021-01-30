using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CustomerController : MonoBehaviour
{
    public CapsuleCollider mainColl;
    public Vector2 speedLimits = new Vector2(4f, 12f);

    public float maxDistance = 20f;
    public float pushForce = 1f;
    public float runTreshold = 6f;

    public Transform modelsParent;

    NavMeshAgent agent;
    Animator anim;
    List<GameObject> models = new List<GameObject>();

    bool isDisabled = false;

    private void Start()
    {
        foreach (Transform t in modelsParent)
        {
            models.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }

        var index = Random.Range(0, models.Count);
        models[index].SetActive(true);
        anim = models[index].GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(speedLimits.x, speedLimits.y);
        PickNewPoint();
        SetMoveAnim();
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
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
        PickNewPoint();
        agent.isStopped = true;
        agent.speed = Random.Range(speedLimits.x, speedLimits.y);
        SetIdleAnim();
        yield return new WaitForSeconds(Random.Range(3f, 10f));
        agent.isStopped = false;
        SetMoveAnim();
    }

    public void OnPlayerCollision (Vector3 playerPos)
    {
        if (!isDisabled)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetTrigger("Collided");

            agent.isStopped = true;
            StartCoroutine(CollisionWalkDelay());
            isDisabled = true;

            var customerPos = transform.position + (Vector3.up * (agent.height / 2));
            var pushDir = (transform.position - playerPos).normalized;

            pushDir.y = 0;

            transform.forward = -pushDir;

            RaycastHit castHit;


            if (Physics.CapsuleCast(transform.position, transform.position + (transform.up * agent.height), agent.radius / 2, pushDir, out castHit, pushForce))
            {
                var movePos = castHit.point + ((transform.position - castHit.point).normalized * mainColl.radius);
                transform.DOMove(movePos, 0.5f);
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
        SetMoveAnim();
    }

    private void SetMoveAnim ()
    {
        if (agent.speed < runTreshold)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Run", true);
        }
    }

    private void SetIdleAnim()
    {
        anim.SetTrigger("Idle" + Random.Range(1, 4));
    }
}
