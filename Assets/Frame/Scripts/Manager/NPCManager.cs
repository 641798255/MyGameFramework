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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : ManagerBase {

    private static NPCManager Instance = null;
    private Dictionary<string, GameObject> SonMember = new Dictionary<string, GameObject>();

    public static NPCManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new NPCManager();
        }
        return Instance;
    }

    public void SendToMsg(MsgBase msg)
    {
        if (msg.GetManagerId() == ManagerId.NPCManager)
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
