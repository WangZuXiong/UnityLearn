using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
/// <summary>
/// 组合实体模式
/// </summary>
public class CompositeEntityPattern
{
    //创建依赖对象。
    public class DependentObject1
    {
        private string _data;

        public void SetData(string data)
        {
            _data = data;
        }

        public string GetData()
        {
            return _data;
        }
    }

    public class DependentObject2
    {
        private string _data;

        public void SetData(string data)
        {
            _data = data;
        }

        public string GetData()
        {
            return _data;
        }
    }

    //创建粗粒度对象。

    public class CoarseGrainedObject
    {
        private DependentObject1 _dependentObject1 = new DependentObject1();
        private DependentObject2 _dependentObject2 = new DependentObject2();

        public void SetData(string data1, string data2)
        {
            _dependentObject1.SetData(data1);
            _dependentObject2.SetData(data2);
        }

        public string[] GetData()
        {
            return new string[] { _dependentObject1.GetData(), _dependentObject2.GetData() };
        }
    }

    //创建组合实体。
    public class CompositeEntity
    {
        private CoarseGrainedObject _coarseGrainedObject = new CoarseGrainedObject();

        public void SetData(string data1, string data2)
        {
            _coarseGrainedObject.SetData(data1, data2);
        }

        public string[] GetData()
        {
            return _coarseGrainedObject.GetData();
        }
    }
    //创建使用组合实体的客户端类。

    public class Client
    {
        private CompositeEntity _compositeEntity = new CompositeEntity();

        public void PrintData()
        {
            for (int i = 0; i < _compositeEntity.GetData().Length; i++)
            {
                Debug.Log("Data::" + _compositeEntity.GetData()[i]);
            }
        }

        public void SetData(string data1, string data2)
        {
            _compositeEntity.SetData(data1, data2);
        }
    }

    //使用 Client 来演示组合实体设计模式的用法。
    public void Main()
    {
        Client client = new Client();
        client.SetData("Test", "Data");
        client.PrintData();
        client.SetData("Test 2", "Data 2");
        client.PrintData();
    }
}
