﻿/***
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

public class NetBase : MonoBase {

    public override void ProcessEvent(MsgBase msg)
    {
    }

    private void Awake()
    {
        //UIManager.GetInstance().RigestObj(gameObject.name, gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void RigestSelf(MonoBase script,params ushort[] msgs)
    {
        NetManager.Instance.RigestMsg(script,msgs);
    }

    public void UnRigestSelf(MonoBase script, params ushort[] msgs)
    {
        NetManager.Instance.UnRigestMsg(script, msgs);
    }

    public void SendMsg(MsgBase msg)
    {
        NetManager.Instance.SendToMsg(msg);
    }

    public ushort[] MsgIds;
    private void OnDestroy()
    {
        if (MsgIds!=null)
        {
            UnRigestSelf(this,MsgIds);
        }
    }
}
