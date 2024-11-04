using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class New_Script : MonoBehaviour
{
    #region Khai báo biến
    public GameObject pnl4x4;
    public GameObject nudR4x1;
    public GameObject pnl1x6;
    public GameObject nudS1x6;
    public GameObject pnl2x4;
    public GameObject nudC2x1;
    private GameObject exitBtn;
    public GameObject doneBtn;
    private GameObject parent;
    public Button pbPrefab;
    public TMP_Text nudRPrefab;
    public Button pbsPrefab;
    public TMP_Text nudSPrefab;
    public Button pbcPrefab;
    public TMP_Text nudCPrefab;
    public static bool dirty = false;
    public Button[] Pb_Array;
    public Button[] Pbs_Array;
    public Button[] Pbc_Array;
    public TMP_Text[] nudR_Array;
    public TMP_Text[] nudS_Array;
    public TMP_Text[] nudC_Array;
    public static bool Pnl4x4Dropable = true;
    public static bool Pnl2x4Dropable = false;
    public static List<ColorName> ColorNameList = new List<ColorName>()
    {
     ColorName.Red, ColorName.Orange,
     ColorName.Green, ColorName.Yellow,
     ColorName.Blue, ColorName.Pink
    };
    #endregion
    #region Các phương thức
    void Start()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }
    void Update()
    {
        if (dirty)
        {
            CalculateNudRAndNudS();
            dirty = false;
        }
    }
    public void Generate_Panel(GameObject parentObj)
    {
        parent = parentObj;
        // Debug.Log(parent);
        exitBtn = GameObject.Find("ExitBtn");
        pnl1x6.GetComponent<Image>().enabled = true;
        pnl1x6.GetComponent<CanvasGroup>().alpha = 1;
        nudS1x6.GetComponent<Image>().enabled = true;
        nudS1x6.GetComponent<CanvasGroup>().alpha = 1;
        GenerateGrid4x4();
        GenerateNudR4x1();
        GenerateColumn1x6();
        GenerateNudS1x6();
        GenerateGrid2x4();
        GenerateNudC2x1();
        pnl4x4.SetActive(true);
        nudR4x1.SetActive(true);
        pnl1x6.SetActive(true);
        nudS1x6.SetActive(true);
        doneBtn.SetActive(false);
        Pnl4x4Dropable = true;
        Pnl2x4Dropable = false;
    }
    private void GenerateGrid4x4()
    {
        foreach (Transform child in pnl4x4.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        Pb_Array = new Button[16];
        for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
            {
                Button pb = (Button)Instantiate(pbPrefab, pnl4x4.transform);
                pb.name = "pb" + r.ToString() + c.ToString();
                Pb_Array[r * 4 + c] = pb;
            }
    }
    private void GenerateNudR4x1()
    {
        foreach (Transform child in nudR4x1.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        nudR_Array = new TMP_Text[4];
        for (int r = 0; r < 4; r++)
        {
            TMP_Text nudR = (TMP_Text)Instantiate(nudRPrefab, nudR4x1.transform);
            nudR.name = "nudR" + r.ToString();
            nudR_Array[r] = nudR;
        }
    }
    private void GenerateColumn1x6()
    {
        foreach (Transform child in pnl1x6.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        Pbs_Array = new Button[6];
        for (int c = 0; c < 6; c++)
        {
            Button pbs = (Button)Instantiate(pbsPrefab, pnl1x6.transform);
            pbs.name = "pbs" + c.ToString();
            // Debug.Log(pbs.GetComponent<Button>().colors.normalColor);
            ColorBlock cb = pbs.GetComponent<Button>().colors;
            cb.normalColor = ColorNameList[c].color;
            cb.highlightedColor = ColorNameList[c].color;
            cb.pressedColor = ColorNameList[c].color;
            cb.selectedColor = ColorNameList[c].color;
            pbs.GetComponent<Button>().colors = cb;
            // Debug.Log(pbs.gameObject.GetComponent<Pnl1x6Drag>());
            TMP_Text text = pbs.gameObject.GetComponent<Pnl1x6Drag>().MyTextElement;
            text.text = ColorNameList[c].name;
            Pbs_Array[c] = pbs;
        }
    }
    private void GenerateNudS1x6()
    {
        foreach (Transform child in nudS1x6.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        nudS_Array = new TMP_Text[6];

        for (int r = 0; r < 6; r++)
        {
            TMP_Text nudS = (TMP_Text)Instantiate(nudSPrefab, nudS1x6.transform);
            nudS.name = "nudS" + r.ToString();
            nudS_Array[r] = nudS;
        }
    }
    private void GenerateGrid2x4()
    {
        foreach (Transform child in pnl2x4.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Pbc_Array = new Button[8];
        for (int r = 0; r < 2; r++)
            for (int c = 0; c < 4; c++)
            {
                Button pbc = (Button)Instantiate(pbcPrefab, pnl2x4.transform);
                pbc.name = "pbc" + r.ToString() + c.ToString();
                Pbc_Array[r * 4 + c] = pbc;
            }
    }
    private void GenerateNudC2x1()
    {
        foreach (Transform child in nudC2x1.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        nudC_Array = new TMP_Text[2];
        for (int r = 0; r < 2; r++)
        {
            TMP_Text nudC = (TMP_Text)Instantiate(nudCPrefab, nudC2x1.transform);
            nudC.name = "nudC" + r.ToString();
            nudC_Array[r] = nudC;
        }
    }
    public static void SetDirty()
    {
        dirty = true;
    }
    public static void SetPnl4x4Dropable(bool b)
    {
        Pnl4x4Dropable = b;
    }
    public static void SetPnl2x4Dropable(bool b)
    {
        Pnl2x4Dropable = b;
    }
    public static bool GetPnl4x4Dropable()
    {
        return Pnl4x4Dropable;
    }
    public static bool GetPnl2x4Dropable()
    {
        return Pnl2x4Dropable;
    }
    public static int FindColorInList(ColorName colorName, List<ColorName> colorNameList)
    {
        for (int s = 0; s < colorNameList.Count; s++)
            if (colorNameList[s] == colorName) return s;
        return -1; // Báo sai
    }
    public int ColorNameToCode(ColorName colorName)
    {
        return ColorNameList.IndexOf(colorName);
    }
    public bool Check4RMatch()
    {
        for (int i = 0; i < nudR_Array.Length; i++)
            if (int.Parse(nudR_Array[i].text) != 4)
                return false;
        return true;
    }
    public bool Check4Used2None()
    {
        int Count4 = 0;
        int Count0 = 0;
        for (int s = 0; s < 6; s++)
        {
            int n = int.Parse(nudS_Array[s].text);
            if (n == 4) Count4++;
            if (n == 0) Count0++;
        }
        return Count4 == 4 && Count0 == 2;
    }
    public bool Check2CMacth()
    {
        return (int.Parse(nudC_Array[0].text) == 4 && int.Parse(nudC_Array[1].text) == 4);
    }
    private void DisableUnusedButtons()
    {
        for (int s = 0; s < 6; s++)
        {
            if (int.Parse(nudS_Array[s].text) == 0)
            {
                Pbs_Array[s].GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }
    private void CalculateNudRAndNudS()
    {
        for (int r = 0; r < 4; r++)
            nudR_Array[r].text = "0";
        for (int s = 0; s < 6; s++)
            nudS_Array[s].text = "0";
        for (int r = 0; r < 4; r++)
        {
            List<string> RowColorList = new List<string>();
            for (int c = 0; c < 4; c++)
            {
                int i = r * 4 + c;
                TextMeshProUGUI tmp_text =
               Pb_Array[i].GetComponentInChildren<TextMeshProUGUI>();
                if (tmp_text.text != "")
                {
                    if (!RowColorList.Contains(tmp_text.text))
                    {
                        RowColorList.Add(tmp_text.text);
                        nudR_Array[r].text = (int.Parse(nudR_Array[r].text) + 1).ToString();
                    }
                    int s = FindColorInList(new ColorName(tmp_text.text), ColorNameList);
                    nudS_Array[s].text = (int.Parse(nudS_Array[s].text) + 1).ToString();
                }
            }
        }
        if (Check4RMatch() && Check4Used2None())
        {
            Pnl4x4Dropable = false;
            Pnl2x4Dropable = true;
            DisableUnusedButtons();
        }
        if (!Pnl4x4Dropable && Pnl2x4Dropable)
        {
            nudC_Array[0].text = "0";
            nudC_Array[1].text = "0";
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    int i = r * 4 + c;
                    TextMeshProUGUI tmp_text =
                   Pbc_Array[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (tmp_text.text != "")
                    {
                        nudC_Array[r].text = (int.Parse(nudC_Array[r].text) + 1).ToString();
                        int s = FindColorInList(new ColorName(tmp_text.text), ColorNameList);
                        nudS_Array[s].text = (int.Parse(nudS_Array[s].text) + 1).ToString();
                    }
                }
            }
        }
        if (Check2CMacth())
        {
            Pnl4x4Dropable = false;
            Pnl2x4Dropable = false;
            // Debug.Log("Done");
            doneBtn.SetActive(true); // Do Creation
        }
    }
    public void ExitClick()
    {
        parent.SetActive(true);
        // Debug.Log(parent.GetComponent<Menu_script>());
        //parent.GetComponent<Menu_script>().Resume();
        pnl4x4.SetActive(false);
        nudR4x1.SetActive(false);
        pnl1x6.SetActive(false);
        nudS1x6.SetActive(false);
    }
    private List<int> HoanVi(int n)
    {
        List<int> result = new List<int>();
        List<int> s = new List<int>();
        for (int i = 0; i < n; i++)
            s.Add(i);
        for (int i = 0; i < n; i++)
        {
            int kq = Random.Range(0, s.Count);
            result.Add(s[kq]);
            s.Remove(s[kq]);
        }
        return result;
    }
    private int RC4X4Text(int r, int c)
    {
        string colorName = Pb_Array[r * 4 + c].GetComponent<Pnl4x4Drop>().MyTextElement.text;
        return ColorNameToCode(new ColorName(colorName));
    }
    private int RC2X4Text(int r, int c)
    {
        string colorName = Pbc_Array[r * 4 + c].GetComponent<Pnl2x4Drop>().MyTextElement.text;
        return ColorNameToCode(new ColorName(colorName));
    }
    private void CreateRequest()
    {
        // Debug.Log("Create Request");
        List<int> hv4 = HoanVi(4);
        List<int> hv8 = HoanVi(8);
        string str = "";
        for (int bl = 0; bl < 4; bl++)
        {
            if (str != "") str += ",";
            string f0 = RC4X4Text(0, hv4[bl]).ToString();
            string f1 = RC4X4Text(1, hv4[bl]).ToString();
            string f2 = RC2X4Text(hv8[hv4[bl] * 2] % 2, hv4[bl]).ToString();
            string f3 = RC4X4Text(2, hv4[bl]).ToString();
            string f4 = RC4X4Text(3, hv4[bl]).ToString();
            string f5 = RC2X4Text(hv8[hv4[bl] * 2 + 1] % 2, hv4[bl]).ToString();
            str += f0 + f1 + f2 + f3 + f4 + f5;
        }
        // Debug.Log(str);
        // Menu_script.request = str;
        parent.GetComponent<Menu_script>().Reload(str);
    }
    public void DoneClick()
    {
        CreateRequest();
        parent.SetActive(true);
        // Debug.Log(parent.GetComponent<Menu_script>());
        parent.GetComponent<Menu_script>().Resume();
        pnl4x4.SetActive(false);
        nudR4x1.SetActive(false);
        pnl1x6.SetActive(false);
        nudS1x6.SetActive(false);
    }

    #endregion
}
