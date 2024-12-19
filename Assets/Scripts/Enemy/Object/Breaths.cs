using UnityEngine;

public class Breaths : MonoBehaviour
{
    [SerializeField] private GameObject _parentObj;
    [SerializeField] private GameObject _breath;
    [SerializeField] private GameObject[] _breathPoint;
    [SerializeField, Header("射出角度"), Range(180, 270)] private float _deg;
    private const float BaseDeg = 270;
    
    private void OnEnable()
    {
        var dir = Quaternion.Euler(0, 0, _deg);
        if (_parentObj.transform.rotation.y == 0)
        {
            var addDeg = BaseDeg - _deg;
            dir *= Quaternion.Euler(0, 0, addDeg * 2);
        }
        foreach (var point in _breathPoint)
        {
            Instantiate(_breath, point.transform.position, dir);
        }
        gameObject.SetActive(false);
    }
}
