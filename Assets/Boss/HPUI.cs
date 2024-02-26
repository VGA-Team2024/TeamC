using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TeamC
{
    public class HPUI : MonoBehaviour
    {
        Slider slider;
        BossSuperClass _boss;
        void Start()
        {
            slider = GetComponent<Slider>();
            _boss = FindAnyObjectByType<BossSuperClass>();
        }

        void Update()
        {
            slider.value = (float)_boss.GetHP;
        }
    }
}
