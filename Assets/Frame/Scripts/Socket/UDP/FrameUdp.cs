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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using System.IO;

public enum UdpEvent
{
    Initial=TcpEvent.MaxValue+1,
    SendTo,
    MaxValue
}

public class UdpMsg : MsgBase
{
    public ushort Port;
    public int RecvBufLength;
    public UdpSocketDelegate RecvDelegate;

    public UdpMsg(ushort msgId, ushort port, int recvBufLength, UdpSocketDelegate recvDelegate)
    {
        MsgId = msgId;
        Port = port;
        RecvBufLength = recvBufLength;
        RecvDelegate = recvDelegate;
    }
}

public class UdpSendMsg : MsgBase
{
    public string Ip;
    public ushort Port;
    public byte[] SendData;

    public UdpSendMsg(ushort msgId, string ip, ushort port, byte[] sendData)
    {
        MsgId = msgId;
        Ip = ip;
        Port = port;
        SendData = sendData;
    }
}


public class FrameUdp : NetBase
{

    private UdpSocket _UdpSocket;

    public override void ProcessEvent(MsgBase msg)
    {
        switch (msg.MsgId)
        {
            case (ushort)UdpEvent.Initial:
            {
                UdpMsg temMsg = (UdpMsg) msg;
                _UdpSocket = new UdpSocket();
                _UdpSocket.BindSocket(temMsg.Port,temMsg.RecvBufLength,temMsg.RecvDelegate);
            }
                break;
            case (ushort)UdpEvent.SendTo:
                {
                    UdpSendMsg temMsg = (UdpSendMsg)msg;
                    _UdpSocket.SendData(temMsg.Ip,temMsg.Port,temMsg.SendData);
                }
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        MsgIds = new ushort[]
     {
            (ushort)UdpEvent.Initial,
            (ushort)UdpEvent.SendTo
     };
        RigestSelf(this, MsgIds);
    }
}
