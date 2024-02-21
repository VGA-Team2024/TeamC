using System;
using TMPro;
using UnityEngine;

namespace TeamC
{
	///<summary></summary>
	public class GoldManager : MonoBehaviour, IInitializedTarget
	{
		
		[SerializeField] private TMP_Text goldText;
		private Player _player = new Player();

		private void Start()
		{
			InitializeObject();
		}

		public void InitializeObject()
		{
		}

		public void FinalizeObject()
		{
			
		}
		void ReflectText(decimal value)
		{
			goldText.text = _player.GetCurrentGold().ToString("f0");
		}
		
		
	}
}