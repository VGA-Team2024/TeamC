using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSettings : MonoBehaviour
{
    string effectName;
    [SerializeField] float _lifeTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        effectName = gameObject.name;
        Destroy(gameObject,_lifeTime);
    }
}
