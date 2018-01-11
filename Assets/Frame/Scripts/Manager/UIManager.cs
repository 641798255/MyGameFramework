/***
 * 
 *    Title: "" 游戏框架项目
 *           主题： UI管理类
 *    Description: 
 *           功能：
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase {

    public  static UIManager Instance = new UIManager();
    public Dictionary<string, GameObject> SonMember = new Dictionary<string, GameObject>();

    //public static UIManager GetInstance()
    //{
    //    if (Instance==null)
    //    {
    //        Instance = new UIManager();
    //    }
    //    return Instance;
    //}

    public void SendToMsg(MsgBase msg)
    {
        if (msg.GetManagerId() == ManagerId.UIManager)
        {
            ProcessEvent(msg);
        }
        else
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }

    public void RigestObj(string name, GameObject obj)
    {
        if (!SonMember.ContainsKey(name))
        {
            SonMember.Add(name, obj);
          
        }
    }

    public void UnRigestObj(string name, GameObject obj)
    {
        if (SonMember.ContainsKey(name))
        {
            SonMember.Remove(name);
        }
    }

    public GameObject GetObj(string name)
    {
        if (SonMember.ContainsKey(name))
        {
            return SonMember[name];
        }
        else
        {
            return null;
        }
    }

}
