using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
namespace GUIFX
{
    public class FrameImage : MaskableGraphic
    {
        [SerializeField]
        private Sprite m_Sprite;

        [SerializeField]
        private int m_SegmentCount = 6;

        [SerializeField]
        private float m_OutlineWidth = 10f;

        [SerializeField]
        private bool m_PreserveAspect = false;

        [Range(0, 1)]
        [SerializeField]
        private float m_FillAmount = 1f;

        [SerializeField]
        private float m_UVSpeed = 0.1f; // UV移动速度

        private float m_UVOffset = 0f;
        private Material m_InstancedMaterial;

        public float outlineWidth
        {
            get { return m_OutlineWidth; }
            set
            {
                if (m_OutlineWidth != value)
                {
                    m_OutlineWidth = value;
                    SetVerticesDirty();
                }
            }
        }

        public bool preserveAspect
        {
            get { return m_PreserveAspect; }
            set
            {
                if (m_PreserveAspect != value)
                {
                    m_PreserveAspect = value;
                    SetVerticesDirty();
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
                    SetVerticesDirty();
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
                    SetVerticesDirty();
                }
            }
        }

        public override Texture mainTexture
        {
            get
            {
                if (m_Sprite == null)
                {
                    if (m_InstancedMaterial != null && m_InstancedMaterial.mainTexture != null)
                    {
                        return m_InstancedMaterial.mainTexture;
                    }
                    return s_WhiteTexture;
                }

                return m_Sprite.texture;
            }
        }

        public Sprite sprite
        {
            get { return m_Sprite; }
            set
            {
                if (m_Sprite != value)
                {
                    m_Sprite = value;
                    SetVerticesDirty();
                    SetMaterialDirty();
                }
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // 实例化一个新的材质
            if (m_InstancedMaterial == null)
            {
                m_InstancedMaterial = new Material(Shader.Find("UI/Default"));
            }

            // 设置材质
            this.material = m_InstancedMaterial;

            // 确保纹理的 Wrap Mode 为 Repeat
            if (m_InstancedMaterial.mainTexture != null)
            {
                m_InstancedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
            }
            if (m_Sprite != null && m_Sprite.texture != null)
            {
                // 修改纹理的 wrapMode 为 Repeat
                m_Sprite.texture.wrapMode = TextureWrapMode.Repeat;
                m_InstancedMaterial.mainTexture = m_Sprite.texture;
            }

            InvokeRepeating("UpdateUVOffset", 0f, 0.03f); // 每0.03秒更新一次，约33FPS
        }

        protected override void OnDisable()
        {
            CancelInvoke("UpdateUVOffset");
            base.OnDisable();
        }

        private void UpdateUVOffset()
        {
#if UNITY_EDITOR
            // 确保纹理的 Wrap Mode 为 Repeat
            if (m_InstancedMaterial.mainTexture != null)
            {
                m_InstancedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
            }
            if (m_Sprite != null && m_Sprite.texture != null)
            {
                // 修改纹理的 wrapMode 为 Repeat
                m_Sprite.texture.wrapMode = TextureWrapMode.Repeat;
                m_InstancedMaterial.mainTexture = m_Sprite.texture;
            }
#endif

            // 更新UV偏移
            m_UVOffset += m_UVSpeed * Time.deltaTime;
            m_UVOffset %= 1f;  // 保证m_UVOffset始终在[0,1]范围内
            m_InstancedMaterial.mainTextureOffset = new Vector2(m_UVOffset, 0);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            if (m_Sprite == null)
            {
                return;
            }

            Rect rect = GetPixelAdjustedRect();
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

            Vector4 uv = DataUtility.GetOuterUV(m_Sprite);

            int totalVertCount = (m_SegmentCount + 1) * 2;
            UIVertex[] verts = new UIVertex[totalVertCount];

            for (int i = 0; i <= m_SegmentCount; i++)
            {
                float angle;

                if (m_SegmentCount == 4)
                {
                    // 当 m_SegmentCount == 4 时，确保顶点位于矩形的四个角
                    angle = (45f + i * 90f) * Mathf.Deg2Rad;
                }
                else
                {
                    // 其他情况下，按比例计算角度
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

                verts[i * 2] = CreateVertex(outerPos, new Vector2((float)i / m_SegmentCount, 0));
                verts[i * 2 + 1] = CreateVertex(innerPos, new Vector2((float)i / m_SegmentCount, 1));
            }

            for (int i = 0; i < verts.Length; i++)
            {
                vh.AddVert(verts[i]);
            }

            int fillVertCount = Mathf.CeilToInt(m_FillAmount * totalVertCount);
            fillVertCount = fillVertCount + (fillVertCount % 2); // Ensure even number

            for (int i = 0; i < fillVertCount - 2; i += 2)
            {
                vh.AddTriangle(i, i + 2, i + 1);
                vh.AddTriangle(i + 1, i + 2, i + 3);
            }
        }

        private UIVertex CreateVertex(Vector2 position, Vector2 uv)
        {
            Vector2 uvSprite = new Vector2(
                Mathf.Lerp(m_Sprite.textureRect.xMin, m_Sprite.textureRect.xMax, uv.x) / m_Sprite.texture.width,
                Mathf.Lerp(m_Sprite.textureRect.yMin, m_Sprite.textureRect.yMax, uv.y) / m_Sprite.texture.height
            );

            UIVertex vert = UIVertex.simpleVert;
            vert.position = position;
            vert.color = color;
            vert.uv0 = uvSprite;
            return vert;
        }

        protected override void OnDestroy()
        {
            // 清理实例化的材质
            if (m_InstancedMaterial != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(m_InstancedMaterial);
                }
                else
                {
                    DestroyImmediate(m_InstancedMaterial);
                }
            }
            base.OnDestroy();
        }

    }
}