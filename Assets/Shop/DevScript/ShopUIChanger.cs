using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TeamC
{
	public class ShopUIChanger : ShopSuperClass
	{
		private Button[] _button;
		private Player _player = new();
		private Action<int> _action;

		private void Start()
		{
			_action += ReflectText;
			int i = 0;
			_button = gameObject.GetComponentsInChildren<Button>();
			foreach (var npc in GetNPCShoppedInfo)
			{
				_button[i].onClick.AddListener(() => DecreasePlayerSource(npc.Key, _action));
				i++;
			}
		}

		///<summary>テキストの更新</summary>
		void ReflectText(int level)
		{
			string[] str = gameObject.GetComponentInChildren<Text>().text.Split();
			gameObject.GetComponentInChildren<Text>().text = $"{str[0]} {level}";
		}

		///<summary>ボタンが押せるかどうか</summary>
		/// <param name="value"></param>
		public void IsPushButton(decimal value)
		{
			foreach (Button button in _button)
			{
				//次の価格がほしい
				if (_player.GetCurrentResource() >= value)
				{
					button.interactable = true;
				}
				else
				{
					button.interactable = false;
				}
			}
		}
	}
}