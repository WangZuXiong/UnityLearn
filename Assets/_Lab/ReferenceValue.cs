using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceValue : MonoBehaviour
{
    StudentClass _student;

    private void Awake()
    {
        //1
        StudentClass student = new StudentClass();
        Foo(ref student);
        Debug.Log(student == null);//True




        //2
        StudentClass student2 = new StudentClass();
        student2.Name = "wang";
        _student = student2;
        _student.Name = "Q";
        Debug.Log(student2.Name);//Q



        //3
        List<int> vs = new List<int>() { 1, 2, 3, 4 };
        Foo1(ref vs);//引用传递
        Foo1(vs);//引用传递
        //两者的效果是一样的
        Debug.Log(vs.Count);//0  
    }


    void Foo(ref StudentClass student)
    {
        student = null;
    }


    public void Foo1(ref List<int> vs)
    {
        vs.Clear();
    }

    public void Foo1(List<int> vs)
    {
        vs.Clear();
    }
}
