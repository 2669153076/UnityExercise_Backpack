using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTipPanel : BasePanel
{
    private void Start()
    {
        GetControl<Button>("EnterBtn").onClick.AddListener(() =>
        {
            UIManager.GetInstance().HidePanel("ShopTipPanel");
        });
    }

    public void InitInfo(string str)
    {
        GetControl<Text>("TipText").text = str;
    }
}
