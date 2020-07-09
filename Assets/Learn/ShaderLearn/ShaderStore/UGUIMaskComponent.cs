using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Image))]
public class UGUIMaskComponent : MonoBehaviour
{
    [ContextMenu("Change Sprite")]
    private void Start()
    {
        var img = GetComponent<Image>();
        if (img)
        {
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
            var mat = rawImg.material;
            mat.SetVector("_Pos", new Vector4(0, 0));
            mat.SetVector("_Size", new Vector4(rawImg.texture.width, rawImg.texture.height));
            mat.SetVector("_SubSize", new Vector4(rawImg.texture.width, rawImg.texture.height));
        }
    }
}
