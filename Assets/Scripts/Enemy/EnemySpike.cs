using UnityEngine;

public class EnemySpike : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out IDamageable dmg))
        {
            dmg.TakeDamage(1);
        }
    }
}