using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeamC
{
    public class BossParamDisplayer : MonoBehaviour
    {
        [SerializeField, Header("The Slider To Display Health")]
        private Slider _slider;

        [SerializeField, Header("The Label To Display Ratio")]
        private TMP_Text _healthLabel;

        private BossClass _boss;
        
        private void DispBossParam()
        {
            _slider.value = (float)(_boss.ReturnCurrentBossHP / _boss.ReturnMaxBossHP);
            var max = string.Format("{0:N0}", _boss.ReturnMaxBossHP);
            var current = string.Format("{0:N0}", _boss.ReturnCurrentBossHP);
            _healthLabel.text = current + " / " + max;
        }

        private void Start()
        {
            _boss = GameObject.FindFirstObjectByType<BossClass>();
            DispBossParam();
        }

        private void Update()
        {
            DispBossParam();
        }
    }
}