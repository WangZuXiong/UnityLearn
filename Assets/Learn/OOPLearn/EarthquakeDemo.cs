using UnityEngine;

public class EarthquakeDemo : MonoBehaviour
{
    public delegate void Earthquake();
    public class Earth
    {
        public event Earthquake OnEarthquake;
        public void Quake()
        {
            Debug.Log("地震了");
            OnEarthquake?.Invoke();
        }
    }

    public class Dog
    {
        public Dog(Earth earth)
        {
            earth.OnEarthquake += Run;
        }

        private void Run()
        {
            Debug.Log("run");
        }
    }

    public class House
    {
        public House(Earth earth)
        {
            earth.OnEarthquake += Fall;
        }

        private void Fall()
        {
            Debug.Log("fall");
        }
    }

    public class People
    {
        public People(Earth earth)
        {
            earth.OnEarthquake += Injured;
        }

        private void Injured()
        {
            Debug.Log("injured");
        }
    }


    public void Main()
    {
        Earth earth = new Earth();
        Dog dog = new Dog(earth);
        House house = new House(earth);
        People people = new People(earth);
        earth.Quake();
    }
}
/*
 设计模式题(请写出可以用哪种设计解决以下问题,给出相应的伪代码或示意图)题目:地震了,小狗乱跑、房屋倒塌、人员伤亡要求:
    要有联动性,小狗、房屋、人员的行为是被动的
    要考虑可扩展性,可能还有其他事物也会作出联动反应。
 */
