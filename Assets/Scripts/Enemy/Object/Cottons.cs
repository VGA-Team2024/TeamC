using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> オブジェクトをある地点に設置する </summary>
public class Cottons : MonoBehaviour, ICreateObj
{
    [SerializeField] private BearCotton _cotton;
    [SerializeField, Header("綿の設置場所")] private List<GameObject> _points;
    [SerializeField] private float _height;
    [SerializeField] private float _speed;
    private BearCotton[] _cottons;
    public BearCotton[] cottons => _cottons;
    
    void Start()
    {
        _cottons = new BearCotton[_points.Count];
    }

    public void CreateObject()
    {
        if (_cottons.Any(_ => _ != null)) return;

        for (var i = 0; i < _points.Count; i++)
        {
            var cotton = Instantiate(_cotton, gameObject.transform.position, Quaternion.identity);
            Vector3 velocity = EnemyUtility.CalculateVelocity(transform.position, _points[i].transform.position, _height);
            
            StartCoroutine(MoveCotton(cotton, velocity, _points[i]));
            _cottons[i] = cotton;
        }
    }

    private IEnumerator MoveCotton(BearCotton cotton, Vector3 initialVelocity, GameObject targetObj)
    {
        Vector3 position = cotton.transform.position;
        Vector3 velocity = initialVelocity;

        while (true)
        {
            position += velocity * (Time.deltaTime * _speed);
            velocity += Physics.gravity * (Time.deltaTime * _speed);

            cotton.transform.position = position;

            if ((position - targetObj.transform.position).sqrMagnitude < 0.1f) break;
            if (velocity.y < 0f && position.y < targetObj.transform.position.y - 0.5f) break;

            yield return null;
        }
    }
}
