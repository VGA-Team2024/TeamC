using UnityEngine;

public class EnemyCreate : MonoBehaviour
{
    [SerializeField] private GameObject _damageFloor;
    
    private void OnCollisionEnter(Collision other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Ground")
        {
            if (_damageFloor) Instantiate(_damageFloor, transform.position, Quaternion.identity);
        }
    }
}
