using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCell : BasePanel
{
    private ShopCellInfo shopCellInfo;

    private void Start()
    {
        GetControl<Button>("BuyBtn").onClick.AddListener(BuyItem);
    }

    public void InitInfo(ShopCellInfo info)
    {
        shopCellInfo= info;
        Item item = GameDataMgr.GetInstance().GetItemInfo(info.itemInfo.id);
        //图标
        GetControl<Image>("IconImg").sprite = ResMgr.GetInstance().Load<Sprite>("Icon/" + item.icon);
        //名称
        GetControl<Text>("NameText").text = item.name;
        //PriceIconImg
        GetControl<Image>("PriceIconImg").sprite = ResMgr.GetInstance().Load<Sprite>("Icon/" + (info.priceType == 1 ? "Gold" : "Diamond"));
        //价格
        GetControl<Text>("PriceText").text = info.price.ToString();
        //描述信息
        GetControl<Text>("TipText").text = info.tips;
    }

    /// <summary>
    /// 购买道具
    /// </summary>
    private void BuyItem()
    {
        if(shopCellInfo.priceType == 1&&GameDataMgr.GetInstance().playerInfo.money>=shopCellInfo.priceType)
        {
            EventCenter.GetInstance().EventTrigger<int>("SubMoney", shopCellInfo.price);
            GameDataMgr.GetInstance().playerInfo.AddItem(shopCellInfo.itemInfo);
            TipMgr.GetInstance().ShowShopTipPanel("购买成功");

        }
        else if(shopCellInfo.priceType == 2 && GameDataMgr.GetInstance().playerInfo.gem >= shopCellInfo.priceType)
        {
            EventCenter.GetInstance().EventTrigger<int>("SubGem", shopCellInfo.price);
            GameDataMgr.GetInstance().playerInfo.AddItem(shopCellInfo.itemInfo);
            TipMgr.GetInstance().ShowShopTipPanel("购买成功");
        }
        else
        {
            TipMgr.GetInstance().ShowShopTipPanel("购买失败");
        }
    }
}
