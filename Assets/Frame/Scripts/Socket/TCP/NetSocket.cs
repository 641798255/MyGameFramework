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

public class NetSocket
{

    public delegate void CallBackNormal(bool succes, ErrorSocket errorType, string exception);
    //public delegate void CallBackSend(bool succes, ErrorSocket errorType, string exception);
    public delegate void CallBackRecv(bool succes, ErrorSocket errorType, string exception,byte[] byteMsg,string strMsg);
    //public delegate void CallBackDisConnect(bool succes, ErrorSocket errorType, string exception);

    private CallBackNormal callBackConnect;
    private CallBackNormal callBackSend;
    private CallBackNormal callBackDisConnect;
    private CallBackRecv callBackRecv;
    public enum ErrorSocket
    {
        Sucess,
        TimeOut,
        SocketNull,
        SocketUnConnect,
        ConnectSucces,
        ConnectUncessUnKnow,
        ConnectError,

        SendSucces,
        SendUnSuccesUnKnow,
        RecvUnSuccesUnKnow,

        DisConnectSucces,
        DisConnectUnknow
    }

    private ErrorSocket errorSocket;
    private Socket ClientSocket;
    private string AddressIp;
    private ushort Port;
    private SocketBuffer CalRecvBuffer;
    private byte[] RecvBuffer;

    public NetSocket()
    {
        CalRecvBuffer = new SocketBuffer(6, RecvMsgOverCallBack);
        RecvBuffer = new byte[1024];
    }


    #region 发起连接

    public bool IsConnet()
    {
        if (ClientSocket != null && ClientSocket.Connected)
        {
            return true;
        }
        return false;
    }

    public void AsycnConnet(string ip,ushort port,CallBackNormal connetCallBack,CallBackRecv recvCallBack)
    {
        errorSocket = ErrorSocket.Sucess;
        callBackConnect = connetCallBack;
        callBackRecv = recvCallBack;

        if (ClientSocket != null && ClientSocket.Connected)
        {
            errorSocket = ErrorSocket.ConnectError;

            callBackConnect(false, errorSocket, "connect repeat");
        }
        else if (ClientSocket == null || !ClientSocket.Connected)
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(ipAddress,port);
            IAsyncResult ar = ClientSocket.BeginConnect(endPoint,ConnetCallBack,ClientSocket);
            if (!WriteDot(ar))
            {
                connetCallBack(false,errorSocket,"连接超时");
            }
        }
    }

    private void ConnetCallBack(IAsyncResult ar)
    {
        try
        {
            ClientSocket.EndConnect(ar);
            if (ClientSocket.Connected == false)
            {
                errorSocket = ErrorSocket.ConnectUncessUnKnow;
                callBackConnect(false, errorSocket, "未知连接错误");
                return;
            }
            else
            {
                errorSocket = ErrorSocket.ConnectSucces;
                callBackConnect(true, errorSocket, "连接成功");

            }

        }
        catch (Exception e)
        {
            errorSocket = ErrorSocket.ConnectUncessUnKnow;
            callBackConnect(false, errorSocket, e.ToString());
        }
    }

    #endregion

    #region 接收数据

    public void AsyncRecv()
    {
        if (ClientSocket!= null && ClientSocket.Connected)
        {
            IAsyncResult ar= ClientSocket.BeginReceive(RecvBuffer, 0, RecvBuffer.Length, SocketFlags.None,RecvCallback,ClientSocket);
            if (!WriteDot(ar))
            {
                callBackRecv(false, errorSocket, "接收超时",null,"");
            }
        }
    }

    private void RecvCallback(IAsyncResult ar)
    {
        try
        {
            if (ClientSocket.Connected == false)
            {
                errorSocket = ErrorSocket.RecvUnSuccesUnKnow;
                callBackRecv(false, errorSocket, "未知接收错误",null,"");
                return;
            }
            int length = ClientSocket.EndReceive(ar);
            if (length == 0) return;
            else
            {
                CalRecvBuffer.RecvByte(RecvBuffer,length);
            }

        }
        catch (Exception e)
        {
            errorSocket = ErrorSocket.RecvUnSuccesUnKnow;
            callBackRecv(false, errorSocket, e.ToString(), null, "");
        }
        AsyncRecv();
    }

    #region 接收处理数据回调

    private void RecvMsgOverCallBack(byte[] allByte)
    {
        errorSocket = ErrorSocket.Sucess;
        callBackRecv(true, ErrorSocket.Sucess, "接收成功", allByte, "");
    }

    #endregion

    #endregion

    #region 发送数据

    public void AsycnSend(byte[] senbuffer, CallBackNormal sendCallBack)
    {
        errorSocket = ErrorSocket.Sucess;
        callBackSend = sendCallBack;
        if (ClientSocket == null)
        {
            errorSocket = ErrorSocket.SocketNull;

            callBackSend(false, errorSocket, "socket 不存在");
        }
        else if (!ClientSocket.Connected)
        {
            errorSocket = ErrorSocket.SocketUnConnect;
            callBackSend(false, errorSocket, "连接错误");
        }
        else
        {
            IAsyncResult ar=ClientSocket.BeginReceive(senbuffer, 0, senbuffer.Length, SocketFlags.None, SendCallBack, ClientSocket);
            if (!WriteDot(ar))
            {
                callBackSend(false, errorSocket, "发送超时");
            }
        }
    }

    private void SendCallBack(IAsyncResult ar)
    {
        try
        {
            int sendbyte = ClientSocket.EndSend(ar);
            if (sendbyte > 0)
            {
                callBackSend(true, ErrorSocket.SendSucces, "发送成功");
            }

        }
        catch (Exception e)
        {
            callBackSend(false, ErrorSocket.SendUnSuccesUnKnow, e.ToString());

        }
    }

    #endregion

    #region 断开连接

    public void AsyncDisConnect(CallBackNormal disConnectCallBack)
    {
        try
        {
            errorSocket = ErrorSocket.Sucess;
            callBackDisConnect = disConnectCallBack;
            if (ClientSocket == null)
            {
                disConnectCallBack(false, ErrorSocket.DisConnectUnknow, "socket不存在");
            }
            else if (!ClientSocket.Connected)
            {
                disConnectCallBack(false, ErrorSocket.DisConnectUnknow, "socket已经断开连接");
            }
            else
            {
                IAsyncResult ar=ClientSocket.BeginDisconnect(false,DisConnectCallBack,ClientSocket);
                if (!WriteDot(ar))
                {
                    callBackSend(false, errorSocket, "断开连接超时");

                }
            }

        }
        catch(Exception e)
        {
            callBackSend(false, ErrorSocket.DisConnectUnknow, e.ToString());

        }
    }

    private void DisConnectCallBack(IAsyncResult ar)
    {
        try
        {
            ClientSocket.EndDisconnect(ar);
            ClientSocket.Close();
            ClientSocket = null;
            callBackSend(true, ErrorSocket.DisConnectSucces, "断开成功");
        }
        catch (Exception e)
        {
            callBackSend(false, ErrorSocket.DisConnectUnknow, e.ToString());
        }
    }

    #endregion

    #region 检验超时

    private bool WriteDot(IAsyncResult ar)
    {
        int i = 0;
        while (ar.IsCompleted)
        {
            i++;
            if (i >= 20)
            {
                errorSocket = ErrorSocket.TimeOut;
                return false;
            }
            Thread.Sleep(100);
        }
        return true;
    }

    #endregion 
}
