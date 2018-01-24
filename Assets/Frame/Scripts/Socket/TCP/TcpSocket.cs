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

public enum TcpEvent
{
    TcpConnet=ManagerId.NetManager+1,
    TcpSend,


    MaxValue
}

public class TcpConnectMsg:MsgBase
{
    public string Ip;
    public ushort Port;

    public TcpConnectMsg(ushort temId,string ip, ushort port)
    {
        MsgId = temId;
        Ip = ip;
        Port = port;
    }
}

public class TcpMsg : MsgBase
{
    public NetMsgBase NetBase;

    public TcpMsg(ushort msgId,NetMsgBase temBase) 
    {
        MsgId = msgId;
        NetBase = temBase;
    }


}

public class TcpSocket : NetBase
{

    private NetWorkToServer Socket;

    public override void ProcessEvent(MsgBase msg)
    {
        switch (msg.MsgId)
        {
            case (ushort)TcpEvent.TcpConnet:
            {
                TcpConnectMsg temMsg = (TcpConnectMsg) msg;
                Socket = new NetWorkToServer(temMsg.Ip,temMsg.Port);
            }
                break;
            case (ushort)TcpEvent.TcpSend:
            {
                TcpMsg senMsg = (TcpMsg) msg;
                Socket.PutSendMsgToPool(senMsg.NetBase);
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
            (ushort)TcpEvent.TcpConnet,
            (ushort)TcpEvent.TcpSend
        };
        RigestSelf(this,MsgIds);


    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Socket != null)
	    {
	        Socket.Update();
	    }
	}
}
