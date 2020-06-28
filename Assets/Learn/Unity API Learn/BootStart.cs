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
            Mesh mesh = _meshBuilder.BilidMesh();
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            Texture2D texture2D = _textureBuilder.GetTexture();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.SetTexture("_MainTex", texture2D);
        }
    }
}
