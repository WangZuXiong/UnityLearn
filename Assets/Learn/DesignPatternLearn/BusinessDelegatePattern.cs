using UnityEngine;
/// <summary>
/// 业务代表模式
/// </summary>
public class BusinessDelegatePattern
{
    //创建 BusinessService 接口。
    public interface IBusinessService
    {
        public void DoProcessing();
    }
    //创建实体服务类。
    public class EJBService : IBusinessService
    {
        public void DoProcessing()
        {
            Debug.Log("Processing task by invoking EJB Service");
        }
    }

    public class JMSService : IBusinessService
    {
        public void DoProcessing()
        {
            Debug.Log("Processing task by invoking JMS Service");
        }
    }
    //创建业务查询服务。
    public class BusinessLookUp
    {
        public IBusinessService GetBusinessService(string serverType)
        {
            switch (serverType)
            {
                case "EJB": return new EJBService();
                case "JMS": return new JMSService();
                default: return null;
            }
        }
    }
}
