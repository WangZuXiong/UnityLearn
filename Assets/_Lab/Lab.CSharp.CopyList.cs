using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Lab : MonoBehaviour
{
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
}
