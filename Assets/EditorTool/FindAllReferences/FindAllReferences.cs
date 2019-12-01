using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEditor.Animations;


namespace StarryLab
{
    public class FindAllReferences : EditorWindow
    {
        // selected
    	static Object m_SelectedObj;
        static string m_SelectedObjPath;
        static bool m_SelectedIsPrefab;
        static bool m_SelectedIsMainAsset;
        static bool m_IsFindingGameObject;
        static bool m_IsFindingAsset;
        static List<Object> m_SelectedObjects = new List<Object>();
        // result
        static Dictionary<Object, List<HierarchyResult>> m_HierarchyResults = new Dictionary<Object, List<HierarchyResult>>();
        static Dictionary<Object, List<ProjectResult>> m_ProjectResults = new Dictionary<Object, List<ProjectResult>>();
        // gui
        GUIContent m_HierarchyLabelIcon;
        GUIContent m_SceneLabelIcon;
        GUIContent m_ProjectLabelIcon;
        GUIStyle m_HeaderStyle;
        GUIStyle m_TitleStyle;
        GUIStyle m_LabelStyle;
        const float labelTextOffset = 10;
        Vector2 m_ScrollPosition;
        bool m_Redraw;
        static int m_Progress, m_TotalCount;


    	#region MenuItems
        [MenuItem("GameObject/\u273F Find All References", true)]
    	static bool FindInHierarchyValidate()
    	{
    		return Selection.activeGameObject != null;
    	}

        [MenuItem("GameObject/\u273F Find All References", false, 20)]
    	static void FindInHierarchy()
    	{
    		//selected gameObject and its components
    		m_SelectedObj = Selection.activeGameObject;
            m_SelectedObjects.Clear();
            m_SelectedObjects.Add(m_SelectedObj);
            m_SelectedObjects.AddRange(Selection.activeGameObject.GetComponents<Component>());

            m_IsFindingGameObject = true;
            m_IsFindingAsset = false;

            m_HierarchyResults.Clear();
            m_ProjectResults.Clear();

            CheckScene(EditorSceneManager.GetActiveScene(), true);
            
            EditorUtility.ClearProgressBar();

    		ShowWindow();
    	}

        [MenuItem("Assets/\u273F Find All References", true)]
        static bool FindInProjectValidate()
        {
            if (Selection.activeObject != null)
                return !(Selection.activeObject is DefaultAsset);
            return false;
        }

        [MenuItem("Assets/\u273F Find All References", false, 0)]
        static void FindInProject()
        {
            m_SelectedObj = Selection.activeObject;
            if (PrefabUtility.GetPrefabType(m_SelectedObj) == PrefabType.Prefab)
            {
                m_SelectedIsPrefab = true;
                if (!AssetDatabase.IsMainAsset(m_SelectedObj))
                {
                    m_SelectedObj = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(m_SelectedObj.GetInstanceID()));
                }
            }
            m_SelectedObjPath = AssetDatabase.GetAssetPath(m_SelectedObj);
            m_SelectedIsMainAsset = AssetDatabase.IsMainAsset(m_SelectedObj);

            m_IsFindingAsset = true;
            m_IsFindingGameObject = false;

            m_HierarchyResults.Clear();
            m_ProjectResults.Clear();

            CheckScene(EditorSceneManager.GetActiveScene(), true);
            CheckAssets();

            //DoFindInHierarchy(MatchInProject, true);

            //DoFindInProject();

            EditorUtility.ClearProgressBar();

            ShowWindow();
        }
        #endregion

        #region Methods
        static void CheckScene(Scene scene, bool inHierarchy, Object asset = null)
        {
            List<GameObject> allObjects = new List<GameObject>();
            allObjects.AddRange(scene.GetRootGameObjects());
            if (inHierarchy && EditorApplication.isPlaying)
            {
                GameObject tmp = new GameObject();
                DontDestroyOnLoad(tmp);
                allObjects.AddRange(tmp.scene.GetRootGameObjects());
                allObjects.Remove(tmp);
                DestroyImmediate(tmp);
            }

            if (m_IsFindingGameObject)
            {
                m_TotalCount = allObjects.Count;
                m_Progress = 0;
            }

            foreach (GameObject go in allObjects)
            {
                if (m_IsFindingGameObject)
                    EditorUtility.DisplayProgressBar("Hierarchy", "Searching...", 1f*++m_Progress/m_TotalCount);

                CheckGameObject(go, inHierarchy, asset);
            }
        }

