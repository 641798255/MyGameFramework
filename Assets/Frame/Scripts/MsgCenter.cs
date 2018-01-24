/***
 * 
 *    Title: "" XXX项目
 *           主题： 
 *    Description: 
 *           功能：
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgCenter : MonoBehaviour {

    void Awake()
    {
        Instance = new MsgCenter();
        //Instance = GetInstance();
        //gameObject.AddComponent<UIManager>();
        //gameObject.AddComponent<NPCManager>();
    }

    public static MsgCenter Instance;

    //public static MsgCenter GetInstance()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = new MsgCenter();
    //    }
    //    return Instance;
    //}
      public void SendToMsg(MsgBase msg)
    {
        AnalysisMsg(msg);
    }

    private void AnalysisMsg(MsgBase msg)
    {
        ManagerId id = msg.GetManagerId();
        switch (id)
        {
            case ManagerId.GameManager:
                break;
            case ManagerId.UIManager:
                UIManager.Instance.ProcessEvent(msg);
                break;
            case ManagerId.AssetManager:
                break;
            case ManagerId.AudioManager:
                break;
            case ManagerId.NPCManager:
                break;
            case ManagerId.CharactorManager:
                break;
            case ManagerId.NetManager:
                break;
            case ManagerId.UIManager2:
                break;
            default:
                break;
        }
    }
}
