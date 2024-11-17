using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 游戏数据管理类
/// </summary>
public class GameDataMgr : BaseManager<GameDataMgr>
{
    private Dictionary<int,Item> itemDic = new Dictionary<int,Item>();
    private static string PlayerInfo_URL = Application.persistentDataPath+"/PlayerInfo.txt";

    public PlayerInfo playerInfo;   //玩家数据
    public List<ShopCellInfo> shopCellInfoList = new List<ShopCellInfo>();

    /// <summary>
    /// 初始化数据
    /// </summary>
   public void Init()
    {
        string info = ResMgr.GetInstance().Load<TextAsset>("Json/ItemInfo").text;
        Items items = JsonUtility.FromJson<Items>(info);
        for (int i = 0; i < items.itemList.Count; i++)
        {
            itemDic.Add(items.itemList[i].id, items.itemList[i]);
        }

        if(File.Exists(PlayerInfo_URL))
        {
            playerInfo = LoadPlayerInfo();

        }
        else
        {
            playerInfo = new PlayerInfo();

            SavePlayerInfo();
        }

        string shopInfo = ResMgr.GetInstance().Load<TextAsset>("Json/ShopInfo").text;
        Shops shops = JsonUtility.FromJson<Shops>(shopInfo);
        shopCellInfoList = shops.info;

        EventCenter.GetInstance().AddEventListener<int>("SubMoney", SubMoney);
        EventCenter.GetInstance().AddEventListener<int>("SubGem", SubGem);
        EventCenter.GetInstance().AddEventListener<int>("AddMoney", AddMoney);
        EventCenter.GetInstance().AddEventListener<int>("AddGem", AddGem);
    }

    /// <summary>
    /// 根据ID获取物品信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item GetItemInfo(int id)
    {
        if(itemDic.ContainsKey(id))
            return itemDic[id];
        return null;
    }

    /// <summary>
    /// 保存角色信息
    /// </summary>
    public void SavePlayerInfo()
    {
        string jsonStr = JsonUtility.ToJson(playerInfo);
        File.WriteAllBytes(PlayerInfo_URL, Encoding.UTF8.GetBytes(jsonStr));
    }
    /// <summary>
    /// 读取角色信息
    /// </summary>
    /// <returns></returns>
    public PlayerInfo LoadPlayerInfo()
    {
        return JsonUtility.FromJson<PlayerInfo>(Encoding.UTF8.GetString(File.ReadAllBytes(PlayerInfo_URL)));
    }

    private void SubMoney(int money)
    {
        playerInfo.SubMoney(money);
        SavePlayerInfo();
    }
    private void SubGem(int gem)
    {
        playerInfo.SubMoney(gem);
        SavePlayerInfo();
    }
    private void AddMoney(int money)
    {
        playerInfo.AddMoney(money);
        SavePlayerInfo();
    }
    private void AddGem(int gem)
    {
        playerInfo.AddGem(gem);
        SavePlayerInfo();
    }

}
