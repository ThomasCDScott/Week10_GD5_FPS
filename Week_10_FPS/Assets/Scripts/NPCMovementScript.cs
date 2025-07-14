using UnityEngine;
using UnityEngine.AI;

public class NPCMovementScript : MonoBehaviour
{

    NavMeshAgent _agent;
    Transform player;
    Vector3 currentDestination;
    [SerializeField] float followDistance;
    WaypointManager _wm;
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentDestination = _wm.waypoints[Random.Range(0, _wm.waypoints.Length)].position;
        _wm = FindFirstObjectByType<WaypointManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < followDistance)
        {
            if(Vector3.Distance(player.position, transform.position) < 2)
            {
                Attack();
            }
            else
            {
                Follow();
            }
            

            
            Follow();
        }
        else
        {
            Search();
        }

    }

    void Attack()
    {
        _agent.SetDestination(transform.position);
        anim.SetTrigger("Stab");
    }

    void Follow()
    {
        _agent.SetDestination(player.position);
    }

    

    void Search()
    {
        if(Vector3.Distance(currentDestination,transform.position) < 5)
        {
            currentDestination = _wm.waypoints[Random.Range(0, _wm.waypoints.Length)].position;
            // currentDestination = transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        }
        _agent.SetDestination(currentDestination);
    }
}
