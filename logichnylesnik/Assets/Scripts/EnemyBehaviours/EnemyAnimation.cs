using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    public void SetAttacked()
    {
        GetComponent<Animator>().SetTrigger("Attacked");
    }

    public void SetDeath()
    {
        GetComponent<Animator>().SetTrigger("Death");
    }
}
