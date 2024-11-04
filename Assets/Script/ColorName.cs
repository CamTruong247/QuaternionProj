using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorName
{
    #region Khai báo biến
    public string name;
    public Color color;
    public static ColorName Red = new ColorName("Red", Color.red);
    public static ColorName Orange = new ColorName("Orange", new Color32(0xFF, 0x8C, 0x64,
    0xFF));
    public static ColorName Green = new ColorName("Green", Color.green);
    public static ColorName Yellow = new ColorName("Yellow", new Color32(0xFF, 0xD3, 0x0B,
    0xFF));
    public static ColorName Blue = new ColorName("Blue", new Color32(0x0A, 0x09, 0xFf, 0xFF));
    public static ColorName Pink = new ColorName("Pink", new Color32(0xF7, 0x07, 0xB6, 0xFF));
    #endregion
    #region Các hàm tạo
    public ColorName(string name, Color color)
    {
        this.name = name;
        this.color = color;
    }
    public ColorName(string name)
    {
        this.name = name;
        switch (name)
        {
            case "Red":
                {
                    this.color = Red.color;
                    break;
                }
            case "Orange":
                {
                    this.color = Orange.color;
                    break;
                }
            case "Green":
                {
                    this.color = Green.color;
                    break;
                }
            case "Yellow":
                {
                    this.color = Yellow.color;
                    break;
                }
            case "Blue":
                {
                    this.color = Blue.color;
                    break;
                }
            case "Pink":
                {
                    this.color = Pink.color;
                    break;
                }
        }
    }

    #endregion
    #region Các phương thức đè lên
    public static bool operator ==(ColorName cn1, ColorName cn2)
    {
        // Debug.Log("cn1="+cn1 + ", cn2="+cn2);
        return cn1.name == cn2.name;
    }
    public static bool operator !=(ColorName cn1, ColorName cn2)
    {
        return cn1.name != cn2.name;
    }
    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        var item = obj as ColorName;
        return name.Equals(item.name);
    }
    #endregion
}
