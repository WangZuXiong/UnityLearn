using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 工厂模式
/// 实际运用：加载数字图集中的单个sprite，UI框架的设计
/// </summary>
public class Factory : MonoBehaviour
{

    public Fish GetFish(string fishName)
    {
        if (fishName == "sharkFish")
        {
            return new SharkFish();
        }
        else
        {
            return new TunasFish();
        }
    }
}



public class Fish
{
    public virtual void Swim()
    {
        Debug.Log("游泳");
    }
}
class SharkFish : Fish
{
    public override void Swim()
    {
        base.Swim();
        Debug.Log("我是一条鲨鱼");
    }
}
class TunasFish : Fish
{
    public override void Swim()
    {
        base.Swim();
        Debug.Log("我是一条金枪鱼");
    }
}