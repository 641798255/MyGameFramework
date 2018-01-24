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

public delegate void UdpSocketDelegate(byte[] buffer,int dwCount,string temIp,ushort temPort);

public class UdpSocket : MonoBehaviour
{

    private UdpSocketDelegate UdpDelegate;
    private IPEndPoint UdpEnd;
    private Socket _UdpSocket;
    private byte[] RecvData;
    private Thread RecvThread;
    private bool IsRunning = true;

    public bool BindSocket(ushort port, int bufferLength,UdpSocketDelegate temdelegate)
    {
        UdpEnd = new IPEndPoint(IPAddress.Any,port);
        UdpConnect();
        UdpDelegate = temdelegate;
        RecvData = new byte[bufferLength];
        RecvThread = new Thread(RecvDataThread);
        RecvThread.Start();
        return true;
    }

    public void UdpConnect()
    {
        _UdpSocket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
        _UdpSocket.Bind(UdpEnd);
    }

    public void RecvDataThread()
    {
        while (IsRunning)
        {
            if (_UdpSocket == null || _UdpSocket.Available < 1)
            {
                Thread.Sleep(100);
                continue;
            }
            else
            {
                lock (this)
                {
                    IPEndPoint sender = new IPEndPoint(IPAddress.Any,0);
                    EndPoint remote = (EndPoint)sender;
                    int myCount = _UdpSocket.ReceiveFrom(RecvData,ref remote);
                    if (UdpDelegate != null)
                    {
                        UdpDelegate(RecvData,myCount,remote.AddressFamily.ToString(),(ushort)sender.Port);
                    }
                }
            }
        }
    }

    public int SendData(string ip,ushort port,byte[] data)
    {
        IPEndPoint sendToIp = new IPEndPoint(IPAddress.Parse(ip),port);
        if (!_UdpSocket.Connected)
        {
            UdpConnect();
        }
        int mySend = _UdpSocket.SendTo(data,data.Length,SocketFlags.None,sendToIp);
        return mySend;
    }

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
