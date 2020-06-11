using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class SortLanguageConfig : EditorWindow
{
    private string _inputPath;

    private struct Config
    {
        public string Key;
        public string Value;
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUIContent inputFolderContent = new GUIContent("Input Folder", "json文件");
        EditorGUIUtility.labelWidth = 120.0f;
        EditorGUILayout.TextField(inputFolderContent, _inputPath, GUILayout.MinWidth(120), GUILayout.MaxWidth(500));
        if (GUILayout.Button(new GUIContent("选择语言配置文件"), GUILayout.MinWidth(80), GUILayout.MaxWidth(100)))
        {
            _inputPath = EditorUtility.OpenFilePanel("Select Json Files", _inputPath, "json");
        }
        GUILayout.EndHorizontal();



        if (GUILayout.Button("Sort"))
        {
            var list = ReadFile();

            for (int i = 0; i < list.Count; i++)
            {
                Debug.LogError(list[i].Key);
                Debug.LogError(list[i].Value);
            }
            SortList(list);
            WriteFile(list);
        }
    }

    private List<Config> ReadFile()
    {
        var sr = new StreamReader(_inputPath);
        string line;
        var list = new List<Config>();
        Config config;
        while ((line = sr.ReadLine()) != null)
        {
            Debug.LogError(line);
            //var index = line.IndexOf("\"", 1, 1);
            //if (index == -1)
            //{
            //    continue;
            //}
            //var key = line.Substring(0, index - 1);
            //config = new Config
            //{
            //    Key = key,
            //    Value = line
            //};
            //list.Add(config);
        }
        sr.Dispose();
        return list;
    }


    private void WriteFile(List<Config> configs)
    {
        //using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        //{
        //    using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
        //    {
        //        textWriter.Write(str);
        //    }
        //}
    }


    private void SortList(List<Config> configs)
    {
        configs.Sort((x, y) => x.Key.CompareTo(y.Key));
    }

    [MenuItem("Tools/SortLanguageConfig")]
    private static void ShowWindow()
    {
        var window = GetWindow(typeof(SortLanguageConfig), false);
        window.titleContent = new GUIContent("语言配置文件按照key排序");
        window.Show();
    }
}
