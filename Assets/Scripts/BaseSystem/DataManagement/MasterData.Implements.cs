using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;

using DataManagement;

namespace DataManagement
{
    /// <summary>
    /// マスターデータ管理クラス
    /// NOTE: このクラスは破壊的変更を行う可能性があるので注意
    /// </summary>
    public partial class MasterData
    {
        //設定
        const string DataPrefix = "DataAsset/MasterData";


        //マスターデータ読み込みリスト
        static public TextMaster TextMaster { get; private set; }
        static public EnemyMaster EnemyMaster { get; private set; }


        //読み込み処理
        async UniTask MasterDataLoad()
        {
            //マスタ読み込み
            TextMaster = new TextMaster();
            EnemyMaster = new EnemyMaster();

            await UniTask.WhenAll(new List<UniTask>()
            {
                TextMaster.Marshal(),
                EnemyMaster.Marshal(),
            });
        }
    }
}

/*
//// マスターデータ実装の流れ ////

1. スプレッドシートにシートを増やします

2. スプレッドシート用のデータ定義を追加します
「SpreadSheetDataFormat.cs」に、1行のデータとその配列をDataというメンバで持つクラスの2つを定義してください
既にあるデータを参考にするとよいです

3. ゲーム中に使うマスターデータクラスを宣言する
「MasterDataFormat.cs」にクラスを宣言します。
追加の方法は、後に書いてある「クラス定義について」を読んでください

4. このクラスに追加したクラスを追記します。
・マスタのプロパティの追加
・new
・Marshal関数の追加
をそれぞれ行ってください。
*/

namespace Sample
{
    // (2)でこういうクラスを追加したと仮定します
    namespace SpreadSheet
    {
        /// <summary>
        /// テキストデータ
        /// </summary>
        [Serializable]
        public class SampleData
        {
            public int Id;
            public string Name;
            public int Atk;
            public string Text;
        }

        [Serializable]
        public class SampleMaster : SpreadSheetDataObject
        {
            public SampleData[] Data;
        }
    }


    // (3)でこういうクラスを追加したと仮定します
    [Serializable]
    public class SampleData
    {
        public int Id;
        public string Name;
        public int Atk;
    };

    /// <summary>
    /// マスタ宣言の例です
    /// </summary>
    [Serializable] //Serializableは忘れないようにしましょう
    public class SampleMaster : MasterDataBase<int, SampleData>
    {
        //必ずMasterDataBaseクラスを継承しましょう。
        //Tは <マスターデータのキーの型, マスターデータのデータクラスの型> を指定します。
        //1つ目はキーとなるデータです。マスターデータで探す際にキーとするものを選びましょう。intかstringになります。
        //2つ目は(3)で定義したデータ型です。
        //
        //NOTE: マスターデータは辞書配列で持っています。
        //      ローカルにデータとして保存できるようにしたいのですが、辞書配列はデフォルトではシリアライズできません。
        //      SerializableDictionaryを使用しています。


        //MasterNameはクラス名と合わせてください
        public override string MasterName => "SampleMaster";

        public const int INVALID_DATA = -1;

        //スプレッドシートからデータを読み込み展開をする関数です
        public override async UniTask Marshal()
        {
            // マスタ読み込み処理です。
            // (2)で定義したマスタデータの型を指定し、それと対応するスプレッドシートのシート名を入力してください。
            // 正常に処理されれば戻り値にスプレッドシートのデータが入った状態で返却されます。
            // 正常に処理されていない場合はパース処理かGASスクリプトに問題があります。解決できない場合は連絡してください。
            var master = await MasterData.LoadMasterData<SpreadSheet.SampleMaster>("JP_Text");

            // 整形処理です。パーサーとも言います。
            pretty(master.Data, (SpreadSheet.SampleData data) => {
                //スプレッドシートのデータを必要に応じて整形して格納します。
                var sampleData = new SampleData();
                sampleData.Id = data.Id;
                sampleData.Name = data.Name;
                sampleData.Atk = data.Atk;

                //特定のデータを無視したりもできます
                if (data.Atk == 0)
                {
                    UnityEngine.Debug.Log("多分怪しいデータなので無視する");
                    return (INVALID_DATA, null);
                }

                //MasterDataBaseで指定したキーとデータクラスを返却します
                return (data.Id, sampleData);
            });


            // 整形処理は、特にデータをチェックする必要がなければ以下のように1行で書くこともできます
            //pretty(master.Data, (SpreadSheet.SampleData data) => { return (data.Id, new SampleData(){ Id = data.Id, Atk = data.Atk, Name = data.Name }); });
        }
    };
}