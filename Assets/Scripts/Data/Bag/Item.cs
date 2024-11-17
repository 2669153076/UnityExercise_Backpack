using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包物品
/// </summary>
[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public string icon;
    public int type;
    public int price;
    public int equipType;
    public string tips;
}

/// <summary>
/// 中间数据结构，用于Json存取
/// </summary>
public class Items
{
    public List<Item> itemList;
}
