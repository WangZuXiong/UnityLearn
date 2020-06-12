using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 命令模式
/// </summary>
public class CommandPattern
{
    //创建一个命令接口。
    public interface IOrder
    {
        void Execute();
    }
    //创建一个请求类。 股票
    public class Stock
    {
        private string _name = "ABC";
        private int _quantity = 10;

        public void Buy()
        {
            Debug.Log("Buy  " + "_name:" + _name + "_quantity:" + _quantity);
        }

        public void Sell()
        {
            Debug.Log("Sell  " + "_name:" + _name + "_quantity:" + _quantity);
        }
    }
    //创建实现了 Order 接口的实体类。

    public class BuyStock : IOrder
    {
        private Stock _abcStock;
        public BuyStock(Stock abcStock)
        {
            _abcStock = abcStock;
        }

        public void Execute()
        {
            _abcStock.Sell();
        }
    }

    public class SellStock : IOrder
    {
        private Stock _abcStock;
        public SellStock(Stock abcStock)
        {
            _abcStock = abcStock;
        }

        public void Execute()
        {
            _abcStock.Sell();
        }
    }
    //创建命令调用类。//经纪人
    public class Broker
    {
        private List<IOrder> _orderList = new List<IOrder>();

        public void TakeOrder(IOrder order)
        {
            _orderList.Add(order);
        }

        public void PlaceOrders()
        {
            for (int i = 0; i < _orderList.Count; i++)
            {
                _orderList[i].Execute();
            }   
        }
    }

    public void Main()
    {
        Stock abcStock = new Stock();
        BuyStock buyStockOrder = new BuyStock(abcStock);
        SellStock sellStockOrder = new SellStock(abcStock);

        Broker broker = new Broker();
        broker.TakeOrder(buyStockOrder);
        broker.TakeOrder(sellStockOrder);

        broker.PlaceOrders();
    }
}
