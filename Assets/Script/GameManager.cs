using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Khai báo biến
    public static GameManager gm; // instance
    public int ActiveBlockId = 0;
    #endregion
    #region Các phương thức
    void Awake()
    {
        gm = this;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
