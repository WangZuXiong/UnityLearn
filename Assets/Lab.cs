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

//[ExecuteInEditMode]
public class Lab : MonoBehaviour, IPointerEnterHandler
{
    private WebSocket webSocket;
    private Student _student;
    public AnimationCurve curve;
    private FileStream fileStream;
    [SerializeField] private List<int> Vs;

    [SerializeField]
    private Image _image;
    [SerializeField]
    private RawImage _rawImage;

    private void Awake()
    {
        var pool = new ObjectPoolSimple<GameObject>(3);

        var temp = pool.New();


        var x = UnityEngine.Random.Range(0, 1f);

        temp.gameObject.GetComponent<Image>().color = Color.white * x;
        pool.Clear();
    }

    private void Start()
    {
        //A n = new A();
        //A m = new A();

        Debug.Log("" + Cow.count);
        Cow cow1 = new Cow();
        Cow cow2 = new Cow();
        Debug.Log("" + Cow.count);
    }


    public class Cow
    {
        public static int count;


        static Cow()
        {
            count++;
        }
    }



    class A
    {
        public A()
        {
            Debug.Log("构造A");
        }

        ~A()
        {
            Debug.Log("clear");
        }
    }

    private void Update()
    {

    }

    public void OnDestroy()
    {

    }

