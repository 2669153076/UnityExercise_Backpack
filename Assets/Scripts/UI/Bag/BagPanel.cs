using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

/// <summary>
/// 背包物品分类
/// </summary>
public enum E_BagItemType
{
    Prop=1,   //道具
    Equip =2,  //装备
    Frag=3    //碎片
}

/// <summary>
/// 背包面板
/// </summary>
public class BagPanel : BasePanel
{
    public Transform content;
    private List<ItemCell> cellList = new List<ItemCell>();

    private void Start()
    {
        GetControl<Button>("CloseBtn").onClick.AddListener(() =>
        {
            UIManager.GetInstance().HidePanel("BagPanel");
        });
    }

    private void OnEnable()
    {
        GetControl<Toggle>("PropTog").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("EquipTog").onValueChanged.AddListener(ToggleValueChange);
        GetControl<Toggle>("FragTog").onValueChanged.AddListener(ToggleValueChange);

        ChangeType(E_BagItemType.Prop);
    }

    private void ToggleValueChange(bool value)
    {
        if(GetControl<Toggle>("PropTog").isOn)
        {
            ChangeType(E_BagItemType.Prop);
        }
        else if (GetControl<Toggle>("EquipTog").isOn)
        {
            ChangeType(E_BagItemType.Equip);
        }
        else
        {
            ChangeType(E_BagItemType.Frag);
        }
    }

    public void ChangeType(E_BagItemType type)
    {
        List<ItemInfo> items = GameDataMgr.GetInstance().playerInfo.propList;

        switch (type)
        {
            case E_BagItemType.Equip:
                items = GameDataMgr.GetInstance().playerInfo.equipList;
                break;
            case E_BagItemType.Frag:
                items = GameDataMgr.GetInstance().playerInfo.fragList;
                break;
        }

        for (int i = 0; i < cellList.Count; i++)
        {
            Destroy(cellList[i].gameObject);
        }
        cellList.Clear();

        for (int i = 0; i < items.Count; i++)
        {
            ItemCell cell = ResMgr.GetInstance().Load<GameObject>("Items/ItemCell").GetComponent<ItemCell>();
            cell.transform.SetParent(content);
            cell.InitInfo(items[i]);
            cellList.Add(cell);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < cellList.Count; i++)
        {
            if (cellList[i] != null && cellList[i].gameObject != null)
            {
                Destroy(cellList[i].gameObject);
            }
        }

        cellList.Clear();

        GetControl<Toggle>("PropTog").onValueChanged.RemoveListener(ToggleValueChange);
        GetControl<Toggle>("EquipTog").onValueChanged.RemoveListener(ToggleValueChange);
        GetControl<Toggle>("FragTog").onValueChanged.RemoveListener(ToggleValueChange);
    }
}
