using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Lab : MonoBehaviour
{
    void FindAPITest()
    {
        //temp activate
        //var temp = GameObject.Find("Temp");
        //Debug.LogError(temp.name);//Temp

        //temp inactivate
        //var temp = GameObject.Find("Temp");
        //Debug.LogError(temp.name);//NullReferenceException: Object reference not set to an instance of an object

        //temp activate
        //var temp = transform.Find("Temp");
        //Debug.LogError(temp.name);//Temp

        //temp inactivate
        var temp = transform.Find("Temp");
        Debug.LogError(temp.name);//Temp
    }

    /// <summary>
    /// 坐标
    /// </summary>
    private void InverseTransformPointAPITest()
    {
        var cube = transform.Find("Cube");
        var sphere = transform.Find("Sphere");


        Debug.Log(cube.position);
        Debug.Log(sphere.position);

        Debug.Log(cube.InverseTransformPoint(sphere.position));
        Debug.Log(sphere.InverseTransformPoint(cube.position));
    }

    private void RenderMaterialTest()
    {
        GetComponent<Renderer>().material.color = Color.white * UnityEngine.Random.Range(0, 1f);
    }

    private void RenderShareMaterialTest()
    {
        GetComponent<Renderer>().sharedMaterial.color = Color.white * UnityEngine.Random.Range(0, 1f);
    }

    /// <summary>
    /// 合并mesh
    /// </summary>
    private void CombineMesh()
    {
        var meshFilters = GetComponentsInChildren<MeshFilter>();
        //结构，用于描述将使用 Mesh.CombineMeshes 组合的网格。
        var combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;//返回网格过滤器的共享网格。
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;//localToWorldMatrix
            meshFilters[i].gameObject.SetActive(false);
        }

        var meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combine);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 合并mesh
    /// </summary>
    private void CombineMesh1()
    {
        var meshFilters = GetComponentsInChildren<MeshFilter>();
        var combine = new CombineInstance[meshFilters.Length];

        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        var materials = new Material[meshRenderers.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            materials[i] = meshRenderers[i].sharedMaterial;
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        var meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combine, false);
        gameObject.AddComponent<MeshRenderer>().sharedMaterials = materials;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 屏幕截图
    /// </summary>
    private void RenderTextureLab()
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
        renderTexture.filterMode = FilterMode.Bilinear;
        RenderTexture.active = renderTexture;
        Camera camera = Camera.main;
        camera.targetTexture = renderTexture;
        camera.Render();
        RawImage rawImage = transform.Find("RawImage").GetComponent<RawImage>();
        rawImage.texture = renderTexture;
        RenderTexture.active = null;
        camera.targetTexture = null;
        //RenderTexture.GetTemporary这个api要和RenderTexture.ReleaseTemporary 配套使用否则会内存泄漏
        //RenderTexture.ReleaseTemporary(renderTexture);
    }

}