    private void MemoryTest()
    {
        Texture2D texture = Resources.Load<Texture2D>("ATM");
        _rawImage.texture = texture;
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

    private void ProfilerDemo()
    {
        Profiler.BeginSample("10 x 10");
        new Texture2D(10, 10);
        Profiler.EndSample();

        Profiler.BeginSample("1024 x 1024");
        new Texture2D(1024, 1024);
        Profiler.EndSample();

        //推荐写法
        using (new ProfilerMarker("Test1").Auto())
        {
            float x = 10l;
        }
    }



    /// <summary>
    /// 把所有的语言包按照模块名称写到多个
    /// </summary>
    /// <param name="json"></param>
    private void WriteAllLanguageInTexts(string json)
    {
        Dictionary<string, string> languageDict = new Dictionary<string, string>();

        var jsonArray = JSONNode.Parse(json);

        foreach (KeyValuePair<string, JSONNode> temp in (JSONClass)jsonArray)
        {
            if (languageDict.ContainsKey(temp.Key))
            {
                Debug.LogWarning("Duplicate string: " + temp.Key);
            }
            else
            {
                languageDict.Add(temp.Key, temp.Value);
            }
        }

        Debug.LogError("languageDict.Count:" + languageDict.Count);


        var enumerator = languageDict.GetEnumerator();
        var keyValuePairs = new Dictionary<string, Dictionary<string, string>>();

        var content = string.Empty;
        while (enumerator.MoveNext())
        {
            string key = enumerator.Current.Key;
            string value = enumerator.Current.Value;
            //业务模块名称
            string moduleName = key.Substring(0, key.IndexOf("/") + 1);
            moduleName = moduleName.Replace("/", "");
            if (!keyValuePairs.ContainsKey(moduleName))
            {
                var temp = new Dictionary<string, string>();
                keyValuePairs.Add(moduleName, temp);
            }
            keyValuePairs[moduleName].Add(key, value);
        }


        //写入本地
        string localPath = @"C:\Users\admin\Desktop\Language";
        int count = 0;
        foreach (var item in keyValuePairs)
        {
            count += item.Value.Count;
            Language language = new Language();
            language.languageItems = new List<LanguageItem>();
            foreach (var item1 in item.Value)
            {
                LanguageItem languageItem = new LanguageItem(item1.Key, item1.Value);
                language.languageItems.Add(languageItem);
            }

            content = JsonUtility.ToJson(language, true);

            string fileName = item.Key + ".json";
            FileUtility.WriteTextToLaocal(localPath, fileName, content);
        }
        Debug.LogError("按照业务模块划分之后的：" + count);

    }


    /// <summary>
    /// 把所有的语言包写到一个文本中
    /// </summary>
    /// <param name="json"></param>
    private void WriteAllLanguageInOneText(string json)
    {
        Dictionary<string, string> languageDict = new Dictionary<string, string>();

        var jsonArray = JSONNode.Parse(json);

        foreach (KeyValuePair<string, JSONNode> temp in (JSONClass)jsonArray)
        {
            if (languageDict.ContainsKey(temp.Key))
            {
                Debug.LogWarning("Duplicate string: " + temp.Key);
            }
            else
            {
                languageDict.Add(temp.Key, temp.Value);
            }
        }

        Debug.LogError("languageDict.Count:" + languageDict.Count);


        var enumerator = languageDict.GetEnumerator();
        var keyValuePairs = new Dictionary<string, Dictionary<string, string>>();

        var content = string.Empty;
        while (enumerator.MoveNext())
        {
            string key = enumerator.Current.Key;
            string value = enumerator.Current.Value;
            //业务模块名称
            string moduleName = key.Substring(0, key.IndexOf("/") + 1);
            if (!keyValuePairs.ContainsKey(moduleName))
            {
                var temp = new Dictionary<string, string>();
                keyValuePairs.Add(moduleName, temp);
            }
            keyValuePairs[moduleName].Add(key, value);
        }
        Root root = new Root();
        root.languages = new List<Language>();
        int count = 0;
        foreach (var item in keyValuePairs)
        {
            count += item.Value.Count;
            Language language = new Language();
            language.languageItems = new List<LanguageItem>();
            foreach (var item1 in item.Value)
            {
                LanguageItem languageItem = new LanguageItem(item1.Key, item1.Value);
                language.languageItems.Add(languageItem);
            }
            root.languages.Add(language);
            //continue;
        }
        Debug.LogError("按照业务模块划分之后的：" + count);

        content = JsonUtility.ToJson(root, transform);

        //写入本地
        string localPath = @"C:\Users\admin\Desktop\Language";
        string fileName = "NewLanguage.json";

        FileUtility.WriteTextToLaocal(localPath, fileName, content);
    }

    [Serializable]
    public class Root
    {
        public List<Language> languages;
    }

    [Serializable]
    public class Language
    {
        public List<LanguageItem> languageItems;
    }

    [Serializable]
    public class LanguageItem
    {
        public string Key;
        public string Value;


        public LanguageItem(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

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
        throw new System.NotImplementedException();
    }

    public async Task SetAsync()
    {
        Debug.Log(00000000000000);


        await Task.Delay(4 * 1000);

        Debug.Log(1111);
    }

    /// <summary>
    /// 屏幕截图
    /// </summary>
    private void RenderTextureLab()
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(100, 100, 0);
        renderTexture.filterMode = FilterMode.Bilinear;
        RenderTexture.active = renderTexture;
        Camera camera = Camera.main;
        camera.targetTexture = renderTexture;
        camera.Render();
        RawImage rawImage = transform.Find("RawImage").GetComponent<RawImage>();
        rawImage.texture = renderTexture;
        RenderTexture.active = null;
        camera.targetTexture = null;
        //RenderTexture.GetTemporary这个api要和RenderTexture.ReleaseTemporary 配套使用否则会内存泄漏
        //RenderTexture.ReleaseTemporary(renderTexture);
    }

    /// <summary>
    /// 朗母达表达式排序
    /// </summary>
    private void LongmudaSort()
    {
        var list = new List<Student>
            {
                new Student (10,160),
                new Student (20,170),
                new Student (20,150)
            };

        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].ToString());
        }

        Debug.LogError("==============");

        //按照年龄排序
        //如果年龄一致的话则按照身高排序

        //list.Sort((item1, item2) =>
        //{
        //    if (item1.Age == item2.Age)
        //    {
        //        return item1.Height.CompareTo(item2.Height);
        //    }
        //    return item1.Age.CompareTo(item2.Age);
        //});

        //权重大的排前面
        list.Sort((item1, item2) => item1.Age.CompareTo(item2.Age) * 2 + item1.Height.CompareTo(item2.Height));


        //list.AddRange(new List<Student>());
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].ToString());
        }
    }

    /// <summary>
    /// 向右操作运算符
    /// 向左操作运算符
    /// </summary>
    private void OperatorTest()
    {
        int m = 8;
        Debug.LogError(m >> 1);//十进制转化为二进制 8 = 1000 ，1向右边移动1位，0100，即4
        Debug.LogError(m >> 2);//十进制转化为二进制 8 = 1000 ，1向右边移动2位，0010，即2
        Debug.LogError(m >> 3);//十进制转化为二进制 8 = 1000 ，1向右边移动3位，0001，即1

        int n = 2;
        Debug.LogError(n << 1);//十进制转化为二进制 2 = 10 ，1向左边移动1位，100，即4
        Debug.LogError(n << 2);//十进制转化为二进制 2 = 10 ，1向左边移动2位，1000，即8
        Debug.LogError(n << 3);//十进制转化为二进制 2 = 10 ，1向左边移动3位，10000，即16
    }

    private void Texture2DTest()
    {
        Texture2D texture2D = Resources.Load<Texture2D>("");
        texture2D.GetPixel(1, 1);   //返回坐标处的像素颜色
                                    //可用于不规则的点击区域判断
    }
}
public class Student
{
    public int Age;
    public int Height;

    public Student()
    {

    }

    public Student(int age, int height)
    {
        Age = age;
        Height = height;
    }

    public override string ToString()
    {
        return ("Age:" + Age + " Height:" + Height);
    }
}