using UnityEngine;
/// <summary>
/// 业务代表模式
/// </summary>
public class BusinessDelegatePattern
{
    //创建 BusinessService 接口。
    public interface IBusinessService
    {
        void DoProcessing();
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
    //创建业务代表。
    public class BusinessDelegate
    {
        private BusinessLookUp _businessLookUp = new BusinessLookUp();
        private IBusinessService _businessService;
        private string _serverType;

        public void SetServerType(string serverType)
        {
            _serverType = serverType;
        }

        public void DoTask()
        {
            _businessService = _businessLookUp.GetBusinessService(_serverType);
            _businessService.DoProcessing();
        }
    }
    //创建客户端。
    public class Client
    {
        private BusinessDelegate _businessDelegate;
        public Client(BusinessDelegate businessDelegate)
        {
            _businessDelegate = businessDelegate;
        }

        public void DoTask()
        {
            _businessDelegate.DoTask();
        }
    }

    public void Main()
    {
        BusinessDelegate businessDelegate = new BusinessDelegate();
        businessDelegate.SetServerType("EJB");

        Client client = new Client(businessDelegate);
        client.DoTask();


        businessDelegate.SetServerType("JMS");
        client.DoTask();
    }
}
