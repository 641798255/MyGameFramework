/***
 * 
 *    Title: "" 游戏框架项目
 *           主题： 消息传递中心
 *    Description: 
 *           功能：处理各manager模块之间的消息
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

public class MsgBase
{
    public ushort MsgId;

    
    public ManagerId GetManagerId()
    {
        int id= MsgId/FrameTools.MsgSpan;
        return (ManagerId)(id*FrameTools.MsgSpan);
    }

    public MsgBase()
    {
        MsgId = 0;
    }

    public MsgBase(ushort msgId)
    {
        MsgId = msgId;
    }
}

public class MsgBaseTransform:MsgBase
{
    public Transform tranform;
    
    public MsgBaseTransform(ushort msg,Transform temTransform):base(msg)
    {
        //MsgId = msg;
        tranform = temTransform;
    }
}
