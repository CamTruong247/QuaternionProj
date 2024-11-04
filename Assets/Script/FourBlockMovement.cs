using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FourBlockMovement : MonoBehaviour
{
    #region Khai báo biến
    public GameManager gameManager;
    public GameObject[] blocks;

    private int activeBlockId = 0;
    public List<string> RecordList;

    void Awake()
    {
        RecordList = PlayerPrefsExtra.GetList<string>("recordList", new List<string>());
        // Debug.Log("Danh sách ban đầu: "+ RecordList.Count);
    }

    #endregion
    #region Các phương thức
    void Start()
    {
        activeBlockId = gameManager.ActiveBlockId;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            SetActiveBlockId(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SetActiveBlockId(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetActiveBlockId(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SetActiveBlockId(3);
        }
    }

    private void SetActiveBlockId(int n)
    {
        gameManager.ActiveBlockId = activeBlockId = n;
    }

    public void Display(string str)
    {
        int bl = 0;
        foreach (string s in str.Split(','))
        {
            SetActiveBlockId(bl);
            Movement movement = blocks[bl].GetComponent<Movement>();
            movement.Display(s);
            // Debug.Log(s);
            bl++;
        }
    }

    public Vector3 DirCodeToVector(int dirCode)
    {
        List<Vector3> list = new List<Vector3>() { Vector3.up, Vector3.back, Vector3.left,Vector3.down, Vector3.forward, Vector3.right};
        // Debug.Log("DirCodeToVector="+ list[dirCode]);
        return list[dirCode];
    }

    public int BlockFaceCenter(int bl, int directionCode)
    {
        Movement movement = blocks[bl].GetComponent<Movement>();
        Collider[] hitColliders = Physics.OverlapSphere(blocks[bl].transform.position + 0.5f *DirCodeToVector(directionCode),
        0.2f);
        // Debug.Log("hitColliders.Count="+ hitColliders.Count());
        foreach (Collider hitCollider in hitColliders)
        {
            int code = -1;
            if (int.TryParse(hitCollider.name, out code))
            {
                 //Debug.Log("Trả vè " + code);
                return code;
            }
        }
        // Debug.Log("Báo sai");
        return -1; // Báo sai
    }
    public string MaterialToName(Material material)
    {
        string s = material.name;
        return s.Substring(0, s.IndexOf(" "));
    }
    public string GetBlockFaces(int bl)
    {
        Debug.Log("Vào GetBlockFaces với bl=" + bl.ToString());
        string fstr = "";
        for (int f = 0; f < 6; f++)
        {
            Movement movement = blocks[bl].GetComponent<Movement>();
            //Debug.Log("f=" + f+ ", movement="+ movement);
            //Debug.Log("Vào CodeToRealFace(" + bl + "," + f + ")");
            GameObject realFace = movement.CodeToRealFace(BlockFaceCenter(bl, f));
            //Debug.Log("realFace="+ realFace);
            // Material[] materials = realFace.GetComponent<MeshRenderer>().materials;
            Material material = realFace.GetComponent<MeshRenderer>().material;
            // Debug.Log("material.name="+MaterialToName(material));
            int nf = New_Script.FindColorInList(new ColorName(MaterialToName(material)),New_Script.ColorNameList);
            fstr += nf.ToString();
        }
        return fstr;
    }

    public string GetFourBlockFaces()
    {
        string fstring = "";
        for (int bl = 0; bl < 4; bl++) //4
        {
            SetActiveBlockId(bl);
            if (fstring != "") fstring += ",";
            fstring += GetBlockFaces(bl);
        }
        return fstring;
    }
    public void AppendRecord(string str)
    {
        RecordList.Add(str);
        PlayerPrefsExtra.SetList("recordList", RecordList);
        PlayerPrefs.Save();
    }
    public void RemoveRecord(int recNo)
    {
        if (recNo >= 0 && recNo < RecordList.Count())
        {
            RecordList.RemoveAt(recNo);
            PlayerPrefsExtra.SetList("recordList", RecordList);
            PlayerPrefs.Save();
            // Debug.Log("Đã hủy bỏ " + recNo);
        }
    }
    public string ReadRecord(int recordNo)
    {
        return RecordList[recordNo];
    }
    public int GetNumberOfRecords()
    {
        return RecordList.Count;
    }
    public void ResetRotations()
    {
        for (int bl = 0; bl < 4; bl++)
        {
            SetActiveBlockId(bl);
            Movement movement = blocks[bl].GetComponent<Movement>();
            movement.ResetRotation();
        }
    }
    public void AllRightRotate()
    {
        for (int bl = 0; bl < 4; bl++)
        {
            // Debug.Log("Vào AllRightRotate với bl=" + bl);
            SetActiveBlockId(bl);
            Movement movement = blocks[bl].GetComponent<Movement>();
            movement.RightRotate(true);
        }
    }

    #endregion
}
