using System;
using UnityEngine;

public struct CMD
{
    public const string GetOperations = "http://192.168.3.16:6666/allianceC/getOperations?user={0}";
    public const string AddOperation = "http://192.168.3.16:6666/allianceC/addOperation?user={0}&operation={1}";
}

public class MessageSender
{
    public static void AddOperation(string groupName, BaseMsg msg)
    {
        var msgStr = JsonUtility.ToJson(msg);
        var uri = string.Format(CMD.AddOperation, groupName, msgStr);
        WebRequestManager.GetRequest(uri, null, null);
    }


    public static void GetOperations(string groupName)
    {
        var getOperationUri = string.Format(CMD.GetOperations, groupName);

        WebRequestManager.GetRequest(getOperationUri, (msgStrs) =>
        {
            try
            {
                var oldValue = new string(new char[] { '\"', ',', '\"' });
                var newValue = new string(new char[] { '\"', '@', '\"' });

                msgStrs = msgStrs.Replace(oldValue, newValue);
                msgStrs = msgStrs.Replace("[", string.Empty).Replace("]", string.Empty);
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

                    Debug.Log(msgStr);
                    MessageHandler.Func(msgStr);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }, null);
    }

}
