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

class Animal
{
    public void call() { Debug.LogError("无声的叫唤"); }
}

class Dog : Animal
{
    // new的作用是隐藏父类的同名方法
    public new void call() { Debug.LogError("叫声：汪～汪～汪～"); }
    public void smell() { Debug.LogError("嗅觉相当不错！"); }
}


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
    [SerializeField]
    public int X = 111;
    [SerializeField]
    public Lab RefLab;

    private void Awake()
    {
        List<int> vs = new List<int>();
        vs.Add(1);


        Debug.LogError(vs[0]);


        //var newVs = vs;
        //newVs[0] = 2;
        //Debug.LogError(vs[0]);
        TestList(vs);


        Debug.LogError(vs[0]);
    }


    public GameObject Cube;
    public Texture2D texture2D;


    private void TestList(List<int> vs)
    {
        var newVs = vs;
        newVs[0] = 2;
        Debug.LogError(vs[0]);

    }


    private void Start()
    {
        RenderTextureLab();
    }


    private void CopyList()
    {
        //======================值类型List======================
        //var oldList = new List<int>();
        //oldList.Add(5);

        //浅拷贝
        //var newList = oldList;
        //newList[0] *= 5;
        //Debug.Log(oldList[0]);//25

        //深拷贝
        //var newList = new List<int>(oldList);
        //newList[0] *= 5;
        //Debug.Log(oldList[0]);//5

        //======================引用型List======================
        var oldList = new List<CopyListClass>();
        CopyListClass item = new CopyListClass();
        item.X = 5;
        oldList.Add(item);
        //浅拷贝1
        //var newList = new List<CopyListClass>(oldList);
        //newList[0].X *= 5;
        //Debug.Log(oldList[0].X);//25

        //浅拷贝2
        //var newArr = new CopyListClass[oldList.Count];
        //oldList.CopyTo(newArr);
        //newArr[0].X *= 5;
        //Debug.Log(oldList[0].X);//25

        //浅拷贝3
        //var newList = new List<CopyListClass>();
        //for (int i = 0; i < oldList.Count; i++)
        //{
        //    newList.Add(oldList[i]);
        //}
        //newList[0].X *= 5;
        //Debug.Log(oldList[0].X);//25



        //using (new ProfilerMarker("Test_Awake").Auto())
        //{
        //    Debug.LogError(100);
        //};






        //深拷贝1
        //var newList = new List<CopyListClass>();
        //for (int i = 0; i < oldList.Count; i++)
        //{
        //    newList.Add(oldList[i].Clone() as CopyListClass);
        //}
        //newList[0].X *= 5;
        //Debug.Log(oldList[0].X);//5


        //深拷贝2
        var newList = new List<CopyListClass>();
        for (int i = 0; i < oldList.Count; i++)
        {
            newList.Add(oldList[i].MyClone());
        }
        newList[0].X *= 5;
        Debug.Log(oldList[0].X);
    }


    class CopyListClass //: ICloneable
    {
        public int X;

        //public object Clone()
        //{
        //    return MemberwiseClone();
        //}


        //ProfilerDemo();

        //using (new ProfilerMarker("Test_Start").Auto())
        //{
        //    Debug.LogError("Test_Start");
        //};

        public CopyListClass MyClone()
        {
            var temp = new CopyListClass();
            temp.X = X;
            return temp;
        }

    }



    /// <summary>
    /// 坐标
    /// </summary>
    private void InverseTransformPointAPITest()
    {
        var cube = transform.Find("Cube");
        var sphere = transform.Find("Sphere");


        Debug.Log(cube.position);
        Debug.Log(sphere.position);

        Debug.Log(cube.InverseTransformPoint(sphere.position));
        Debug.Log(sphere.InverseTransformPoint(cube.position));
    }

    public int RomanToInt(string s)
    {
        var _dict = new Dictionary<char, int>
        {
            { 'I', 1 },
            { 'V', 5 },
            { 'X', 10 },
            { 'L', 50 },
            { 'C', 100 },
            { 'D', 500 },
            { 'M', 1000 }
        };

        var chars = s.ToCharArray();

        if (chars.Length == 1)
        {
            return _dict[chars[0]];
        }

        var result = 0;
        for (int i = 0; i < chars.Length - 1; i++)
        {
            var next = _dict[chars[i + 1]];
            var current = _dict[chars[i]];

            if (current >= next)
            {
                result += current;
            }
            else
            {
                result += (next - current);
            }
        }
        return result;
    }

    public string LongestCommonPrefix(string[] strs)
    {
        if (strs.Length <= 0 && strs == null)
        {
            return string.Empty;
        }
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < strs[0].Length; i++)
        {
            var temp = strs[0][i];

            var count = 0;
            for (int j = 1; j < strs.Length; j++)
            {
                if (strs[j].Length > i && strs[j][i].Equals(temp))
                {
                    count++;
                }
            }
            //全部一致
            if (count == strs.Length - 1)
            {
                stringBuilder.Append(temp);
            }
            else
            {
                return stringBuilder.ToString();
            }
        }
        return stringBuilder.ToString();
    }


    public string CountAndSay(int n)
    {
        if (n == 1)
        {
            return "1";
        }

        var last = CountAndSay(n - 1);
        StringBuilder stringBuilder = new StringBuilder();
        var count = 0;
        char temp = last[0];
        for (int i = 0; i < last.Length; i++)
        {
            if (temp.Equals(last[i]))
            {
                count++;
            }
            else
            {
                stringBuilder.Append(count);
                stringBuilder.Append(temp);

                count = 1;
                temp = last[i];
            }

            //如果没有下一个元素
            if (i >= last.Length - 1)
            {
                stringBuilder.Append(count);
                stringBuilder.Append(temp);
            }
        }
        return stringBuilder.ToString();
    }


    public bool IsValid(string s)
    {
        if (s.Length % 2 != 0)
        {
            return false;
        }

        var charStack = new Stack<char>();
        for (int idx = 0; idx < s.Length; idx++)
        {
            if (s[idx] == '(')
            {
                charStack.Push(s[idx]);
            }
            else if (s[idx] == '[')
            {
                charStack.Push(s[idx]);
            }
            else if (s[idx] == '{')
            {
                charStack.Push(s[idx]);
            }
            else if (s[idx] == ')')
            {
                if (charStack.Count < 1)
                {
                    return false;
                }
                if (charStack.Peek() == '(')
                {
                    charStack.Pop();
                }
                else
                {
                    return false;
                }
            }
            else if (s[idx] == ']')
            {
                if (charStack.Count < 1)
                {
                    return false;
                }
                if (charStack.Peek() == '[')
                {
                    charStack.Pop();
                }
                else
                {
                    return false;
                }
            }
            else if (s[idx] == '}')
            {
                if (charStack.Count < 1)
                {
                    return false;
                }
                if (charStack.Peek() == '{')
                {
                    charStack.Pop();
                }
                else
                {
                    return false;
                }
            }
            if (charStack.Count > s.Length - idx - 1)
            {
                return false;
            }
        }
        if (charStack.Count > 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 返回char列表的最后一个元素
    /// </summary>
    /// <param name="charList"></param>
    /// <returns></returns>
    private char GetLastChar(List<char> charList)
    {

        return charList[charList.Count - 1];
    }


    public bool IsPalindrome(int x)
    {
        // 特殊情况：
        // 如上所述，当 x < 0 时，x 不是回文数。
        // 同样地，如果数字的最后一位是 0，为了使该数字为回文，
        // 则其第一位数字也应该是 0
        // 只有 0 满足这一属性
        if (x < 0 || (x % 10 == 0 && x != 0))
        {
            return false;
        }

        int revertedNumber = 0;
        while (x > revertedNumber)
        {
            revertedNumber = revertedNumber * 10 + x % 10;
            x /= 10;
        }

        // 当数字长度为奇数时，我们可以通过 revertedNumber/10 去除处于中位的数字。
        // 例如，当输入为 12321 时，在 while 循环的末尾我们可以得到 x = 12，revertedNumber = 123，
        // 由于处于中位的数字不影响回文（它总是与自己相等），所以我们可以简单地将其去除。
        return x == revertedNumber || x == revertedNumber / 10;
    }

    private void RenderMaterialTest()
    {
        GetComponent<Renderer>().material.color = Color.white * UnityEngine.Random.Range(0, 1f);
    }

    private void RenderShareMaterialTest()
    {
        GetComponent<Renderer>().sharedMaterial.color = Color.white * UnityEngine.Random.Range(0, 1f);
    }


    /// <summary>
    /// 合并mesh
    /// </summary>
    private void CombineMesh()
    {
        var meshFilters = GetComponentsInChildren<MeshFilter>();
        var combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        var meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combine);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 合并mesh
    /// </summary>
    private void CombineMesh1()
    {
        var meshFilters = GetComponentsInChildren<MeshFilter>();
        var combine = new CombineInstance[meshFilters.Length];

        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        var materials = new Material[meshRenderers.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            materials[i] = meshRenderers[i].sharedMaterial;
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        var meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combine, false);
        gameObject.AddComponent<MeshRenderer>().sharedMaterials = materials;
        gameObject.SetActive(true);
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

        //using (new ProfilerMarker("Test_Update").Auto())
        //{
        //    Debug.LogError("Test_Update");
        //};
        if (Input.GetMouseButtonDown(1))
        {
            Resources.UnloadUnusedAssets();
        }
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
        //var dict = new Dictionary<int, int>();

        //for (int i = 0; i < 1000; i++)
        //{
        //    dict.Add(i, i);
        //}

        //using (new ProfilerMarker("Test1").Auto())
        //{
        //    foreach (var item in dict)
        //    {
        //        Debug.Log(item.Key + item.Value);
        //    }
        //}

        //using (new ProfilerMarker("Test2").Auto())
        //{
        //    var temp = dict.GetEnumerator();
        //    while (temp.MoveNext())
        //    {
        //        Debug.Log(temp.Current.Key + temp.Current.Value);
        //    }
        //}


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


        //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //stopwatch.Start();
        //for (int i = 0; i < 100; i++)
        //{
        //    Debug.LogError("111");
        //}
        //stopwatch.Stop();
        //Debug.LogError(stopwatch.ElapsedMilliseconds);

        Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
        for (int i = 0; i < 1000; i++)
        {
            keyValuePairs.Add(i, "123456");
        }
        using (new ProfilerMarker("Test_GetEnumerator").Auto())
        {
            var enumerator = keyValuePairs.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Debug.LogError(enumerator.Current.Key);
            }
        };

        using (new ProfilerMarker("Test_foreach").Auto())
        {
            foreach (KeyValuePair<int, string> item in keyValuePairs)
            {
                Debug.LogError(item.Key);
            }
        };
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
        RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
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
        //var list = new List<Student>
        //    {
        //        new Student (10,160),
        //        new Student (20,170),
        //        new Student (20,150)
        //    };

        //for (int i = 0; i < list.Count; i++)
        //{
        //    Debug.Log(list[i].ToString());
        //}

        //Debug.LogError("==============");

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

        ////权重大的排前面
        //list.Sort((item1, item2) => item1.Age.CompareTo(item2.Age) * 2 + item1.Height.CompareTo(item2.Height));


        ////list.AddRange(new List<Student>());
        //for (int i = 0; i < list.Count; i++)
        //{
        //    Debug.Log(list[i].ToString());
        //}



        List<Tuple<int, int>> tmp = new List<Tuple<int, int>>()
        {
            new Tuple<int,int>(2,1),
            new Tuple<int,int>(53,1),
            new Tuple<int,int>(12,1),
            new Tuple<int,int>(22,3),
            new Tuple<int,int>(1,2),
        };

        //先按照Item2升序排列 Item2相同的情况下在按照Item1降序排列
        //tmp.Sort((x, y) =>
        //(x.Item1.CompareTo(y.Item1) * -1
        //+ x.Item2.CompareTo(y.Item2) * 2));

        //for (int i = 0; i < tmp.Count; i++)
        //{
        //    Debug.LogError(tmp[i].Item1 + "," + tmp[i].Item2);
        //}
        //53,1
        //12,1
        //2,1
        //1,2
        //22,3


        //先按照Item2降序排列 Item2相同的情况下在按照Item1降序排列
        // tmp.Sort((x, y) =>
        //-(x.Item1.CompareTo(y.Item1)
        //+ x.Item2.CompareTo(y.Item2) * 2));
        // for (int i = 0; i < tmp.Count; i++)
        // {
        //     Debug.LogError(tmp[i].Item1 + "," + tmp[i].Item2);
        // }
        //22,3
        //1,2
        //53,1
        //12,1
        //2,1


        //先按照Item2升序排列 Item2相同的情况下在按照Item1升序排列
        // tmp.Sort((x, y) =>
        //(x.Item1.CompareTo(y.Item1) * 1
        //+ x.Item2.CompareTo(y.Item2) * 2));
        // for (int i = 0; i < tmp.Count; i++)
        // {
        //     Debug.LogError(tmp[i].Item1 + "," + tmp[i].Item2);
        // }
        //2,1
        //12,1
        //53,1
        //1,2
        //22,3



        //先按照Item2升序排列 Item2相同的情况下在按照Item1降序排列
        // tmp.Sort((x, y) =>
        //(y.Item1.CompareTo(x.Item1) * 1
        //+ x.Item2.CompareTo(y.Item2) * 2));
        // for (int i = 0; i < tmp.Count; i++)
        // {
        //     Debug.LogError(tmp[i].Item1 + "," + tmp[i].Item2);
        // }
        //53,1
        //12,1
        //2,1
        //1,2
        //22,3


        //先按照Item2降序排列 Item2相同的情况下在按照Item1降序排列
        tmp.Sort((x, y) =>
       (y.Item1.CompareTo(x.Item1) * 1
       + y.Item2.CompareTo(x.Item2) * 2));
        for (int i = 0; i < tmp.Count; i++)
        {
            Debug.LogError(tmp[i].Item1 + "," + tmp[i].Item2);
        }
        //22,3
        //1,2
        //53,1
        //12,1
        //2,1
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