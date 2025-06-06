using ColossalFramework.UI;
using UnityEngine;

namespace SleepyCommon
{
    public class UIInfoLabel
    {
        public static readonly string kCursorInfoNormalColor = "<color #87d3ff>";
        public static readonly string kCursorInfoCloseColorTag = "</color>";

        private UILabel? m_infoLabel = null;
        private UIComponent? m_parent = null;
        private string m_text = string.Empty;

        // ----------------------------------------------------------------------------------------
        public UIInfoLabel(UIComponent parent)
        {
            m_parent = parent;
            m_infoLabel = UnityEngine.Object.Instantiate(ToolBase.cursorInfoLabel.gameObject, ToolBase.cursorInfoLabel.transform.parent).GetComponent<UILabel>();

            // Hook into parents events
            parent.eventVisibilityChanged += OnVisibilityChanged;
            parent.eventPositionChanged += OnPostitionChanged;

        }

        public string text
        {
            get
            {
                return m_text;
            }
            set
            {
                if (m_text != value) 
                {
                    m_text = value;

                    if (string.IsNullOrEmpty(m_text))
                    {
                        Hide();
                    }
                    else 
                    {
                        SetLabelText();
                    }
                }
            }
        }

        public void Show()
        {
            if (m_infoLabel is not null && 
                m_parent is not null && 
                !m_infoLabel.isVisible &&
                !string.IsNullOrEmpty(m_text))
            {
                SetLabelText();
                m_infoLabel.Show();
            }
        }

        public void Hide()
        {
            if (m_infoLabel is not null)
            {
                m_infoLabel.Hide();
            }
        }

        protected void OnVisibilityChanged(UIComponent component, bool bVisible)
        {
            if (bVisible)
            {
                PostitionInfoLabel();
            }
            else
            {
                if (m_infoLabel is not null)
                {
                    m_infoLabel.Hide();
                }
            }
        }

        private void SetLabelText()
        {
            if (m_infoLabel is not null)
            {
                m_infoLabel.text = kCursorInfoNormalColor + m_text + kCursorInfoCloseColorTag;
                PostitionInfoLabel();
            } 
        }

        protected void OnPostitionChanged(UIComponent component, Vector2 position)
        {
            PostitionInfoLabel();
        }

        private void PostitionInfoLabel()
        {
            if (m_infoLabel is not null && m_parent is not null)
            {
                // Display tooltip next to main panel
                UIView uiview = m_infoLabel.GetUIView();

                Vector2 vector = (ToolBase.fullscreenContainer is null) ? uiview.GetScreenResolution() : ToolBase.fullscreenContainer.size;

                float fScreenMiddle = vector.x * 0.5f;
                float fParentPanelMiddle = m_parent.relativePosition.x + (m_parent.width * 0.5f);

                Vector3 newRelativePosition;
                if (fParentPanelMiddle < fScreenMiddle)
                {
                    // To the right
                    newRelativePosition = m_parent.relativePosition + new Vector3(m_parent.width + 6, 0.0f);
                }
                else
                {
                    // To the left
                    newRelativePosition = m_parent.relativePosition - new Vector3(m_infoLabel.width + 6, 0.0f);
                }
                
                // Check its within screen
                if (newRelativePosition.x < 0f)
                {
                    newRelativePosition.x = 0f;
                }
                if (newRelativePosition.x + m_infoLabel.width > vector.x)
                {
                    newRelativePosition.x = vector.x - m_infoLabel.width;
                }

                m_infoLabel.relativePosition = newRelativePosition;
            }
        }
    }
}
