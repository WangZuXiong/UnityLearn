using System.Collections;
using UnityEngine;

public static class UserAuthorizationManager
{
    public static UserAuthorizationImpl _userAuthorizationImpl = new UserAuthorizationImpl();

    public static bool CheckUserAuthorization(UserAuthorization userAuthorization)
    {
        switch (userAuthorization)
        {
            case UserAuthorization.Microphone: return _userAuthorizationImpl.HasMicrophoneUserAuthorization;
            case UserAuthorization.WebCam: return true;
            default: return true;
        }
    }
}

public class UserAuthorizationImpl
{
    public bool HasMicrophoneUserAuthorization { get; private set; } = false;

    public UserAuthorizationImpl()
    {
        MonoObject.Instance.StartCoroutine(RequestUserAuthorization());
    }

    IEnumerator RequestUserAuthorization()
    {
        yield return Application.RequestUserAuthorization(/*UserAuthorization.WebCam |*/ UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(/*UserAuthorization.WebCam |*/ UserAuthorization.Microphone))
        {
            HasMicrophoneUserAuthorization = true;
        }
        else
        {

        }
    }
}