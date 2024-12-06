#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class RenameToolEditor : EditorWindow
{
    private List<Object> _obj = new();
    private string _prefix;
    private string _suffix;
    private int _startIndex;
    private int _fill0;
    private bool _fold = true;
    private bool _sequenceMode;
    private int _selected;
    private bool _doRename;

    [MenuItem("Tools/RenameToolEditor")]
    private static void Init()
    {
        GetWindow<RenameToolEditor>();
    }

    private void OnGUI()
    {
        _selected = GUILayout.Toolbar(_selected, new[] { "文字数追加","連番" });

        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("接頭語");
            _prefix = EditorGUILayout.TextField(_prefix);
        }

        if (_selected == 1)
        {
            _sequenceMode = EditorGUILayout.Toggle("連続モード", _sequenceMode);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("開始番号");
                _startIndex = EditorGUILayout.IntField(_startIndex);
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("接尾語");
            _suffix = EditorGUILayout.TextField(_suffix);
        }

        if (_selected == 1)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("0埋め桁数");
                _fill0 = EditorGUILayout.IntField(_fill0);
            }
        }
        
        GUILayout.Space(20);

        _obj = _obj.Where(x => x != null).ToList();

        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            if (_fold == EditorGUILayout.Foldout(_fold, "選択対象", true))
            {
                EditorGUI.indentLevel++;

                for (int i = 0; i < _obj.Count; i++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        var temp = _obj[i];

                        if (GUILayout.Button("↑", GUILayout.Width(20)))
                        {
                            _obj[i] = _obj[(i - 1 + _obj.Count) % _obj.Count];
                            _obj[(i - 1) % _obj.Count] = temp;
                        }

                        if (GUILayout.Button("x", GUILayout.Width(20)))
                        {
                            _obj[i] = null;
                        }

                        _obj[i] = EditorGUILayout.ObjectField(_obj[i], typeof(Object), false);
                        GUILayout.FlexibleSpace();

                        Object currentObj = _obj[i];
                        string newName = "";
                        switch (_selected)
                        {
                            case 0:
                            {
                                newName = $"{_prefix}{currentObj.name}{_suffix}";
                            }
                                break;
                            case 1:
                            {
                                newName = $"{_prefix}{(_startIndex + i).ToString($"D{_fill0}")}{_suffix}";
                            }
                                break;
                        }
                        GUILayout.Label(newName);
                        if (_doRename)
                        {
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(currentObj), newName);
                        }
                    }
                }

                EditorGUI.indentLevel--;
            }

            if (_doRename)
            {
                if (_sequenceMode)
                {
                    _startIndex += _obj.Count;
                    _obj = new();
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("選択追加", GUILayout.Height(40)))
                {
                    var newObjs = Selection.GetFiltered(typeof(Object),SelectionMode.Assets).ToList();
                    newObjs = newObjs.OrderBy(x => x.name).ToList();
                    _obj.AddRange(newObjs);
                    _obj = _obj.Distinct().ToList();
                }

                if (GUILayout.Button("選択全解除", GUILayout.Height(40)))
                {
                    _obj = new List<Object>();
                }
            }
        }

        _doRename = false;

        if (GUILayout.Button("リネーム", GUILayout.Height(40)))
        {
            if (EditorUtility.DisplayDialog("実行します", "実行しますか", "OK", "cancel"))
            {
                _doRename = true;
            }
        }
    }
}
#endif