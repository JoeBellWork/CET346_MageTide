using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIbehaviour : MonoBehaviour
{
    // nav mesh control
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isGround, isPlayer;
    private Vector3 tempHold;

    [Space (10)]
    // desinations
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    [Space(10)]
    // attacks
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    [Space(10)]
    // state controls
    public float sightRange, attackRange;
    public bool playerInAttackRange, playerInSightRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if(!playerInSightRange && !playerInAttackRange)
        {
            Patrol();
        }

        if(playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }

        if(playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void Patrol()
    {
        if(!walkPointSet)
        {
            SearchWalkPoint();
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalk = transform.position - walkPoint;
        if(distanceToWalk.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
        {
            walkPointSet = true;

            StartCoroutine(ResetPatrolWithTime());
        }
    }


    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            // add shooting code towards player/camera

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    IEnumerator ResetPatrolWithTime()
    {
        Debug.Log("A");
        tempHold = walkPoint;
        yield return new WaitForSeconds(10f);
        if(tempHold == walkPoint)
        {
            walkPointSet = false;
            Debug.Log("B");
        }
    }
}
