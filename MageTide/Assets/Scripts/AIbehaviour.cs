using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIbehaviour : MonoBehaviour
{
    // nav mesh control
    public NavMeshAgent agent;
    public bool isGuardian;
    public Transform player;
    public LayerMask isGround, isPlayer;
    private Vector3 tempHold, fleePos, directionToPlayer;

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

    [Space(10)]    
    public Transform eye, shootPoint;
    public Rigidbody bulletRB;
    private Vector3 vO;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("VR camera").transform;
    }

    private void FixedUpdate()
    {
        if(isGuardian)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

            if (!playerInSightRange && !playerInAttackRange)
            {
                Patrol();
            }

            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }

            if (playerInAttackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
            if (!playerInSightRange)
            {
                Patrol();
            }
            else
            {
                PreyFlee();
            }
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

    private void PreyFlee()
    {
        directionToPlayer = transform.position - player.position;
        fleePos = transform.position + directionToPlayer;
        agent.SetDestination(fleePos);
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
            LaunchBullet();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        // defining X and Y for distance and targeting
        Vector3 distance = target - origin; //fin difference between enemy and player

        //plot in 2D space
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        // float for distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float velocityXZ = Sxz / time; // velocity = distance / time
        float velocityY = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time; // verticle velocity 

        Vector3 result = distanceXZ.normalized;
        result *= velocityXZ;
        result.y = velocityY;
        return result;
    }

    public void LaunchBullet()
    {
        vO = CalculateVelocity(player.transform.position, shootPoint.position, 1.5f);
        eye.rotation = Quaternion.LookRotation(vO);

        Rigidbody obj = Instantiate(bulletRB, shootPoint.transform.position, eye.rotation);
        obj.velocity = vO;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    IEnumerator ResetPatrolWithTime()
    {
        tempHold = walkPoint;
        yield return new WaitForSeconds(10f);
        if(tempHold == walkPoint)
        {
            walkPointSet = false;
        }
    }
}
