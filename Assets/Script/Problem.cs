using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem
{
    #region Khai báo biến
    private GameObject fourBlocks;
    private GameObject ui_Panel;
    private string DeBai;
    private int[,] BlockFaces; // 4x6
    private OrderList ExtraFaceColors;
    private int[,] SurroundFaceColors; // 4x4
    private List<int>[] SurroundList = new List<int>[]
     { new List<int>() { 5, 1, 2, 4},
       new List<int>() { 0, 5, 3, 2 },
       new List<int>() { 0, 1, 3, 4 },
       new List<int>() { 2, 1, 5, 4 },
       new List<int>() { 0, 2, 3, 5 },
       new List<int>() { 0, 4, 3, 1 }
     };
    public List<Solution> Solutions;
    private string sss = "";
    #endregion
    #region Các hàm tạo
    public Problem(string request)
    {
        DeBai = request;
        BlockFaces = new int[4, 6];
        string[] BlockString = DeBai.Split(',');
        for (int bl = 0; bl < 4; bl++)
        {
            for (int f = 0; f < 6; f++)
                BlockFaces[bl, f] = int.Parse(BlockString[bl].Substring(f, 1));
        }
        SurroundFaceColors = new int[4, 4];
        // Debug.Log("Bài toán được tạo");
        Solutions = new List<Solution>();
    }
    #endregion
    #region Các phương thức
    private OrderList FindExtraFaceColors()
    {
        OrderList ol = new OrderList();
        OrderList activeColors = new OrderList();
        for (int bl = 0; bl < 4; bl++)
        {
            for (int f = 0; f < 6; f++)
            {
                ol.Add(BlockFaces[bl, f]);
                if (!activeColors.Contains(BlockFaces[bl, f]))
                    activeColors.Add(BlockFaces[bl, f]);
            }
        }
        foreach (int i in activeColors)
        {
            for (int j = 0; j < 4; j++)
                ol.Remove(i);
        }
        return ol;
    }
    private bool CheckFaceDistincts(int sf)
    {
        List<int> ColorCodes = new List<int>();
        for (int bl = 0; bl < 4; bl++)
        {
            if (!ColorCodes.Contains(SurroundFaceColors[bl, sf]))
                ColorCodes.Add(SurroundFaceColors[bl, sf]);
        }
        if (ColorCodes.Count == 4)
        {
            if (sss != "") sss += ",";
            sss += SurroundFaceColors[0, sf].ToString()
            + SurroundFaceColors[1, sf].ToString()
            + SurroundFaceColors[2, sf].ToString()
            + SurroundFaceColors[3, sf].ToString();
        }
        return ColorCodes.Count == 4;
    }
    private bool Check4FaceDistincts()
    {
        sss = "";
        for (int sf = 0; sf < 4; sf++)
            if (!CheckFaceDistincts(sf)) return false;
        /// Debug.Log("sss=" + sss);
        return true;
    }
    private void FindSurroundFaceColor(int i0, int i1, int i2, int i3)
    {
        for (int i = 0; i < 4; i++) // 4 mặt xung quanh block 0
            SurroundFaceColors[0, i] = BlockFaces[0, SurroundList[i0][i]];
        for (int s1 = 0; s1 < 4; s1++)
        {
            for (int i = 0; i < 4; i++) // 4 mặt xung quanh của block 1
                SurroundFaceColors[1, i] = BlockFaces[1, SurroundList[i1][(i + s1) % 4]];
            for (int s2 = 0; s2 < 4; s2++)
            {
                for (int i = 0; i < 4; i++) // 4 mặt xung quanh của block 2
                    SurroundFaceColors[2, i] = BlockFaces[2, SurroundList[i2][(i + s2) % 4]];
                for (int s3 = 0; s3 < 4; s3++)
                {
                    for (int i = 0; i < 4; i++) // 4 mặt xung quanh của Block 3
                        SurroundFaceColors[3, i] = BlockFaces[3, SurroundList[i3][(i + s3) % 4]];
                    if (Check4FaceDistincts()) // Kiểm 4 mặt, mỗi mặt đủ 4 màu
                    {
                        string TrucXuyen = i0.ToString() + i1.ToString() + i2.ToString() + i3.ToString();
                        string CacMat = "0" + s1.ToString() + s2.ToString() + s3.ToString();
                        Solutions.Add(new Solution(TrucXuyen, CacMat));
                        // Debug.Log("Có nghiệm: TrucXuyen="+TrucXuyen + ", CacMat=" + CacMat);
                    }
                }
            }
        }
    }
    private void FindMainAxis3(int i0, int i1, int i2, OrderList axisFaceList2)
    {
        for (int i3 = 0; i3 < 6; i3++) // Phải cần 6
        {
            //if (i3 >= 3) Debug.Log("i3=" + i3);
            int ii = (i3 + 3) % 6;
            OrderList axisFaceList3 = axisFaceList2.Copy();
            axisFaceList3.Add(new OrderList(new int[] { BlockFaces[3, i3], BlockFaces[3, ii] }));
            // Debug.Log("axisFaceList3=" + axisFaceList3.ToString() + " ??? " + ExtraFaceColors);
            // Debug.Log("Vào FindMainAxis3() với " + axisFaceList3.ToString());
            if (axisFaceList3.isSubList(ExtraFaceColors))
            {
                //Debug.Log("Trục xuyên "
                // + "("+i0+","+(i0+3)%6 +")->" + BlockFaces[0,i0].ToString()
                // + BlockFaces[0,(i0+3)%6].ToString()
                // + ", ("+i1+","+(i1+3)%6 +")->" + BlockFaces[1,i1].ToString()
                // + BlockFaces[1,(i1+3)%6].ToString()
                // + ", ("+i2+","+(i2+3)%6 +")->" + BlockFaces[2,i2].ToString()
                // + BlockFaces[2,(i2+3)%6].ToString()
                // + ", ("+i3+","+(i3+3)%6 +")->" + BlockFaces[3,i3].ToString()
                // + BlockFaces[3,(i3+3)%6].ToString()
                // + " " + axisFaceList3.ToString());
                FindSurroundFaceColor(i0, i1, i2, i3);
            }
        }
    }
    private void FindMainAxis2(int i0, int i1, OrderList axisFaceList1)
    {
        for (int i2 = 0; i2 < 6; i2++) // Phải cần 6
        {
            //if (i2 >= 3) Debug.Log("i2=" + i2);
            int ii = (i2 + 3) % 6;
            OrderList axisFaceList2 = axisFaceList1.Copy();
            axisFaceList2.Add(new OrderList(new int[] { BlockFaces[2, i2], BlockFaces[2, ii] }));
            // Debug.Log("Vào FindMainAxis2() với " + axisFaceList2.ToString());
            if (axisFaceList2.isSubList(ExtraFaceColors))
                FindMainAxis3(i0, i1, i2, axisFaceList2);
        }
    }
    private void FindMainAxis1(int i0, OrderList axisFaceList0)
    {
        for (int i1 = 0; i1 < 6; i1++) // Phải cần 6
        {
            //if (i1 >= 3) Debug.Log("i1="+i1);
            int ii = (i1 + 3) % 6;
            OrderList axisFaceList1 = axisFaceList0.Copy();
            axisFaceList1.Add(new OrderList(new int[] { BlockFaces[1, i1], BlockFaces[1, ii] }));
            // Debug.Log("Vào FindMainAxis1() với " + axisFaceList1.ToString());
            if (axisFaceList1.isSubList(ExtraFaceColors))
                FindMainAxis2(i0, i1, axisFaceList1);
        }
    }
    private void FindMainAxis0()
    {
        for (int i0 = 0; i0 < 6; i0++) // Chỉ cần 3 / 6
        {
            OrderList axisFaceList0 = new OrderList(new int[] { BlockFaces[0, i0], BlockFaces[0, (i0 + 3)% 6] });
            // Debug.Log("Vào FindMainAxis0() với "+ axisFaceList0);
            if (axisFaceList0.isSubList(ExtraFaceColors))
                FindMainAxis1(i0, axisFaceList0);
        }
    }
    public void Solve()
    {
        ExtraFaceColors = FindExtraFaceColors();
        // Debug.Log(ExtraFaceColors.ToString());
        FindMainAxis0();
        // Debug.Log("Number of solutions="+Solutions.Count);

    }
    public string Decode(string trucXuyen, string cacMat)
    {
        string ss = "";
        for (int bl = 0; bl < 4; bl++)
        {
            if (ss != "") ss += ",";
            int s_index = int.Parse(trucXuyen.Substring(bl, 1));
            int f = int.Parse(cacMat.Substring(bl, 1));
            string s = BlockFaces[bl, SurroundList[s_index][f]].ToString()
            + BlockFaces[bl, SurroundList[s_index][(f + 1) % 4]].ToString()
            // + s_index.ToString()
            + BlockFaces[bl, s_index].ToString()
            + BlockFaces[bl, SurroundList[s_index][(f + 2) % 4]].ToString()
            + BlockFaces[bl, SurroundList[s_index][(f + 3) % 4]].ToString()
            + BlockFaces[bl, (s_index + 3) % 6].ToString();
            // Debug.Log("bl="+bl+", s=" + s);
            ss += s;
        }
        // Debug.Log("ss=" + ss);
        return ss;
    }
    public string FindAxisAction(int bl, int tx)
    {
        // Debug.Log("tx=" + tx);
        switch (tx)
        {
            case 0: return bl.ToString() + "FN";
            case 1: return bl.ToString() + "TC"; // mới sửa lại
            case 2: return "";
            case 3: return bl.ToString() + "FC";
            case 4: return bl.ToString() + "TN";
            case 5: return bl.ToString() + "TC," + bl.ToString() + "TC";
        }
        return "";
    }
    private bool Compare4of6(string s1, string s2)
    {
        for (int i = 0; i < 3; i++)
        {
            int ii = i + 3;
            bool b = (s1.Substring(i, 1) == s2.Substring(i, 1) && s1.Substring(ii, 1) == s2.Substring(ii, 1))
            || (s1.Substring(i, 1) == s2.Substring(ii, 1) && s1.Substring(ii, 1) == s2.Substring(i, 1));
            if (!b) return false;
        }
        return true;
    }
    private bool Compare6of6(string s1, string s2)
    {
        for (int i = 0; i < 6; i++)
            if (s1.Substring(i, 1) != s2.Substring(i, 1))
                return false;
        return true;
    }
    public bool CheckActionList(string trucXuyen, string cacMat, string decoded)
    {
        // return true;
        string[] blockCodes = decoded.Split(",");
        // Debug.Log("blockCodes.Length=" +blockCodes.Length);
        fourBlocks = GameObject.Find("FourBlocks");
        ui_Panel = GameObject.Find("UI_Panel");
        FourBlockMovement fourBlockMovement = fourBlocks.GetComponent<FourBlockMovement>();
        Ui_Panel_Script ui_Panel_script = ui_Panel.GetComponent<Ui_Panel_Script>();
        string al = "";
        for (int bl = 0; bl < 4; bl++)
        {
            int tx = int.Parse(trucXuyen.Substring(bl, 1));
            string s = FindAxisAction(bl, tx);
            if (s != "")
            {
                ui_Panel_script.MultiplePlay(s);
                if (al != "") al += ",";
                al += s;
            }
            int cm = int.Parse(cacMat.Substring(bl, 1));
            List<string> RRotates = new List<string>()
            { "",
            bl.ToString()+"RC",
            bl.ToString()+"RC,"+bl.ToString()+"RC",
            bl.ToString()+"RN"};
            s = RRotates[cm];
            if (s != "")
            {
                ui_Panel_script.SinglePlay(s);
                if (al != "") al += ",";
                al += s;
            }
            if (!Compare6of6(blockCodes[bl], fourBlockMovement.GetBlockFaces(bl)))
            {
                // Debug.Log("Check Bị sai (" + bl + ": tx=" +tx+"): " + blockCodes[bl] + "!=" +fourBlockMovement.GetBlockFaces(bl));
                return false;
            }
        }
        return true;
    }
    public string ActionList(string trucXuyen, string cacMat, string decoded)
    {
        string[] blockCodes = decoded.Split(",");
        //Debug.Log("blockCodes.Length=" +blockCodes.Length);
        fourBlocks = GameObject.Find("FourBlocks");
        ui_Panel = GameObject.Find("UI_Panel");
        FourBlockMovement fourBlockMovement = fourBlocks.GetComponent<FourBlockMovement>();
        Ui_Panel_Script ui_Panel_script = ui_Panel.GetComponent<Ui_Panel_Script>();
        string al = "";
        for (int bl = 0; bl < 4; bl++)
        {
            int tx = int.Parse(trucXuyen.Substring(bl, 1));
            string s = FindAxisAction(bl, tx);
            if (s != "")
            {
                ui_Panel_script.MultiplePlay(s);
                if (al != "") al += ",";
                al += s;
            }
            //int nCounter = 0;
            int cm = int.Parse(cacMat.Substring(bl, 1));
            List<string> RRotates = new List<string>()
            { "",
            bl.ToString()+"RC",
            bl.ToString()+"RC,"+bl.ToString()+"RC",
            bl.ToString()+"RN"};
            s = RRotates[cm];
            if (s != "")
            {
                ui_Panel_script.SinglePlay(s);
                if (al != "") al += ",";
                al += s;
            }
            // if (!Compare4of6(blockCodes[bl],fourBlockMovement.GetBlockFaces(bl)))
            if (!Compare6of6(blockCodes[bl], fourBlockMovement.GetBlockFaces(bl)))
            {
                Debug.Log("Bị sai (" + bl + "): " + blockCodes[bl] + "!="
               + fourBlockMovement.GetBlockFaces(bl));
            }
        }
        // Debug.Log("fourBlockMovement.GetFourBlockFaces()="
        // + fourBlockMovement.GetFourBlockFaces());
        return al;
    }
    #endregion
}