        static void CheckGameObject(GameObject go, bool inHierarchy, Object asset = null)
        {
            if (m_IsFindingAsset)
            {
                if (m_SelectedIsPrefab && PrefabUtility.GetPrefabParent(go) == m_SelectedObj)
                {
                    if (inHierarchy)
                    {
                        HierarchyResult r = new HierarchyResult() {
                            tip = "<prefab>"
                        };

                        SaveHierarchyResult(go, r);
                    }
                    else
                    {
                        string childpath = go.name;
                        Transform parent = go.transform.parent;
                        while (parent != null)
                        {
                            childpath = parent.name + " \u27A4 " + childpath;
                            parent = parent.parent;
                        }

                        ProjectResult r = new ProjectResult() {
                            childPath = childpath
                        };

                        SaveProjectResult(asset, r);
                    }
                    return;
                }
            }

            foreach (Component c in go.GetComponents<Component>())
            {
                CheckReference(c, inHierarchy, asset);
            }

            foreach (Transform child in go.transform)
            {
                CheckGameObject(child.gameObject, inHierarchy, asset);
            }
        }

        static void CheckReference(Object obj, bool inHierarchy, Object asset = null)
        {
            if (obj == null || obj is Transform)
                return;

            SerializedObject so = new SerializedObject(obj);
            SerializedProperty p = so.GetIterator();
            do
            {
                if (p.propertyType != SerializedPropertyType.ObjectReference || p.objectReferenceValue == null ||
                   (inHierarchy && p.propertyPath == "m_GameObject"))
                    continue;

                bool isMatched = false;

                if (m_IsFindingGameObject)
                {
                    if (m_SelectedObjects.Contains(p.objectReferenceValue))
                    {
                        isMatched = true;
                    }
                }
                else if (m_IsFindingAsset)
                {
                    if (p.objectReferenceValue == m_SelectedObj ||
                       (m_SelectedIsMainAsset && AssetDatabase.GetAssetPath(p.objectReferenceValue) == m_SelectedObjPath))
                    {
                        isMatched = true;
                    }
                }

                if (isMatched)
                {
                    if (inHierarchy)
                    {
                        Component c = obj as Component;
                        HierarchyResult r = new HierarchyResult() {
                            component = c,
                            property = p.objectReferenceValue,
                            propertyName = GetPropertyName(c, p)
                        };

                        if (obj is MonoBehaviour)
                        {
                            MonoScript script = MonoScript.FromMonoBehaviour(obj as MonoBehaviour);
                            string path = AssetDatabase.GetAssetPath(script.GetInstanceID());
                            if (path.StartsWith("Assets/"))
                                r.script = script;
                        }

                        if (r.script == null && r.propertyName.StartsWith("m_"))
                        {
                            r.propertyName = r.propertyName.Substring(2);
                        }

                        SaveHierarchyResult(c.gameObject, r);
                    }
                    else if (asset is SceneAsset || asset is GameObject)
                    {
                        Component c = obj as Component;

                        ProjectResult r = new ProjectResult() {
                            childPath = GetPropertyPath(asset, c, p),
                            propertyName = GetPropertyName(c, p),
                            referenceObject = p.objectReferenceValue
                        };

                        SaveProjectResult(asset, r);
                    }
                    else
                    {
                        ProjectResult r = new ProjectResult() {
                            childPath = p.propertyPath,
                            propertyName = p.displayName,
                            referenceObject = p.objectReferenceValue
                        };

                        SaveProjectResult(asset, r);
                    }
                }
            }
            while (p.Next(true));
        }

        static void CheckAssets()
        {
            m_SelectedObjPath = AssetDatabase.GetAssetPath(m_SelectedObj.GetInstanceID());
            string selectedGUID = AssetDatabase.AssetPathToGUID(m_SelectedObjPath);

            //filter
            string filter = "t:Prefab t:Scene t:ScriptableObject";
            if (m_SelectedObj is Texture2D || m_SelectedObj is Sprite || m_SelectedObj is Shader)
            {
                filter += " t:Material";
            }
            else if (m_SelectedObj is AnimationClip)
            {
                filter += " t:AnimatorController";
            }

            //all objects to check
            string[] fileGUIDs = AssetDatabase.FindAssets(filter);
            //Debug.Log("files count: " + fileGUIDs.Length);

            m_Progress = 0;
            m_TotalCount = fileGUIDs.Length;

            //prefab, scene, script, material, shader files
            foreach (string guid in fileGUIDs)
            {
                if (guid == selectedGUID)
                    continue;

                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                EditorUtility.DisplayProgressBar("Project", "Searching...", 1f * ++m_Progress / m_TotalCount);

                string[] dependencies = FindAllReferencesHelper.GetDependencies(guid);
                foreach (string dependeFile in dependencies)
                {
                    if (dependeFile.Equals(selectedGUID))
                    {
                        Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
                        CheckDependentFile(obj);
                        break;
                    }
                }
            }
        }

