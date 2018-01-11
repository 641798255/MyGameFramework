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


public class EventNode
{
    public MonoBase Data;
    public EventNode Next;
    public EventNode(MonoBase data)
    {
        Data = data;
        Next = null;
    }
}
public class ManagerBase : MonoBase {

    public Dictionary<ushort, EventNode> EventTree=new Dictionary<ushort, EventNode>();

    public override void ProcessEvent(MsgBase msg)
    {
        if (!EventTree.ContainsKey(msg.MsgId))
        {
            Debug.LogError("未注册MsgId====" + msg.MsgId);
            Debug.LogError("MsgManager===" + (ushort)msg.GetManagerId());
            return;
        }
        else
        {
            EventNode node = EventTree[msg.MsgId];
         
            do
            {
                node.Data.ProcessEvent(msg);
                Debug.Log(node.Data);
                node = node.Next;
            } while (node!=null);
        }
    }

    public void RigestMsg(MonoBase script,params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            EventNode node = new EventNode(script);
            RigestMsg(msgs[i],node);
        }
    }

    public void RigestMsg(ushort msgId, EventNode node)
    {
        if (!EventTree.ContainsKey(msgId))
        {
            EventTree.Add(msgId, node);

        }
        else
        {
            EventNode temp = EventTree[msgId];
            while (temp.Next!=null)
            {
                temp = temp.Next;
            }
            temp.Next = node;
        }
    }

    public void UnRigestMsg(MonoBase script, params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            UnRigestMsg(msgs[i],script);
        }
    }

    public void UnRigestMsg(ushort msgId, MonoBase node)
    {
        if (!EventTree.ContainsKey(msgId))
        {
            Debug.Log("Not Contain id====" + msgId);
            return;
        }
        else
        {
            EventNode tempNode= EventTree[msgId];
            if (tempNode.Data==node)
            {
                EventNode header = tempNode;
                if (header.Next != null)
                {
                    header.Data = tempNode.Next.Data;
                    header.Next = tempNode.Next.Next;
                }
                else
                {
                    EventTree.Remove(msgId);
                }

            }
            else
            {
                while (tempNode.Next!=null&&tempNode.Next.Data!=node)
                {
                    tempNode = tempNode.Next;
                }
                if (tempNode.Next != null&&tempNode.Next.Next != null)
                {
                    tempNode.Next = tempNode.Next.Next;
                }
                else
                {
                    tempNode.Next = null;
                }
            }
        }
    }

    
}
