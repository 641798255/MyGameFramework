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

public class Client : MonoBehaviour
{

     Socket ClientSocket;
    private SocketBuffer socketBuffer;
        
    Byte[] data = new Byte[1024];
	// Use this for initialization
	void Start ()
	{
	    socketBuffer = new SocketBuffer(6, RecvMsgOver);
	    Initial();
	}

    private void Initial()
    {
        IPAddress address = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(address,10010);
        ClientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        ClientSocket.BeginConnect(endPoint,ConnetCallBack,ClientSocket);
    }

    #region 处理数据长度回调

    private void RecvMsgOver(byte[] allData)
    {

    }

    #endregion

    #region  连接回调

    private void ConnetCallBack(IAsyncResult ar)
    {
        ClientSocket.EndConnect(ar);
        Debug.Log("connet with server finish");
     

    }

    #endregion

    #region 接收数据
    public void BeginRecv()
    {
        ClientSocket.BeginReceive(data, 0, 1024, SocketFlags.None, RecvCallBack, this);
    }


    private void RecvCallBack(IAsyncResult ar)
    {
        int length = ClientSocket.EndReceive(ar);
        socketBuffer.RecvByte(data,length);
        //string temString = Encoding.Default.GetString(data, 0, length);
        //Debug.Log("client recv======" + temString);
    }
    #endregion

    #region 发送数据

    public void Send(string temString)
    {
        Byte[] data = Encoding.Default.GetBytes(temString);
        ClientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallBack, this);
    }

    private void SendCallBack(IAsyncResult ar)
    {
        int length = ClientSocket.EndSend(ar);
        Debug.Log("client sendcount======" + length);
    }

    #endregion

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Send("12345678");
        }
        BeginRecv();
    }
}
