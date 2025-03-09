using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public ZombieData data;
    NavMeshAgent agent;
    Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = data.moveSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;
        agent.SetDestination(player.position);
    }
}