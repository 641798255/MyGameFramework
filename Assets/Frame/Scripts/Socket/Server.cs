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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;


public class SocketState
{
    public Socket _Socket;
    public Byte[] Buffer;

    public SocketState(Socket socket)
    {
        _Socket = socket;
        Buffer = new Byte[1024];
    }

    #region 接收数据
    public void BeginRecv()
    {
        _Socket.BeginReceive(Buffer,0,1024,SocketFlags.None,RecvCallBack,this);
    }
  

    private void RecvCallBack(IAsyncResult ar)
    {
        int length = _Socket.EndReceive(ar);
        string temString = Encoding.Default.GetString(Buffer,0,length);
        Debug.Log("server recv======"+temString);

        Send(temString);
    }
    #endregion

    #region 发送数据

    public void Send(string temString)
    {
        Byte[] data = Encoding.Default.GetBytes(temString);
        _Socket.BeginSend(data,0,data.Length,SocketFlags.None,SendCallBack,this);
    }

    private void SendCallBack(IAsyncResult ar)
    {
        int length = _Socket.EndSend(ar);
        Debug.Log("server sendcount======" + length);
  


    }

    #endregion
}

public class Server :MonoBehaviour{ 

    private Socket _ServerSocket;
    private List<SocketState> SocketArr;


    private void Start()
    {
        InitialServer();
    }

    public void InitialServer()
    {
        IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 10010);
        _ServerSocket = new Socket(endpoint.AddressFamily, SocketType.Stream,ProtocolType.Tcp);
        _ServerSocket.Bind(endpoint);
        _ServerSocket.Listen(100);
        SocketArr = new List<SocketState>();
        Thread acceptThread = new Thread(ListenAccept);
        acceptThread.Start();
    }

    #region 接收请求

    private bool IsRunnning = true;
    private void ListenAccept()
    {
        while (IsRunnning)
        {
            try
            {
                _ServerSocket.BeginAccept(new AsyncCallback(AsyncAcceptCallback),_ServerSocket);
            }
            catch(Exception e)
            {
            }
            Thread.Sleep(1000);
        }
    }

     void AsyncAcceptCallback(IAsyncResult ar)
     {
         Socket listener = (Socket)ar.AsyncState;
         Socket clientSocket = listener.EndAccept(ar);
         SocketState temState = new SocketState(clientSocket);
         SocketArr.Add(temState);
     }

    #endregion
    #region 接收数据



    #endregion

    void Update()
    {
        if (SocketArr.Count>0)
        {
            for (int i = 0; i < SocketArr.Count; i++)
            {
                SocketArr[i].BeginRecv();
            }
        }
    }

    private void OnApplicationQuit()
    {
        _ServerSocket.Shutdown(SocketShutdown.Both);
        _ServerSocket.Close();
    }


}
