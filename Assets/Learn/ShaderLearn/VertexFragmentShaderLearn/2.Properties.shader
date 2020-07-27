//属性在 Properties 代码块中的单独行上列出。
//每个属性都以内部名称（__Color、MainTex__）开头。
//后面括号中显示的是在检视面板中显示的名称以及属性类型。随后列出此属性的默认值：
Shader "ShaderLearn/2.Properties"
{
    Properties
    {
        //滑动条
        _TestRange ("Test Range", Range (1, 5)) = 2
        //数字
        _TestFloat ("Test Float", Float) = 1.0
        _TestInt ("Test Int", Int) = 1
        //颜色
        _TestColor ("Test Color", Color) = (1,1,1,1)
        //矢量
        _TestVector ("Test Vector", Vector) = (1,1,1,1)//Shader Lab里面的Vector是四维的
        //纹理
        _Test2D_1 ("Test 2D 1", 2D) = "white" {}
        _Test2D_2 ("Test 2D 2", 2D) = "red" {}
        _TestCube ("Test Cube", Cube) = "" {}
        _Test3D ("Test 3D", 3D) = "" {} 
    }
    SubShader
    {
        Pass
        {
          
        }
    }
}
