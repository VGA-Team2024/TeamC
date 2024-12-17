using UnityEngine;

public class Breaths : MonoBehaviour
{
    [SerializeField] private GameObject _breath;
    [SerializeField] private GameObject[] _breathPoint;
    [SerializeField, Header("射出角度"), Range(180, 270)] private float _deg;
    
    private void OnEnable()
    {
        foreach (var point in _breathPoint)
        {
            Instantiate(_breath, point.transform.position, Quaternion.Euler(0, 0, _deg));
        }
        gameObject.SetActive(false);
    }
}
