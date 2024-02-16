using UnityEngine;
using UnityEngine.UI;

namespace TeamC
{
	///<summary>魔法使いのボタン</summary>
	public class ShopWizard : ShopSuperClass, IInitializedTarget
	{
		private int _currentlevel = 1;
		private float _currentprice = 100;
		private Player _player = new();
		
		private void Start()
		{
			InitializeObject();
			gameObject.GetComponent<Button>().onClick.AddListener(()=>BuyWizard());
		}

		public void InitializeObject()
		{
			_currentlevel = 1;
			_currentprice = 10;
		}

		public void FinalizeObject()
		{
			
		}
		
		///<summary>ボタンが押せるかどうか</summary>
		void IsPushButton(decimal value)
		{
			if (_player.GetCurrentResource() >= (decimal)_currentprice)
			{
				gameObject.GetComponent<Button>().interactable = true;
			}
			else
			{
				gameObject.GetComponent<Button>().interactable = false;
			}
		}

		///<summary>買う時の処理</summary>
		void BuyWizard()
		{
			if (_player.GetCurrentResource() >= (decimal)_currentprice)
			{
				//GoldManager.Instance.UseGold(_currentPrice);
				_currentlevel++;
				_currentprice *= 1.15f;
				gameObject.GetComponentInChildren<Text>().text = $"魔法使い レベル:{_currentlevel}  {_currentprice}G";
			}
			else
				Debug.Log("お金が足りません");
		}
	}
}