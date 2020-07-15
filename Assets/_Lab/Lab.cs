using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

//[ExecuteInEditMode]
public partial class Lab : MonoBehaviour, IPointerEnterHandler
{
    public int _tempValue;

    private void Start()
    {
      
    }

    private int FindSecondMaxNum(int[] vs)
    {
        var max = int.MinValue;
        var secondMax = int.MinValue;

        for (int i = 0; i < vs.Length; i++)
        {
            if (vs[i] > max)
            {
                secondMax = max;
                max = vs[i];
            }
            else
            {
                if (vs[i] > secondMax)
                {
                    secondMax = vs[i];
                }
            }
        }
        return secondMax;
    }

    private void TestMyListRemove()
    {
        List<int> tempList = new List<int>();

        for (int i = 0; i < 1000; i++)
        {
            tempList.Add(i);
        }
        using (new ProfilerMarker("ListRemoveAdvance").Auto())
        {
            UnityUtility.ListRemoveAdvance(tempList, 1);//288b 2.25ms
        }

        using (new ProfilerMarker("ListRemove").Auto())
        {
            tempList.Remove(1);//0 b 0.7ms
        }


        for (int i = 0; i < tempList.Count; i++)
        {
            Debug.Log(tempList[i]);
        }
    }


    private void StructTest()
    {
        //类使用前必须new关键字实例化，Struct不需要
        Vector2 vector2;
        vector2.x = 1;
        vector2.y = 1;
        Debug.Log(vector2.ToString());//(1.0, 1.0)
    }


    public Vector3 target;
    public float speed;



    private void Update()
    {
        if (Vector3.Distance(transform.position, target) > 1)
        {
            // 1
            //transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);

            // 2
            //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            //3
            //Vector3 translation = (target - transform.position).normalized;
            //transform.Translate(translation * speed * Time.deltaTime);

            //4
            //transform.GetComponent<Rigidbody>().MovePosition(target);

            //5
            //Vector3 translation = (target - transform.position).normalized;
            //transform.GetComponent<Rigidbody>().velocity = translation * speed * Time.deltaTime;



        }



        if (Input.GetMouseButtonDown(1))
        {
            //BulletBehaviour();



        }
        else if (Input.GetMouseButtonDown(0))
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

    }
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
}
