using UnityEngine;
using UnityEngine.UI;

public class Test2 : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var fileName = (transform.childCount + 1).ToString();
            var g = new GameObject(fileName, typeof(RawImage));
            g.transform.SetParent(transform);

            RefCommonLoader.LoadTextureByAssetTypeAndName(g, fileName, (t) =>
            {
                g.GetComponent<RawImage>().texture = t;
            });
        }
    }
}
