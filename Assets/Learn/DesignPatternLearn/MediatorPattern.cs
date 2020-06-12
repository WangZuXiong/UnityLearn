using UnityEngine;
/// <summary>
/// 中介者模式
/// </summary>
public class MediatorPattern : MonoBehaviour
{
    public class ChatRoom
    {
        public static void ShowMessage(User user, string message)
        {
            Debug.Log(user.GetName() + ":" + message);
        }
    }

    public class User
    {
        private string _name;

        public string GetName()
        {
            return _name;
        }

        public void SetName(string name)
        {
            this._name = name;
        }

        public User(string name)
        {
            this._name = name;
        }

        public void SendMessage(string message)
        {
            ChatRoom.ShowMessage(this, message);
        }
    }

    public void Main()
    {
        User robert = new User("Robert");
        User john = new User("John");

        robert.SendMessage("Hi! John!");
        john.SendMessage("Hi! Robert!");
    }

}
