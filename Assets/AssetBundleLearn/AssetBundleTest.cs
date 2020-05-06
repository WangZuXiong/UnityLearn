using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetBundleTest : MonoBehaviour
{
    [SerializeField]
    private string _name1 = "cube";
    [SerializeField]
    private string _name2 = "cube1";
    [SerializeField]
    private string _path1 = "AssetBundleLearn/AssetBundles/cube";
    [SerializeField]
    private string _path2 = "AssetBundleLearn/AssetBundles/cube1";
    [SerializeField]
    private Transform _root;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Renderer _cube;

    private void Start()
    {
        _path1 = Application.dataPath + "/" + _path1;
        _path2 = Application.dataPath + "/" + _path2;

        //LoadAssetBundle.Instance.LoadAssetBundleAsync<GameObject>(_name1, _path1, (t) =>
        //{
        //    Instantiate(t, _root);
        //});


        //LoadAssetBundle.Instance.LoadAssetBundleAsync<Texture2D>(_name2, _path2, (t) =>
        //{
        //    _texture2D = t;
        //    _sprite = Sprite.Create(t, new Rect(Vector2.zero, new Vector2(t.width, t.height)), Vector2.zero);
        //    _image.sprite = _sprite;
        //});


        //LoadAssetBundle.Instance.LoadAssetBundleAsync<Texture2D>(_name1, _path1, (tx) =>
        //{
        //    LoadAssetBundle.Instance.LoadAssetBundleAsync<Material>(_name2, _path2, (t) =>
        //    {
        //        _cube.material = t;
        //    });
        //});


        var path = "http://192.168.1.243:8082/basketball/theme_activity/configure/Android/1";
        LoadAssetBundle.Instance.LoadAssetBundleAsync(path, (t) =>
        {
            var original = t.LoadAsset<GameObject>("1");
            Instantiate(original, GameObject.Find("Canvas").transform);
            //t.transform.SetParent(GameObject.Find("Canvas").transform);
            t.Unload(false);


            //转化为二进制

            //写入本地
        });

    }
    private Texture2D _texture2D;
    private Sprite _sprite;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _image.sprite = null;
            Destroy(_sprite);
            _sprite = null;

            //AssetBundle.UnloadAllAssetBundles(true);


            DestroyImmediate(_texture2D, true);
            _texture2D = null;

        }
    }
}
