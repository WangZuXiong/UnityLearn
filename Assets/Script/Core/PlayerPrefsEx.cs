public static class PlayerPrefs
{
    public static void SetBool(string key, bool value)
    {
        UnityEngine.PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        var temp = UnityEngine.PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
        return temp == 1;
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        return UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void SetInt(string key, int value)
    {
        UnityEngine.PlayerPrefs.SetInt(key, value);
    }

    public static float GetFloat(string key, float defaultValue = 0)
    {
        return UnityEngine.PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static void SetFloat(string key, float value)
    {
        UnityEngine.PlayerPrefs.SetFloat(key, value);
    }

    public static string GetString(string key, string defaultValue = "")
    {
        return UnityEngine.PlayerPrefs.GetString(key, defaultValue);
    }

    public static void SetString(string key, string value)
    {
        UnityEngine.PlayerPrefs.SetString(key, value);
    }


    public static bool HasKey(string key)
    {
        return UnityEngine.PlayerPrefs.HasKey(key);
    }


    public static void DeleteKey(string key)
    {
        UnityEngine.PlayerPrefs.DeleteKey(key);
    }

    public static void DeleteAll()
    {
        UnityEngine.PlayerPrefs.DeleteAll();
    }


    public static void Save()
    {
        UnityEngine.PlayerPrefs.Save();
    }
}
