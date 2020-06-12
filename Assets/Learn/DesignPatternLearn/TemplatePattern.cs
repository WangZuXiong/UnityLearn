using UnityEngine;
/// <summary>
/// 模板模式
/// </summary>
public class TemplatePattern
{
    //创建一个抽象类，它的模板方法被设置为 final。
    public abstract class Game
    {
        public abstract void Initialize();

        public abstract void StartPlay();

        public abstract void EndPlay();

        public /*sealed*/ void Play()
        {
            Initialize();

            StartPlay();

            EndPlay();
        }
    }

    //创建扩展了上述类的实体类。
    public class Cricket : Game
    {
        public override void EndPlay()
        {
            Debug.Log("Cricket EndPlay");
        }

        public override void Initialize()
        {
            Debug.Log("Cricket Initialize");
        }

        public override void StartPlay()
        {
            Debug.Log("Cricket StartPlay");
        }
    }

    public class Football : Game
    {
        public override void EndPlay()
        {
            Debug.Log("Football EndPlay");
        }

        public override void Initialize()
        {
            Debug.Log("Football Initialize");
        }

        public override void StartPlay()
        {
            Debug.Log("Football StartPlay");
        }
    }

    //使用 Game 的模板方法 play() 来演示游戏的定义方式。
    public void Main()
    {
        Game game = new Cricket();
        game.Play();

        game = new Football();
        game.Play();
    }
}
