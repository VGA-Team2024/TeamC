using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SgLibUnite.Singleton;

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
    public class Shop : SingletonBaseClass<Shop>, IShop, IInitializedTarget
    {
        [SerializeField] private TMP_Text[] _npcLabels;

        [SerializeField] private Button[] _shopButton;

        private Player _player;

        [SerializeField, Tooltip("購入可能なNPCのリスト"), Header("The Buyable NPCs")]
        private List<NPCDataTemplate> npcList;

        /// <summary> NPCの名前に対応したそのNPCの購入数を格納 TName, TBoughtCount </summary>
        protected Dictionary<string, int> _npcShopHistory = new Dictionary<string, int>(); // name, bought-count

        public void InitializeObject()
        {
            ClientDataTemplate data = new ClientDataTemplate();
            try
            {
                data = GameObject.FindFirstObjectByType<ClientDataSaverSuperClass>().ReadData();

                if (data != null)
                {
                    // get each npc level to calculate bought-count
                    var wror = data._saveWarriorLevel;
                    var wzrd = data._saveWizardLevel;
                    var thf = data._saveThiefLevel;
                    var hrmt = data._saveHermitLevel;
                    var pt = data._savePoetLevel;

                    this._npcShopHistory.Add("Warrior", wror);
                    this._npcShopHistory.Add("Wizard", wzrd);
                    this._npcShopHistory.Add("Thief", thf);
                    this._npcShopHistory.Add("Hermit", hrmt);
                    this._npcShopHistory.Add("Poet", pt);
                }
            }
            catch (FileNotFoundException e)
            {
                this._npcShopHistory.Add("Warrior", 0);
                this._npcShopHistory.Add("Wizard", 0);
                this._npcShopHistory.Add("Thief", 0);
                this._npcShopHistory.Add("Hermit", 0);
                this._npcShopHistory.Add("Poet", 0);
                // make exist file
                GameObject.FindFirstObjectByType<ClientDataSaverSuperClass>().SaveData();
                throw;
            }

            _player = GameObject.FindFirstObjectByType<Player>();
        }

        public void PauseObject()
        {
            throw new NotImplementedException();
        }

        public void ResumeObject()
        {
            throw new NotImplementedException();
        }

        public void FinalizeObject()
        {
            // save shop info etc...
        }

        /// <summary> 購入処理。プレイヤーのリソースを減らす処理と、購入数の＋１処理のみ </summary>
        public void DecreasePlayerSource(string npcName, decimal cost, Action<int, string> taskToInstantiate)
        {
            // get player
            var player = GameObject.FindFirstObjectByType<Player>();
            // process buying 
            // get bought count
            var boughtCnt = 0;
            this._npcShopHistory.TryGetValue(npcName, out boughtCnt);
            player.DecreasePlayerGold(cost);
            // task to instantiate
            taskToInstantiate(boughtCnt + 1, npcName);
            // increment bought count 
            _npcShopHistory.Remove(npcName);
            _npcShopHistory.Add(npcName, boughtCnt + 1);
        }

        /// <summary> NPC名に応じたNPCの購入数に応じた価格を算出して返す </summary>
        public decimal CalculateCostToBuy(string npcName)
        {
            if (_npcShopHistory.Count == 0) return 1;
            // get bought count
            var boughtCnt = 0;
            _npcShopHistory.TryGetValue(npcName, out boughtCnt);
            // get target npc
            var target = npcList.Where(_ => _.Name == npcName).ToList();

            decimal cost = 0;
            // get base-price
            if (target.Count == 1) cost = (decimal)target[0].BasePrice;
            // calculate cost
            for (int i = 0; i < boughtCnt; i++)
            {
                cost *= (decimal)1.15;
            }

            return cost;
        }

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
            var cgold = GameObject.FindFirstObjectByType<Player>().GetCurrentGold();
            var cost = CalculateNPCCost(name);

            if (cgold >= cost)
            {
                Debug.Log("YOU CAN BUY!");
                // Superクラスでは、購入数に応じてコストを算出し、
                // それをプレイヤーへコストの適応をして、購入数を＋１しただけ
                DecreasePlayerSource(name, cost, TaskToInstantiateNPC);
                UpdateButtonDisplayInfo(name);
            }
            else
            {
                Debug.Log("NEVER!");
            }
        }

        void UpdateButtonDisplayInfo(string name)
        {
            var cnt = 0;
            var index = name switch { "Warrior" => 0, "Wizard" => 1, "Thief" => 2, "Hermit" => 3, "Poet" => 4 };
            _npcShopHistory.TryGetValue(
                index switch { 0 => "Warrior", 1 => "Wizard", 2 => "Thief", 3 => "Hermit", 4 => "Poet" },
                out cnt
            );

            var costTxt = (CalculateCostToBuy(
                index switch
                {
                    0 => "Warrior", 1 => "Wizard",
                    2 => "Thief", 3 => "Hermit",
                    4 => "Poet"
                }
            ).ToString("F0") + "G");

            _shopButton[index].GetComponentInChildren<TMP_Text>().text = " " +
                                                                         index switch
                                                                         {
                                                                             0 => "戦士", 1 => "魔法使い", 2 => "盗賊",
                                                                             3 => "仙人", 4 => "詩人"
                                                                         }
                                                                         + "Lv"
                                                                         + (cnt > -1
                                                                             ? (cnt).ToString()
                                                                             : String.Empty)
                                                                         + " "
                                                                         + costTxt;

            switch (name)
            {
                case "Warrior":
                    FindFirstObjectByType<Warrior>().TaskOnShopBoughtCharacter(cnt);
                    break;
                case "Wizard":
                    FindFirstObjectByType<Wizard>().TaskOnShopBoughtCharacter(cnt);
                    break;
                case "Thief":
                    FindFirstObjectByType<Thief>().TaskOnShopBoughtCharacter(cnt);
                    break;
                case "Hermit":
                    FindFirstObjectByType<Hermit>().TaskOnShopBoughtCharacter(cnt);
                    break;
                case "Poet":
                    FindFirstObjectByType<Poet>().TaskOnShopBoughtCharacter(cnt);
                    break;
            }
        }

        void UpdateButtonDisplayInfo()
        {
            var cnt = 0;
            //テキストの更新
            for (int i = 0; i < _shopButton.Length; i++)
            {
                _npcShopHistory.TryGetValue(
                    i switch { 0 => "Warrior", 1 => "Wizard", 2 => "Thief", 3 => "Hermit", 4 => "Poet" },
                    out cnt
                );

                var costTxt = (CalculateCostToBuy(
                    i switch
                    {
                        0 => "Warrior", 1 => "Wizard",
                        2 => "Thief", 3 => "Hermit",
                        4 => "Poet"
                    }
                ).ToString("F0") + "G");

                _shopButton[i].GetComponentInChildren<TMP_Text>().text = " " +
                                                                         i switch
                                                                         {
                                                                             0 => "戦士", 1 => "魔法使い", 2 => "盗賊",
                                                                             3 => "仙人", 4 => "詩人"
                                                                         }
                                                                         + "Lv"
                                                                         + (cnt > -1
                                                                             ? (cnt).ToString()
                                                                             : String.Empty)
                                                                         + " "
                                                                         + costTxt;
            }
        }

        /// <summary> NPCの購入数に応じた価格の算出 </summary>
        public decimal CalculateNPCCost(string npcName)
        {
            return CalculateCostToBuy(npcName);
        }

        protected override void ToDoAtAwakeSingleton()
        {
        }

        private void Start()
        {
        }

        private void Update()
        {
            UpdateButtonDisplayInfo();
        }
    }
}