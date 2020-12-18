using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TextureHelper : EditorWindow
{
    private string _inputPath;
    private bool _isToBigger = false;

    [MenuItem("Tools/SimpleTool/裁剪or补齐 Texture")]
    private static void Init()
    {
        var window = GetWindow(typeof(TextureHelper), false);
        window.titleContent = new GUIContent("裁剪/补齐 Texture");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.TextField(new GUIContent("图片(PNG/JPG)", "选择 PNG/JPG文件"), _inputPath, GUILayout.MinWidth(120), GUILayout.MaxWidth(500));
        if (GUILayout.Button(new GUIContent("选择"), GUILayout.Width(80), GUILayout.Height(20)))
        {
            var tempPath = EditorUtility.OpenFilePanel("选择图片", _inputPath, "png,jpg");
            _inputPath = string.IsNullOrEmpty(tempPath) ? _inputPath : tempPath;
        }
        GUILayout.EndHorizontal();

        if (!string.IsNullOrEmpty(_inputPath))
        {
            var bytes = File.ReadAllBytes(_inputPath);
            Texture2D sourceTexture2D = new Texture2D(10, 10);
            sourceTexture2D.LoadImage(bytes);

            Texture miniTex = AssetPreview.GetMiniThumbnail(sourceTexture2D);
            var scale = (float)miniTex.height / 100;
            GUILayout.Box(miniTex, GUILayout.Width(miniTex.width / scale), GUILayout.Height(100));

            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(!_isToBigger, new GUIContent("裁剪（变小）"), GUILayout.MinWidth(80), GUILayout.MaxWidth(100)))
            {
                _isToBigger = false;
            }
            if (GUILayout.Toggle(_isToBigger, new GUIContent("补齐（变大）"), GUILayout.MinWidth(80), GUILayout.MaxWidth(100)))
            {
                _isToBigger = true;
            }
            GUILayout.EndHorizontal();

            var index = _inputPath.LastIndexOf('/');
            var fileName = _inputPath.Substring(index + 1, _inputPath.Length - index - 1);
            GUILayout.Label(string.Format("文件名：{0}", fileName));
            GUILayout.Label(string.Format("处理前: {0} x {1}", sourceTexture2D.width.ToString(), sourceTexture2D.height.ToString()));
            (int width, int height) after = _isToBigger ? SizeCeiling((sourceTexture2D.width, sourceTexture2D.height)) : SizeFloor((sourceTexture2D.width, sourceTexture2D.height));
            GUILayout.Label(string.Format("处理后: {0} x {1}", after.width.ToString(), after.height.ToString()));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("保存(覆盖)"), GUILayout.MinWidth(80), GUILayout.MaxWidth(100)))
            {
                var isJpg = _inputPath.ToLower().Contains(".jpg");
                Texture2D newTexture2D = new Texture2D(after.width, after.height);
                TexureOperation(sourceTexture2D, newTexture2D, _isToBigger);
                bytes = Texture2D2Bytes(newTexture2D, isJpg);
                Save(bytes, _inputPath);
                EditorUtility.DisplayDialog("Tips", "保存成功", "好的");
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button(new GUIContent("另存为"), GUILayout.MinWidth(80), GUILayout.MaxWidth(100)))
            {
                fileName = fileName.ToLower().Replace(".jpg", string.Empty).Replace(".png", string.Empty);
                var tempPath = EditorUtility.SaveFilePanel("图片另存为", _inputPath, fileName + "_new", "png,jpg");

                if (!string.IsNullOrEmpty(tempPath))
                {
                    var isJpg = _inputPath.ToLower().Contains(".jpg");
                    Texture2D newTexture2D = new Texture2D(after.width, after.height);
                    TexureOperation(sourceTexture2D, newTexture2D, _isToBigger);
                    bytes = Texture2D2Bytes(newTexture2D, isJpg);
                    Save(bytes, tempPath);
                    EditorUtility.DisplayDialog("Tips", "保存成功", "好的");
                    AssetDatabase.Refresh();
                }
            }
            GUILayout.EndHorizontal();

            GUI.color = new Color(1.0f, 0.95f, 0.8f, 1.0f);
            GUILayout.Label("示例：5x5的图片\"补齐\"之后是8x8;\"裁剪\"之后是4x4");
        }
    }

    private void TexureOperation(Texture2D source, Texture2D @new, bool isToBigger)
    {
        (int width, int height) offset = ((int)(Mathf.Abs(@new.width - source.width) * 0.5f), (int)(Mathf.Abs(@new.height - source.height) * 0.5f));

        for (int i = 0; i < @new.width; i++)
        {
            for (int j = 0; j < @new.height; j++)
            {
                if (isToBigger)
                {
                    @new.SetPixel(i + offset.width, j + offset.height, source.GetPixel(i, j));
                }
                else
                {
                    @new.SetPixel(i, j, source.GetPixel(i + offset.width, j + offset.height));
                }
            }
        }
    }

    private void Save(byte[] bytes, string path)
    {
        FileStream fileStream = File.Create(path);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
        fileStream.Dispose();
    }


    private byte[] Texture2D2Bytes(Texture2D texture2D, bool isJpg)
    {
        return isJpg ? texture2D.EncodeToJPG() : texture2D.EncodeToPNG();
    }


    /// <summary>
    /// 向上取整4
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private (int width, int height) SizeCeiling((int width, int height) input)
    {
        return (Ceiling(input.width), Ceiling(input.height));

        int Ceiling(int x)
        {
            if (x % 4 != 0)
            {
                return 4 * ((x / 4) + 1);
            }

            return x;
        }
    }

    /// <summary>
    /// 向下取整4
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private (int width, int height) SizeFloor((int width, int height) input)
    {
        return (Floor(input.width), Floor(input.height));

        int Floor(int x)
        {
            if (x % 4 != 0)
            {
                return 4 * (x / 4);
            }
            return x;
        }
    }
}
