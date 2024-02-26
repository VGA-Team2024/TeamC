using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private TMP_Text[] _npcLabels;

        [SerializeField] private Button[] _shopButton;

        private Dictionary<string, TMP_Text> _npcNameLabelDic;
        private Dictionary<string, Button> _npcNameButtonDic;
        
        private Player _player = new();
        
        void TaskToInstantiateNPC(int boughtCnt, string name)
        {
            // ★NPCをシーン場へInstanciateする処理はSuperクラスには書いていない★
            if (boughtCnt == 0)
            {
            	_npcNameLabelDic[name].gameObject.SetActive(true);
            }
        }

        /// <summary> ボタンからNPC名を渡して購入時にこれをボタンから呼び出す </summary>
        public void BuyNPC(string name)
        {
            if (_player.GetCurrentGold() >= CalculateNPCCost(name))
            {
                // Superクラスでは、購入数に応じてコストを算出し、
                // それをプレイヤーへコストの適応をして、購入数を＋１しただけ
                base.DecreasePlayerSource(name, CalculateNPCCost(name), TaskToInstantiateNPC);
                UpdateButtonDisplayInfo();
            }
        }

        void UpdateButtonDisplayInfo()
        {
            //テキストの更新
            _npcNameButtonDic[name].GetComponentInChildren<Text>().text =
                $"{name} Lv{GetNPCShopHistory[name]} {CalculateNPCCost(name).ToString("F0")}G";
        }

        ///<summary>ボタンが押せるかどうか</summary>
        /// <param name="value"></param>
        public void IsPushButton(decimal value)
        {
            foreach (Button button in _shopButton)
            {
                if (_player.GetCurrentGold() >= CalculateNPCCost(button.name))
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
        public decimal CalculateNPCCost(string npcName)
        {
            return base.CalculateCostToBuy(npcName);
        }

        private void Start()
        {
            if (GetNPCShopHistory.Keys == null) return;
            //名前で検索できるように
            List<string> names = new List<string>();
            
            for (int i = 0; i < names.Count; i++)
            {
            	_npcLabels[i].gameObject.SetActive(false);
            	_npcNameLabelDic.Add(names[i], _npcLabels[i]);
            	_npcNameButtonDic.Add(names[i], _shopButton[i]);
            	_shopButton[i].GetComponentInChildren<TMP_Text>().text = $"{names[i]} lv0 {CalculateNPCCost(names[i])}";
            }
        }
    }
}
