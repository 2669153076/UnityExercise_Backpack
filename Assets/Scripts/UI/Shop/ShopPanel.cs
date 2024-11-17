using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : BasePanel
{
    public Transform content;
    private void Start()
    {
        GetControl<Button>("CloseBtn").onClick.AddListener(() =>
        {
            UIManager.GetInstance().HidePanel("ShopPanel");
        });
    }

    public override void ShowSelf()
    {
        base.ShowSelf();
        for (int i = 0; i<GameDataMgr.GetInstance().shopCellInfoList.Count; i++)
        {
            ShopCell cell = ResMgr.GetInstance().Load<GameObject>("Items/ShopCell").GetComponent<ShopCell>();
            cell.transform.SetParent(content);
            cell.transform.localScale = Vector3.one;    //设置相对缩放大小 避免显示出错
            cell.InitInfo(GameDataMgr.GetInstance().shopCellInfoList[i]);
        }
    }
}
