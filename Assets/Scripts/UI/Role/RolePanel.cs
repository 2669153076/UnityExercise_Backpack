using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RolePanel : BasePanel
{
    public ItemCell helmItem;
    public ItemCell amuletItem;
    public ItemCell weaponItem;
    public ItemCell clothesItem;
    public ItemCell beltItem;
    public ItemCell shoeItem;

    public override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case "CloseBtn":
                UIManager.GetInstance().HidePanel("RolePanel");
                break;
        }
    }

    public void UpdateRolePanel()
    {
        List<ItemInfo> curEquip = GameDataMgr.GetInstance().playerInfo.curEquipList;
        Item itemInfo;

        helmItem= null;
        amuletItem = null;
        weaponItem = null;
        clothesItem = null;
        beltItem = null;
        shoeItem = null;

        for (int i = 0; i < curEquip.Count; i++)
        {
            itemInfo = GameDataMgr.GetInstance().GetItemInfo(curEquip[i].id);
            switch (itemInfo.equipType)
            {
                case (int)E_ItemType.Helm:
                    helmItem.InitInfo(curEquip[i]);
                    break;
                case (int)E_ItemType.Amulet:
                    amuletItem.InitInfo(curEquip[i]);
                    break;
                case (int)E_ItemType.Weapon:
                    weaponItem.InitInfo(curEquip[i]);
                    break;
                case (int)E_ItemType.Clothes:
                    clothesItem.InitInfo(curEquip[i]);
                    break;
                case (int)E_ItemType.Belt:
                    beltItem.InitInfo(curEquip[i]);
                    break;
                case (int)E_ItemType.Shoe:
                    shoeItem.InitInfo(curEquip[i]);
                    break;
            }
        }
    }

    public override void ShowSelf()
    {
        base.ShowSelf();
        UpdateRolePanel();
    }
}
