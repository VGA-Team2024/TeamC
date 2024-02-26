using System;
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
        [SerializeField] private TMP_Text[] _npcLabels;

        [SerializeField] private Button[] _shopButton;

        private Player _player = new();

        void TaskToInstantiateNPC(int boughtCnt, string name)
        {
            switch (name)
            {
                case "Warrior":
                    GameObject.FindFirstObjectByType<Warrior>().SetActivation((boughtCnt >= 1));
                    break;
                case "Wizard":
                    GameObject.FindFirstObjectByType<Wizard>().SetActivation((boughtCnt >= 1));
                    break;
                case "Thief":
                    GameObject.FindFirstObjectByType<Thief>().SetActivation((boughtCnt >= 1));
                    break;
                case "Hermit":
                    GameObject.FindFirstObjectByType<Hermit>().SetActivation((boughtCnt >= 1));
                    break;
                case "Poet":
                    GameObject.FindFirstObjectByType<Poet>().SetActivation((boughtCnt >= 1));
                    break;
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
                UpdateButtonDisplayInfo(name);
            }
        }

        void UpdateButtonDisplayInfo(string name)
        {
            //テキストの更新
            // _npcNameButtonDic[name].GetComponentInChildren<TMP_Text>().text =
            //     $"{name} Lv{GetNPCShopHistory[name]} {CalculateNPCCost(name).ToString("F0")}G";
        }

        /// <summary> NPCの購入数に応じた価格の算出 </summary>
        public decimal CalculateNPCCost(string npcName)
        {
            return base.CalculateCostToBuy(npcName);
        }

        private void LateUpdate()
        {
            // 各購入ボタンへコストの表示
            for (int i = 0; i < _shopButton.Length; ++i)
            {
                switch (i)
                {
                    case 0:
                        _shopButton[i].GetComponentInChildren<TMP_Text>().text = "戦士 : " + CalculateNPCCost("Warrior").ToString();
                        break;
                    case 1:
                        _shopButton[i].GetComponentInChildren<TMP_Text>().text = "魔法使い : " + CalculateNPCCost("Wizard").ToString();
                        break;
                    case 2:
                        _shopButton[i].GetComponentInChildren<TMP_Text>().text = "盗賊 : " + CalculateNPCCost("Thief").ToString();
                        break;
                    case 3:
                        _shopButton[i].GetComponentInChildren<TMP_Text>().text = "仙人 : " + CalculateNPCCost("Hermit").ToString();
                        break;
                    case 4:
                        _shopButton[i].GetComponentInChildren<TMP_Text>().text = "詩人 : " + CalculateNPCCost("Poet").ToString();
                        break;
                }
            }
        }
    }
}