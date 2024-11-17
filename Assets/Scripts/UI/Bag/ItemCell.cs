using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum E_ItemType
{
    Bag = 0,    //背包内
    Helm,   //头盔
    Amulet, //项链
    Weapon, //武器
    Clothes,    //衣服
    Belt,   //腰带
    Shoe    //鞋子
}

/// <summary>
/// 单个道具
/// </summary>
public class ItemCell : BasePanel
{
    private ItemInfo _itemInfo;
    private EventTrigger eventTrigger;

    public E_ItemType itemType = E_ItemType.Bag;

    public Image bgImg;
    public Image iconImg;

    private bool isOpenDrag = false;    

    public ItemInfo itemInfo
    {
        get { return _itemInfo; }
    }

    protected override void Awake()
    {
        base.Awake();
        iconImg = GetControl<Image>("IconImg");
        iconImg.gameObject.SetActive(false);
        bgImg = GetControl<Image>("BgImg");
        eventTrigger = bgImg.gameObject.GetComponent<EventTrigger>();  //Start的生命周期在InitInfo之后
    }

    private void Start()
    {
         //声明一个鼠标进入事件
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener(EnterItemCell);

        //声明一个鼠标退出事件
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener(ExitItemCell);


        eventTrigger.triggers.Add(enter);
        eventTrigger.triggers.Add(exit);
    }

    public void InitInfo(ItemInfo info)
    {
        this._itemInfo = info;

        if(info == null)
        {
            iconImg.gameObject.SetActive(false);
            return;
        }

        iconImg.gameObject.SetActive(true);

        Item item = GameDataMgr.GetInstance().GetItemInfo(info.id);
        iconImg.sprite = ResMgr.GetInstance().Load<Sprite>("Icon/" + item.icon);
        if(itemType == E_ItemType.Bag)
            GetControl<Text>("NumText").text = info.num.ToString();

        if (item.equipType != (int)E_ItemType.Bag)
        {
            OpenDragEvent();
        }
    }

    /// <summary>
    /// 添加拖曳事件
    /// </summary>
    private void OpenDragEvent()
    {
        if (isOpenDrag)
            return;
        isOpenDrag = true;

        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener(BeginDragItemCell);

        EventTrigger.Entry drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener(DragItemCell);

        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener(EndDragItemCell);

        eventTrigger.triggers.Add(beginDrag);
        eventTrigger.triggers.Add(drag);
        eventTrigger.triggers.Add(endDrag);
    }

    private void EnterItemCell(BaseEventData eventData)
    {

        EventCenter.GetInstance().EventTrigger<ItemCell>("EnterItemCell", this);

    }

    private void ExitItemCell(BaseEventData eventData)
    {

        EventCenter.GetInstance().EventTrigger<ItemCell>("ExitItemCell", this);

    }

    private void BeginDragItemCell(BaseEventData eventData)
    {
        EventCenter.GetInstance().EventTrigger<ItemCell>("BeginDragItemCell", this);
    }

    private void DragItemCell(BaseEventData eventData)
    {
        EventCenter.GetInstance().EventTrigger<BaseEventData>("DragItemCell", eventData);

    }

    private void EndDragItemCell(BaseEventData eventData)
    {
        EventCenter.GetInstance().EventTrigger<ItemCell>("EndDragItemCell", this);
    }


}
