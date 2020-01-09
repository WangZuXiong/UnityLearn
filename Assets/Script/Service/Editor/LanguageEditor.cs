using UnityEngine;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(LanguageText), true)]
public class LanguageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
            return;
        Debug.LogError("OnInspectorGUI");

        LanguageText temp = target as LanguageText;
        var value = LanguageService.Instance.GetString(temp.Key);
        if (!string.IsNullOrEmpty(value))
        {
            temp.GetComponent<Text>().text = value;
        }
        else
        {
            EditorGUILayout.LabelField("Error", "找不到对应的文案配置：key=" + temp.Key);
        }

        //EditorGUILayout.TextField("Test", "123")
        EditorGUILayout.Popup("Test", 1, new string[] { "1", "2" });



        //Debug.LogError();

        //var service = LanguageService.Instance;
        //if (service == null || service.Strings == null)
        //{
        //    var p = EditorGUILayout.TextField("Key", Target.Key);
        //    if (p != Target.Key)
        //    {
        //        Target.Key = p;
        //        EditorUtility.SetDirty(target);
        //    }
        //    EditorGUILayout.LabelField("Error ", "ILocalizationService Not Found");
        //}
        //else
        //{
        //    var languages = service.LanguageNames.ToArray();
        //    var languageIdx = Array.IndexOf(languages, service.Language.Name);
        //    var language = EditorGUILayout.Popup("Language", languageIdx, languages);
        //    if (language != languageIdx)
        //    {
        //        Target.Language = languages[language];
        //        service.Language = new LanguageInfo(languages[language]);

        //        EditorUtility.SetDirty(target);
        //    }
        //    if (!string.IsNullOrEmpty(Target.Key))
        //    {
        //        Target.Value = service.GetStringByKey(Target.Key, string.Empty);
        //    }
        //    var files = service.StringsByFile.Select(o => o.Key).ToArray();

        //    var findex = Array.IndexOf(files, Target.File);
        //    var fi = EditorGUILayout.Popup("File", findex, files);

        //    if (findex == -1)
        //    {
        //        Target.File = files[0];
        //        EditorUtility.SetDirty(target);
        //    }
        //    if (fi != findex)
        //    {
        //        Target.File = files[fi];
        //        EditorUtility.SetDirty(target);
        //    }
        //    //
        //    if (!string.IsNullOrEmpty(Target.File))
        //    {
        //        var words = service.StringsByFile[Target.File].Select(o => o.Key).ToArray();
        //        var index = Array.IndexOf(words, Target.Key);

        //        var i = EditorGUILayout.Popup("Keys", index, words);

        //        if (i != index)
        //        {
        //            Target.Key = words[i];
        //            Target.Value = service.GetStringByKey(Target.Key, string.Empty);
        //            EditorUtility.SetDirty(target);
        //            _inputKey = Target.Key;
        //        }

        //    }

        //    if (!string.IsNullOrEmpty(Target.File))
        //    {
        //        _inputKey = EditorGUILayout.TextField("Key", _inputKey);

        //        if (!string.IsNullOrEmpty(_inputKey))
        //        {
        //            Target.Key = _inputKey;
        //            Target.Value = service.GetStringByKey(Target.Key, string.Empty);
        //            EditorUtility.SetDirty(target);
        //        }

        //    }

        //    if (!string.IsNullOrEmpty(Target.Value))
        //    {
        //        EditorGUILayout.LabelField("Value ", Target.Value);
        //        Target.GetComponent<UnityEngine.UI.Text>().text = Target.Value;
        //    }
        //}


    }
}

