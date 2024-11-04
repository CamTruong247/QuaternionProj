using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pnl1x6Drag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Khai báo biến
    public static GameObject ItemBeingDragged;
    public static string DataTransferred;
    public TextMeshProUGUI MyTextElement;
    // private RectTransform rectTransform;
    private Transform canvas;
    private CanvasGroup canvasGroup;
    public GameObject pbsPrefab;
    #endregion

    #region Các phương thức
    private void Awake()
    {
        // rectTransform = GetComponent<RectTransform>(); ;
        canvas = GameObject.FindGameObjectWithTag("UI Canvas").transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject dupplicate = Instantiate(pbsPrefab);
        ItemBeingDragged = dupplicate;
        RectTransform rectTransform = GetComponent<RectTransform>();
        RectTransform rt = dupplicate.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        ColorBlock cb = this.GetComponent<Button>().colors;
        ItemBeingDragged.GetComponent<Button>().colors = cb;
        ItemBeingDragged.transform.SetParent(canvas);
        DataTransferred = MyTextElement.text;
        ItemBeingDragged.GetComponent<CanvasGroup>().alpha = 0.6f;
        ItemBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        // rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        ItemBeingDragged.transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("OnEndDrag");
        ItemBeingDragged.transform.position = eventData.position; // Thêm lệnh này
        ItemBeingDragged.GetComponent<CanvasGroup>().alpha = 1;
        ItemBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(Pnl1x6Drag.ItemBeingDragged);
        Pnl1x6Drag.ItemBeingDragged = null;
    }
    public void OnPointerDown(PointerEventData eventDta)
    {
        // Debug.Log("OnPointerDown");
    }
    public void SetDraggable(bool b)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = b;
    }
    #endregion
}
