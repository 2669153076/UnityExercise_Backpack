using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagMgr : BaseManager<BagMgr>
{
    private ItemCell curSelItemCell;    //当前选中的道具格子
    private ItemCell willEnterItemCell; //将要拖进的道具格子

    private Image curSelItemImg;    //当前选中的道具图片精灵

    private bool isDrag = false;


    public void Init()
    {
        EventCenter.GetInstance().AddEventListener<ItemCell>("BeginDragItemCell", BeginDragItemCell);
        EventCenter.GetInstance().AddEventListener<BaseEventData>("DragItemCell", DragItemCell);
        EventCenter.GetInstance().AddEventListener<ItemCell>("EndDragItemCell", EndDragItemCell);
        EventCenter.GetInstance().AddEventListener<ItemCell>("EnterItemCell", EnterItemCell);
        EventCenter.GetInstance().AddEventListener<ItemCell>("ExitItemCell", ExitItemCell);
    }

    private void EnterItemCell(ItemCell itemCell)
    {
        if (isDrag)
        {
            willEnterItemCell = itemCell;
            return;
        }

        if(itemCell.itemInfo == null)
        {
            return;
        }

        UIManager.GetInstance().ShowPanel<TipsPanel>("TipsPanel", E_UI_Layer.Top, (panel) =>
        {
            //更新显示信息
            panel.InitInfo(itemCell.itemInfo);
            //更新位置
            //panel.transform.position = GetControl<Image>("IconImg").transform.position;
            panel.transform.position = itemCell.bgImg.transform.position;

            if (isDrag)
            {
                UIManager.GetInstance().HidePanel("TipsPanel");
            }
        });
    }

    private void ExitItemCell(ItemCell itemCell)
    {
        if (isDrag) { willEnterItemCell = null; return; }


        if (itemCell.itemInfo == null)
        {
            return;
        }

        UIManager.GetInstance().HidePanel("TipsPanel");
    }

    private void BeginDragItemCell(ItemCell itemCell)
    {
        UIManager.GetInstance().HidePanel("TipsPanel");
        isDrag = true;
        curSelItemCell = itemCell;

        PoolMgr.GetInstance().Get("Items/EquipIconImg", (_obj) =>
        {
            curSelItemImg = _obj.GetComponent<Image>();
            curSelItemImg.sprite = itemCell.iconImg.sprite;

            curSelItemImg.transform.SetParent(UIManager.GetInstance().canvas);
            curSelItemImg.transform.localScale = Vector3.one;

            if(!isDrag)
            {
                //如果异步加载结束 拖动结束 将物体放入缓存池
                PoolMgr.GetInstance().Push(curSelItemImg.name, curSelItemImg.gameObject);
                curSelItemImg = null;
            }
        });
    }

    private void DragItemCell(BaseEventData eventData)
    {
        if(curSelItemImg == null) return;

        Vector2 localPos;
        //坐标转换 屏幕坐标转本地坐标
        //参数一：想要改变的目标对象的父对象
        //参数二：屏幕上的点，相当于鼠标位置
        //参数三：摄像机，相当于UI摄像机
        //参数四：转换后的点
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.GetInstance().canvas, (eventData as PointerEventData).position, (eventData as PointerEventData).pressEventCamera, out localPos);
        curSelItemImg.transform.localPosition = localPos;

    }

    private void EndDragItemCell(ItemCell itemCell)
    {
        isDrag= false;

        UpdateEquipIconImg();

        curSelItemCell = null;
        willEnterItemCell = null;
        if(curSelItemImg == null) return;

        PoolMgr.GetInstance().Push(curSelItemImg.name,curSelItemImg.gameObject);
        curSelItemImg = null;
    }

    /// <summary>
    /// 切换装备
    /// </summary>
    public void UpdateEquipIconImg()
    {
        //如果当前选中的格子类型是背包道具
        if(curSelItemCell.itemType==E_ItemType.Bag)
        {
            //如果willEnterItemCell(将要进入的格子)不为空 并且类型不是背包类型
            if (willEnterItemCell!=null&&willEnterItemCell.itemType!=E_ItemType.Bag)
            {
                Item info = GameDataMgr.GetInstance().GetItemInfo(curSelItemCell.itemInfo.id);
                //如果将要进入的格子类型和当前选中的格子类型相同
                if((int)willEnterItemCell.itemType == info.equipType)
                {
                    //如果将要进入的格子内为空
                    if(willEnterItemCell.itemInfo == null)
                    {
                        GameDataMgr.GetInstance().playerInfo.curEquipList.Add(curSelItemCell.itemInfo); //装备列表中添加
                        GameDataMgr.GetInstance().playerInfo.equipList.Remove(curSelItemCell.itemInfo); //背包武器列表中移除

                        
                    }
                    else
                    {
                        //置换装备
                        GameDataMgr.GetInstance().playerInfo.curEquipList.Remove(willEnterItemCell.itemInfo); //装备列表中移除
                        GameDataMgr.GetInstance().playerInfo.curEquipList.Add(curSelItemCell.itemInfo); //装备列表中添加
                        GameDataMgr.GetInstance().playerInfo.equipList.Remove(curSelItemCell.itemInfo); //背包武器列表中移除
                        GameDataMgr.GetInstance().playerInfo.equipList.Add(willEnterItemCell.itemInfo); //背包武器列表中添加

                    }
                    UIManager.GetInstance().GetPanel<BagPanel>("BagPanel").ChangeType(E_BagItemType.Equip); //更新背包面板，使武器页签显示
                    UIManager.GetInstance().GetPanel<RolePanel>("RolePanel").UpdateRolePanel(); //更新角色面板
                    GameDataMgr.GetInstance().SavePlayerInfo();
                }
            }
        }
        else
        {
            //将装备拖到一个空的地方或者拖到别的装备栏
            if (willEnterItemCell == null||willEnterItemCell.itemType!=E_ItemType.Bag)
            {
                GameDataMgr.GetInstance().playerInfo.curEquipList.Remove(willEnterItemCell.itemInfo);
                GameDataMgr.GetInstance().playerInfo.equipList.Add(willEnterItemCell.itemInfo);
            }
            else if(willEnterItemCell!=null||willEnterItemCell.itemType==E_ItemType.Bag) 
            {
                Item info = GameDataMgr.GetInstance().GetItemInfo(curSelItemCell.itemInfo.id);

                //置换装备
                GameDataMgr.GetInstance().playerInfo.curEquipList.Remove(willEnterItemCell.itemInfo); //装备列表中移除
                GameDataMgr.GetInstance().playerInfo.curEquipList.Add(curSelItemCell.itemInfo); //装备列表中添加
                GameDataMgr.GetInstance().playerInfo.equipList.Remove(curSelItemCell.itemInfo); //背包武器列表中移除
                GameDataMgr.GetInstance().playerInfo.equipList.Add(willEnterItemCell.itemInfo); //背包武器列表中添加


            }
        }
    }
}
