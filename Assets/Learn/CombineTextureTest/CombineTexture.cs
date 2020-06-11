using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CombineTexture : EditorWindow
{
    static Texture2D Combine(Texture2D[] texs, ValueTuple<int, int>[] offests, int size)
    {
        Texture2D @out = new Texture2D(size, size, TextureFormat.ARGB32, true);

        for (int i = 0; i < texs.Length; i++)
        {
            var tex = texs[i];
            var offset = offests[i];
            var width = tex.width;
            var height = tex.height;

            RenderTexture tmp = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(tex, tmp);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = tmp;
            Texture2D @new = new Texture2D(width, height);
            @new.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            @new.Apply();
            @new.SetPixels(offset.Item1, offset.Item2, width, height, @new.GetPixels());
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(tmp);
        }

        return @out;
    }

    [MenuItem("Tools/Texture Combine")]
    static void CombineTexture1()
    {
        Texture2D texture2D1 = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Color/black.png");
        Texture2D texture2D2 = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Color/red.png");
        Texture2D @out = Combine(
            new[] { texture2D1, texture2D2 },//贴图
            new[] { (0, 0), (600, 600) },//偏移
            1024);//最终贴图的大小

        File.WriteAllBytes("Assets/Out.png", @out.EncodeToPNG());
        AssetDatabase.Refresh();
    }
}
