using UnityEngine; // Ensure this is present
using UnityEngine.AI;

public class MeleeZombie : ZombieController
{
    [Header("Melee Zombie Settings")]
    public float meleeDamage = 10f;

    protected override void AttackPlayer()
    {
        base.AttackPlayer();
    }
}