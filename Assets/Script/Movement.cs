using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Khai báo biến
    public int blockId = 0; // Not active
    private int activeBlockId;
    public GameManager gameManager;
    public GameObject Top;
    public GameObject Front;
    public GameObject Left;
    public GameObject Down;
    public GameObject Back;
    public GameObject Right;
    public Material Red;
    public Material Orange;
    public Material Green;
    public Material Yellow;
    public Material Blue;
    public Material Pink;
    public List<GameObject> FaceNameList = new List<GameObject>();
    public List<Material> MaterialList = new List<Material>();
    private int FaceLayer = 3;
    #endregion
    #region Các phương thức
    void Start()
    {
        activeBlockId = gameManager.ActiveBlockId;
        FaceNameList.Add(Top);
        FaceNameList.Add(Front);
        FaceNameList.Add(Left);
        FaceNameList.Add(Down);
        FaceNameList.Add(Back);
        FaceNameList.Add(Right);
        MaterialList.Add(Red);
        MaterialList.Add(Orange);
        MaterialList.Add(Green);
        MaterialList.Add(Yellow);
        MaterialList.Add(Blue);
        MaterialList.Add(Pink);
    }
    void Update()
    {
        activeBlockId = gameManager.ActiveBlockId;
        if (blockId == activeBlockId)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                bool clockwise = !(Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift));
                // Debug.Log(clockwise);
                TopRotate(clockwise);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                bool clockwise = !(Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift));
                FrontRotate(clockwise);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                bool clockwise = !(Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift));
                RightRotate(clockwise);
            }
        }
    }
    public void TopRotate(bool clockwise)
    {
        // Debug.Log("Top Rotate");
        int angleDegree = clockwise ? 90 : -90;
        // Debug.Log(angleDegree);
        gameObject.transform.rotation = Quaternion.Euler(0, angleDegree, 0) *
       gameObject.transform.rotation;
        // Debug.Log(CodeToRealFace(0) );
    }
    public void FrontRotate(bool clockwise)
    {
        // Debug.Log("Front Rotate");
        int angleDegree = clockwise ? -90 : 90;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angleDegree) *
       gameObject.transform.rotation;
        // Debug.Log(CodeToRealFace(0));
    }

    public void RightRotate(bool clockwise)
    {
        // Debug.Log("Right Rotate");
        int angleDegree = clockwise ? 90 : -90;
        gameObject.transform.rotation = Quaternion.Euler(angleDegree, 0, 0) *
       gameObject.transform.rotation;
    }
    public void Display(string s)
    {
        for (int f = 0; f < 6; f++)
        {
            GameObject FaceName = FaceNameList[f];
            MeshRenderer meshRenderer = FaceName.GetComponent<MeshRenderer>();
            meshRenderer.material = MaterialList[int.Parse(s.Substring(f, 1))];
        }
    }
    public GameObject CodeToRealFace(int faceCode)
    {
        List<Vector3> v3List = new List<Vector3>() { new Vector3(0, 0.5f, 0), new Vector3(0, 0, -0.5f),
                                                     new Vector3(-0.5f, 0, 0), new Vector3(0, -0.5f, 0),
                                                     new Vector3(0, 0, 0.5f), new Vector3(0.5f, 0, 0) };
        Vector3 v3Direction = v3List[faceCode];
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(v3Direction), out hit, 0.48f,1 << FaceLayer))
        {
            int realFaceCode = int.Parse(hit.collider.name);
            Debug.Log("blockId="+ blockId + ", faceCode = " + faceCode +" -> realFaceCode=" +realFaceCode);
            switch (realFaceCode)
            {
                case 0: return Top;
                case 1: return Front;
                case 2: return Left;
                case 3: return Down;
                case 4: return Back;
                case 5: return Right;
            }
        }
        return null;
    }
    public void ResetRotation()
    {
        // Debug.Log("Reset Rotation " + blockId);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    #endregion
}
