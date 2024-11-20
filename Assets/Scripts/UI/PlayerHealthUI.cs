
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    List<GameObject> _playerHealthUIs;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _playerHealthUIs.Add(transform.GetChild(i).gameObject);
        }
    }

    public void PlayerHealthUpdate(int healthValue)
    {
        foreach (var healthUI in _playerHealthUIs)
        {
            healthUI.SetActive(false);
        }

        for (int i = 0; i < healthValue; i++)
        {
            _playerHealthUIs[i].SetActive(true);
        }
    }
}
