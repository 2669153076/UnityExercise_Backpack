using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 商店道具信息
/// </summary>
[System.Serializable]
public class ShopCellInfo
{
    public int id;
    public ItemInfo itemInfo;
    public int priceType;
    public int price;
    public string tips;
}

/// <summary>
/// 作为Json读取的中间数据结构
/// </summary>
public class Shops
{
    public List<ShopCellInfo> info;
}
