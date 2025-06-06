using ColossalFramework.UI;
using UnityEngine;

namespace SleepyCommon
{
    public class UILabelLiveTooltip : UITruncateLabel
    {
        private UIConstantFontTooltip? m_tooltipWindow = null;
        private TextAnchor m_tooltipTextAnchor = TextAnchor.MiddleCenter;
        private int m_fontSize = 14;

        // ----------------------------------------------------------------------------------------
        public int TooltipFontSize
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

        // ----------------------------------------------------------------------------------------
        public UILabelLiveTooltip()
        {
            eventTooltipTextChanged += OnTooltipTextChanged;
        }

        protected void OnTooltipTextChanged(UIComponent component, string text)
        {
            if (m_tooltipWindow != null)
            {
                if (text.Length > 0)
                {
                    m_tooltipWindow.SetTooltip(tooltip);
                }
                else
                {
                    HideTooltip();
                }
            }
        }

        public bool IsTooltipVisible()
        {
            return m_tooltipWindow != null && m_tooltipWindow.Visible;
        }

        public void ShowTooltip()
        {
            if (tooltip != "" && m_tooltipWindow is null)
            {
                m_tooltipWindow = UIConstantFontTooltip.Create(this, tooltip, TooltipFontSize, m_tooltipTextAnchor);
            }
        }

        public void HideTooltip()
        {
            if (m_tooltipWindow != null)
            {
                m_tooltipWindow.Visible = false;
                Destroy(m_tooltipWindow);
                m_tooltipWindow = null;
            }
        }

        public void SetTooltipTextAnchor(TextAnchor textAnchor)
        {
            m_tooltipTextAnchor = textAnchor;
        }

        protected override void OnTooltipHover(UIMouseEventParameter p)
        {
            if (m_tooltipWindow != null && !IsTooltipVisible())
            {
                if (Time.realtimeSinceStartup - m_HoveringStartTime > 0.4f)
                { 
                    m_tooltipWindow.SetTooltip(tooltip);
                    m_tooltipWindow.Show();
                }
            }
        }

        protected override void OnTooltipEnter(UIMouseEventParameter p)
        {
            base.OnTooltipEnter(p);

            if (tooltipBox != null)
            {
                tooltipBox.isVisible = false;
            }
            
            ShowTooltip();
        }

        protected override void OnTooltipLeave(UIMouseEventParameter p)
        {
            HideTooltip();
            base.OnTooltipLeave(p);
        }
    }
}
