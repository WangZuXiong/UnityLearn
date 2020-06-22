using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 观察者模式
/// </summary>
public class ObserverPattern : MonoBehaviour
{
    public class Subject
    {
        private List<Observer> _observers = new List<Observer>();

        private int _state;

        public int GetState()
        {
            return _state;
        }

        public void SetState(int state)
        {
            _state = state;
            NotifyAllObservers();
        }

        public void Attach(Observer observer)
        {
            _observers.Add(observer);
        }

        public void NotifyAllObservers()
        {
            for (int i = 0; i < _observers.Count; i++)
            {
                _observers[i].Update();
            }
        }
    }

    public abstract class Observer
    {
        protected Subject Subject;
        public abstract void Update();
    }

    //创建实体观察者类。
    public class BinaryObserver : Observer
    {
        public BinaryObserver(Subject subject)
        {
            Subject = subject;
            Subject.Attach(this);
        }

        public override void Update()
        {
            Debug.Log("BinaryObserver" + Convert.ToInt32(Subject.GetState().ToString(), 2).ToString());
        }
    }

    public class OctalObserver : Observer
    {
        public OctalObserver(Subject subject)
        {
            Subject = subject;
            Subject.Attach(this);
        }

        public override void Update()
        {
            Debug.Log("OctalObserver" + Convert.ToInt32(Subject.GetState().ToString(), 8).ToString());
        }
    }

    public class HexaObserver : Observer
    {
        public HexaObserver(Subject subject)
        {
            Subject = subject;
            Subject.Attach(this);
        }

        public override void Update()
        {
            Debug.Log("HexaObserver" + Convert.ToInt32(Subject.GetState().ToString(), 16).ToString());
        }
    }

    public void Main()
    {
        Subject subject = new Subject();

        new HexaObserver(subject);
        new BinaryObserver(subject);
        new OctalObserver(subject);
        subject.SetState(1);
    }
}
