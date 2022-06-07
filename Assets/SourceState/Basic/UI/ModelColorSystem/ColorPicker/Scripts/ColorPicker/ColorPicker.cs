using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ColorUiTools
{
    public class ColorPicker : UIBehaviour
    {
        [Serializable]
        public class ColorPickerEvent : UnityEvent<Color> { }

        [SerializeField]
        private ColorPickerEvent m_onPicker = new ColorPickerEvent();
        public ColorPickerEvent onPicker
        {
            get { return m_onPicker; }
            set { m_onPicker = value; }
        }

        [SerializeField]
        private Color m_Color = default;
        public Color Color
        {
            get { return m_Color; }
            set
            {
                m_Color = new Color(value.r, value.g, value.b, value.a);
                resetColorPicker();
                if (null != onPicker)
                    onPicker.Invoke(m_Color);
            }
        }

        #region base field
        private Transform m_transform = null;

        private Transform m_colorPalette = null;

        private Transform m_coloredPate = null;
        #endregion

        #region MainColorTape && ColoredTape

        //Brightness && Saturation && Hue ColoredTape
        private Image m_mainColorTape = null;
        private ColoredTape m_firstLayerCT = null;
        private ColoredTape m_secondLayerCT = null;


        private Slider m_verticalCTSlider = null;
        private ColoredTape m_verticalFirstCT = null;

        #endregion



        #region RGBA颜色条
        private InputField m_rValue = null;
        private Slider m_rSlider = null;
        private ColoredTape m_rColoredTape = null;

        private InputField m_gValue = null;
        private Slider m_gSlider = null;
        private ColoredTape m_gColoredTape = null;


        private InputField m_bValue = null;
        private Slider m_bSlider = null;
        private ColoredTape m_bColoredTape = null;

        private InputField m_aValue = null;
        private Slider m_aSlider = null;
        private ColoredTape m_aColoredTape = null;
        #endregion

        #region 十六进制颜色
        private InputField m_hexColor = null;
        #endregion

        #region 游标
        private Transform m_nonius = null;

        #endregion

        /// <summary>
        /// reset color picker 
        /// </summary>
        private void resetColorPicker()
        {
            SetHexByColor();
            m_mainColorTape.color = Color;
            SetRGBA();
        }

        #region UIBehaviour functions
        protected override void Start()
        {
            m_transform = this.transform;
            m_mainColorTape = m_transform.Find("MainColor").GetComponent<Image>();

            #region  RGBA ColoredTape RGBA组件获取与事件监听
            var RGBA = m_transform.Find("RGBA");

            
            m_rValue = RGBA.Find("R/Value").GetComponent<InputField>();
            m_rSlider = RGBA.Find("R/Slider").GetComponent<Slider>();
            m_rSlider.onValueChanged.AddListener(OnRedSliderChanged);
            m_rColoredTape = m_rSlider.transform.Find("ColoredTape").GetComponent<ColoredTape>();
            m_rValue.onValueChanged.AddListener(SetColorbyR);


            m_gValue = RGBA.Find("G/Value").GetComponent<InputField>();
            m_gSlider = RGBA.Find("G/Slider").GetComponent<Slider>();
            m_gSlider.onValueChanged.AddListener(OnGreenSliderChanged);
            m_gColoredTape = m_gSlider.transform.Find("ColoredTape").GetComponent<ColoredTape>();
            m_gValue.onValueChanged.AddListener(SetColorbyG);


            m_bValue = RGBA.Find("B/Value").GetComponent<InputField>();
            m_bSlider = RGBA.Find("B/Slider").GetComponent<Slider>();
            m_bSlider.onValueChanged.AddListener(OnBlueSliderChanged);
            m_bColoredTape = m_bSlider.transform.Find("ColoredTape").GetComponent<ColoredTape>();
            m_bValue.onValueChanged.AddListener(SetColorbyB);

            m_aValue = RGBA.Find("A/Value").GetComponent<InputField>();
            m_aSlider = RGBA.Find("A/Slider").GetComponent<Slider>();
            m_aSlider.onValueChanged.AddListener(OnAlphaSliderChanged);
            m_aColoredTape = m_aSlider.transform.Find("ColoredTape").GetComponent<ColoredTape>();
            m_aValue.onValueChanged.AddListener(SetColorbyA);
            #endregion


            // 调色板
            m_colorPalette = m_transform.Find("ColorPalette");
            m_nonius = m_colorPalette.Find("ColorNonius");
            m_firstLayerCT = m_colorPalette.Find("FirstLayerColoredTape").GetComponent<ColoredTape>();
            m_secondLayerCT = m_colorPalette.Find("SecondLayerColoredTape").GetComponent<ColoredTape>();


            // 垂直滑动条
            m_coloredPate = m_transform.Find("ColoredTapeSlider");
            m_verticalCTSlider = m_coloredPate.GetComponent<Slider>();
            m_verticalFirstCT = m_coloredPate.Find("FirstLayerColoredTape").GetComponent<ColoredTape>();
            m_verticalCTSlider.onValueChanged.AddListener(verticalSliderChanged);

            // 初始化操作
            InitHexColor();
            Color = Color.white;

        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_rValue.onValueChanged.RemoveAllListeners();
            m_gValue.onValueChanged.RemoveAllListeners();
            m_bValue.onValueChanged.RemoveAllListeners();
            m_aValue.onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region vertical ct && main ct

        /// <summary>
        /// set color by slider 
        /// </summary>
        private void verticalSliderChanged(float value)
        {
            float height = m_verticalFirstCT.transform.GetComponent<RectTransform>().sizeDelta.y;
            Color color = m_verticalFirstCT.GetColor(new Vector2(0, height * (1 - value) - height / 2.0f));
            changedFirstLayerColoredTape(color);
            Color = mixedTwoColoredTapeColor(m_nonius.localPosition);
        }

        /// <summary>
        /// get nonius position by color
        /// </summary>
        /// <param name="color0"></param>
        /// <returns></returns>
        private Vector2 getPositionByMixedCT(Color color0)
        {
            var result = Vector2.zero;
            var size = m_colorPalette.GetComponent<RectTransform>().sizeDelta;

            return result;
        }

        /// <summary>
        /// set color for vertical slider 
        /// </summary>
        /// <param name="value"></param>
        private void setValueForVerticalSlider(float value)
        {
            m_verticalCTSlider.onValueChanged.RemoveAllListeners();
            m_verticalCTSlider.value = value;
            m_verticalCTSlider.onValueChanged.AddListener(verticalSliderChanged);
        }

        /// <summary>
        /// vetify first ct color
        /// </summary>
        /// <param name="color"></param>
        private void changedFirstLayerColoredTape(Color color)
        {
            m_firstLayerCT.SetColors(new Color[] { Color.white, color });
        }

        /// <summary>
        /// get color from mixed cts
        /// </summary>
        /// <param name="noniusPos"></param>
        /// <returns></returns>
        private Color mixedTwoColoredTapeColor(Vector2 noniusPos)
        {
            Color first = m_firstLayerCT.GetColor(noniusPos);
            Color second = m_secondLayerCT.GetColor(noniusPos);
            float alpha = second.a;
            Color result = Color.white;
            result = new Color(second.r, second.g, second.b) * alpha +
                new Color(first.r, first.g, first.b) * (1 - alpha);
            result.a = Color.a;
            return result;
        }

        #endregion


        #region RGBA input

        /// <summary>
        /// set RGBA inputfield
        /// </summary>
        private void SetRGBA()
        {
            var red = Color.r;
            var green = Color.g;
            var blue = Color.b;
            setRedInputFieldValue((int)(red * 255));
            setRedSliderValue(red);

            SetGreenInputFieldValue((int)(green * 255));
            SetGreenSliderValue(green);

            SetBlueInputFieldValue((int)(blue * 255));
            SetBlueSliderValue(blue);

            m_aValue.text = ((int)(Color.a * 255)).ToString();
            //set red 
            var startColor = new Color(0, green, blue, 1);
            var endColor = new Color(1, green, blue, 1);
            m_rColoredTape.SetColors(new Color[] { startColor, endColor });
            //set green
            startColor = new Color(red, 0, blue, 1);
            endColor = new Color(red, 1, blue, 1);
            m_gColoredTape.SetColors(new Color[] { startColor, endColor });
            //set blue
            startColor = new Color(red, green, 0, 1);
            endColor = new Color(red, green, 1, 1);
            m_bColoredTape.SetColors(new Color[] { startColor, endColor });
        }

        /// <summary>
        /// R inputfiled 
        /// </summary>
        /// <param name="red"></param>
        private void SetColorbyR(string red)
        {
            var value = 0;
            if (!int.TryParse(red, out value))
            {
                m_rValue.text = "0";
                return;
            }
            setRedSliderValue(value / 255.0f);
            Color = new Color((float)value / 255.0f, Color.g, Color.b, Color.a);
            SetNoniusPositionByColor();
        }

        private void setRedInputFieldValue(float value)
        {
            m_rValue.onValueChanged.RemoveAllListeners();
            m_rValue.text = ((int)value).ToString();
            m_rValue.onValueChanged.AddListener(SetColorbyR);
        }

        private void setRedSliderValue(float value)
        {
            m_rSlider.onValueChanged.RemoveAllListeners();
            m_rSlider.value = value;
            m_rSlider.onValueChanged.AddListener(OnRedSliderChanged);
        }

        /// <summary>
        /// G inputfiled 
        /// </summary>
        /// <param name="green"></param>
        private void SetColorbyG(string green)
        {
            var value = 0;
            if (!int.TryParse(green, out value))
            {
                m_gValue.text = "0";
                return;
            }
            SetGreenSliderValue(value / 255.0f);
            Color = new Color(Color.r, value / 255.0f, Color.b, Color.a);
            SetNoniusPositionByColor();
        }

        private void SetGreenInputFieldValue(float value)
        {
            m_gValue.onValueChanged.RemoveAllListeners();
            m_gValue.text = ((int)value).ToString();
            m_gValue.onValueChanged.AddListener(SetColorbyG);
        }

        private void SetGreenSliderValue(float value)
        {
            m_gSlider.onValueChanged.RemoveAllListeners();
            m_gSlider.value = value;
            m_gSlider.onValueChanged.AddListener(OnGreenSliderChanged);
        }

        /// <summary>
        /// blue inputfiled 
        /// </summary>
        /// <param name="blue"></param>
        private void SetColorbyB(string blue)
        {
            var value = 0;
            if (!int.TryParse(blue, out value))
            {
                m_bValue.text = "0";
                return;
            }
            SetBlueSliderValue(value / 255.0f);
            Color = new Color(Color.r, Color.g, value / 255.0f, Color.a);
            SetNoniusPositionByColor();
        }

        private void SetBlueInputFieldValue(float value)
        {
            m_bValue.onValueChanged.RemoveAllListeners();
            m_bValue.text = ((int)value).ToString();
            m_bValue.onValueChanged.AddListener(SetColorbyB);
        }

        private void SetBlueSliderValue(float value)
        {
            m_bSlider.onValueChanged.RemoveAllListeners();
            m_bSlider.value = value;
            m_bSlider.onValueChanged.AddListener(OnBlueSliderChanged);
        }

        /// <summary>
        /// alpha inputfiled 
        /// </summary>
        /// <param name="alpha"></param>
        private void SetColorbyA(string alpha)
        {
            var value = 0;
            if (!int.TryParse(alpha, out value))
            {
                m_aValue.text = "0";
                return;
            }
            m_aSlider.onValueChanged.RemoveAllListeners();
            m_aSlider.value = value / 255.0f;
            m_aSlider.onValueChanged.AddListener(OnAlphaSliderChanged);
            Color = new Color(Color.r, Color.g, Color.b, value / 255.0f);
            SetNoniusPositionByColor();
        }

        /// <summary>
        /// change colro by red slider
        /// </summary>
        /// <param name="value"></param>
        private void OnRedSliderChanged(float value)
        {
            Color = new Color(value, Color.g, Color.b, Color.a);
            changedFirstLayerColoredTape(Color);
            setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
        }

        /// <summary>
        /// change color by green slider
        /// </summary>
        /// <param name="value"></param>
        private void OnGreenSliderChanged(float value)
        {
            Color = new Color(Color.r, value, Color.b, Color.a);
            changedFirstLayerColoredTape(Color);
            setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
        }

        /// <summary>
        /// change color by blue slider
        /// </summary>
        /// <param name="value"></param>
        private void OnBlueSliderChanged(float value)
        {
            Color = new Color(Color.r, Color.g, value, Color.a);
            changedFirstLayerColoredTape(Color);
            setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
        }

        /// <summary>
        /// change color by alpha slider 
        /// </summary>
        /// <param name="value"></param>
        private void OnAlphaSliderChanged(float value)
        {
            Color = new Color(Color.r, Color.g, Color.b, value);
        }

        #endregion

        #region hex color
        private void InitHexColor()
        {
            m_hexColor = m_transform.Find("HexColor/Value").GetComponent<InputField>();
            m_hexColor.onValueChanged.AddListener(SetColorByHex);
        }

        private void SetColorByHex(string hexColor)
        {
            //TODO
        }

        private void SetHexByColor()
        {
            string hexValue = "";
            hexValue += DecimalToHexadecimal((int)(Color.r * 255));
            hexValue += DecimalToHexadecimal((int)(Color.g * 255));
            hexValue += DecimalToHexadecimal((int)(Color.b * 255));
            hexValue += DecimalToHexadecimal((int)(Color.a * 255));
            m_hexColor.text = hexValue;
        }

        private string DecimalToHexadecimal(int dec)
        {
            string result = Convert.ToString(dec, 16);
            result = result.ToUpper();
            if (result.Length.Equals(1))
                result = "0" + result;
            return result;
        }

        #endregion



        /// <summary>
        /// 根据游标设置颜色
        /// </summary>
        /// <param name="noniusPos"></param>
        public void SuckColorByNonius(Vector3 noniusPos)
        {
            Color = mixedTwoColoredTapeColor(noniusPos);
            m_mainColorTape.color = Color;
        }

        /// <summary>
        /// 根据颜色设置游标位置
        /// </summary>
        private void SetNoniusPositionByColor()
        {
            changedFirstLayerColoredTape(Color);
            m_nonius.localPosition = m_transform.GetComponent<RectTransform>().sizeDelta;
            setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
        }

    }
}