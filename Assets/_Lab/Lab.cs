using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

//[ExecuteInEditMode]
public partial class Lab : MonoBehaviour, IPointerEnterHandler
{

    public GameObject tips;

    public float _space = 0.5f;

    public Plane plane;


    public Color color;

    [ContextMenu("Start")]
    private void Start()
    {
        //Plane plane = new Plane(Vector3.up, Vector3.zero);

        //Destroy(gameObject,)

        //ProfilerDemo();

        var t = Debug1();

        func = () =>
       {
           Debug.LogError(t.Current);
           Debug.LogError(t.MoveNext());
       };






        coroutine1 = StartCoroutine(t);



    }

    Action func;
    private Coroutine coroutine1;


    IEnumerator Debug1()
    {
        var i = 1;
        while (i <= 3)
        {
            i++;
            yield return new WaitForSecondsRealtime(1);
            Debug.LogError(1);

            func?.Invoke();
        }
    }

    [ContextMenu("Test")]
    private void Test()
    {
        //自身SetActive(false)
        //          gameObject.activeSelf = false gameObject.activeInHierarchy = false
        //自身SetActive(true) 父节点SetActive(false)
        //          gameObject.activeSelf = false gameObject.activeInHierarchy = false

        //The local active state of this GameObject. (Read Only)
        //这个游戏对象的本地活动状态。(只读)
        Debug.LogError(gameObject.activeSelf);

        //Defines whether the GameObject is active in the Scene.
        //定义游戏对象在场景中是否处于活动状态。
        Debug.LogError(gameObject.activeInHierarchy);
    }

    IEnumerator Print()
    {
        while (true)
        {
            Debug.LogError(1);
            yield return new WaitForSeconds(1);
        }
    }

    float temp;

    private void Update()
    {

        var data = "12132";
        if (Input.GetMouseButtonDown(1))
        {
            WebRequestManager.Put("http://192.168.1.114:8087/pushC/checkToken", data, (t) => { }, null);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            WebRequestManager.Put("https://acc.nbabm.com/pushC/checkToken", data, (t) => { }, null);
        }
    }





    void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
    public void OnDestroy()
    {

    }

    ComponentPool<MonoBehaviour> gameObjectPool;

    public AudioClip audioClip;


    void ThreadTest()
    {

        SynchronizationContext synchronizationContext = SynchronizationContext.Current;

        Thread thread = new Thread(() =>
        {
            Thread.Sleep(2000);

            synchronizationContext.Post(t =>
            {
                //可以调用Unity Engine 相关API
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            }, null);

        });

        thread.Start();


        Loom.Instance.RunAsync(() =>
        {
            Debug.Log("CurrentThread:" + Thread.CurrentThread.Name);//Thread Pool Worker
            Debug.Log(1111);

            //GameObject gameObject = new GameObject();

            //线程完成后再主线程中的操作
            Loom.Instance.QueueOnMainThread((t) =>
            {
                Debug.Log("CurrentThread:" + Thread.CurrentThread.Name);//主线程
                Debug.Log(2222);

            }, null);

        });

        Debug.Log(3333);
    }





    private void Print(float p)
    {
        Debug.Log(p);
        //Debug.Log(Time.realtimeSinceStartup.ToString());
    }

