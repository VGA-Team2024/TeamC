using UnityEngine;
using UnityEngine.UI;

namespace TeamC
{
	///<summary>詩人のボタン</summary>
	public class ShopPoets : ShopSuperClass, IInitializedTarget
	{
		private int _currentLevel = 1;
		private float _currentPrice = 100f;
		private Player _player = new();
		
		private void Start()
		{
			InitializeObject();
			gameObject.GetComponent<Button>().onClick.AddListener(()=>BuyPoets());
		}

		public void InitializeObject()
		{
			_currentLevel = 1;
			_currentPrice = 1000f;
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
		void BuyPoets()
		{
			if (_player.GetCurrentResource() >= (decimal)_currentPrice)
			{
				//GoldManager.Instance.UseGold(_currentPrice);
				_currentLevel++;
				_currentPrice *= 1.15f;
				gameObject.GetComponentInChildren<Text>().text = $"詩人 レベル:{_currentLevel}  {_currentPrice}G";
			}
			else
			{
				Debug.Log("お金が足りません");
			}
		}
	}
}