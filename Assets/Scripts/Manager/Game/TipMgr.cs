using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipMgr : BaseManager<TipMgr>
{
   public void ShowShopTipPanel(string info)
    {
        UIManager.GetInstance().ShowPanel<ShopTipPanel>("ShopTipPanel", E_UI_Layer.System, (panel) => {
            panel.InitInfo(info);
        });
    }
}
