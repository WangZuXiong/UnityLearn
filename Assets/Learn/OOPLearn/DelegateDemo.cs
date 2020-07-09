using UnityEngine;

public class DelegateDemo : MonoBehaviour
{
    public delegate void Raise(string hand);
    public delegate void Fall();

    public class A
    {
        public event Raise OnRaise;
        public event Fall OnFall;
        public void Raise(string hand)
        {
            if (hand == "L")
            {
                OnRaise?.Invoke("L");
            }
            else if (hand == "R")
            {
                OnRaise?.Invoke("R");
            }
        }

        public void Fall()
        {
            OnFall?.Invoke();
        }
    }


    public class B
    {
        public B(A a)
        {
            a.OnRaise += OnARaise;
            a.OnFall += Atk;
        }

        void OnARaise(string hand)
        {
            if (hand.Equals("L"))
            {
                Atk();
            }
        }

        void Atk()
        {
            Debug.Log("a 杀出");
        }
    }

    public class C
    {
        public C(A a)
        {
            a.OnRaise += OnARaise;
            a.OnFall += Atk;
        }

        void OnARaise(string hand)
        {
            if (hand.Equals("R"))
            {
                Atk();
            }
        }

        void Atk()
        {
            Debug.Log("b 杀出");
        }
    }


    private void Main()
    {
        A a = new A();
        //观察者模式
        B b = new B(a);
        C c = new C(a);

        a.Raise("L");
        a.Raise("R");
        a.Fall();
    }
}

/*
 首领A要搞一场鸿门宴，吩咐部下B和C各自带队埋伏在屏风两侧，约定以杯为令：
    若左手举杯，则B带队杀出；
    若右手举杯，则C带队杀出；
    若直接摔杯，则B和C同时杀出。B和C袭击的具体方法，首领A并不关心。
 */
