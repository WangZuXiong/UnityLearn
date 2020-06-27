using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MeshLearn : MonoBehaviour
{
    [SerializeField]
    private Mesh _bodyMesh;
    [SerializeField]
    private TextAsset _faceTextAsset;
    [SerializeField]
    private TextAsset _chinTextAsset;
    [SerializeField]
    private TextAsset _hairTextAssets;

    private void Awake()
    {
        var mesh = BilidMesh();
        GetComponent<MeshFilter>().mesh = mesh;
        //SaveMesh(GetComponent<MeshFilter>().mesh);
    }


    private Mesh BilidMesh()
    {
        List<Vector3> tempVertices = new List<Vector3>();
        List<Vector3> tempNormals = new List<Vector3>();
        List<int> tempTriangles = new List<int>();
        List<Vector2> tempChannel0Uvs = new List<Vector2>();
        List<Vector4> tempChannel2Uvs = new List<Vector4>();
        List<Vector4> tempChannel1Uvs = new List<Vector4>();

        //body

        //vertices 返回顶点位置的副本或分配新顶点位置数组。
        Vector3[] bodyVertices = new Vector3[_bodyMesh.vertices.Length];
        //normals 网格的法线。
        Vector3[] bodyNormals = new Vector3[_bodyMesh.normals.Length];
        //triangles 包含网格中所有三角形的数组。
        int[] bodyTriangles = new int[_bodyMesh.triangles.Length];
        Array.Copy(_bodyMesh.vertices, bodyVertices, bodyVertices.Length);
        Array.Copy(_bodyMesh.normals, bodyNormals, bodyNormals.Length);
        Array.Copy(_bodyMesh.triangles, bodyTriangles, bodyTriangles.Length);

        List<Vector2> bodyChannel0Uvs = new List<Vector2>();
        List<Vector4> bodyChannel1Uvs = new List<Vector4>();
        List<Vector4> bodyChannel2Uvs = new List<Vector4>();
        //	获取网格的 UV。
        _bodyMesh.GetUVs(0, bodyChannel0Uvs);
        _bodyMesh.GetUVs(1, bodyChannel1Uvs);
        _bodyMesh.GetUVs(2, bodyChannel2Uvs);

        Resources.UnloadAsset(_bodyMesh);

        tempVertices.AddRange(bodyVertices);
        tempNormals.AddRange(bodyNormals);
        tempTriangles.AddRange(bodyTriangles);
        tempChannel0Uvs.AddRange(bodyChannel0Uvs);
        tempChannel1Uvs.AddRange(bodyChannel1Uvs);
        tempChannel2Uvs.AddRange(bodyChannel2Uvs);


        //face chin hair
        byte[] faceBytes = _faceTextAsset.bytes;
        byte[] chinBytes = _chinTextAsset.bytes;
        byte[] hairBytes = _hairTextAssets.bytes;
        int unitLen = sizeof(float) * (3 + 3 + 2 + 4 + 4);
        int faceVerCount = faceBytes.Length / unitLen;
        int chinVerCount = chinBytes.Length / unitLen;
        int hairVerCount = hairBytes.Length / unitLen;

        List<ValueTuple<byte[], int, int>> temps = new List<(byte[], int, int)>
        {
            (faceBytes, faceVerCount, bodyVertices.Length),
            (chinBytes, chinVerCount, bodyVertices.Length + faceVerCount),
            (hairBytes, hairVerCount, bodyVertices.Length + faceVerCount + chinVerCount)
        };

        for (int i = 0; i < temps.Count; i++)
        {
            var data = temps[i].Item1;
            var verCount = temps[i].Item2;
            var offset = temps[i].Item3;

            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    for (int j = 0; j < verCount; j++)
                    {
                        tempTriangles.Add(j + offset);
                        tempVertices.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
                        tempNormals.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
                        tempChannel0Uvs.Add(new Vector2(reader.ReadSingle(), reader.ReadSingle()));
                        tempChannel1Uvs.Add(new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
                        tempChannel2Uvs.Add(new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
                    }
                }
            }
        }

        Mesh tempMesh = new Mesh();
        //分配新的顶点位置数组。
        tempMesh.SetVertices(tempVertices);
        //为子网格设置三角形列表。
        tempMesh.SetTriangles(tempTriangles, 0);
        //设置网格的法线。
        tempMesh.SetNormals(tempNormals);
        //设置网格的 UV
        tempMesh.SetUVs(0, tempChannel0Uvs);
        tempMesh.SetUVs(1, tempChannel1Uvs);
        tempMesh.SetUVs(2, tempChannel2Uvs);
        //从顶点重新计算网格的包围体。
        tempMesh.RecalculateBounds();

        return tempMesh;
    }


    private void SaveMesh(Mesh mesh)
    {
        AssetDatabase.CreateAsset(mesh, "Assets/Learn/Unity API Learn/new Mesh.asset");
        AssetDatabase.Refresh();
    }
}