    void LoadAssetbundle()
    {
        //AssetConfig assetConfig = new AssetConfig();
        //assetConfig.BaseUrl = "http://192.168.1.243:8082/basketball";
        //assetConfig.RelativeUrl = "theme_activity/configure/Android";
        //assetConfig.FileName = "3";
        //assetConfig.Version = 3;

        //Assets\StreamingAssets
        //\StreamingAssets

        //AssetBundleConfig manifestBundle = new AssetBundleConfig
        //{
        //    BaseUrl = @"E:\wangzuxiong\Unity Project\UnityLearn\Assets",
        //    RelativeUrl = "StreamingAssets",
        //    FileName = "image.manifest",
        //    Version = 1
        //};

        //DownloadAssetManager.DownloadAssetBundleAsync(manifestBundle, (t) =>
        //{
        //    AssetBundleManifest manifest = t.LoadAsset<AssetBundleManifest>("image");

        //    Debug.Log(manifest == null);
        //    Debug.Log(string.Join("\n", manifest.GetAllDependencies("image")));
        //}, null);

        AssetBundleConfig spriteAtlas = new AssetBundleConfig
        {
            BaseUrl = Application.dataPath,
            RelativeUrl = "StreamingAssets",
            FileName = "spriteatlas",
            Version = 1
        };

        DownloadAssetManager.DownloadAssetBundleAsync(spriteAtlas, (sa) =>
        {
            AssetBundleConfig image = new AssetBundleConfig
            {
                BaseUrl = Application.dataPath,
                RelativeUrl = "StreamingAssets",
                FileName = "image",
                Version = 1
            };

            DownloadAssetManager.DownloadAssetBundleAsync(image, (t) =>
            {
                GameObject.Instantiate(t.LoadAllAssets<GameObject>()[0], transform);
            }, null);
        }, null);



        //var path = "http://192.168.1.243:8082/basketball/theme_activity/configure/ThemeActivityConfig_1.json";
        //DownloadAssetManager.DownloadAssetBundleAsync(path, (t) =>
        //{
        //    Debug.Log(t);
        //}, null);
    }




    public Vector3 target;
    public float speed;

    public int index;



    [SerializeField]
    private float _force = 500;


    /// <summary>
    /// 判断两个矩形是否相交
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private bool IsIntersect(Rect a, Rect b)
    {
        /*  
        public RectTransform A;
        public RectTransform B;
        Rect a = new Rect(A.anchoredPosition.x, A.anchoredPosition.y, A.rect.width, A.rect.height);
        Rect b = new Rect(B.anchoredPosition.x, B.anchoredPosition.y, B.rect.width, B.rect.height);
        Debug.Log(a.Overlaps(b, true) + "===" + a.Overlaps(b, false) + "=====" + IsIntersect(a, b));
         */
        return b.xMax > a.xMin
            && b.xMin < a.xMax
            && b.yMax > a.yMin
            && b.yMin < a.yMax;
    }





