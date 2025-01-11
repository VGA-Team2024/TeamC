using UnityEngine;

public class NeedleCollider : MonoBehaviour
{
    [SerializeField] private int _damage;
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (other.gameObject.TryGetComponent(out IDamageable dmg) && other.gameObject.TryGetComponent(out IBlowable blo))
        {
            dmg.TakeDamage(_damage);
            blo.BlownAway(gameObject.transform.position);
        }
    }
}
