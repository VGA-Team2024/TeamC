using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TeamC
{
    public class BossHPDisplayer : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        [SerializeField] private TMP_Text _hpLabel;
        
        Boss _boss;
        
        void Start()
        {
            _boss = FindAnyObjectByType<Boss>();
            _slider.value = (float)(_boss.GetHP / _boss.GetMaxHPAtCurrentFloor);
            var txt_max = string.Format("{0:N0}", _boss.GetMaxHPAtCurrentFloor);
            var txt_current = string.Format("{0:N0}", _boss.GetHP);
            _hpLabel.text = txt_current + " / " + txt_max;
        }

        void Update()
        {
            _slider.value = (float)(_boss.GetHP / _boss.GetMaxHPAtCurrentFloor);
            var txt_max = string.Format("{0:N0}", _boss.GetMaxHPAtCurrentFloor);
            var txt_current = string.Format("{0:N0}", _boss.GetHP);
            _hpLabel.text = txt_current + " / " + txt_max;
        }
    }
}