    /// <summary>
    /// 会产生GC的API
    /// </summary>
    /// <param name="arg0"></param>
    void GCAPITest(params object[] arg0)
    {
        using (new ProfilerMarker("Marker_0").Auto())
        {
            for (int i = 0; i < 10000; i++)
            {
                int x = Convert.ToInt32(arg0[0]);//GC Alloc 0B
            }
        };

        using (new ProfilerMarker("Marker_1").Auto())
        {
            int y = Convert.ToInt32(arg0[1]);//GC Alloc 0B
        };

        using (new ProfilerMarker("Marker_2").Auto())
        {
            bool isShow = Convert.ToBoolean(arg0[2]);//GC Alloc 0B
        };

        using (new ProfilerMarker("Marker_3").Auto())
        {
            string.Format("{0}", 1);//GC Alloc 76B
        };

        using (new ProfilerMarker("Marker_4").Auto())
        {
            string.Format("{0}", 1.ToString());//GC Alloc 56B
        };

        using (new ProfilerMarker("Marker_5").Auto())
        {
            int x = (int)arg0[0];//GC Alloc 0B
        };

        using (new ProfilerMarker("Marker_6").Auto())
        {
            for (int i = 0; i < 100; i++)
            {
                Vector2 vector2 = new Vector2(0, 0);//GC Alloc 0B  Time 0ms
            }
        };


        using (new ProfilerMarker("Marker_7").Auto())
        {
            Vector2 vector2;
            for (int i = 0; i < 100; i++)
            {
                vector2 = new Vector2(0, 0);//GC Alloc 0B  Time 0ms
            }
        };



        using (new ProfilerMarker("Marker_8").Auto())
        {
            for (int i = 0; i < 100; i++)
            {
                StudentClass student = new StudentClass();//GC Alloc 3.1kB  Time 0.06ms
            }
        };


        using (new ProfilerMarker("Marker_9").Auto())
        {
            StudentClass student;
            for (int i = 0; i < 100; i++)
            {
                student = new StudentClass();//GC Alloc 3.1kB  Time 0.01ms
            }
        };



        using (new ProfilerMarker("Marker_10").Auto())
        {
            for (int i = 0; i < 100; i++)
            {
                StudentStruct student = new StudentStruct();//GC Alloc 0B  Time 0ms
            }
        };


        using (new ProfilerMarker("Marker_11").Auto())
        {
            StudentStruct student;
            for (int i = 0; i < 100; i++)
            {
                student = new StudentStruct();//GC Alloc 0B  Time 0ms
            }
        };


        using (new ProfilerMarker("Marker_12").Auto())
        {
            gameObject.SetActive(false);//GC Alloc 0.6kB  Time 0.35ms
        };

        using (new ProfilerMarker("Marker_13").Auto())
        {
            //transform.GetChild(0);//GC Alloc 0B  Time 0.9ms
        };


        using (new ProfilerMarker("Marker_14").Auto())
        {
            var str = transform.name;//GC Alloc 38B
        };

        using (new ProfilerMarker("Marker_15").Auto())
        {
            transform.name = "1";//GC Alloc 0B
        };

        using (new ProfilerMarker("Marker_16").Auto())
        {
            Resources.Load<Texture>("ATM");// 50b
        };

        using (new ProfilerMarker("Marker_17").Auto())
        {
            //GameObject.Instantiate(Temp);//3.6kb  2ms
        };


        using (new ProfilerMarker("Marker_GetComponent").Auto())
        {
            for (int i = 0; i < 100; i++)
            {
                var lab = GetComponent<Lab>();//
            }
        };

        using (new ProfilerMarker("Marker_Destroy").Auto())
        {
            //Destroy(Temp);//0.6kb 0.42ms
        };
    }

    private void GCTest()
    {
        FileStream fileStream;
        fileStream = new FileStream(Application.streamingAssetsPath + "/1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        fileStream = null;
        fileStream = new FileStream(Application.streamingAssetsPath + "/1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        fileStream = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
        fileStream = new FileStream(Application.streamingAssetsPath + "/1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }





    private void ReadSprites()
    {
        var sprites = Resources.LoadAll<Sprite>("ATM");
        var imgs = transform.GetComponentsInChildren<Image>();
        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i].sprite = sprites[i];
        }
    }

    private void ReadSpriteAltas()
    {
        var sprites = Resources.Load<SpriteAtlas>("ATM1");
        var count = sprites.spriteCount;
        var spriteArray = new Sprite[count];
        sprites.GetSprites(spriteArray);
        var list = spriteArray.ToList();
        //list.Sort((Sprite x, Sprite y) =>
        //{
        //    if (int.Parse(x.name) > int.Parse(y.name))
        //    {
        //        return x;
        //    }
        //    else
        //    {
        //        return y;
        //    }
        //});

        list.Sort(delegate (Sprite p1, Sprite p2)
        {
            Debug.Log((p1.name.Substring(p1.name.IndexOf("_") + 1, p1.name.Length - p1.name.IndexOf("_") - 1)).Replace("(Clone)", ""));
            var index1 = (p1.name.Substring(p1.name.IndexOf("_") + 1, p1.name.Length - p1.name.IndexOf("_") - 1)).Replace("(Clone)", "");
            var index2 = (p2.name.Substring(p2.name.IndexOf("_") + 2, p2.name.Length - p2.name.IndexOf("_") - 2)).Replace("(Clone)", "");
            return index1.CompareTo(index2);//升序
        });

        var imgs = transform.GetComponentsInChildren<Image>();
        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i].sprite = list[i];
        }
    }

    ///// <summary>
    ///// 把所有的语言包按照模块名称写到多个
    ///// </summary>
    ///// <param name="json"></param>
    //private void WriteAllLanguageInTexts(string json)
    //{
    //    Dictionary<string, string> languageDict = new Dictionary<string, string>();

