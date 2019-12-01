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

    private void Awake()
    {
        //Test1();
        BoxTest(EA.TestA);
        BoxTest(EB.TestB);
    }

    private void Start()
    {

    }

    int n;
    private void Update()
    {
        using (new ProfilerMarker("Test 1").Auto())
        {
            int m = 100;
        }

        using (new ProfilerMarker("Test 2").Auto())
        {
            n = 100;
        }
    }

    public void OnDestroy()
    {

    }


    private void Test1()
    {
        //但是值类型就不一样了。如下代码所示，因为结构体是数值类型，所以int t2 = t1;的时候需要将结构体里的每一个数据都进行一次完全拷贝，
        //如果结构体比较大那么拷贝势必就会慢，但是优点就是没有GC。
        int t1 = 10;
        int t2 = t1;
        t1 = 100;

        Debug.Log(t1);//100
        Debug.Log(t2);//10


        //可以看出class的赋值是非常廉价的，代码中的 MyClass temp2 = temp1; 就是添加了一个新指针指向了一块相同的地址而已。
        MyClass temp1 = new MyClass();
        MyClass temp2 = temp1;
        temp1.x = 10;
        temp2.x = 100;

        Debug.Log(temp1.x);//100
        Debug.Log(temp2.x);//100

        //参数传递，值类型参数传递其实就是在栈上拷贝了一份新的，此时栈上有两个int 值都是100。方法体内改了参数并不会影响外面的，结果肯定还是100。
        int a = 100;
        Add(a);
        Debug.Log(a);//100

        //再来看看类对象，这时候你可能会有疑问，传入的类对象是否也进行了拷贝，答案是肯定的。此时堆上只有1个MyClass对象，但是有两个指针指向它。
        //MyClassAdd 方法体内接收的就已经是拷贝出来的指针，但是由于两个指针指向了同一块内存，所以改了方法体里的外面的也就跟着变了，结果就是101了。
        MyClass myClass1 = new MyClass
        {
            x = 100
        };
        MyClassAdd(myClass1);
        Debug.Log(myClass1.x);//101

        //我们再来改造一下，MyClassAdd2 方法体内的a指向另一块地址，然后在操作它。
        //很显然无法影响到外面传入的 MyClass 对象，里面虽然改了，外面肯定还是100了
        MyClass myClass2 = new MyClass
        {
            x = 100
        };

        MyClassAdd2(myClass2);
        Debug.Log(myClass2.x);//100


        //****总的来说数值类型传递慢，但是没GC ，class类型传递快，但有GC****

        //还有一个ref out的用法，就是把真正的引用传进去，而不是拷贝，如下代码所示，因为是引用所以值类型数据传进去也会跟着改了。
        int a1 = 100;
        Add(ref a1);
        Debug.Log(a1);//101
    }

    private void Add(ref int value)
    {
        value++;
    }

    private void MyClassAdd2(MyClass myClass)
    {
        myClass = new MyClass();
        myClass.x++;
    }

    void MyClassAdd(MyClass myClass)
    {
        myClass.x++;
    }
    void Add(int value)
    {
        value++;
    }

    class MyClass
    {
        public int x;
    }

    //再来说一下装箱和拆箱，
    //装箱就是将int值类型数据转成object类对象，拆箱就是将object类对象转回值类型数据，
    //写法上是可以进行隐式转换的。如下代码所示，我们来看个经典的例子。
    private void BoxTest(object obj)//传进来就被隐形装箱了
    {
        if (obj is EA)
        {
            Debug.Log((EA)obj);//这里显式拆箱了
        }

        if (obj is EB)
        {
            Debug.Log((EB)obj);//这里显式拆箱了
        }
    }

    enum EA
    {
        TestA = 0
    }

    enum EB
    {
        TestB = 0
    }

    private void BoxTest2()
    {
        int a = 100;
        string.Format("{0}", a);//被隐式装箱了
        //正确写法
        string.Format("{0}", a.ToString());
    }

    //可以自己封装一层 避免使用不当 造成不必要到装箱拆箱
    private string MyFormat(string foramt, params string[] strs)
    {
        return string.Format(foramt, strs);
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
        //Profiler.BeginSample("10 x 10");
        //new Texture2D(10, 10);
        //Profiler.EndSample();

        //Profiler.BeginSample("1024 x 1024");
        //new Texture2D(1024, 1024);
        //Profiler.EndSample();

        ////推荐写法
        //using (new ProfilerMarker("Test1").Auto())
        //{
        //    float x = 10l;
        //}


        //using (new ProfilerMarker("10 x 10").Auto())
        //{
        //    var texture = new Texture2D(10, 10);
        //    Resources.UnloadAsset(texture);
        //}

        //using (new ProfilerMarker("1024 x 1024").Auto())
        //{
        //    //new Texture2D(1024, 1024);
        //}

        using (new ProfilerMarker("Test1").Auto())
        {
            A a = new A();
        }
    }

    class A
    {
        ~A()
        {
            Debug.Log("clear");
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