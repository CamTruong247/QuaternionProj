using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderList : List<int>
{
    #region Khai báo biến

    #endregion
    #region Các hàm tạo
    public OrderList()
    {
        // Nothing
    }
    public OrderList(int[] list) : base(new List<int>())
    {
        Array.Sort(list);
        for (int i = 0; i < list.Length; i++)
            base.Add(list[i]);
    }
    public OrderList Copy()
    {
        OrderList newList = new OrderList();
        foreach (int i in this)
            newList.Add(i);
        return newList;
    }
    #endregion
    #region Các phương thức
    public bool isSubList(OrderList oList)
    {
        OrderList nList = oList.Copy();
        foreach (int i in this)
        {
            if (nList.Contains(i))
                nList.Remove(i);
            else return false;
        }
        return true;
    }
    public new void Add(int n)
    {
        int[] newArr = new int[this.Count + 1];
        for (int i = 0; i < this.Count; i++)
            newArr[i] = base[i];
        newArr[this.Count] = n;
        Array.Sort(newArr);
        base.Clear();
        for (int i = 0; i < newArr.Length; i++)
            base.Add(newArr[i]);
    }
    public void Add(int[] list)
    {
        int[] newArr = new int[this.Count + list.Length];
        for (int i = 0; i < this.Count; i++)
            newArr[i] = base[i];
        for (int i = 0; i < list.Length; i++)
            newArr[this.Count + i] = list[i];
        Array.Sort(newArr);
        base.Clear();
        for (int i = 0; i < newArr.Length; i++)
            base.Add(newArr[i]);
    }
    public void Add(OrderList oList)
    {
        int[] newArr = new int[this.Count + oList.Count];
        for (int i = 0; i < this.Count; i++)
            newArr[i] = base[i];
        for (int i = 0; i < oList.Count; i++)
            newArr[this.Count + i] = oList[i];
        Array.Sort(newArr);
        base.Clear();
        for (int i = 0; i < newArr.Length; i++)
            base.Add(newArr[i]);
    }
    public override string ToString()
    {
        string s = "";
        foreach (int i in this)
        {
            // if (s != "") s += ",";
            s += i.ToString();
        }
        return s;
    }
    #endregion
}
