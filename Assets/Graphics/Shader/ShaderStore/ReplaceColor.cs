using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var img = GetComponent<Image>();
        var mat = img.material;
        var sprite = img.sprite;
        mat.SetVector("_Pos", new Vector4(sprite.rect.x, sprite.rect.y));
        mat.SetVector("_Size", new Vector4(sprite.texture.width, sprite.texture.height));
        mat.SetVector("_SubSize", new Vector4(sprite.rect.width, sprite.rect.height));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
