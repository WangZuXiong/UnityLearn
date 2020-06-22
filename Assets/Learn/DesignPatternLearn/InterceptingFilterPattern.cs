using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 拦截过滤器模式
/// </summary>
public class InterceptingFilterPattern : MonoBehaviour
{
    public interface IFilter
    {
        void Execute(string request);
    }

    public class AuthenticationFilter : IFilter
    {
        public void Execute(string request)
        {
            Debug.Log("Authenticating request:" + request);
        }
    }

    public class DebugFilter : IFilter
    {
        public void Execute(string request)
        {
            Debug.Log("request log:" + request);
        }
    }

    public class Target
    {
        public void Execute(string request)
        {
            Debug.Log("Excuting request :" + request);
        }
    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    public class FilterChain
    {
        private List<IFilter> _filters = new List<IFilter>();
        private Target _target;

        public void AddFilter(IFilter filter)
        {
            _filters.Add(filter);
        }

        public void Excute(string request)
        {
            for (int i = 0; i < _filters.Count; i++)
            {
                _filters[i].Execute(request);
            }

            _target.Execute(request);
        }

        public void SetTarget(Target target)
        {
            _target = target;
        }
    }
    /// <summary>
    /// 创建过滤管理器。
    /// </summary>
    public class FilterManager
    {
        private FilterChain _filterChain;

        public FilterManager(Target target)
        {
            _filterChain = new FilterChain();
            _filterChain.SetTarget(target);
        }

        public void SetFilter(IFilter filter)
        {
            _filterChain.AddFilter(filter);
        }

        public void FilterRequest(string request)
        {
            _filterChain.Excute(request);
        }
    }

    public class Client
    {
        private FilterManager _fiterManager;

        public void SetFilterManager(FilterManager filterManager)
        {
            _fiterManager = filterManager;
        }

        public void SendRequest(string request)
        {
            _fiterManager.FilterRequest(request);
        }
    }

    public void Main()
    {
        FilterManager filterManager = new FilterManager(new Target());
        filterManager.SetFilter(new AuthenticationFilter());
        filterManager.SetFilter(new DebugFilter());

        Client client = new Client();
        client.SetFilterManager(filterManager);
        client.SendRequest("HOME");
    }
}
