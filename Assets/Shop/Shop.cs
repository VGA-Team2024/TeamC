using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TeamC
{
	#region TODO

	// 2/17 菅沼 → 川口
	// SuperClass ではモジュールのInitialize処理とFinalize処理と購入処理の一部しか
	// 実装していない。
	// Init処理とFinal処理ではClientDataの読み書き処理を実装する予定である。
	// 購入処理ではNPC名に対応するNPCの購入数に応じた価格をプレイヤーのリソースから
	// 減算してTaskToInstantiateNPC()を呼び出してNPCの購入数を＋１するだけの処理を実装している
	// 実装してほしいのがあれば逐次教えてください

	#endregion

	/// <summary> ショップ機能を提供する </summary>
	public class Shop : ShopSuperClass
	{
		[SerializeField] private TMP_Text[] npcTexts;
		[SerializeField] private Button[] _button;
		private Dictionary<string, TMP_Text> _npcDic;
		private Dictionary<string, Button> _buttonDic;
		private Player _player = new();
		private Action<decimal, int> _noAction;
		private string _name;
		
		private void Start()
		{
			var list = GetNPCShopHistory.Keys.ToList();
			//名前で検索できるように
			for(int i = 0; i < list.Count; i++)
			{
				npcTexts[i].gameObject.SetActive(false);
				_npcDic.Add(list[i], npcTexts[i]);
				_buttonDic.Add(list[i], _button[i]);
			}
		}

		void TaskToInstantiateNPC(int boughtCnt)
		{
			// ★NPCをシーン場へInstanciateする処理はSuperクラスには書いていない★
			if (boughtCnt == 0)
			{
				_npcDic[_name].gameObject.SetActive(true);
			}
		}

		/// <summary> ボタンからNPC名を渡して購入時にこれをボタンから呼び出す </summary>
		public void BuyNPC(string name)
		{
			if (_player.GetCurrentResource() >= CalculateNPCCost(name))
			{
				_name = name;
				// Superクラスでは、購入数に応じてコストを算出し、
				// それをプレイヤーへコストの適応をして、購入数を＋１しただけ
				base.DecreasePlayerSource(name, CalculateNPCCost(name), TaskToInstantiateNPC, _noAction);
				//テキストの更新
				_buttonDic[name].GetComponentInChildren<Text>().text =
					$"{name} Lv{GetNPCShopHistory[name]} {CalculateNPCCost(name).ToString("F0")}G";
			}
		}
		
		///<summary>ボタンが押せるかどうか</summary>
		/// <param name="value"></param>
		public void IsPushButton(decimal value)
		{
			foreach (Button button in _button)
			{
				if (_player.GetCurrentResource() >= CalculateNPCCost(button.name))
				{
					button.interactable = true;
				}
				else
				{
					button.interactable = false;
				}
			}
		}

		/// <summary> NPCの購入数に応じた価格の算出 </summary>
		public decimal CalculateNPCCost(string npcName) => base.CalculateCostToBuy(npcName);
	}
}