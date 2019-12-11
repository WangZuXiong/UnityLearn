using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorTool
{
    [MenuItem("Tools/create")]
    static void Start1()
    {
        GameObject root = GameObject.Find("Root");

        if (root)
        {
            MeshRenderer[] meshRenderers = root.GetComponentsInChildren<MeshRenderer>();

            GameObject[] gos = new GameObject[meshRenderers.Length];
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                gos[i] = meshRenderers[i].gameObject;
            }

            string scenePath = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).path;
            string meshScenePath = scenePath.Replace(".unity", "_mesh");
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), meshScenePath);

            if (Directory.Exists(fullPath))
                Directory.Delete(fullPath, true);
            Directory.CreateDirectory(fullPath);
            string assetPath = FileUtil.GetProjectRelativePath(fullPath);

            StaticBatchingUtility.Combine(gos, root);
            for (int i = 0; i < gos.Length; i++)
            {
                Mesh mesh = gos[i].GetComponent<MeshFilter>().sharedMesh;
                string meshPath = AssetDatabase.GetAssetPath(mesh);

                if (string.IsNullOrEmpty(meshPath))
                {
                    string path = Path.Combine(assetPath, "111" + ".asset");
                    AssetDatabase.CreateAsset(mesh, path);
                }

            }
        }
        AssetDatabase.Refresh();
        EditorSceneManager.MarkAllScenesDirty();
    }
}
