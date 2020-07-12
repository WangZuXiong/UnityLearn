using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 屏幕后期特效
/// </summary>
public class CustomRender : MonoBehaviour
{
    [SerializeField]
    private Material _mat;
    [SerializeField]
    private Shader _shader;
    private void Awake()
    {
        _mat = new Material(_shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //使用着色器将源纹理复制到目标渲染纹理。
        Graphics.Blit(source, destination, _mat);
    }
}
