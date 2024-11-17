using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private void Start()
    {
        this.GetControl<Button>("RoleBtn").onClick.AddListener(() =>
        {
            UIManager.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Bot);
            UIManager.GetInstance().ShowPanel<RolePanel>("RolePanel", E_UI_Layer.Bot);
        });
        this.GetControl<Button>("ShopBtn").onClick.AddListener(() =>
        {
            UIManager.GetInstance().ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Bot);
        });
        GetControl<Button>("AddMoneyBtn").onClick.AddListener(() =>
        {
            EventCenter.GetInstance().EventTrigger<int>("AddMoney",100);
        });
        GetControl<Button>("AddGemBtn").onClick.AddListener(() =>
        {
            EventCenter.GetInstance().EventTrigger<int>("AddGem",100);
        });
    }

    public override void ShowSelf()
    {
        base.ShowSelf();
        GetControl<Text>("NameText").text = GameDataMgr.GetInstance().playerInfo.name;
        GetControl<Text>("LevText").text = GameDataMgr.GetInstance().playerInfo.lev.ToString();
        GetControl<Text>("MoneyText").text = GameDataMgr.GetInstance().playerInfo.money.ToString();
        GetControl<Text>("GemText").text = GameDataMgr.GetInstance().playerInfo.gem.ToString();
        GetControl<Text>("ProText").text = GameDataMgr.GetInstance().playerInfo.pro.ToString();
        


        EventCenter.GetInstance().AddEventListener<int>("SubMoney", UpdatePanel);
        EventCenter.GetInstance().AddEventListener<int>("SubGem", UpdatePanel);
        EventCenter.GetInstance().AddEventListener<int>("AddMoney", UpdatePanel);
        EventCenter.GetInstance().AddEventListener<int>("AddGem", UpdatePanel);
    }

    private void UpdatePanel(int money)
    {
        GetControl<Text>("MoneyText").text = GameDataMgr.GetInstance().playerInfo.money.ToString();
        GetControl<Text>("GemText").text = GameDataMgr.GetInstance().playerInfo.gem.ToString();
    }

    public override void HideSelf()
    {
        base.HideSelf();

        EventCenter.GetInstance().RemoveEventListener<int>("SubMoney", UpdatePanel);
        EventCenter.GetInstance().RemoveEventListener<int>("SubGem", UpdatePanel);
        EventCenter.GetInstance().RemoveEventListener<int>("AddMoney", UpdatePanel);
        EventCenter.GetInstance().RemoveEventListener<int>("AddGem", UpdatePanel);
    }
}