    //    var jsonArray = JSONNode.Parse(json);

    //    foreach (KeyValuePair<string, JSONNode> temp in (JSONClass)jsonArray)
    //    {
    //        if (languageDict.ContainsKey(temp.Key))
    //        {
    //            Debug.LogWarning("Duplicate string: " + temp.Key);
    //        }
    //        else
    //        {
    //            languageDict.Add(temp.Key, temp.Value);
    //        }
    //    }

    //    Debug.LogError("languageDict.Count:" + languageDict.Count);


    //    var enumerator = languageDict.GetEnumerator();
    //    var keyValuePairs = new Dictionary<string, Dictionary<string, string>>();

    //    var content = string.Empty;
    //    while (enumerator.MoveNext())
    //    {
    //        string key = enumerator.Current.Key;
    //        string value = enumerator.Current.Value;
    //        //业务模块名称
    //        string moduleName = key.Substring(0, key.IndexOf("/") + 1);
    //        moduleName = moduleName.Replace("/", "");
    //        if (!keyValuePairs.ContainsKey(moduleName))
    //        {
    //            var temp = new Dictionary<string, string>();
    //            keyValuePairs.Add(moduleName, temp);
    //        }
    //        keyValuePairs[moduleName].Add(key, value);
    //    }


    //    //写入本地
    //    string localPath = @"C:\Users\admin\Desktop\Language";
    //    int count = 0;
    //    foreach (var item in keyValuePairs)
    //    {
    //        count += item.Value.Count;
    //        Language language = new Language();
    //        language.languageItems = new List<LanguageItem>();
    //        foreach (var item1 in item.Value)
    //        {
    //            LanguageItem languageItem = new LanguageItem(item1.Key, item1.Value);
    //            language.languageItems.Add(languageItem);
    //        }

    //        content = JsonUtility.ToJson(language, true);

    //        string fileName = item.Key + ".json";
    //        FileUtility.WriteTextToLaocal(localPath, fileName, content);
    //    }
    //    Debug.LogError("按照业务模块划分之后的：" + count);

    //}


    ///// <summary>
    ///// 把所有的语言包写到一个文本中
    ///// </summary>
    ///// <param name="json"></param>
    //private void WriteAllLanguageInOneText(string json)
    //{
    //    Dictionary<string, string> languageDict = new Dictionary<string, string>();

    //    var jsonArray = JSONNode.Parse(json);

    //    foreach (KeyValuePair<string, JSONNode> temp in (JSONClass)jsonArray)
    //    {
    //        if (languageDict.ContainsKey(temp.Key))
    //        {
    //            Debug.LogWarning("Duplicate string: " + temp.Key);
    //        }
    //        else
    //        {
    //            languageDict.Add(temp.Key, temp.Value);
    //        }
    //    }

    //    Debug.LogError("languageDict.Count:" + languageDict.Count);


    //    var enumerator = languageDict.GetEnumerator();
    //    var keyValuePairs = new Dictionary<string, Dictionary<string, string>>();

    //    var content = string.Empty;
    //    while (enumerator.MoveNext())
    //    {
    //        string key = enumerator.Current.Key;
    //        string value = enumerator.Current.Value;
    //        //业务模块名称
    //        string moduleName = key.Substring(0, key.IndexOf("/") + 1);
    //        if (!keyValuePairs.ContainsKey(moduleName))
    //        {
    //            var temp = new Dictionary<string, string>();
    //            keyValuePairs.Add(moduleName, temp);
    //        }
    //        keyValuePairs[moduleName].Add(key, value);
    //    }
    //    Root root = new Root();
    //    root.languages = new List<Language>();
    //    int count = 0;
    //    foreach (var item in keyValuePairs)
    //    {
    //        count += item.Value.Count;
    //        Language language = new Language();
    //        language.languageItems = new List<LanguageItem>();
    //        foreach (var item1 in item.Value)
    //        {
    //            LanguageItem languageItem = new LanguageItem(item1.Key, item1.Value);
    //            language.languageItems.Add(languageItem);
    //        }
    //        root.languages.Add(language);
    //        //continue;
    //    }
    //    Debug.LogError("按照业务模块划分之后的：" + count);

