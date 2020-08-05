using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class ThreadWithUnity 
{
    private enum ActionType
    {
        Create,
        Destroy
    }

    private struct Message
    {
        public ActionType Type;
        public string Parm;
    }

    private List<Message> _messageList = new List<Message>();
    private Thread _thread;


    private void Start()
    {
        ThreadStart threadStart = new ThreadStart(SubThread);
        _thread = new Thread(threadStart);
        _thread.Name = "Thread 1";
        _thread.Start();
    }

    private void Update()
    {
        lock (((ICollection)_messageList).SyncRoot)
        {
            if (_messageList.Count > 0)
            {
                HandleMessage(_messageList[0]);
                _messageList.RemoveAt(0);
            }
        }
    }

    private void SubThread()
    {
        _messageList.Add(new Message() { Type = ActionType.Create, Parm = "AAAA" });
        _messageList.Add(new Message() { Type = ActionType.Create, Parm = "BBBB" });
        _messageList.Add(new Message() { Type = ActionType.Create, Parm = "CCCC" });

        Thread.Sleep(3000);

        _messageList.Add(new Message() { Type = ActionType.Destroy, Parm = "AAAA" });
        _messageList.Add(new Message() { Type = ActionType.Destroy, Parm = "BBBB" });
        _messageList.Add(new Message() { Type = ActionType.Destroy, Parm = "CCCC" });
    }

    private void HandleMessage(Message message)
    {
        Console.WriteLine("CurrentThread:" + Thread.CurrentThread.Name);

        switch (message.Type)
        {
            case ActionType.Create:
                GameObject gameObject = new GameObject(message.Parm);
                break;
            case ActionType.Destroy:
                var go = GameObject.Find(message.Parm);
                if (go != null)
                {
                    GameObject.Destroy(go);
                }
                break;

        }
    }
}
