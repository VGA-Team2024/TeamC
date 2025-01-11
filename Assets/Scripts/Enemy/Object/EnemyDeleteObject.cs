using UnityEngine;

public class EnemyDeleteObject : MonoBehaviour
{
    [SerializeField] private int _time;
    
    private void Start()
    {
        Destroy(gameObject, _time);
    }
}
