
using UnityEngine;
using ColorUiTools;
using UnityEngine.UI;

public class ColorPickerTest : MonoBehaviour
{
    /// <summary>
    /// 颜色选择器
    /// </summary>
    public ColorPicker ColorPicker = null;
    /// <summary>
    /// 猪的图片
    /// </summary>
    public Image Image = null;

    private void Awake()
    {
        ColorPicker.onPicker.AddListener(color =>
        {
            // 设置猪的颜色
            Image.color = color;
        });
    }
}