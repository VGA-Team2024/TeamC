using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] private GameObject _healthUIParent;
    [SerializeField] private Image _fairyGaugeImage;
    [SerializeField] private Image _needleImage;

    readonly List<Image> _playerHealthUIs = new List<Image>();
    void Start()
    {
        for (int i = 0; i < _healthUIParent.transform.childCount; i++)
        {
            _playerHealthUIs.Add(_healthUIParent.transform.GetChild(i).gameObject.GetComponent<Image>());
        }
    }

    public void PlayerHealthUpdate(int healthValue)
    {
        foreach (var healthUI in _playerHealthUIs)
        {
            healthUI.enabled = false;
        }

        for (int i = 0; i < healthValue; i++)
        {
            _playerHealthUIs[i].enabled = true;
        }
    }

    /// <summary>
    /// 妖精ゲージの見た目を更新する
    /// </summary>
    public void FairyGaugeUpdate(float value)
    {
        _fairyGaugeImage.fillAmount = value;
    }

    public void CanSpecialAttackUpdate(bool value)
    {
        _needleImage.enabled = value;
    }
}
