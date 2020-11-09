using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameKitEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform), true)]
    public class TransformInspector : Editor
    {
        protected static TransformInspector m_instance;

        public static TransformInspector Instance
        {
            get { return m_instance; }
        }

        protected Transform m_transform;
        protected SerializedProperty m_positionProperty;
        protected SerializedProperty m_rotationProperty;
        protected SerializedProperty m_scaleProperty;

        protected static Type m_transformRotationGUI; //TransformRotationGUI的类型

        /// <summary>
        /// 通过反射获取内部类'TransformRotationGUI'的类型。
        /// </summary>
        protected static Type TransformRotationGUI
        {
            get
            {
                if (m_transformRotationGUI == null)
                {
                    var assembly = Assembly.Load("UnityEditor");
                    m_transformRotationGUI = assembly.GetType("UnityEditor.TransformRotationGUI");
                }

                return m_transformRotationGUI;
            }
        }

        protected object m_rotationGUI; //TransformRotationGUI的实例

        protected static GUIContent m_positionContent;
        protected static GUIContent m_rotationContent;
        protected static GUIContent m_scaleContent;
        protected static GUIContent m_roundContent; //四舍五入到整数的百分之十。

        protected virtual void OnEnable()
        {
            m_instance = this;

            if (this)
            {
                try
                {
                    if (m_positionContent == null)
                    {
                        m_positionContent = EditorGUIUtility.TrTextContent("P", "Reset position of this GameObject relative to the parent.");
                    }

                    if (m_rotationContent == null)
                    {
                        m_rotationContent = EditorGUIUtility.TrTextContent("R", "Reset rotation of this GameObject relative to the parent.");
                    }

                    if (m_scaleContent == null)
                    {
                        m_scaleContent = EditorGUIUtility.TrTextContent("S", "Reset scale of this GameObject relative to the parent.");
                    }

                    if (m_roundContent == null)
                    {
                        m_roundContent = EditorGUIUtility.TrTextContent("≈", "Rounded to the nearest ten percent of an integer.");
                    }

                    m_transform = (Transform) target;
                    var so = serializedObject;
                    m_positionProperty = so.FindProperty("m_LocalPosition");
                    m_rotationProperty = so.FindProperty("m_LocalRotation");
                    m_scaleProperty = so.FindProperty("m_LocalScale");
                    m_rotationGUI = Activator.CreateInstance(TransformRotationGUI);
                    if (m_rotationGUI != null)
                    {
                        var onEnable = m_rotationGUI.GetType().GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.Public);
                        if (onEnable != null)
                        {
                            onEnable.Invoke(m_rotationGUI, new object[] {m_rotationProperty, GUIContent.none});
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        protected virtual void OnDisable()
        {
            m_instance = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            PositionControlsGUI();
            RotationControlsGUI();
            ScaleControlsGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void PositionControlsGUI()
        {
            EditorGUILayout.BeginHorizontal();
            var reset = GUILayout.Button(m_positionContent, GUILayout.Width(20f));
            var round = GUILayout.Button(m_roundContent, GUILayout.Width(20f));
            EditorGUILayout.PropertyField(m_positionProperty, GUIContent.none);

            if (reset)
            {
                m_positionProperty.vector3Value = Vector3.zero;
            }

            if (round)
            {
                var x = Mathf.RoundToInt(m_positionProperty.vector3Value.x * 10) * 0.1f;
                var y = Mathf.RoundToInt(m_positionProperty.vector3Value.y * 10) * 0.1f;
                var z = Mathf.RoundToInt(m_positionProperty.vector3Value.z * 10) * 0.1f;
                m_positionProperty.vector3Value = new Vector3(x, y, z);
            }

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void RotationControlsGUI()
        {
            EditorGUILayout.BeginHorizontal();
            var reset = GUILayout.Button(m_rotationContent, GUILayout.Width(20f));
            var round = GUILayout.Button(m_roundContent, GUILayout.Width(20f));

            if (m_rotationGUI != null)
            {
                var rotationField = m_rotationGUI.GetType().GetMethod("RotationField", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { }, null);
                if (rotationField != null)
                {
                    rotationField.Invoke(m_rotationGUI, null);
                }
            }

            if (reset)
            {
                m_rotationProperty.quaternionValue = Quaternion.identity;
            }

            if (round)
            {
                var eulerAngles = m_transform.localEulerAngles;
                var x = Mathf.RoundToInt(eulerAngles.x * 10) * 0.1f;
                var y = Mathf.RoundToInt(eulerAngles.y * 10) * 0.1f;
                var z = Mathf.RoundToInt(eulerAngles.z * 10) * 0.1f;
                m_rotationProperty.quaternionValue = Quaternion.Euler(x, y, z);
            }

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void ScaleControlsGUI()
        {
            EditorGUILayout.BeginHorizontal();
            var reset = GUILayout.Button(m_scaleContent, GUILayout.Width(20f));
            var round = GUILayout.Button(m_roundContent, GUILayout.Width(20f));

            EditorGUILayout.PropertyField(m_scaleProperty, GUIContent.none);

            if (reset)
            {
                m_scaleProperty.vector3Value = Vector3.one;
            }

            if (round)
            {
                var x = Mathf.RoundToInt(m_scaleProperty.vector3Value.x * 10) * 0.1f;
                var y = Mathf.RoundToInt(m_scaleProperty.vector3Value.y * 10) * 0.1f;
                var z = Mathf.RoundToInt(m_scaleProperty.vector3Value.z * 10) * 0.1f;
                m_scaleProperty.vector3Value = new Vector3(x, y, z);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}