    //    content = JsonUtility.ToJson(root, transform);

    //    //写入本地
    //    string localPath = @"C:\Users\admin\Desktop\Language";
    //    string fileName = "NewLanguage.json";

    //    FileUtility.WriteTextToLaocal(localPath, fileName, content);
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }


    private void Texture2DTest()
    {
        Texture2D texture2D = Resources.Load<Texture2D>("");
        texture2D.GetPixel(1, 1);   //返回坐标处的像素颜色  //可用于不规则的点击区域判断
    }

    private bool CheckInputPositionIsInRect(RectTransform rectTransform)
    {
        var x = rectTransform.rect.x + Screen.width * 0.5f;
        var y = rectTransform.rect.y + Screen.height * 0.5f;

        var width = rectTransform.rect.width;
        var height = rectTransform.rect.height;

        var temp = new Rect(x, y, width, height);
        return temp.Contains(Input.mousePosition);
    }


    public Transform A;
    public Transform B;


    public void GetCustomType()
    {
        Type type = Type.GetType("");
        transform.GetComponent(type);
        gameObject.AddComponent(type);

        transform.GetComponent(typeof(Button));
    }



    /// <summary>
    /// 枚举器
    /// </summary>
    /// <returns></returns>
    private IEnumerator Enumerator()
    {
        yield return null;
    }

    /// <summary>
    /// IEnumerable 可枚举的 实现这个接口就可以使用foreach遍历
    /// </summary>
    class TestA : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}


public struct StudentStruct
{
    public int Age;
    public int Height;
    public string Name;
}

public class GetFieldByAttribute
{
    void FOO()
    {
        Type type = typeof(StudentClass);
        object[] vs = type.GetCustomAttributes(typeof(AttributeTest), false);
        foreach (var item in vs)
        {
            if (item is Attribute)
            {
                AttributeTest attribute = item as AttributeTest;
                int value = attribute.Value;
            }
        }

    }


}


public class AttributeTest : Attribute
{

    public int Value;
    public AttributeTest(int t)
    {
        Value = t;
    }
}

public class StudentClass
{
    [AttributeTest(1)]
    public int Age;
    [AttributeTest(1)]
    public int Height;
    public string Name;

    public void Say(string str)
    {
        Debug.Log("==>" + str);
    }
}




 
public class QuotaCoroutine : MonoBehaviour
{
    // 每帧的额度时间，全局共享
    static float frameQuotaSec = 0.001f;

    static LinkedList<IEnumerator> s_tasks = new LinkedList<IEnumerator>();

    // Use this for initialization
    void Start()
    {
        StartQuotaCoroutine(Task(1, 100));
    }

    // Update is called once per frame
    void Update()
    {
        ScheduleTask();
    }

    void StartQuotaCoroutine(IEnumerator task)
    {
        s_tasks.AddLast(task);
    }

    static void ScheduleTask()
    {
        float timeStart = Time.realtimeSinceStartup;
        while (s_tasks.Count > 0)
        {
            IEnumerator t = s_tasks.First.Value;
            bool taskFinish = false;
            while (Time.realtimeSinceStartup - timeStart < frameQuotaSec)
            {
                // 执行任务的一步, 后续没步骤就是任务完成
                Profiler.BeginSample(string.Format("QuotaTaskStep, f:{0}", Time.frameCount));
                taskFinish = !t.MoveNext();
                Profiler.EndSample();

                if (taskFinish)
                {
                    s_tasks.RemoveFirst();
                    break;
                }
            }

            // 任务没结束执行到这里就是没时间额度了
            if (!taskFinish)
                return;
        }
    }

    IEnumerator Task(int taskId, int stepCount)
    {
        int i = 0;
        while (i < stepCount)
        {
            Debug.LogFormat("{0}.{1}, frame:{2}", taskId, i, Time.frameCount);
            i++;
            yield return null;
        }
    }
}