        static void CheckDependentFile(Object asset)
        {
            if (asset is AnimatorController)
            {
                ProjectResult r = new ProjectResult() {
                    childPath = "animationClips[]",
                    propertyName = "",
                    referenceObject = m_SelectedObj
                };

                SaveProjectResult(asset, r);
            }
            else if (asset is GameObject)
            {
                CheckGameObject(asset as GameObject, false, asset);
            }
            else if (asset is SceneAsset)
            {
                string assetPath = AssetDatabase.GetAssetPath(asset);
                if (EditorSceneManager.GetActiveScene().path != assetPath)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        Scene scene = EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Additive);
                        CheckScene(scene, false, asset);
                        EditorSceneManager.CloseScene(scene, true);
                    }
                    else if (!m_ProjectResults.ContainsKey(asset))
                    {
                        ProjectResult r = new ProjectResult() {
                            tip = "<Cannot show details in playing mode>"
                        };

                        SaveProjectResult(asset, r);
                    }
                }
            }
            else
            {
                CheckReference(asset, false, asset);
            }
        }
    	
        static string GetPropertyName(Component c, SerializedProperty p)
        {
            Object obj = p.objectReferenceValue;
            string name = p.propertyPath;

            if (c != null)
            {
                if (c is Text && name == "m_FontData.m_Font")
                    return "Font";

                if (c is Button && name.StartsWith("m_OnClick."))
                    return name.Replace ("m_OnClick.m_PersistentCalls.m_Calls.Array.data", "OnClick").Replace("m_", "");

                if (c is AudioSource && obj is AudioMixerGroup)
                    return "Output";
            }

            return name.Replace (".Array.data", "");
        }

        static string GetPropertyPath(Object root, Component c, SerializedProperty p)
        {
            string path = "";

            Transform parent = c.transform;
            if (parent.gameObject != root)
                path = c.name;

            path += " \u27A4 (" + c.GetType().Name + ")";

            while ((parent = parent.parent) != null && parent.gameObject != root)
            {
                path = parent.name + " \u27A4 " + path;
            }
            return path;
        }

        static void SaveHierarchyResult(Object obj, HierarchyResult r)
        {
            if (m_HierarchyResults.ContainsKey(obj))
                m_HierarchyResults[obj].Add(r);
            else
                m_HierarchyResults.Add(obj, new List<HierarchyResult>(){r});
        }

        static void SaveProjectResult(Object obj, ProjectResult r)
        {
            if (m_ProjectResults.ContainsKey(obj))
                m_ProjectResults[obj].Add(r);
            else
                m_ProjectResults.Add(obj, new List<ProjectResult>(){ r });
        }
        #endregion

    	#region Window
    	static void ShowWindow()
    	{
            EditorWindow.GetWindow<FindAllReferences>("\u273F References").Init();
    	}

    	void Init()
    	{
            m_HierarchyLabelIcon = new GUIContent(" Hierarchy", EditorGUIUtility.FindTexture("UnityEditor.HierarchyWindow"));
            m_SceneLabelIcon = new GUIContent("Scene", EditorGUIUtility.FindTexture("SceneAsset Icon"));
            m_ProjectLabelIcon = new GUIContent(" Project", EditorGUIUtility.FindTexture("Project"));

    		m_HeaderStyle = new GUIStyle("CN CountBadge");
    		m_HeaderStyle.alignment = TextAnchor.MiddleLeft;
    		m_HeaderStyle.imagePosition = ImagePosition.ImageLeft;
    		m_HeaderStyle.padding = new RectOffset(10, 5, -1, 1);

            m_TitleStyle = new GUIStyle("MeTimeLabel");

    		m_LabelStyle = new GUIStyle("GUIEditor.BreadcrumbMid");
    		m_LabelStyle.alignment = TextAnchor.MiddleLeft;
    		m_LabelStyle.padding = new RectOffset(10, 5, 1, 1);

            m_Redraw = true;
    	}

    	void OnGUI()
    	{
            GUI.SetNextControlName("FAR_empty");
            EditorGUILayout.Separator();

            if (m_SelectedObj == null)
            {
                EditorGUILayout.BeginVertical("RegionBg");
                EditorGUILayout.HelpBox("Usage:\n" +
                    "1. select a object in Hierarchy view or Project view.\n" +
                    "2. right click on object, choose \"Find All References\".",
                    MessageType.Info);
                EditorGUILayout.EndVertical();
                return;
            }

    		//the selected object
    		EditorGUILayout.BeginVertical("RegionBg");
    		GUILayout.Space(-15);
    		EditorGUILayout.ObjectField(m_SelectedObj, m_SelectedObj.GetType(), true);

            string msg = "";
            if (m_HierarchyResults.Count == 0 && m_ProjectResults.Count == 0)
            {
                msg = "Found 0 References";
            }
            else
            {
                if (m_HierarchyResults.Count > 0)
                {
                    int cnt = 0;
                    foreach (var list in m_HierarchyResults.Values)
                        cnt += list.Count;

                    msg += string.Format("Found {0} References In Hierarchy", cnt);
                }
                if (m_ProjectResults.Count > 0)
                {
                    int cnt = 0;
                    foreach (var list in m_ProjectResults.Values)
                        cnt += list.Count;
                    
                    msg += string.Format("\nFound {0} References In Project", cnt);
                }
            }

            EditorGUILayout.HelpBox(msg, MessageType.Info);

            EditorGUILayout.EndVertical();

            GUILayout.Space(15);

    		//show results
            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
    		OnResultsInHierarchyGUI();
            OnResultsInProjectGUI();
            EditorGUILayout.EndScrollView();

            if (m_Redraw)
            {
                m_Redraw = false;
                GUI.FocusControl("FAR_empty");
            }
    	}

    	void OnResultsInHierarchyGUI()
    	{
    		if (m_HierarchyResults.Count == 0)
    			return;

    		EditorGUILayout.BeginVertical("RegionBg");

            //header
    		GUILayout.Space(-15);
    		EditorGUILayout.LabelField(m_HierarchyLabelIcon, m_HeaderStyle, GUILayout.Width(200), GUILayout.Height(20));

            //title
            Rect rect = EditorGUILayout.GetControlRect();
            float controlWidth = rect.width;
            rect.width = 200;
            EditorGUI.LabelField(rect, "gameObject", m_TitleStyle);
            rect.x = rect.width + 20;
            float labelWidth = controlWidth - rect.x - 5;
            rect.width = labelWidth * 0.4f;
            EditorGUI.LabelField(rect, "component", m_TitleStyle);
            rect.x += rect.width; rect.width = labelWidth - rect.width;
            EditorGUI.LabelField(rect, "property", m_TitleStyle);
            GUILayout.Space(5);

            m_SceneLabelIcon.text = " " + EditorSceneManager.GetActiveScene().name;
            EditorGUILayout.LabelField(m_SceneLabelIcon, GUILayout.Height(18));
            GUILayout.Space(5);

    		foreach (var pair in m_HierarchyResults)
    		{
    			for (int i = 0; i < pair.Value.Count; i++)
    			{
    				HierarchyResult r = pair.Value[i];

    				rect = EditorGUILayout.GetControlRect();

                    //gameobject
                    if (i == 0)
                    {
                        rect.width = 200;
                        EditorGUI.ObjectField(rect, pair.Key, typeof(GameObject), true);
                    }
                    else
                    {
                        rect.x += 185; rect.width = 15;
                        EditorGUI.LabelField(rect, "\u27A5");
                    }

                    if (!string.IsNullOrEmpty(r.tip))
                    {
                        rect.x += rect.width + 20;
                        rect.width = labelWidth;
                        EditorGUI.LabelField(rect, r.tip);
                        GUILayout.Space(5);
                        continue;
                    }

                    //component
                    rect.x += rect.width + 20;
                    float contentWidth = labelWidth * 0.4f;
                    rect.width = contentWidth;
                    if (r.script != null)
                    {
                        EditorGUI.LabelField(rect, "", m_LabelStyle);
                        rect.x += labelTextOffset; rect.width -= labelTextOffset; rect.y += 1;
                        EditorGUI.ObjectField(rect, r.script, r.script.GetType(), false);
                        rect.x += contentWidth - labelTextOffset; rect.y -= 1;
                    }
                    else
                    {
                        EditorGUI.LabelField(rect, new GUIContent(AssetPreview.GetMiniThumbnail(r.component)), m_LabelStyle);
                        rect.x += 25; rect.width -= 25;
                        EditorGUI.SelectableLabel(rect, r.component.GetType().Name);
                        rect.x += rect.width;
                    }

                    //property
                    if (!(r.script != null && r.script == m_SelectedObj))
                    {
                        contentWidth = labelWidth - contentWidth;
                        rect.width = contentWidth;
                        EditorGUI.LabelField(rect, "", m_LabelStyle);
                        rect.x += labelTextOffset; rect.width = contentWidth * 0.5f;
                        EditorGUI.SelectableLabel(rect, r.propertyName);
                        rect.x += rect.width; rect.width = contentWidth - rect.width - labelTextOffset; rect.y += 1;
                        EditorGUI.ObjectField(rect, r.property, r.property.GetType(), false);
                    }

                    GUILayout.Space(5);
    			}
    		}

    		EditorGUILayout.EndVertical();
    		GUILayout.Space(15);
    	}

        void OnResultsInProjectGUI()
        {
            if (m_ProjectResults.Count == 0)
                return;

            EditorGUILayout.BeginVertical("RegionBg");

            //header
            GUILayout.Space(-15);
            EditorGUILayout.LabelField(m_ProjectLabelIcon, m_HeaderStyle, GUILayout.Width(200), GUILayout.Height(20));

            //title
            Rect rect = EditorGUILayout.GetControlRect();
            float controlWidth = rect.width;
            rect.width = 200;
            EditorGUI.LabelField(rect, "asset", m_TitleStyle);
            rect.x += rect.width + 20;
            float labelWidth = controlWidth - rect.x - 5;
            rect.width = labelWidth * 0.6f;
            EditorGUI.LabelField(rect, "child path (component)", m_TitleStyle);
            rect.x += rect.width; rect.width = labelWidth - rect.width;
            EditorGUI.LabelField(rect, "property", m_TitleStyle);
            GUILayout.Space(5);

            foreach (var pair in m_ProjectResults)
            {
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    ProjectResult r = pair.Value[i];

                    rect = EditorGUILayout.GetControlRect();

                    //object
                    if (i == 0)
                    {
                        rect.width = 200;
                        EditorGUI.ObjectField(rect, pair.Key, pair.Key.GetType(), false);
                    }
                    else
                    {
                        rect.x += 185; rect.width = 15;
                        EditorGUI.LabelField(rect, "\u27A5");
                    }

                    if (!string.IsNullOrEmpty(r.tip))
                    {
                        rect.x += rect.width + 20;
                        rect.width = labelWidth;
                        EditorGUI.LabelField(rect, r.tip);
                        GUILayout.Space(5);
                        continue;
                    }

                    //child path
                    rect.x += rect.width + 20; rect.width = labelWidth * 0.6f;
                    EditorGUI.LabelField(rect, "", m_LabelStyle);
                    rect.x += labelTextOffset; rect.width -= labelTextOffset;
                    EditorGUI.SelectableLabel(rect, r.childPath);

                    //property
                    if (r.referenceObject != null)
                    {
                        rect.x += rect.width; rect.width = labelWidth - rect.width - labelTextOffset;
                        EditorGUI.LabelField(rect, "", m_LabelStyle);
                        rect.x += labelTextOffset; rect.width -= labelTextOffset;
                        float contentWidth = rect.width;
                        rect.width = contentWidth * 0.4f;
                        EditorGUI.SelectableLabel(rect, r.propertyName);
                        rect.x += rect.width; rect.y += 1; rect.width = contentWidth - rect.width;
                        EditorGUI.ObjectField(rect, r.referenceObject, r.referenceObject.GetType(), false);
                    }

                    GUILayout.Space(5);
                }
            }

            EditorGUILayout.EndVertical();
        }
    	#endregion


    	#region Class
    	class HierarchyResult
    	{
            public string tip;
            public Component component;
            public MonoScript script;
    		public Object property;
            public string propertyName;
    	}

        class ProjectResult
        {
            public string tip;
            public string childPath;
            public string propertyName;
            public Object referenceObject;
        }
    	#endregion
    }
}