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

public class NetWorkToServer
{
    private Queue<NetMsgBase> RecvMsgPool;
    private Queue<NetMsgBase> SendMsgPool;
    private NetSocket ClientSocket;
    private Thread sendThread;



    public NetWorkToServer(string ip,ushort port)
    {
        RecvMsgPool = new Queue<NetMsgBase>();
        SendMsgPool = new Queue<NetMsgBase>();
        ClientSocket = new NetSocket();
        ClientSocket.AsycnConnet(ip,port,AsycnConnetCallBack,AsycnRecvCallBack);
    }

    private void AsycnConnetCallBack(bool success,NetSocket.ErrorSocket errorSocket,string exception)
    {
        if (success)
        {
            sendThread = new Thread(LoopSendMsg);
            sendThread.Start();
        }
    }

    #region 发送消息

    public void PutSendMsgToPool(NetMsgBase msg)
    {
        lock (SendMsgPool)
        {
            SendMsgPool.Enqueue(msg);
        }
    }

    private void LoopSendMsg()
    {
        while (ClientSocket != null && ClientSocket.IsConnet())
        {
            lock (SendMsgPool)
            {
                while (SendMsgPool.Count > 0)
                {
                    NetMsgBase temBody = SendMsgPool.Dequeue();
                    ClientSocket.AsycnSend(temBody.GetNetBytes(), CallBackSend);
                }
            }

            Thread.Sleep(100);
        }
    }

    private void CallBackSend(bool success, NetSocket.ErrorSocket errorSocket, string exception)
    {

    }

    #endregion

    #region 接收消息
    private void AsycnRecvCallBack(bool success, NetSocket.ErrorSocket errorSocket, string exception,byte[] recvBuffer,string recvMsg)
    {
        if (success)
        {
            PutRecvMsgToPool(recvBuffer);
        }
        else
        {

        }

    }
    
    private void PutRecvMsgToPool(byte[] recvMsg)
    {
        NetMsgBase temMsg = new NetMsgBase(recvMsg);
        RecvMsgPool.Enqueue(temMsg);
    }

    public  void Update()
    {
        if (RecvMsgPool != null)
        {
            while (RecvMsgPool.Count > 0)
            {
                NetMsgBase temMsg = RecvMsgPool.Dequeue();
                AnalysisMsgData(temMsg);
            }
        }
    }

    private void AnalysisMsgData(NetMsgBase msg)
    {
        MsgCenter.Instance.SendToMsg(msg);
    }

    #endregion

    
    #region 断开连接

    private void CallBackDisConnect(bool success, NetSocket.ErrorSocket errorSocket, string exception)
    {
        if (success)
        {
            sendThread.Abort();
        }
    }

    public void DisConnect()
    {
        if (ClientSocket != null && ClientSocket.IsConnet())
        {
            ClientSocket.AsyncDisConnect(CallBackDisConnect);
        }
    }

    #endregion
}
