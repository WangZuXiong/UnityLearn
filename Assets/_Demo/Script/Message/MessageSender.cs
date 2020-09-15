using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public struct CMD
{
    public const string GetOperations = "http://192.168.3.16:5555/allianceC/getOperations?user={0}";
    public const string AddOperation = "http://192.168.3.16:5555/allianceC/addOperation?user={0}";
}

public class MessageSender
{
    public static void AddOperation(Operation operation, object body)
    {
        var groupName = GameData.OurPlayerName;
        var uri = string.Format(CMD.AddOperation, groupName);

        int msgType = (int)operation;
        var msg = new Msg
        {
            MsgType = msgType,
            Body = Encoding.UTF8.GetBytes(JsonUtility.ToJson(body))//new List<byte>()
        };
        var msgStr = JsonUtility.ToJson(msg);

        WebRequestManager.Post(uri, msgStr, null, null);
    }


    public static void GetOperations(string groupName)
    {
        var getOperationUri = string.Format(CMD.GetOperations, groupName);


        WebRequestManager.Get(getOperationUri, (msgStrs) =>
        {
            try
            {
                var oldValue = new string(new char[] { '\"', ',', '\"' });
                var newValue = new string(new char[] { '\"', '@', '\"' });

                msgStrs = msgStrs.Replace(oldValue, newValue);
                //msgStrs = msgStrs.Replace("[", string.Empty).Replace("]", string.Empty);


                msgStrs = msgStrs.Remove(0, 1);
                msgStrs = msgStrs.Remove(msgStrs.Length - 1, 1);


                var msgs = msgStrs.Split('@');

                for (int i = 0; i < msgs.Length; i++)
                {
                    var msgStr = msgs[i];

                    if (string.IsNullOrEmpty(msgStr))
                    {
                        return;
                    }

                    msgStr = msgStr.Remove(0, 1);
                    msgStr = msgStr.Remove(msgStr.Length - 1, 1);
                    msgStr = msgStr.Replace("\\", "");

                    //Debug.Log(msgStr);

                    //var msg = JsonUtility.FromJson<Msg>(msgStr);

                    //Debug.Log(msg.MsgType);
                    //var body = System.Text.Encoding.UTF8.GetString(msg.Body.ToArray());
                    //var t = JsonUtility.FromJson<TeamNCity>(body);
                    //Debug.Log(t.CityData.PlayerName);

                    MessageHandler.HandleMsg(msgStr);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }, null);
    }

}
