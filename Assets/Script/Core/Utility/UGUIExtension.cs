using UnityEngine;
using UnityEngine.UI;

public static class UGUIExtension
{
    #region Image
    public static void ColorToGray(this Image image)
    {
        image.color = Color.gray;
    }

    public static void ColorToWhite(this Image image)
    {
        image.color = Color.white;
    }

    public static void ColorToClear(this Image image)
    {
        image.color = Color.clear;
    }

    #endregion

    #region Text

    #endregion
}

