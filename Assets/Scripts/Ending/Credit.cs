using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Ending
{
    public class Credit
    {
        private FadeController _fadeController;
        private Sprite[] _creditSpriteLists;
        private Image _backGround;
        
        public void Initialize(FadeController fadeController,Image backGround,Sprite[] creditSpriteLists)
        {
            if (fadeController == null)
                throw new ArgumentNullException(nameof(fadeController));
            if (backGround == null) 
                throw new ArgumentNullException(nameof(backGround));
            if (creditSpriteLists == null || creditSpriteLists.Length == 0) 
                throw new ArgumentException("creditSpriteLists must not be null or empty", nameof(creditSpriteLists));
    
            _fadeController = fadeController;
            _creditSpriteLists = creditSpriteLists;
            _backGround = backGround;
        }
        
        public async UniTask ShowCredit(float delay,float[] duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            for (int i = 0; i < _creditSpriteLists.Length; i++)
            {
                if (i != 0)
                {
                    await _fadeController.FadeOutAsync(duration[i]);
                }
                
                _backGround.sprite = _creditSpriteLists[i];
                await UniTask.Delay(TimeSpan.FromSeconds(duration[i]));
                await _fadeController.FadeInAsync(duration[i]);
            }
        }
    }
}