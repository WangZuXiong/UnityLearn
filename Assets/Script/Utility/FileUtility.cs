using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileUtility
{

    /// <summary>
    /// 写入文本到本地
    /// </summary>
    /// <param name="path">文本的保存路径</param>
    /// <param name="fileName">文件的名字</param>
    /// <param name="content">写入内容</param>
    public static void WriteTextToLocal(string path, string fileName, string content)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        FileInfo fileInfo = new FileInfo(path + "/" + fileName);
        StreamWriter streamWriter = fileInfo.CreateText();
        streamWriter.WriteLine(content);
        streamWriter.Close();
        streamWriter.Dispose();
    }


    public static void WriteTextToLocal1(string path, string fileName, string content)
    {
        File.WriteAllText(path + "/" + fileName, content);
    }
}

