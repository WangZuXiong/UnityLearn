using UnityEngine;
using UnityEngine.UI;

public class TextureBuilder : MonoBehaviour
{
    [SerializeField]
    private Texture _baseTexture;
    [SerializeField]
    private Sprite _faceSprite;
    [SerializeField]
    private Sprite _hairSprite;
    [SerializeField]
    private Sprite _numberSprite;
    [SerializeField]
    private Sprite[] _nameSprites;


    public Texture2D GetTexture()
    {
        var baseRawImg = transform.Find("Base").GetComponent<RawImage>();
        baseRawImg.texture = _baseTexture;
        baseRawImg.transform.Find("Face").GetComponent<Image>().sprite = _faceSprite;
        baseRawImg.transform.Find("Hair").GetComponent<Image>().sprite = _hairSprite;
        baseRawImg.transform.Find("Number").GetComponent<Image>().sprite = _numberSprite;
        var nameRawImgs = baseRawImg.transform.Find("Name").GetComponentsInChildren<Image>();
        for (int i = 0; i < _nameSprites.Length; i++)
        {
            nameRawImgs[i].sprite = _nameSprites[i];
        }

        Camera renderCamera = Camera.main;
        RenderTexture renderTexture = new RenderTexture(512, 512, 16);
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.anisoLevel = 0;
        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();
        RenderTexture.active = renderTexture;

        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0, false);
        texture2D.Apply(false, true);

        return texture2D;
    }
}
