using UnityEngine;
using UnityEngine.UI;

namespace TeamC
{
	///<summary>仙人のボタン</summary>
	public class ShopHermit : ShopSuperClass, IInitializedTarget
	{
		private int _currentLevel = 1;
		private float _currentPrice = 100;
		private Player _player = new();
		
		private void Start()
		{
			InitializeObject();
			
			GetComponent<Button>().onClick.AddListener(()=>BuyHermit());
		}

		
		public void InitializeObject()
		{
			_currentLevel = 1;
			_currentPrice = 100;
		}

		public void FinalizeObject()
		{
			
		}
		
		///<summary>ボタンが押せるかどうか</summary>
		void IsPushButton(decimal value)
		{
			if (_player.GetCurrentResource() >= (decimal)_currentPrice)
			{
				gameObject.GetComponent<Button>().interactable = true;
			}
			else
			{
				gameObject.GetComponent<Button>().interactable = false;
			}
		}

		///<summary>買う時の処理</summary>
		void BuyHermit()
		{
			if (_player.GetCurrentResource() >= (decimal)_currentPrice)
			{
				_currentLevel++;
				_currentPrice *= 1.15f;
				gameObject.GetComponentInChildren<Text>().text = $"仙人 レベル:{_currentLevel}  {_currentPrice}G";
			}
			else
				Debug.Log("お金が足りません");
		}
	}
}