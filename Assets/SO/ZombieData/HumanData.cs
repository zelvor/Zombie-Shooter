using UnityEngine;

[CreateAssetMenu(fileName = "NewHuman", menuName = "Humans/HumanData")]
public class HumanData : ScriptableObject, IHealthData
{
    public float hp;
    public float moveSpeed;

    public float GetHealth()
    {
        return hp;
    }
}