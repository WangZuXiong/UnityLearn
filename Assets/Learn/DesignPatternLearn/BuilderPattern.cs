using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 建造者模式
/// </summary>
public class BuilderPattern
{
    //创建一个表示食物条目和食物包装的接口。

    public interface IItem
    {
        string GetName();
        IPacking GetPack();

        public float GetPrice();
    }


    public interface IPacking
    {
        string Pack();
    }

    //创建实现 Packing 接口的实体类。

    public class Wrapper : IPacking
    {
        public string Pack()
        {
            return "Wrapper";
        }
    }

    public class Bottle : IPacking
    {
        public string Pack()
        {
            return "Bottle";
        }
    }

    //创建实现 Item 接口的抽象类，该类提供了默认的功能。
    public abstract class Burger : IItem
    {
        public abstract string GetName();

        public IPacking GetPack()
        {
            return new Wrapper();
        }

        public abstract float GetPrice();
    }

    public abstract class ColdDrink : IItem
    {
        public abstract string GetName();

        public IPacking GetPack()
        {
            return new Bottle();
        }

        public abstract float GetPrice();
    }


    public class VegBurger : Burger
    {
        public override string GetName()
        {
            return "Veg Burger";
        }

        public override float GetPrice()
        {
            return 25;
        }
    }

    public class ChickenBurger : Burger
    {
        public override string GetName()
        {
            return "Chicken Burger";
        }

        public override float GetPrice()
        {
            return 50;
        }
    }

    public class Coke : ColdDrink
    {
        public override string GetName()
        {
            return "Coke";
        }

        public override float GetPrice()
        {
            return 30;
        }
    }

    public class Pepsi : ColdDrink
    {
        public override string GetName()
        {
            return "Pepsi";
        }

        public override float GetPrice()
        {
            return 35;
        }
    }

    public class Meal
    {
        private List<IItem> _items = new List<IItem>();

        public void AddItem(IItem item)
        {
            _items.Add(item);
        }

        public float GetCost()
        {
            float cost = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                cost += _items[i].GetPrice();
            }
            return cost;
        }

        public void ShowItems()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                Debug.Log("Name::" + _items[i].GetName());
                Debug.Log("Pack::" + _items[i].GetPack());
                Debug.Log("Price::" + _items[i].GetPrice());
                Debug.Log("====================");
            }
        }
    }

    //创建一个 MealBuilder 类，实际的 builder 类负责创建 Meal 对象。
    public class MealBuilder
    {
        public Meal PrepareVegMeal()
        {
            Meal meal = new Meal();
            meal.AddItem(new VegBurger());
            meal.AddItem(new Coke());
            return meal;
        }

        public Meal PrepareNonVegMeal()
        {
            Meal meal = new Meal();
            meal.AddItem(new ChickenBurger());
            meal.AddItem(new Pepsi());
            return meal;
        }
    }


    public void Main()
    {
        MealBuilder mealBuilder = new MealBuilder();
        var vegMeal = mealBuilder.PrepareVegMeal();
        Debug.Log("Veg Meal:");
        vegMeal.ShowItems();
        Debug.Log("Total Cost: " + vegMeal.GetCost());

    }
}
