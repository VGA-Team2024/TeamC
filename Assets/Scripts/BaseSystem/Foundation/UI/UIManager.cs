using UISystem;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.AddressableAssets;
using Unity.VisualScripting.Antlr3.Runtime;
#if USE_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif

namespace UISystem
{
    public class UIManager
    {
        static UIManager _instance = new UIManager();
        //public static UIManager Instance => _instance;
        private UIManager() { }

        RectTransform _root;
        UIView _current = null;
        Stack<UIView> _uiStack = new Stack<UIView>();

        Dictionary<int, GameObject> _sceneCache = new Dictionary<int, GameObject>();


        public static void Setup(ViewID entry)
        {
            _instance._uiStack.Clear();

            var rootCanvas = GameObject.FindObjectOfType<Canvas>();
            _instance._root = rootCanvas.GetComponent<RectTransform>();
            _instance.LoadView(entry);
        }

        public static void ClearCache()
        {
            foreach (var scene in _instance._sceneCache)
            {
                Addressables.Release(scene);
            }
        }

        GameObject LoadView(ViewID next, bool isStack = false)
        {
            GameObject sceneOrigin = null;
            if (_sceneCache.ContainsKey((int)next))
            {
                sceneOrigin = _sceneCache[(int)next];
            }
            else
            {
                sceneOrigin = Addressables.LoadAssetAsync<GameObject>(string.Format("Assets/Scenes/Game/UI/{0}.prefab", next.ToString())).WaitForCompletion();
                _sceneCache[(int)next] = sceneOrigin;
            }

            if (sceneOrigin == default)
            {
                Debug.LogError($"{next.ToString()}: シーンの読み込みに失敗");
                return null;
            }

            return sceneOrigin;
        }

        static UIView CreateView(GameObject sceneOrigin)
        {
            //Viewを生成する
            var scene = GameObject.Instantiate(sceneOrigin, _instance._root);
            var view = scene.GetComponent<UIView>();
            if (view == default)
            {
                Debug.LogError($"{sceneOrigin.name}: シーン管理スクリプトの読み込みに失敗");
                return null;
            }

            view.Enter();

            return view;
        }


        public static void ChangeView(ViewID next)
        {
            var origin = _instance.LoadView(next);

            //現在のビューがあれば閉じる
            _instance._current?.Exit();

            //Viewを生成する
            var view = CreateView(origin);
            if (view == null) return;

            GameObject.Destroy(_instance._current.gameObject);

            _instance._current = view;
        }

        public static void StackView(ViewID next)
        {
            var origin = _instance.LoadView(next);

            //Viewを生成する
            var view = CreateView(origin);
            if (view == null) return;

            _instance._uiStack.Push(_instance._current);

            _instance._current = view;
        }

        public static bool Back()
        {
            if (!_instance._current) return false;

            _instance._current.Exit();

            if (_instance._uiStack.Count > 0)
            {
                GameObject.Destroy(_instance._current.gameObject);

                _instance._current = _instance._uiStack.Pop();

                return true;
            }

            return false;
        }
    }
}