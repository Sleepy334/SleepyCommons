using ColossalFramework.UI;
using UnityEngine;

namespace SleepyCommon
{
    internal class UIConstantFontTooltip : GUIWindow
    {
        private string m_tooltip = "";
        private int m_fontSize = 14;

        private TextGenerator? m_generator = null;
        private TextGenerationSettings? m_textSettings = null;
        private static Font? s_ConstantWidthFont = null;

        // ----------------------------------------------------------------------------------------
        public int FontSize
        {
            get
            {
                return m_fontSize;
            }
            set
            {
                m_fontSize = value;
            }
        }
        
        private UIConstantFontTooltip()
            : base("UIConstantFontTooltip", new Rect(16.0f, 16.0f, 100f, 100f), false, false)
        {
        }

        public static UIConstantFontTooltip Create(UIComponent parent, string tooltip, int fontSize, TextAnchor textAnchor)
        {
            var go = new GameObject("UIConstantFontTooltip");
            go.transform.parent = parent.transform;

            UIConstantFontTooltip viewer = go.AddComponent<UIConstantFontTooltip>();
            viewer.FontSize = fontSize;
            Vector2 mousePos = UIScaler.MousePosition;
            viewer.SetTooltipTextAnchor(textAnchor);
            viewer.MoveResize(new Rect(mousePos.x, mousePos.y, 400, 200));
            viewer.m_tooltip = tooltip;

            return viewer;
        }

        public void SetTooltip(string sTooltip)
        {
            m_tooltip = sTooltip;

            if (Visible)
            {
                Vector2 oSize = GetPreferredSize();
                Vector2 mousePos = UIScaler.MousePosition;
                Rect oTooltipPos = new Rect(mousePos.x + 20, mousePos.y - oSize.y - 20, oSize.x, oSize.y);
                MoveResize(oTooltipPos);
                Visible = true;
            }
        }

        private Vector2 GetPreferredSize()
        {
            GetCharacterWidthAndHeight(out float fCharacterWidth, out float fCharacterheight);

            int iLines;
            string sLine = Utils.GetLongestLine(m_tooltip, out iLines);
            int iMaxLength = sLine.Length;
            float fWidth = iMaxLength * fCharacterWidth + 20;
            float fHeight = iLines * fCharacterheight + 20;

            return new Vector2(fWidth, fHeight);
        }

        public void Show()
        {
            if (!Visible)
            {
                Vector2 oSize = GetPreferredSize();
                Vector2 mousePos = UIScaler.MousePosition;
                Rect oTooltipPos = new Rect(mousePos.x + 20, mousePos.y - oSize.y - 20, oSize.x, oSize.y);
                MoveResize(oTooltipPos);
                Visible = true;
            }
        }

        protected override void DrawWindow()
        {
            GUILayout.BeginVertical();
            GUILayout.Label(m_tooltip, GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
        }

        private void GetCharacterWidthAndHeight(out float fCharacterWidth, out float fCharacterheight)
        {
            if (m_textSettings == null)
            {
                TextGenerationSettings settings = new TextGenerationSettings();
                settings.generationExtents = Vector2.zero;
                settings.textAnchor = m_tooltipTextAnchor;
                settings.alignByGeometry = false;
                settings.scaleFactor = 1.0f;
                settings.color = Color.red;
                settings.font = GetConstantWidthFont(FontSize);// m_skin.font;
                settings.fontSize = FontSize;
                settings.fontStyle = FontStyle.Bold;
                settings.pivot = Vector2.zero;
                settings.richText = false;
                settings.lineSpacing = 1.0f;
                settings.resizeTextForBestFit = false;
                settings.updateBounds = false;
                settings.horizontalOverflow = HorizontalWrapMode.Overflow;
                settings.verticalOverflow = VerticalWrapMode.Overflow;

                m_textSettings = settings;
            }

            // Generate font to get font width and height
            if (m_generator == null)
            {
                m_generator = new TextGenerator();
            }

            fCharacterWidth = m_generator.GetPreferredWidth("M", m_textSettings.Value);
            fCharacterheight = m_generator.GetPreferredHeight("Mg", m_textSettings.Value) * 1.3f; // leading scaling
        }

        public override void UpdateFont()
        {
            if (m_skin != null)
            {
                m_skin.font = GetConstantWidthFont(FontSize);
            }
        }

        private static Font GetConstantWidthFont(int fontSize)
        {
            if (s_ConstantWidthFont == null)
            {
                s_ConstantWidthFont = Font.CreateDynamicFontFromOSFont("Courier New Bold", fontSize);
            }
            return s_ConstantWidthFont;
        }
    }
}
