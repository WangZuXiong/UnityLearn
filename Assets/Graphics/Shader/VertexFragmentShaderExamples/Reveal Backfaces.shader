Shader "Reveal Backfaces" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    SubShader {
        // 渲染对象的正面部分。
        // 我们使用简单的白色材质，并应用主纹理。
        Pass {
            Material {
                Diffuse (1,1,1,1)
            }
            Lighting On
            SetTexture [_MainTex] {
                Combine Primary * Texture
            }
        }

        // 现在，我们将背面三角形渲染成
        // 世界上最刺激的颜色：亮粉色！
        Pass {
            Color (1,0,1,1)
            Cull Front
        }
    }
}