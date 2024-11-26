
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    List<Image> _playerHealthUIs = new List<Image>();
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _playerHealthUIs.Add(transform.GetChild(i).gameObject.GetComponent<Image>());
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

    public void PlayerHealthDamage()
    {
        _playerHealthUIs[transform.childCount - 1].enabled = false;
    }
}
