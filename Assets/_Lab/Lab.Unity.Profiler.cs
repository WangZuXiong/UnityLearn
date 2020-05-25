using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public partial class Lab : MonoBehaviour
{
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
}
