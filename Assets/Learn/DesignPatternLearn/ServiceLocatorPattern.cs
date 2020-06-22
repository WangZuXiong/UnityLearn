using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 服务定位器模式
/// </summary>
public class ServiceLocatorPattern : MonoBehaviour
{
    public interface IService
    {
        string GetName();
        void Excute();
    }

    public class Service1 : IService
    {
        public void Excute()
        {
            Debug.Log("Excute service 1");
        }

        public string GetName()
        {
            return "Service 1";
        }
    }

    public class Service2 : IService
    {
        public void Excute()
        {
            Debug.Log("Excute service 2");
        }

        public string GetName()
        {
            return "Service 2";
        }
    }


    public class InitialContext
    {
        public object LookUp(string jndiName)
        {
            if (jndiName.Equals("SEVICE1"))
            {
                Debug.Log("Looking up and creating a new Service1 object");
                return new Service1();
            }
            else if (jndiName.Equals("SEVICE2"))
            {
                Debug.Log("Looking up and creating a new Service1 object");
                return new Service2();
            }
            return null;
        }
    }

    //创建缓存 Cache。
    public class Cache
    {
        private List<IService> _services;

        public Cache()
        {
            _services = new List<IService>();
        }


        public IService GetService(string serviceName)
        {
            return _services.Find(item => item.GetName().Equals(serviceName));
        }

        public void AddService(IService service)
        {
            if (GetService(service.GetName()) == null)
            {
                _services.Add(service);
            }
        }
    }
    /// <summary>
    /// 创建定位服务器
    /// </summary>
    public class ServiceLocator
    {
        private static Cache _cache = new Cache();

        public static IService GetService(string jndiName)
        {
            IService service = _cache.GetService(jndiName);

            if (service != null)
            {
                return service;
            }

            InitialContext context = new InitialContext();
            IService service1 = (IService)context.LookUp(jndiName);
            _cache.AddService(service1);
            return service1;
        }
    }


    public void Main()
    {
        IService service = ServiceLocator.GetService("Service1");
        service.Excute();
        service = ServiceLocator.GetService("Service2");
        service.Excute();

        service = ServiceLocator.GetService("Service1");
        service.Excute();

        service = ServiceLocator.GetService("Service1");
        service.Excute();
    }
}
