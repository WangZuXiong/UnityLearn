using UnityEngine;
using UnityEngine.Serialization;

namespace foo
{
    public sealed class AssetRefComponent : MonoBehaviour
    {
        /// <summary>
        /// 是否已经通过 SetAsset 设置过资源，如果此值为 true Awake 的时候不应该增加引用计数
        /// </summary>
        private bool m_AssetSetted = false;
#if UNITY_EDITOR
        private string m_debugNodePath;
#endif
        public string AssetPath;
        /// <summary>
        /// 这里用 System.Type 不能用 Instantiate 正确实例化
        /// </summary>
        public string AssetTypeName;

        public int RecycledCount { get; private set; } = 0;

        public float LastRecycledTime { get; private set; } = 0.0f;

        public string PropertyName;

        public bool HaveRelease { get; set; } = false;

        private void Awake()
        {
            hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;

            if (!m_AssetSetted)
            {
                // 通过 Instantiate 实例化的时候保证引用计数正确
                if (!string.IsNullOrEmpty(AssetPath))
                {
                    GResources.AddReferenceCount(AssetPath);
                }
            }
        }

        /// <summary>
        /// OnDestroy 在对象没有被激活的情况下不会被调用，具体见文档或自行测试
        /// 所以这里的 RemoveAsset 有可能不会被调用到，这种情况下
        /// 外部最好在 Destroy 调用之前,调用 ManualRelease
        /// </summary>
        private void OnDestroy()
        {
            RemoveAsset();
        }

#if UNITY_EDITOR
        /// <summary>
        /// 加个保险
        /// </summary>
        ~AssetRefComponent()
        {
            //if (!string.IsNullOrEmpty(AssetPath))
            //{
            //    Debug.LogError("AssetRefComponent : " + m_debugNodePath + " : 没有正确释放资源引用计数 : " + AssetPath);
            //}
        }
#endif

        /// <summary>
        /// 设置临时资源的移除时间，只有propertyName不为空才可以
        /// </summary>
        /// <param name="duraTime"></param>
        public void SetOneShotTime(float duraTime)
        {
            if (duraTime < 0.0f)
            {
                CancelInvoke("OneShotRemoveAsset");
                return;
            }
            if (!string.IsNullOrEmpty(PropertyName))
            {
                Invoke(nameof(OneShotRemoveAsset), duraTime * Time.timeScale);
            }
        }

        public void AddRecycledCount()
        {
            RecycledCount += 1;
            LastRecycledTime = Time.realtimeSinceStartup;
        }

        private void SetAsset(string path, System.Type type, string propertyName)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            m_AssetSetted = true;
            AssetPath = path;
            AssetTypeName = type.Name;
            PropertyName = propertyName;
            GResources.AddReferenceCount(AssetPath);

            SetOneShotTime(-1.0f);

#if UNITY_EDITOR
            m_debugNodePath = transform.GetNodePath();
#endif      
        }

        private void RemoveAsset()
        {
            AssetTypeName = string.Empty;
            PropertyName = string.Empty;
            if (!string.IsNullOrEmpty(AssetPath))
            {
                GResources.DecReferenceCount(AssetPath);
            }
            AssetPath = string.Empty;
        }

        private void OneShotRemoveAsset()
        {
            RemoveAsset();
            Destroy(this);
        }

        public static void Release(GameObject go)
        {
            if (null == go)
            {
                return;
            }
            AssetRefComponent[] refs = go.GetComponentsInChildren<AssetRefComponent>(true);
            for (int i = 0; i < refs.Length; ++i)
            {
                refs[i].RemoveAsset();
                Destroy(refs[i]);
            }

        }

        public static AssetRefComponent Assign(GameObject go, System.Type assetType, string assetPath)
        {
            return Assign(go, assetType, assetPath, "");
        }

        public static AssetRefComponent Assign(GameObject go, System.Type assetType, string assetPath, string propertyName)
        {
            if (null == go || null == assetType)
            {
                return null;
            }
#if UNITY_EDITOR
            if (null == propertyName)
            {
                Debug.LogError("[EditorOnly] propertyName can't is null");
            }
#endif
            AssetRefComponent tag = Get(go, assetType, propertyName);
            if (null == tag)
            {
                if (string.IsNullOrEmpty(assetPath))
                {
                    return null;
                }
                tag = go.AddComponent<AssetRefComponent>();
            }
            tag.RemoveAsset();
            tag.HaveRelease = false;
            tag.SetAsset(assetPath, assetType, propertyName);
            return tag;
        }

        public static AssetRefComponent Get(GameObject go, System.Type assetType)
        {
            return Get(go, assetType, "");
        }

        public static AssetRefComponent Get(GameObject go, System.Type assetType, string propertyName)
        {
            if (null == go || null == assetType)
            {
                return null;
            }
#if UNITY_EDITOR
            if (null == propertyName)
            {
                Debug.LogError("[EditorOnly] propertyName can't is null");
            }
#endif
            AssetRefComponent[] tags = go.GetComponents<AssetRefComponent>();
            for (int i = 0; i < tags.Length; ++i)
            {
                AssetRefComponent tag = tags[i];
                if (tag.AssetTypeName == assetType.Name && tag.PropertyName == propertyName)
                {
                    return tag;
                }
            }
            return null;
        }

        public static AssetRefComponent GetInParent(Transform node, System.Type assetType)
        {
            if (null == node || null == assetType)
            {
                return null;
            }
            do
            {
                AssetRefComponent[] tags = node.GetComponents<AssetRefComponent>();
                for (int i = 0; i < tags.Length; ++i)
                {
                    AssetRefComponent tag = tags[i];
                    if (tag.AssetTypeName == assetType.Name)
                    {
                        return tag;
                    }
                }
                node = node.parent;

            } while (null != node);

            return null;
        }
    }
}


