using UnityEditor;
using UnityEngine;
namespace GUIFX
{
    [CustomEditor(typeof(FrameImageInOut))]
    [CanEditMultipleObjects] // 添加此行以支持多物体编辑
    public class FrameImageInOutEditor : Editor
    {
        private const string IconPath = "Assets/GUI FX/UI Frame FX/Scripts/Editor/InOut.png";
        private static Texture2D iconTexture;

        [InitializeOnLoadMethod]
        private static void SetIcon()
        {
            iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
            if (iconTexture != null)
            {
                // 获取 FrameImageInOut 的 MonoScript
                var scripts = MonoImporter.GetAllRuntimeMonoScripts();
                foreach (var script in scripts)
                {
                    if (script.GetClass() == typeof(FrameImageInOut))
                    {
                        EditorGUIUtility.SetIconForObject(script, iconTexture);
                        break;
                    }
                }
            }
        }

        public override void OnInspectorGUI()
        {
            // 绘制您的自定义属性
            serializedObject.Update();

            // 使用 SerializedProperty 来显示和编辑属性
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Sprite"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_SegmentCount"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_OutlineWidth"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PreserveAspect"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FillAmount"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_UVSpeed"));

            // 绘制继承的属性
            DrawPropertiesExcluding(serializedObject, "m_Script", "m_Sprite", "m_SegmentCount", "m_OutlineWidth", "m_PreserveAspect", "m_FillAmount", "m_UVSpeed");

            serializedObject.ApplyModifiedProperties();
        }
    }
}