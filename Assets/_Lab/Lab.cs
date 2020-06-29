using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Profiling;
using Unity.Profiling;
using UnityEditor;
using System.Text;
using UnityEngine.Networking;
using System.Collections;
using System.Threading;
using System.ComponentModel;

//[ExecuteInEditMode]
public partial class Lab : MonoBehaviour, IPointerEnterHandler
{
    public LayerMask lm;
    private WebSocket webSocket;
    private StudentClass _student;
    public AnimationCurve curve;
    private FileStream fileStream;
    [SerializeField] private List<int> Vs;

    [SerializeField]
    private Image _image;
    [SerializeField]
    private RawImage _rawImage;
    [SerializeField]
    public int X = 111;
    [SerializeField]
    public Lab RefLab;
    public GameObject Cube;
    public Texture2D texture2D;
    public Transform Canvas;
    public Vector3 _vector3;
    public Matrix4x4 _matrix4X4;

    public int _tempValue;
    public int TempValue
    {
        get
        {
            return _tempValue;
        }

        set
        {
            _tempValue = value;
        }
    }

    private void Start()
    {
        //GCAPITest(0, 1, true);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {

        }
        else if (Input.GetMouseButtonDown(0))
        {

        }
    }

    public void OnDestroy()
    {

    }

    void ValueRefTest()
    {

        string str1 = "1";//str1 ---> new string("1")
        string str2 = str1;//str2 --> str1传递引用 
        str2 = "2";//str2 --> new string("2") 传引用，str2指向一个新的字符串，str1没有改变
        Debug.Log(str1);//1
        //但是string又有值传递的效果，这bai是因为string是常量，不能更改

        StudentClass studentClass1 = new StudentClass();
        studentClass1.Age = 1;
        StudentClass studentClass2 = studentClass1;
        studentClass2.Age = 2;
        Debug.Log(studentClass1.Age);//2


        StudentStruct studentStruct1 = new StudentStruct();
        studentStruct1.Age = 1;

        StudentStruct studentStruct2 = studentStruct1;
        studentStruct2.Age = 2;
        Debug.Log(studentStruct1.Age);//3
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
            transform.GetChild(0).GetChild(1);//GC Alloc 0B  Time 0.9ms
        };


        using (new ProfilerMarker("Marker_14").Auto())
        {
            var str = transform.name;//GC Alloc 38B
        };

        using (new ProfilerMarker("Marker_15").Auto())
        {
            transform.name = "1";//GC Alloc 0B
        };
    }

    private void GCTest()
    {
        fileStream = new FileStream(Application.streamingAssetsPath + "/1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        fileStream = null;
        fileStream = new FileStream(Application.streamingAssetsPath + "/1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        fileStream = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
        fileStream = new FileStream(Application.streamingAssetsPath + "/1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }
    void ListCountTest()
    {
        List<int> vs = new List<int>(10);
        vs.Add(1);
        vs.Clear();
        Debug.Log(vs.Count);//0
    }

    void EncryptionTest()
    {
        var str = "123";
        var bytes = Encoding.UTF8.GetBytes(str);
        Debug.LogError(string.Join(",", bytes));
        var strFromBytes = Encoding.UTF8.GetString(bytes);
        Debug.LogError(strFromBytes);



        Debug.LogError("========加密========");
        //加密
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] ^= 1;
        }
        Debug.LogError(string.Join(",", bytes));
        var strFromBytes1 = Encoding.UTF8.GetString(bytes);
        Debug.LogError(strFromBytes1);



        Debug.LogError("========解密========");
        //解密
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] ^= 1;
        }
        Debug.LogError(string.Join(",", bytes));
        var strFromBytes2 = Encoding.UTF8.GetString(bytes);
        Debug.LogError(strFromBytes2);
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

    private void Func1(string name)
    {
        Debug.LogErrorFormat(name);
    }

    private void Func2(string name)
    {
        Debug.LogErrorFormat("Func2");
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


    [ContextMenu("Play")]

    public async static void GetInfoAsync()
    {
        await GetData();
        await GetData<int>(1);
        Debug.Log(222);
    }

    static Task GetData()
    {
        Debug.Log(000);
        return null;
    }

    static Task<T> GetData<T>(int a)
    {
        Debug.Log(111);
        return null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public async Task SetAsync()
    {
        Debug.Log(00000000000000);


        await Task.Delay(4 * 1000);

        Debug.Log(1111);
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
}
