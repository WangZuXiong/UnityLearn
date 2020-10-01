using UnityEngine;

namespace foo
{
    public class AssetSetter : MonoBehaviour
    {
        private GameObject _requestImage = null;
        private string _requestAssetPath;
        private AssetLoadCallback _requestCallback = null;

        public static AssetSetter Get(GameObject go)
        {
            return Extends.AttachUniqueComponent<AssetSetter>(go);
        }

        public static void SetImageSetter(GameObject obj, string url, string savePath, string fileName, AssetLoadCallback callback, bool cache, AssetLoadCallback erroBack = null, bool textureCanNull = false, bool isPreload = false)
        {
            if (obj == null)
            {
                return;
            }

            AssetSetter setter = Get(obj.gameObject);
            setter._requestImage = obj;
            setter._requestAssetPath = url;
            setter._requestCallback = callback;

            GResources.LoadAsync(url, savePath, fileName, OnImageSpriteLoaded, setter, cache, textureCanNull, erroBack, isPreload);
        }

        private static void OnImageSpriteLoaded(string assetPath, Object asset, object userData)
        {
            AssetSetter setter = userData as AssetSetter;

            if (null == setter || null == setter.gameObject || null == setter._requestImage)
            {
#if UNITY_EDITOR
                Debug.LogError("[EditorOnly] load sprite error , path := " + assetPath);
#endif
                return;
            }
            //保证异步加载取最后的那次的正确的值
            if (assetPath != setter._requestAssetPath)
            {
                return;
            }
            AssetRefComponent.Assign(setter.gameObject, typeof(Sprite), assetPath);
            if (null != setter._requestCallback)
            {
                setter._requestCallback.Invoke(assetPath, asset, userData);
            }
        }
    }
}

