using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu_script : MonoBehaviour
{
    #region Khai báo biến
    GameObject workSpace;
    GameObject ui_Panel;
    GameObject create_Panel;
    GameObject fourBlocks;
    public GameObject buttons;
    public GameObject sliderPanel;
    public GameObject solutionPanel;
    public GameObject recordSlider;
    public GameObject solutionSlider;
    public TMP_Text RecordNoText;
    public TMP_Text SolutionNoText;
    public TMP_Text ActionListText;
    public GameObject solveButton;
    public GameObject scrollView;
    public Transform viewContent;
    public TMP_Text stepTextPrefab;
    public GameObject autoButton;
    public GameObject clearButton;
    public GameObject removeRecordPanel;
    public TMP_Text recNoText;
    private string request = "";
    private int RecNo; // Có thể cần gán =0
    private Slider recSlider;
    private Slider slnSlider;
    private Problem problem;
    private List<TMP_Text> stepList;
    FourBlockMovement fourBlockMovement;
    private Vector3[] fourBlockPostions;

    #endregion
    #region Các phương thức
    void Start()
    {
        workSpace = GameObject.Find("WorkSpace");
        ui_Panel = GameObject.Find("UI_Panel");
        fourBlocks = GameObject.Find("FourBlocks");
        create_Panel = GameObject.Find("Create_Panel");
        create_Panel.GetComponent<Image>().enabled = false;
        create_Panel.GetComponent<Image>().enabled = false;
        create_Panel.GetComponent<CanvasGroup>().alpha = 0;
        solveButton.SetActive(request != "");
    }
    void Update()
    {

    }
    public void CreateNew()
    {
        // Debug.Log("Create New ...");
        workSpace.SetActive(false);
        ui_Panel.SetActive(false);
        create_Panel.GetComponent<Image>().enabled = true;
        create_Panel.GetComponent<CanvasGroup>().alpha = 1;
        New_Script new_script = create_Panel.GetComponent<New_Script>();
        new_script.Generate_Panel(this.gameObject); // Thêm parameter
        buttons.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void Open()
    {
        fourBlockMovement = fourBlocks.GetComponent<FourBlockMovement>();
        sliderPanel.SetActive(true);
        buttons.SetActive(false);
        solveButton.SetActive(false);
        //autoButton.SetActive(false);
        //clearButton.SetActive(false);
        recSlider = recordSlider.GetComponent<Slider>();
        recSlider.maxValue = fourBlockMovement.RecordList.Count - 1;
        recSlider.onValueChanged.AddListener(delegate { RecordValueChangeCheck(); });
    }
    public void RecordValueChangeCheck()
    {
        if (recSlider != null)
        {
            RecordNoText.text = recSlider.value.ToString();
        }
    }
    public void SolutionValueChangeCheck()
    {
        if (slnSlider != null)
        {
            SolutionNoText.text = slnSlider.value.ToString();
        }
    }
    public void RecordOkBtnClick()
    {
        fourBlockMovement = fourBlocks.GetComponent<FourBlockMovement>();
        fourBlockMovement.ResetRotations(); // Cần có
        RecNo = int.Parse(RecordNoText.text); // Đang sửa chỗ này
        request = fourBlockMovement.ReadRecord(RecNo);
        fourBlockMovement.Display(request);
        sliderPanel.SetActive(false);
        buttons.SetActive(true);
        solveButton.SetActive(true);
    }
    private void RemoveAllChildrenInContentView()
    {
        int numChildren = viewContent.childCount;
        // Debug.Log("numChildren="+ numChildren);
        for (int i = numChildren - 1; i >= 0; i--)
        {
            GameObject.Destroy(viewContent.GetChild(i).gameObject);
        }
    }
    public void ShowSteps(string actionList)
    {
        string[] steps = ("Start," + actionList + (actionList.Count() > 0 ? "," : "") + "End").Split(",");
        RemoveAllChildrenInContentView();
        stepList = new List<TMP_Text>();

        int ii = 0;
        foreach (string step in steps)
        {
            TMP_Text myText = Instantiate(stepTextPrefab, viewContent);
            myText.name = "Step" + ii.ToString("00");
            // Debug.Log(myText.name);
            myText.text = step;
            stepList.Add(myText);
            ii++;
        }
    }
    public void DoubleCheckSolutions()
    {
        // Debug.Log("Vào DoubleCheckSolutions: Count="+ problem.Solutions.Count);
        for (int SolNo = problem.Solutions.Count - 1; SolNo >= 0; SolNo--)
        {
            string trucXuyen = problem.Solutions[SolNo].TrucXuyen;
            string cacMat = problem.Solutions[SolNo].CacMat;
            // Debug.Log("request=" + request + ", Truc Xuyen=" + trucXuyen + ", Cac Mat=" + cacMat);
            string decoded = problem.Decode(trucXuyen, cacMat);
            // Debug.Log("decoded=" + decoded);
            scrollView.SetActive(true);
            autoButton.SetActive(true);
            clearButton.SetActive(true);
            bool check = problem.CheckActionList(trucXuyen, cacMat, decoded);
            if (!check)
            {
                // Debug.Log("Bỏ đi " + trucXuyen + ", " + cacMat);
                problem.Solutions.RemoveAt(SolNo);
            }

            fourBlockMovement = fourBlocks.GetComponent<FourBlockMovement>();
            fourBlockMovement.ResetRotations(); // Cần trả về start trước
            fourBlockMovement.Display(request);
            solutionPanel.SetActive(false);
            buttons.SetActive(true);
        }
        // Debug.Log("Ra DoubleCheckSolutions: Count=" + problem.Solutions.Count);
    }
    public void SolutionOkBtnClick()
    {
        if (problem.Solutions.Count() > 0)
        {
            int SlnNo = int.Parse(SolutionNoText.text);
            // Debug.Log("SlnNo=" + SlnNo + ", problem.Solutions.Count()=" +problem.Solutions.Count());
            string trucXuyen = problem.Solutions[SlnNo].TrucXuyen;
            string cacMat = problem.Solutions[SlnNo].CacMat;
            // Debug.Log("request=" + request + ", Truc Xuyen=" +trucXuyen+", Cac Mat="+cacMat);
            string decoded = problem.Decode(trucXuyen, cacMat);
            // Debug.Log("decoded=" + decoded);
            scrollView.SetActive(true);
            autoButton.SetActive(true);
            clearButton.SetActive(true);
            string actionList = problem.ActionList(trucXuyen, cacMat, decoded); // Ở đây
                                                                                // Debug.Log(actionList);
            ActionListText.text = actionList;
            fourBlockMovement = fourBlocks.GetComponent<FourBlockMovement>();
            fourBlockMovement.ResetRotations(); // Cần trả về start trước
            fourBlockMovement.Display(request);
            ShowSteps(actionList);
            PrepareSteps();
            solutionPanel.SetActive(false);
            buttons.SetActive(true);
        }
        else
        {
            // Debug.Log("Remove incorect problem record "+ RecNo);
            removeRecordPanel.SetActive(true);
            recNoText.text = RecNo.ToString();
        }
    }
    public void ClearStepColors()
    {
        for (int st = 0; st < stepList.Count; st++)
        {
            TextMeshProUGUI tmp_UGUI = stepList[st].GetComponent<TextMeshProUGUI>();
            tmp_UGUI.color = Color.white;
        }
    }
    private void PrepareSteps()
    {
        Ui_Panel_Script ui_panel_script = ui_Panel.GetComponent<Ui_Panel_Script>();
        ui_panel_script.SetActiveBlockId(-1);
        ui_panel_script.TurnOffButtons();
        ui_panel_script.SetRotateButtonsEnable(false);
    }
    private void StartSteps()
    {
        // Debug.Log("Có chạy StartSteps()");
        fourBlockPostions = new Vector3[4];
        for (int bl = 0; bl < 4; bl++)
        {
            fourBlockPostions[bl] = fourBlockMovement.blocks[bl].transform.position; // x khác nhau: -3; -1; 1; 3
        }
        fourBlockMovement.ResetRotations();
    }
    private Vector3 center(Vector3[] pos)
    {
        Vector3 newPos = Vector3.zero;
        for (int bl = 0; bl < 4; bl++)
        {
            newPos += fourBlockPostions[bl];
        }
        return newPos / 4;
    }
    private IEnumerator EndSteps()
    {
        for (int bl = 0; bl < 4; bl++)
        {
            fourBlockMovement.blocks[bl].transform.position = center(fourBlockPostions) + (bl - 1.5f) *
           Vector3.right;
        }
        for (int times = 0; times < 8; times++)
        {
            fourBlockMovement.AllRightRotate();
            // Debug.Log("Có chạy: " + times);
            yield return new WaitForSeconds(2);
        }
        for (int bl = 0; bl < 4; bl++)
        {
            fourBlockMovement.blocks[bl].transform.position = fourBlockPostions[bl];
        }
    }
    private IEnumerator Delay()
    {
        //Debug.Log("Wait...");
        Ui_Panel_Script ui_panel_script = ui_Panel.GetComponent<Ui_Panel_Script>();
        for (int st = 0; st < stepList.Count; st++)
        {
            ClearStepColors();
            TextMeshProUGUI tmp_UGUI = stepList[st].GetComponent<TextMeshProUGUI>();
            tmp_UGUI.color = Color.red;
            if (stepList[st].text == "Start")
            {
                StartSteps();
            }
            else if (stepList[st].text == "End")
            {
                yield return new WaitForSeconds(2);
                StartCoroutine(EndSteps());
            }
            else
            {
                ui_panel_script.SinglePlay(stepList[st].text);
            }
            yield return new WaitForSeconds(2);
            //Debug.Log("After yield ...");
        }
    }
    public void AutoBtnClick()
    {
        // Debug.Log("Auto Button Click");
        // FourBlockMovement fourBlockMovement =
        fourBlocks.GetComponent<FourBlockMovement>();
        StartCoroutine(Delay());
    }
    public void ClearBtnClick()
    {
        solutionPanel.SetActive(false);
        //ActionListText.text = "";
        //scrollView.SetActive(false);
        //autoButton.SetActive(false);
        //clearButton.SetActive(false);
    }
    public void RandomRecord()
    {
        FourBlockMovement fourBlockMovement = fourBlocks.GetComponent<FourBlockMovement>();
        int Recs = fourBlockMovement.GetNumberOfRecords();
        if (Recs > 0)
        {
            fourBlockMovement.ResetRotations();
            RecNo = Random.Range(0, Recs); // Đang sửa chỗ này
            request = fourBlockMovement.ReadRecord(RecNo);
            fourBlockMovement.Display(request);
            solveButton.SetActive(true);
        }
        sliderPanel.SetActive(false);
        solveButton.SetActive(true);
        ClearBtnClick();
    }
    public void SolveProblem()
    {
        // Debug.Log("Solve Problem "+ request);
        problem = new Problem(request);
        problem.Solve();
        DoubleCheckSolutions();
        solutionPanel.SetActive(true);
        buttons.SetActive(false);
        autoButton.SetActive(false);
        clearButton.SetActive(false);
        slnSlider = solutionSlider.GetComponent<Slider>();
        slnSlider.maxValue = problem.Solutions.Count - 1;
        slnSlider.onValueChanged.AddListener(delegate { SolutionValueChangeCheck(); });
        // Debug.Log("slnSlider.value="+ slnSlider.value+ ", slnSlider.maxValue="+ slnSlider.maxValue);
    }
    public void Resume()
    {
        workSpace.SetActive(true);
        ui_Panel.SetActive(true);
        create_Panel.GetComponent<Image>().enabled = false;
        create_Panel.GetComponent<CanvasGroup>().alpha = 0;
    }
    public void Reload(string str)
    {
        request = str;
        workSpace.SetActive(true);
        ui_Panel.SetActive(true);
        // Debug.Log(request);
        FourBlockMovement fourBlockMovement = fourBlocks.GetComponent<FourBlockMovement>();
        // Debug.Log(fourBlockMovement);
        fourBlockMovement.ResetRotations();
        fourBlockMovement.Display(request);
        Ui_Panel_Script ui_panel_script = ui_Panel.GetComponent<Ui_Panel_Script>();
        ui_panel_script.MutipleRandomClick();
        request = fourBlockMovement.GetFourBlockFaces(); // Cần lưu chuỗi này thành file
        fourBlockMovement.AppendRecord(request);
        fourBlockMovement.ResetRotations(); // Cần có
        fourBlockMovement.Display(request);
        solveButton.SetActive(true);
        ClearBtnClick();
    }
    public void RemoveBtnClick()
    {
        // Debug.Log("Remove Button Click");
        FourBlockMovement fourBlockMovement = fourBlocks.GetComponent<FourBlockMovement>();
        fourBlockMovement.RemoveRecord(RecNo);
        removeRecordPanel.SetActive(false);
        solveButton.SetActive(false);
    }
    public void IgnoreBtnClick()
    {
        // Debug.Log("Ignore Button Click");
        removeRecordPanel.SetActive(false);
    }

    #endregion
}
