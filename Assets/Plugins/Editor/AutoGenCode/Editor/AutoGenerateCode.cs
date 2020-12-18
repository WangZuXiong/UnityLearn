using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

public static class AutoGenerateCode
{
    /// <summary>
    /// 代码的输出路径
    /// </summary>
    private const string ScriptOutputPath = "Script";

    /// <summary>
    /// 模板的路径
    /// </summary>
    private const string StencilPath = "Assets/AutoGenCode/Stencil";
    private const string BtnMemberStr = "private Button {0};";
    private const string BtnAssignmentAndAddListenerStr = "{0} = transform.Find(\"{1}\").GetComponent<Button>();\n{0}.onClick.AddListener(On{0}Click);";
    private const string BtnMethod = "private void On{0}Click( )\n{\n}";


    [MenuItem("GameObject/Gen Code", false, 22)]
    static void GenCode()
    {
        var temp = Selection.activeTransform;
        if (temp == null)
        {
            return;
        }

        var btns = temp.GetComponentsInChildren<Button>(true);

        List<string> vs = new List<string>();
        for (int i = 0; i < btns.Length; i++)
        {
            var path = GetTransformFullPath(temp, btns[i].transform);
            var str = string.Format("Button {0} = transform.Find(\"{1}\").GetComponent<Button>();\n{0}.onClick.AddListener(On{0}Click);", btns[i].transform.name, path);
            vs.Add(str);
        }
        var result = string.Join("\n", vs.ToArray());

        TextEditor textEditor = new TextEditor
        {
            text = result
        };
        textEditor.OnFocus();
        textEditor.Copy();
    }

    [MenuItem("GameObject/Gen View Script", false, 22)]
    static void GenViewScript()
    {
        var temp = Selection.activeTransform;
        if (temp == null)
        {
            return;
        }
        //已经存在的话就提示是否要覆盖，是否需提交备份
        var fileName = string.Format("{0}View.cs", temp.name);
        var path = Path.Combine(Application.dataPath, ScriptOutputPath, fileName);

        bool isCover = true;
        if (File.Exists(path))
        {
            var message = string.Format("已经存在文件：\n{0}\n是否覆盖", path);
            isCover = EditorUtility.DisplayDialog("提示", message, "确认覆盖");
        }

        if (!isCover)
        {
            return;
        }

        var stencilStringFormat = AssetDatabase.LoadAssetAtPath<TextAsset>(Path.Combine(StencilPath, "View.txt"));

        //button
        List<string> btnMemberStrs = new List<string>();
        List<string> btnAssignmentAndAddListenerStrs = new List<string>();

        List<string> btnMethodStrs = new List<string>();

        var btns = temp.GetComponentsInChildren<Button>(true);

        for (int i = 0; i < btns.Length; i++)
        {
            var btnName = btns[i].transform.name;
            var btnFullPath = GetTransformFullPath(temp, btns[i].transform);

            btnMemberStrs.Add(string.Format(BtnMemberStr, btnName));
            btnAssignmentAndAddListenerStrs.Add(string.Format(BtnAssignmentAndAddListenerStr, btnName, btnFullPath));
            btnMethodStrs.Add(StringFormat(BtnMethod, btnName));
        }

        var btnMember = string.Join("\n", btnMemberStrs.ToArray());
        var btnAssignmentAndAddListener = string.Join("\n", btnAssignmentAndAddListenerStrs.ToArray());
        var btnMethod = string.Join("\n", btnMethodStrs.ToArray());


        var result = StringFormat(stencilStringFormat.text, temp.name, btnMember, btnAssignmentAndAddListener, btnMethod);





        File.WriteAllText(path, result);
        AssetDatabase.Refresh();
    }

    private static string GetTransformFullPath(Transform parent, Transform target)
    {
        List<string> vs = new List<string>();

        while (target.parent != null && !target.Equals(parent))
        {
            vs.Add(target.name);
            target = target.parent;
        }
        vs.Reverse();

        return string.Join("/", vs.ToArray());
    }

    private static string StringFormat(string stringFormat, params string[] vs)
    {
        for (int i = 0; i < vs.Length; i++)
        {
            stringFormat = stringFormat.Replace("{" + i.ToString() + "}", vs[i]);
        }
        return stringFormat;
    }


}
