using System.Collections.Generic;
using UnityEngine;

namespace GUIFX
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class FrameSpriteInOut : MonoBehaviour
    {
        [SerializeField]
        private Sprite m_Sprite;

        [SerializeField]
        private int m_SegmentCount = 6;

        [SerializeField]
        private float m_OutlineWidth = 0.1f; // World units

        [SerializeField]
        private bool m_PreserveAspect = false;

        [Range(0, 1)]
        [SerializeField]
        private float m_FillAmount = 1f;

        [SerializeField]
        private float m_UVSpeed = 0.1f;

        [SerializeField]
        private Color m_Color = Color.white;

        private float m_UVOffset = 0f;
        private MeshFilter m_MeshFilter;
        private MeshRenderer m_MeshRenderer;
        private Mesh m_Mesh;
        
        private List<Vector3> m_Vertices = new List<Vector3>();
        private List<Color> m_Colors = new List<Color>();
        private List<Vector2> m_UVs = new List<Vector2>();
        private List<int> m_Triangles = new List<int>();

        public Sprite sprite
        {
            get { return m_Sprite; }
            set
            {
                if (m_Sprite != value)
                {
                    m_Sprite = value;
                    RebuildMesh();
                    UpdateMaterial();
                }
            }
        }

        public float outlineWidth
        {
            get { return m_OutlineWidth; }
            set
            {
                if (m_OutlineWidth != value)
                {
                    m_OutlineWidth = value;
                    RebuildMesh();
                }
            }
        }

        public float fillAmount
        {
            get { return m_FillAmount; }
            set
            {
                if (m_FillAmount != value)
                {
                    m_FillAmount = Mathf.Clamp01(value);
                    RebuildMesh();
                }
            }
        }

        public int segmentCount
        {
            get { return m_SegmentCount; }
            set
            {
                if (m_SegmentCount != value)
                {
                    m_SegmentCount = Mathf.Max(3, value);
                    RebuildMesh();
                }
            }
        }

        public Color color
        {
            get { return m_Color; }
            set
            {
                if (m_Color != value)
                {
                    m_Color = value;
                    RebuildMesh();
                }
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_MeshFilter = GetComponent<MeshFilter>();
            m_MeshRenderer = GetComponent<MeshRenderer>();
            
            if (m_MeshFilter.sharedMesh != null)
            {
                if (m_MeshFilter.sharedMesh.name != "FrameSpriteInOutMesh")
                {
                    m_Mesh = new Mesh();
                    m_Mesh.name = "FrameSpriteInOutMesh";
                    m_MeshFilter.mesh = m_Mesh;
                }
                else
                {
                    m_Mesh = m_MeshFilter.sharedMesh;
                    // Ensure readability
                    if (!m_Mesh.isReadable) 
                    {
                        m_Mesh = new Mesh(); 
                        m_Mesh.name = "FrameSpriteInOutMesh";
                        m_MeshFilter.mesh = m_Mesh; 
                    }
                }
            }
            else
            {
                m_Mesh = new Mesh();
                m_Mesh.name = "FrameSpriteInOutMesh";
                m_MeshFilter.mesh = m_Mesh;
            }
            
            m_Mesh.MarkDynamic();
        }

        private void OnEnable()
        {
            Initialize();
            UpdateMaterial();
            RebuildMesh();
            
            InvokeRepeating("UpdateUVOffset", 0f, 0.03f);
        }

        private void OnDisable()
        {
            CancelInvoke("UpdateUVOffset");
        }

        private void OnValidate()
        {
            if (m_MeshFilter == null) m_MeshFilter = GetComponent<MeshFilter>();
            if (m_MeshRenderer == null) m_MeshRenderer = GetComponent<MeshRenderer>();
            RebuildMesh();
            UpdateMaterial();
        }

        private void UpdateUVOffset()
        {
            if (m_Sprite == null || m_MeshRenderer == null) return;

#if UNITY_EDITOR
            UpdateMaterialWrapMode();
#endif

            m_UVOffset += m_UVSpeed * 0.03f;
            m_UVOffset = m_UVOffset > 1 ? -1 : m_UVOffset;
            
            // Rebuild mesh to update vertex UVs
            // Optimization: Could split Mesh Update to only SetUVs
            RebuildMesh();
        }

        private void UpdateMaterialWrapMode()
        {
            if (m_MeshRenderer == null) return;
            Material mat = Application.isPlaying ? m_MeshRenderer.material : m_MeshRenderer.sharedMaterial;
            
            if (mat != null && mat.mainTexture != null)
            {
                if (mat.mainTexture.wrapMode != TextureWrapMode.Clamp)
                    mat.mainTexture.wrapMode = TextureWrapMode.Clamp;
            }
            if (m_Sprite != null && m_Sprite.texture != null)
            {
                if (m_Sprite.texture.wrapMode != TextureWrapMode.Clamp)
                    m_Sprite.texture.wrapMode = TextureWrapMode.Clamp;
            }
        }

        private void UpdateMaterial()
        {
            if (m_MeshRenderer == null) return;
            
            Material mat = Application.isPlaying ? m_MeshRenderer.material : m_MeshRenderer.sharedMaterial;
            
            if (mat == null)
            {
                Shader shader = Shader.Find("Sprites/Default");
                if (shader != null)
                {
                    mat = new Material(shader);
                    m_MeshRenderer.material = mat;
                }
            }

            if (m_Sprite != null && mat != null)
            {
                mat.mainTexture = m_Sprite.texture;
                if (mat.mainTexture != null) mat.mainTexture.wrapMode = TextureWrapMode.Clamp;
            }
        }

        public void RebuildMesh()
        {
            if (m_Mesh == null) Initialize();
            m_Mesh.Clear();
            
            m_Vertices.Clear();
            m_Colors.Clear();
            m_UVs.Clear();
            m_Triangles.Clear();

            if (m_Sprite == null) return;

            Bounds b = m_Sprite.bounds;
            Rect rect = new Rect(b.min.x, b.min.y, b.size.x, b.size.y);

            Vector4 outer;
            if (m_SegmentCount == 4)
            {
                float dx = rect.width * (Mathf.Sqrt(2) - 1) * .5f;
                float dy = rect.height * (Mathf.Sqrt(2) - 1) * .5f;
                outer = new Vector4(rect.xMin - dx, rect.yMin - dy, rect.xMax + dx, rect.yMax + dy);
            }
            else
                outer = new Vector4(rect.xMin, rect.yMin, rect.xMax, rect.yMax);

            Vector4 inner = new Vector4(
                outer.x + m_OutlineWidth,
                outer.y + m_OutlineWidth,
                outer.z - m_OutlineWidth,
                outer.w - m_OutlineWidth
            );

            int totalVertCount = (m_SegmentCount + 1) * 2;

            for (int i = 0; i <= m_SegmentCount; i++)
            {
                float angle;
                if (m_SegmentCount == 4)
                {
                    angle = (45f + i * 90f) * Mathf.Deg2Rad;
                }
                else
                {
                    angle = ((float)i / m_SegmentCount * 360f + 90) * Mathf.Deg2Rad;
                }

                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);

                Vector2 outerPos = new Vector2(
                    Mathf.Lerp(outer.x, outer.z, (cos + 1) / 2),
                    Mathf.Lerp(outer.y, outer.w, (sin + 1) / 2)
                );
                Vector2 innerPos = new Vector2(
                    Mathf.Lerp(inner.x, inner.z, (cos + 1) / 2),
                    Mathf.Lerp(inner.y, inner.w, (sin + 1) / 2)
                );

                float uvX = (float)i / m_SegmentCount - 0.03f + m_UVOffset;

                AddVertex(outerPos, new Vector2(uvX, 0));
                AddVertex(innerPos, new Vector2(uvX, 1));
            }

            int fillVertCount = Mathf.CeilToInt(m_FillAmount * totalVertCount);
            fillVertCount = fillVertCount + (fillVertCount % 2); 

            for (int i = 0; i < fillVertCount - 2; i += 2)
            {
                m_Triangles.Add(i);
                m_Triangles.Add(i + 2);
                m_Triangles.Add(i + 1);

                m_Triangles.Add(i + 1);
                m_Triangles.Add(i + 2);
                m_Triangles.Add(i + 3);
            }

            m_Mesh.SetVertices(m_Vertices);
            m_Mesh.SetColors(m_Colors);
            m_Mesh.SetUVs(0, m_UVs);
            m_Mesh.SetTriangles(m_Triangles, 0);
            m_Mesh.RecalculateBounds();
        }

        private void AddVertex(Vector2 position, Vector2 uv)
        {
            m_Vertices.Add(position);
            m_Colors.Add(m_Color);

            if (m_Sprite != null && m_Sprite.texture != null)
            {
                Rect tr = m_Sprite.textureRect;
                float w = m_Sprite.texture.width;
                float h = m_Sprite.texture.height;

                Vector2 finalUV = new Vector2(
                    Mathf.Lerp(tr.xMin, tr.xMax, uv.x) / w,
                    Mathf.Lerp(tr.yMin, tr.yMax, uv.y) / h
                );
                m_UVs.Add(finalUV);
            }
            else
            {
                m_UVs.Add(uv);
            }
        }
    }
}
