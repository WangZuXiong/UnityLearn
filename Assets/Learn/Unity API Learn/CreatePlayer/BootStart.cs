using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unity
{
    public class BootStart : MonoBehaviour
    {
        [SerializeField]
        private MeshBuilder _meshBuilder;
        [SerializeField]
        private TextureBuilder _textureBuilder;

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

        private Matrix4x4 Str2Matrix(string str)
        {
            var floatStr = str.Split('\t');

            Matrix4x4 matrix4X4 = new Matrix4x4
            {
                m00 = float.Parse(floatStr[0]),
                m01 = float.Parse(floatStr[1]),
                m02 = float.Parse(floatStr[2]),
                m03 = float.Parse(floatStr[3]),

                m10 = float.Parse(floatStr[4]),
                m11 = float.Parse(floatStr[5]),
                m12 = float.Parse(floatStr[6]),
                m13 = float.Parse(floatStr[7]),

                m20 = float.Parse(floatStr[8]),
                m21 = float.Parse(floatStr[9]),
                m22 = float.Parse(floatStr[10]),
                m23 = float.Parse(floatStr[11]),

                m30 = float.Parse(floatStr[12]),
                m31 = float.Parse(floatStr[13]),
                m32 = float.Parse(floatStr[14]),
                m33 = float.Parse(floatStr[15])
            };

            return matrix4X4;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(PlayAnimation());
            }
        }

        private IEnumerator PlayAnimation()
        {
            TextAsset[] textAssets = Resources.LoadAll<TextAsset>("Matrixs");
            for (int i = 0; i < textAssets.Length; i++)
            {
                var text = textAssets[i].text;

                var strs = text.Split('\n');

                List<Matrix4x4> matrixs = new List<Matrix4x4>();
                for (int j = 0; j < strs.Length;)
                {
                    if (string.IsNullOrEmpty(strs[j]))
                    {
                        j++;
                        continue;
                    }

                    List<string> temp = new List<string>
                    {
                        strs[j],
                        strs[j + 1],
                        strs[j + 2],
                        strs[j + 3]
                    };
                    j += 4;

                    matrixs.Add(Str2Matrix(string.Join("\t", temp)));
                }
             

                GetComponent<MeshRenderer>().material.SetMatrixArray("_SkeletonMatrices", matrixs.ToArray());
                Debug.Log("play" + textAssets[i].name);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
