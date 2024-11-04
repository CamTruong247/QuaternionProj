using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pnl2x4Drop : MonoBehaviour, IDropHandler
{
    #region Khai báo biến
    public TextMeshProUGUI MyTextElement;
    #endregion
    #region Các phương thức
    public void OnDrop(PointerEventData eventData)
    {
        if (New_Script.GetPnl2x4Dropable())
        {
            // Debug.Log("OnDrop 2x4");
            if (Pnl1x6Drag.ItemBeingDragged != null)
            {
                ColorBlock dragCb = Pnl1x6Drag.ItemBeingDragged.GetComponent<Button>().colors;
                this.GetComponent<Button>().colors = dragCb;
                // Debug.Log(Pnl1x6Drag.DataTransferred);
                MyTextElement.text = Pnl1x6Drag.DataTransferred;
                New_Script.SetDirty();
            }
        }
    }

    #endregion
}
