using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField, InspectorVariantName("最初のリス地")]
    private bool _firstPoint;
    [SerializeField] private Sprite Enable;
    [SerializeField] private Sprite Disable;

    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        if (_firstPoint)
        {
            GameOverManager.I.Point = this;
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameOverManager.I.Point = this;
        if(Enable != null)
            _spriteRenderer.sprite = Enable;
    }

    public void PointDisable()
    {
        if(Disable != null)
            _spriteRenderer.sprite = Disable;
    }
}
