using System;
using UnityEngine;

namespace TeamC
{
    /// <summary>魔法使いの効果</summary>
    [CreateAssetMenu(fileName = "GeneratedWizardEffects", menuName = "CreateWizardEffects")]
    public class WizardEffects : ScriptableObject
    {
        [SerializeField, Header("攻撃のダメージの倍率")] private double magnification = 1.25;

        private Wizard _wizard;
        private Player _player;
        private Poet _poet;
        private int _wizardLevel;
        private int _poetEffectMagnification;

        /// <summary>魔法使いの効果発動時の処理</summary>
        public void OnWizardEffects()
        {
            // 魔法使い、プレイヤー、詩人の検索    
            if (_wizard == null)
                _wizard = FindFirstObjectByType<Wizard>();
            if (_player == null)
                _player = FindFirstObjectByType<Player>();
            if (_poet == null)
                _poet = FindFirstObjectByType<Poet>();

            // 現在の魔法使いのレベルを取得
            int currentWizardLevel = _wizard.GetCurrentLevel();
            // 現在の詩人の効果倍率を取得
            int currentPoetEffectMagnification = _poet.GetEffectMagnification();

            // 記録されているレベルが違えば
            if (currentWizardLevel != _wizardLevel || currentPoetEffectMagnification != _poetEffectMagnification)
            {
                // プレイヤーのダメージを増加させる1×1.25のレベル-1乗増加
                decimal wizardEffect = (decimal)(Math.Pow(magnification, currentWizardLevel - 1) + 1);
                Debug.Log($"Wizards Poop:{wizardEffect.ToString("N0")}");
                //プレイヤーのタップ時ダメージにwizardEffectを掛ける
                var dmg = decimal.Multiply(_player.GetPlayerApplayingDamage, wizardEffect);
                dmg = decimal.Multiply(dmg, currentPoetEffectMagnification);
                _player.SetPlayerApplayingDamage(dmg);

                // レベルの記録
                _wizardLevel = currentWizardLevel;
                _poetEffectMagnification = currentPoetEffectMagnification;

#if UNITY_EDITOR
                Debug.Log($"魔法使いがプレイヤーのタップダメージを{wizardEffect}倍させています!");
#endif
            }
        }
    }
}