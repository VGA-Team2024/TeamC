using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Title
{
    public class OpeningTitleText : MonoBehaviour
    {
        [SerializeField, InspectorVariantName("表示したいテキスト昇順")] private string[] _subTitle;
        [SerializeField,InspectorVariantName("テキストを表示する間隔")] private float[] _textDuration;
        [SerializeField,InspectorVariantName("テキストコンポーネント")] private TextMeshProUGUI _text;
        
        public async UniTask ShowTitle()
        {
            if (_subTitle.Length != _textDuration.Length)
            {
                Debug.LogError("テキストの数と間隔の数が一致していません");
                return;
            }

            _text.gameObject.SetActive(true);
            
            // テキストを順番に表示する
            for (int i = 0; i < _subTitle.Length; i++)
            {
                // テキストの間隔分待つ
                await UniTask.Delay(TimeSpan.FromSeconds(_textDuration[i]));
                _text.text = _subTitle[i]; 
            }
        }
    }
}