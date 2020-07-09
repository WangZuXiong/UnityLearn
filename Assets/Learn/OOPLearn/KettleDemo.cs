using System.Threading;
using UnityEngine;

public class KettleDemo : MonoBehaviour
{

    public delegate void HeatWater(float temperature);
    public delegate void BoilingWater();
    /// <summary>
    /// 热水壶
    /// </summary>
    public class Kettle
    {
        public event HeatWater HeatWaterEvent;
        public event BoilingWater BoilingWaterEvent;


        /// <summary>
        /// 温度
        /// </summary>
        public float Temperature;

        /// <summary>
        /// 加热
        /// </summary>
        public void Heat()
        {
            Temperature += 10;
            HeatWaterEvent?.Invoke(Temperature);

            if (Temperature > 96)
            {
                BoilingWaterEvent?.Invoke();
            }
        }
    }

    /// <summary>
    /// 液晶屏
    /// </summary>
    public class LCDScreen
    {
        /// <summary>
        /// 显示
        /// </summary>
        public void Display(string str)
        {
            Debug.Log("LCDScreen:" + str);
        }
    }

    /// <summary>
    /// 扬声器
    /// </summary>
    public class Loudspeaker
    {
        /// <summary>
        /// 发出声音
        /// </summary>
        public void Speak(string str)
        {
            Debug.Log("Speak:" + str);
        }
    }


    public void Main()
    {
        Thread thread = new Thread(Func);
        thread.Start();
    }


    private void Func()
    {
        Kettle kettle = new Kettle();
        LCDScreen lCDScreen = new LCDScreen();
        kettle.HeatWaterEvent += new HeatWater((t) =>
        {
            lCDScreen.Display("温度：" + t.ToString());
        });

        Loudspeaker loudspeaker = new Loudspeaker();
        kettle.BoilingWaterEvent += new BoilingWater(() =>
        {
            loudspeaker.Speak("水开了");
        });

        while (kettle.Temperature < 100)
        {
            Thread.Sleep(500);
            kettle.Heat();
        }
    }
}

/*
 面向对象的方法，实现热水壶烧开水，并有如下功能
    水壶上的液晶屏会一直显示温度的变化
    假设水温超过96度时，扬声器发出提示音，告诉用户水开了
 */
