using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;


namespace StarryLab
{
    [InitializeOnLoad]
    public class FindAllReferencesHelper : AssetPostprocessor {

        static string m_CachePath;

        static FindAllReferencesHelper()
        {
            m_CachePath = Path.GetFullPath(Path.Combine(Application.dataPath, "../Library/FindAllReferences"));
            if (!Directory.Exists(m_CachePath))
            {
                Directory.CreateDirectory(m_CachePath);
                BuildCaches();
                Debug.Log("[FindAllReferences] Caches Path: " + m_CachePath);
            }
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string str in importedAssets)
            {
                //Debug.Log("Reimported Asset: " + str);
                string ext = Path.GetExtension(str);
                if (ext == ".prefab" || ext == ".unity" || ext == ".mat" || ext == ".controller" || ext == ".asset")
                {
                    string guid = AssetDatabase.AssetPathToGUID(str);
                    string[] dependencies = AssetDatabase.GetDependencies(str);
                    CreateCache(guid, dependencies);
                }
            }

            foreach (string str in deletedAssets)
            {
                //Debug.Log("Deleted Asset: " + str);
                string cache = Path.Combine(m_CachePath, Path.GetFileName(str));
                if (File.Exists(cache))
                {
                    File.Delete(cache);
                }
            }
        }

        public static string[] GetDependencies(string guid)
        {
            string[] dependencies;
            if (LoadCache(guid, out dependencies))
            {
                return dependencies;
            }

            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            dependencies = AssetDatabase.GetDependencies(assetPath, false);

            if (!Directory.Exists(m_CachePath))
            {
                Directory.CreateDirectory(m_CachePath);
            }
            CreateCache(guid, dependencies);

            return dependencies;
        }

        static void BuildCaches()
        {
            EditorUtility.DisplayProgressBar("[FindAllReferences] - Building caches ...", "To path: " + m_CachePath, 0f);

            string filter = "t:Prefab t:Scene t:ScriptableObject t:Material t:AnimatorController";
            string[] guids = AssetDatabase.FindAssets(filter);

            for (int i = 0; i < guids.Length; i++)
            {
                CreateCache(guids[i]);
                EditorUtility.DisplayProgressBar("[FindAllReferences] - Building caches ...", "To path: " + m_CachePath, 1f*i/guids.Length);
            }

            EditorUtility.ClearProgressBar();
        }

        public static bool LoadCache(string guid, out string[] dependencies)
        {
            string cachePath = Path.Combine(m_CachePath, guid);
            if (!File.Exists(cachePath))
            {
                dependencies = null;
                return false;
            }
            
            StreamReader sr = new StreamReader(cachePath);
            dependencies = sr.ReadLine().Split('|');
            sr.Close();

            return true;
        }

        public static void CreateCache(string guid)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            CreateCache(guid, AssetDatabase.GetDependencies(assetPath, false));
        }

        public static void CreateCache(string guid, string[] dependencies)
        {
            StreamWriter sw = new StreamWriter(Path.Combine(m_CachePath, guid));
            for (int i = 0; i < dependencies.Length; i++)
            {
                dependencies[i] = AssetDatabase.AssetPathToGUID(dependencies[i]);
            }
            sw.WriteLine(string.Join("|", dependencies));
            sw.Close();
        }
    }
}
