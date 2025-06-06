﻿using ColossalFramework.UI;
using System;
using UnityEngine;

namespace SleepyCommon
{
	public class SettingsSlider
    {
		const int iSLIDER_PANEL_HEIGHT = 34;

		public UIPanel? m_panel = null;
		public UILabel? m_label = null;
		public int m_iDecimalPlaces = 0;

		private UISlider? m_slider = null;
		private string m_sText = "";
		private float m_fValue;
		private ICities.OnValueChanged? m_eventCallback = null;
		private bool m_bPercent = false;

        public float height
        {
            get
            {
				if (m_panel is not null)
				{
                    return m_panel.height;
                }
				return 0.0f;
            }
            set
            {
                if (m_panel is not null)
                {
                    m_panel.height = value;
                }
            }
        }

        public bool Percent
		{
			get 
			{ 
				return m_bPercent; 
			}
			set
			{
				m_bPercent = value;
                UpdateLabel();
			}
		}

		// Uses the default settings font
        public static SettingsSlider CreateSettingsStyle(UIHelper helper, LayoutDirection direction, string sText, int iLabelWidth, int iSliderWidth, float fMin, float fMax, float fStep, float fDefault, int iDecimalPlaces, ICities.OnValueChanged eventCallback)
        {
            UIFont defaultFont = UIFonts.Regular;
			defaultFont.size = 16;
            return Create((UIPanel)helper.self, direction, sText, defaultFont, 1.125f, iLabelWidth, iSliderWidth, fMin, fMax, fStep, fDefault, iDecimalPlaces, eventCallback);
        }

        public static SettingsSlider Create(UIHelper helper, LayoutDirection direction, string sText, UIFont font, float fTextScale, int iLabelWidth, int iSliderWidth, float fMin, float fMax, float fStep, float fDefault, int iDecimalPlaces, ICities.OnValueChanged eventCallback)
		{
            return Create((UIPanel)helper.self, direction, sText, font, fTextScale, iLabelWidth, iSliderWidth, fMin, fMax, fStep, fDefault, iDecimalPlaces, eventCallback);
        }

        public static SettingsSlider Create(UIPanel parent, LayoutDirection direction, string sText, UIFont font, float fTextScale, int iLabelWidth, int iSliderWidth, float fMin, float fMax, float fStep, float fDefault, int iDecimalPlaces, ICities.OnValueChanged eventCallback)
		{
			SettingsSlider oSlider = new SettingsSlider();
			oSlider.m_fValue = fDefault;
			oSlider.m_eventCallback = eventCallback;
            oSlider.m_iDecimalPlaces = iDecimalPlaces;
            oSlider.m_sText = sText;

            // Panel
            oSlider.m_panel = parent.AddUIComponent<UIPanel>();
			oSlider.m_panel.autoLayout = true;
			oSlider.m_panel.autoLayoutDirection = direction;
            oSlider.m_panel.padding.top = 6;
            oSlider.m_panel.autoSize = false;
			oSlider.m_panel.width = iLabelWidth + iSliderWidth + 10;
			oSlider.m_panel.height = (direction == LayoutDirection.Horizontal) ? iSLIDER_PANEL_HEIGHT : 2 * iSLIDER_PANEL_HEIGHT;
			//oSlider.m_panel.backgroundSprite = "InfoviewPanel";
			//oSlider.m_panel.color = Color.red;

			// Label
			oSlider.m_label = oSlider.m_panel.AddUIComponent<UILabel>();
			if (oSlider.m_label is not null)
			{
				oSlider.m_label.autoSize = false;
				oSlider.m_label.width = (direction == LayoutDirection.Vertical) ? oSlider.m_panel.width : iLabelWidth;
				oSlider.m_label.height = 28;
				oSlider.m_label.text = oSlider.m_sText + ": " + fDefault;
				oSlider.m_label.font = font;
                oSlider.m_label.textScale = fTextScale;
				//oSlider.m_label.backgroundSprite = "InfoviewPanel";
				//oSlider.m_label.color = Color.blue;
			}

			// Slider
			oSlider.m_slider = CreateSlider(oSlider.m_panel, (direction == LayoutDirection.Vertical) ? oSlider.m_panel.width : iSliderWidth, 30, fMin, fMax);
			oSlider.m_slider.value = (float) Math.Round(fDefault, oSlider.m_iDecimalPlaces);
			oSlider.m_slider.stepSize = fStep;
            oSlider.m_slider.eventValueChanged += delegate (UIComponent c, float val)
			{
				oSlider.OnSliderValueChanged(val);
			};

			// Replace slider with a new one
			GameObject.Destroy(oSlider.m_slider.thumbObject.gameObject);
			UISprite thumb = oSlider.m_slider.AddUIComponent<UISprite>();
			thumb.size = new Vector2(16, 16);
			thumb.position = new Vector2(0, 0);
			thumb.spriteName = "InfoIconBaseHovered";
			oSlider.m_slider.thumbObject = thumb;

            oSlider.UpdateLabel();

            return oSlider;
		}

		public static UISlider CreateSlider(UIPanel parent, float width, float height, float min, float max)
		{
			UIPanel bg = parent.AddUIComponent<UIPanel>();
			bg.name = "sliderPanel";
			bg.autoLayout = false;
			bg.padding = new RectOffset(0, 0, 10, 0);
			bg.size = new Vector2(width, height);
			//bg.backgroundSprite = "InfoviewPanel";
			//bg.color = Color.green;

			UISlider slider = bg.AddUIComponent<UISlider>();
			slider.autoSize = false;
			slider.area = new Vector4(8, 0, bg.width, 15);
			slider.width = bg.width - 10;
			slider.height = 1;
			slider.relativePosition = new Vector3(8, slider.relativePosition.y + 10);
			slider.backgroundSprite = "SubBarButtonBasePressed";
			slider.fillPadding = new RectOffset(6, 6, 0, 0);
			slider.maxValue = max;
			slider.minValue = min;

			UISprite thumb = slider.AddUIComponent<UISprite>();
			thumb.size = new Vector2(16, 16);
			thumb.position = new Vector2(0, 0);
			thumb.spriteName = "InfoIconBaseHovered";

			slider.value = 0.0f;
			slider.thumbObject = thumb;

			return slider;
		}

		public void SetTooltip(string sTooltip)
		{
			if (m_slider is not null)
			{
				m_slider.tooltip = sTooltip;
			}
		}

		public void OnSliderValueChanged(float fValue)
        {
			m_fValue = fValue;
			UpdateLabel();
			if (m_eventCallback is not null)
            {
				m_eventCallback(fValue);
			}
		}

		private void UpdateLabel()
		{
            if (m_label is not null)
            {

                m_label.text = $"{m_sText}: {m_fValue.ToString($"N{m_iDecimalPlaces}")}{(Percent ? "%" : "")}";
            }
        }

		public void Enable(bool bEnable)
		{
			if (m_label is not null)
            {
				m_label.isEnabled = bEnable;
				m_label.disabledTextColor = Color.grey;
			}
			if (m_slider is not null)
            {
				m_slider.isEnabled = bEnable;
			}
		}

		public void SetValue(float fValue)
        {
			if (m_slider is not null)
			{
				m_slider.value = fValue;
			}
		}

		public void Destroy()
        {
			if (m_label is not null)
            {
				UnityEngine.Object.Destroy(m_label.gameObject);
				m_label = null;

			}
			if (m_slider is not null)
			{
				UnityEngine.Object.Destroy(m_slider.gameObject);
				m_slider = null;
			}
		}
	}

    
}
