using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TipsPanel : BasePanel
{
    
    public void InitInfo(ItemInfo info)
    {
        Item item = GameDataMgr.GetInstance().GetItemInfo(info.id);

        GetControl<Text>("NameText").text = item.name;
        GetControl<Text>("NumText").text = "数量: " + info.num;
        GetControl<Image>("IconImg").sprite = ResMgr.GetInstance().Load<Sprite>("Icon/" + item.icon);
        GetControl<Text>("DescText").text = item.tips;
    }
    
}
