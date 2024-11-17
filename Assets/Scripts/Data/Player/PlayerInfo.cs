using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public string name;
    public int lev;
    public int money;
    public int gem;
    public int pro; //体力
    public List<ItemInfo> propList; //道具
    public List<ItemInfo> equipList;    //装备
    public List<ItemInfo> fragList;     //碎片
    public List<ItemInfo> curEquipList; //当前所装备的武器列表

    public PlayerInfo()
    {
        name = "Player";
        lev = 1;
        money = 9999;
        gem = 0;
        pro = 100;
        propList = new List<ItemInfo>() { new ItemInfo() { id = 3, num = 12 } };
        equipList = new List<ItemInfo>() { new ItemInfo() { id = 1, num = 1 }, new ItemInfo() { id = 2, num = 1 } };
        curEquipList = new List<ItemInfo>();
    }

    /// <summary>
    /// 添加道具
    /// </summary>
    /// <param name="info"></param>
    public void AddItem(ItemInfo info)
    {
        Item item = GameDataMgr.GetInstance().GetItemInfo(info.id);

        switch (item.type)
        {
            case (int)E_BagItemType.Prop:
                CheckList(propList, info);
                break;
            case (int)E_BagItemType.Equip:
                CheckList(equipList, info);
                break;
            case (int)E_BagItemType.Frag:
                CheckList(fragList, info);
                break;
        }
    }

    private void CheckList(List<ItemInfo> infoList, ItemInfo info)
    {
        for (int i = 0; i < infoList.Count; i++)
        {
            if (info.id == infoList[i].id && infoList[i].num + info.num <= 99)
            {
                infoList[i].num += info.num;
                return;
            }
            else if (info.id == infoList[i].id && infoList[i].num < 99 && infoList[i].num + info.num > 99)
            {
                int tempNum = infoList[i].num + info.num - 99;
                infoList[i].num = 99;
                CheckList(infoList, new ItemInfo() { id = info.id, num = tempNum }) ;
                return;
            }
            else if (i + 1 >= infoList.Count)
            {
                infoList.Add(info);
            }
        }
    }

    /// <summary>
    /// 减少金币
    /// </summary>
    /// <param name="money"></param>
    public void SubMoney(int money)
    {
        if (money < 0 && this.money < money)
            return;
        this.money -= money;
    }

    /// <summary>
    /// 减少钻石
    /// </summary>
    /// <param name="gem"></param>
    public void SubGem(int gem)
    {
        if (gem < 0 && this.gem < gem)
            return;
        this.gem -= gem;
    }

    public void AddMoney(int money)
    {
        this.money += money;
    }
    public void AddGem(int gem)
    {
        this.gem += gem;
    }
}

[System.Serializable]
public class ItemInfo
{
    public int id;
    public int num;
}