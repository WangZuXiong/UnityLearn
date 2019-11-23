using UnityEngine;
using UnityEngine.UI;

public class UGUIMaskTest : MonoBehaviour
{
    private void Start()
    {
        var original = Resources.Load<Material>("UGUIMaskMat");
        var newMat = Instantiate(original);

        var img = GetComponent<Image>();
        if (img != null)
        {
            img.material = newMat;
            var mat = img.material;
            var sprite = img.sprite;
            mat.SetVector("_Pos", new Vector4(sprite.rect.x, sprite.rect.y));
            mat.SetVector("_Size", new Vector4(sprite.texture.width, sprite.texture.height));
            mat.SetVector("_SubSize", new Vector4(sprite.rect.width, sprite.rect.height));
            return;
        }

        var rawImg = GetComponent<RawImage>();
        if (rawImg)
        {
            rawImg.material = newMat;
            var mat = rawImg.material;
            mat.SetVector("_Pos", Vector4.zero);
            mat.SetVector("_Size", new Vector4(rawImg.texture.width, rawImg.texture.height));
            mat.SetVector("_SubSize", new Vector4(rawImg.texture.width, rawImg.texture.height));
        }
    }
}
