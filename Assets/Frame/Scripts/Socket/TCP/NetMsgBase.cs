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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

public class NetMsgBase : MsgBase
{

    private byte[] Buffer;

    public NetMsgBase(byte[] arr)
    {
        Buffer = arr;
        MsgId = BitConverter.ToUInt16(arr,4);
    }

    public byte[] GetNetBytes()
    {
        return Buffer;
    }
}
