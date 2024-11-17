using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameDataMgr.GetInstance().Init();
        UIManager.GetInstance().ShowPanel<MainPanel>("MainPanel", E_UI_Layer.Bot);
        BagMgr.GetInstance().Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
