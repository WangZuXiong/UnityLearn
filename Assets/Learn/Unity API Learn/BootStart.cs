using UnityEngine;
using UnityEngine.UI;

namespace unity
{
    public class BootStart : MonoBehaviour
    {
        [SerializeField]
        private MeshBuilder _meshBuilder;
        [SerializeField]
        private TextureBuilder _textureBuilder;
        [SerializeField]
        private TextAsset _matrilxTex;

        private void Awake()
        {
            //create mesh
            Mesh mesh = _meshBuilder.BilidMesh();
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            //create texture
            Texture2D texture2D = _textureBuilder.GetTexture();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.SetTexture("_MainTex", texture2D);
        }



        private void Update()
        {
      
            //set shader matrix4x4

            Matrix4x4[] matrix4X4s = new Matrix4x4[17];

            var tempList = _matrilxTex.ToString().Split('@');
            for (int i = 0; i < tempList.Length; i++)
            {
                var str = tempList[i].Split(',');
                Matrix4x4 matrix4X4 = new Matrix4x4();

                matrix4X4.m00 = float.Parse(str[0]);
                matrix4X4.m01 = float.Parse(str[1]);
                matrix4X4.m02 = float.Parse(str[2]);
                matrix4X4.m03 = float.Parse(str[3]);

                matrix4X4.m10 = float.Parse(str[4]);
                matrix4X4.m11 = float.Parse(str[5]);
                matrix4X4.m12 = float.Parse(str[6]);
                matrix4X4.m13 = float.Parse(str[7]);

                matrix4X4.m20 = float.Parse(str[8]);
                matrix4X4.m21 = float.Parse(str[9]);
                matrix4X4.m22 = float.Parse(str[10]);
                matrix4X4.m23 = float.Parse(str[11]);

                matrix4X4.m30 = float.Parse(str[12]);
                matrix4X4.m31 = float.Parse(str[13]);
                matrix4X4.m32 = float.Parse(str[14]);
                matrix4X4.m33 = float.Parse(str[15]);

                Debug.Log(matrix4X4.ToString());

                matrix4X4s[i] = matrix4X4;
            }
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.SetMatrixArray("_skeletalMatrix", matrix4X4s);
        }
    }
}
