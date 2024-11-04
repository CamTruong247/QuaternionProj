using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Panel_Script : MonoBehaviour
{
    #region Khai báo biến
    public Sprite[] ImageOff;
    public Sprite[] ImageOn;
    public Sprite[] WindlassOff;
    public Sprite[] WindlassOn;
    public GameManager gameManager;
    public GameObject fourBlocks;
    private int activeBlockId = 0;
    private string activeAxisCode = "";
    #endregion
    #region Các phương thức
    void Start()
    {

    }
    void Update()
    {

    }

    public void TurnOffButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject button = GameObject.Find("Button" + i.ToString());
            Image image = button.GetComponent<Image>();
            image.sprite = ImageOff[i];
        }
    }
    public void ButtonClick(int n)
    {
        // Debug.Log("Button" + n + " is clicked");
        TurnOffButtons();
        GameObject buttonN = GameObject.Find("Button" + n.ToString());
        Image image = buttonN.GetComponent<Image>();
        // Debug.Log(image.sprite);
        image.sprite = ImageOn[n];

        SetActiveBlockId(n);
    }

    public void SetActiveBlockId(int n)
    {
        gameManager.ActiveBlockId = activeBlockId = n;
    }
    public string AxisCodeToName(string s)
    {
        switch (s)
        {
            case "F": return "Front";
            case "T": return "Top";
            case "R": return "Right";
        }
        return "";
    }
    public int AxisCodeToId(string s)
    {
        switch (s)
        {
            case "F": return 0;
            case "T": return 1;
            case "R": return 2;
        }
        return -1; // Báo sai
    }
    public void TurnOffWindlassses()
    {
        foreach (string s in new List<string> { "F", "T", "R" })
        {
            GameObject button = GameObject.Find(AxisCodeToName(s).ToString() + "Axis");
            Image image = button.GetComponent<Image>();
            image.sprite = WindlassOff[AxisCodeToId(s)];
        }
    }
    public void TurnOnWindlass(string s)
    {
        activeAxisCode = s;
        GameObject button = GameObject.Find(AxisCodeToName(s).ToString() + "Axis");
        Image image = button.GetComponent<Image>();
        image.sprite = WindlassOn[AxisCodeToId(s)];
        SetRotateButtonsEnable(true);
    }
    public void SetRotateButtonsEnable(bool b)
    {
        foreach (string s in new List<string>() { "NCKDH", "CCKDH" })
        {
            GameObject buttonRotate = GameObject.Find(s);
            // Debug.Log(buttonRotate);
            Button button = buttonRotate.GetComponent<Button>();
            // Debug.Log(button);
            button.interactable = b; // true
        }
    }
    public void AxisClick(string s)
    {
        if (activeBlockId >= 0)
        {
            TurnOffWindlassses();
            TurnOnWindlass(s);
        }
    }
    public void Rotate(bool clockwise)
    {
        if (activeBlockId >= 0)
        {
            GameObject block = GameObject.Find("FourBlocks/Block" + activeBlockId.ToString());
            // Debug.Log(block);
            Movement script = block.GetComponent<Movement>();
            // Debug.Log(script);

            switch (activeAxisCode)
            {
                case "F":
                    script.FrontRotate(clockwise);
                    break;
                case "T":
                    script.TopRotate(clockwise);
                    break;
                case "R":
                    script.RightRotate(clockwise);
                    break;
            }

        }
    }
    public void SinglePlay(string action)
    {
        // Debug.Log("action=" + action);
        int bl = int.Parse(action.Substring(0, 1));
        SetActiveBlockId(bl);
        activeAxisCode = action.Substring(1, 1);
        bool clockwize = action.Substring(2, 1) == "C";
        Rotate(clockwize);
    }
    public void MultiplePlay(string actions)
    {
        foreach (string action in actions.Split(","))
            SinglePlay(action);
    }
    public void SingleRandomClick()
    {
        // Debug.Log("Single Random Click");
        string actionCodeList = "TFR";
        int bl = Random.Range(0, 4);
        string axisCode = actionCodeList.Substring(Random.Range(0, 3), 1);
        string clockwizeCode = (Random.Range(0, 1) == 0) ? "N" : "C";
        string s = bl.ToString() + axisCode + clockwizeCode;
        SinglePlay(s);
    }
    public void MutipleRandomClick()
    {
        // Debug.Log("Multiple Random Click");
        int times = Random.Range(5, 25);
        for (int t = 0; t < times; t++)
            SingleRandomClick();
    }
    #endregion
}
