using UnityEditor;
using UnityEngine;
namespace GUIFX
{
    [CustomEditor(typeof(FrameImage))]
    [CanEditMultipleObjects]
    public class FrameImageEditor : Editor
    {
        private const string IconPath = "Assets/GUI FX/UI Frame FX/Scripts/Editor/Loop.png";
        private static Texture2D iconTexture;

        [InitializeOnLoadMethod]
        private static void SetIcon()
        {
            iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
            if (iconTexture != null)
            {
                // »ñÈ¡ FrameImage µÄ MonoScript
                var scripts = MonoImporter.GetAllRuntimeMonoScripts();
                foreach (var script in scripts)
                {
                    if (script.GetClass() == typeof(FrameImage))
                    {
                        EditorGUIUtility.SetIconForObject(script, iconTexture);
                        break;
                    }
                }
            }
        }

        private SerializedProperty m_Sprite;
        private SerializedProperty m_SegmentCount;
        private SerializedProperty m_OutlineWidth;
        private SerializedProperty m_PreserveAspect;
        private SerializedProperty m_FillAmount;
        private SerializedProperty m_UVSpeed;

        private void OnEnable()
        {
            // Find properties
            m_Sprite = serializedObject.FindProperty("m_Sprite");
            m_SegmentCount = serializedObject.FindProperty("m_SegmentCount");
            m_OutlineWidth = serializedObject.FindProperty("m_OutlineWidth");
            m_PreserveAspect = serializedObject.FindProperty("m_PreserveAspect");
            m_FillAmount = serializedObject.FindProperty("m_FillAmount");
            m_UVSpeed = serializedObject.FindProperty("m_UVSpeed");
        }

        public override void OnInspectorGUI()
        {
            // Update the serializedObject
            serializedObject.Update();

            // Draw properties
            EditorGUILayout.PropertyField(m_Sprite);
            EditorGUILayout.PropertyField(m_SegmentCount);
            EditorGUILayout.PropertyField(m_OutlineWidth);
            EditorGUILayout.PropertyField(m_PreserveAspect);
            EditorGUILayout.PropertyField(m_FillAmount);
            EditorGUILayout.PropertyField(m_UVSpeed);

            // Draw remaining properties
            DrawPropertiesExcluding(serializedObject, "m_Script", "m_Sprite", "m_SegmentCount", "m_OutlineWidth", "m_PreserveAspect", "m_FillAmount", "m_UVSpeed");

            // Apply modifications
            serializedObject.ApplyModifiedProperties();
        }
    }
}