using ColossalFramework.UI;

namespace SleepyCommon
{
    public class UIToggleButton : UIButton
    {
        bool m_bStateOn = true;
        private KnownColor m_onColor = KnownColor.lightBlue;
        private KnownColor m_offColor = KnownColor.grey;

        public bool StateOn
        {
            get 
            { 
                return m_bStateOn; 
            }
            set 
            {
                m_bStateOn = value;
                UpdateButton();
            }
        }

        public KnownColor onColor
        {
            get
            {
                return m_onColor;
            }
            set
            {
                m_onColor = value;
                UpdateButton();
            }
        }

        public KnownColor offColor
        {
            get
            {
                return m_offColor;
            }
            set
            {
                m_offColor = value;
                UpdateButton();
            }
        }

        public override void Start()
        {
            UpdateButton();
            base.Start();
        }

        public void Toggle()
        {
            m_bStateOn = !m_bStateOn; 
            UpdateButton();
        }

        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            Toggle();
            base.OnMouseDown(p);
        }

        private void UpdateButton()
        {
            if (m_bStateOn)
            {
                color = m_onColor;
                hoveredColor = m_onColor;
                pressedColor = m_onColor;
                focusedColor = m_onColor;
            }
            else
            {
                color = m_offColor;
                hoveredColor = m_offColor;
                pressedColor = m_offColor;
                focusedColor = m_offColor;
            }
        }
    }
}
