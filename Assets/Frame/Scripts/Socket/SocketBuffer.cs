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

public class SocketBuffer
{

    private byte[] HeadData;
    private byte HeadLength=6;
    private byte[] AllRecvData;
    private int CurrentRecvLength;
    private int AllDataLength;

    public SocketBuffer(byte headLength,CallBackRecvOver callback)
    {
        HeadLength = headLength;
        HeadData = new byte[HeadLength];
        callBackRecv = callback;
    }

    public void RecvByte(byte[] recvByte,int realLength)
    {
        if (realLength < 0) return;
        if (CurrentRecvLength < HeadData.Length)
        {
            RecvHead(recvByte, realLength);
        }
        else
        {
            int temLength = CurrentRecvLength + realLength;
            if (temLength == AllDataLength)
            {
                RecvOneAll(recvByte, realLength);
            }
            else if (temLength > AllDataLength)
            {
                RecvLarge(recvByte, realLength);

            }
            else
            {
                RecvSmall(recvByte, realLength);

            }
        }
    }

    private void RecvHead(byte[] recvByte, int realLength)
    {
        int temReal = HeadData.Length - CurrentRecvLength;
        int temLength=CurrentRecvLength+realLength;
        if (temLength < HeadData.Length)
        {
            Buffer.BlockCopy(recvByte, 0, HeadData, CurrentRecvLength, realLength);
            CurrentRecvLength += realLength;
        }
        else
        {
            Buffer.BlockCopy(recvByte, 0, HeadData, CurrentRecvLength, temLength);
            CurrentRecvLength += temLength;
            AllDataLength = BitConverter.ToInt32(HeadData,0)+HeadLength;
            AllRecvData = new byte[AllDataLength];
            Buffer.BlockCopy(HeadData, 0, AllRecvData, 0, HeadLength);
            int temRemain = realLength - temReal;
            if (temRemain > 0)
            {
                byte[] temByte = new byte[temRemain];
                Buffer.BlockCopy(recvByte, temReal, temByte, 0, temRemain);
                RecvByte(temByte, temRemain);
            }
            else
            {
                RecvMsgOver();
            }
        }

    }

    void RecvOneAll(byte[] recvByte, int realLength)
    {
        Buffer.BlockCopy(recvByte, 0, AllRecvData, CurrentRecvLength, realLength);
        CurrentRecvLength += realLength;
        RecvMsgOver();
    }


    void RecvLarge(byte[] recvByte, int realLength)
    {
        int temLength = AllDataLength - CurrentRecvLength;
        Buffer.BlockCopy(recvByte, 0, AllRecvData, CurrentRecvLength, temLength);
        CurrentRecvLength += temLength;
        RecvMsgOver();

        int RemainLength = realLength - temLength;
        byte[] remainByte = new byte[RemainLength];
        Buffer.BlockCopy(recvByte, temLength, remainByte, 0, RemainLength);
        RecvByte(remainByte,RemainLength);

    }

    void RecvSmall(byte[] recvByte, int realLength)
    {
        Buffer.BlockCopy(recvByte, 0, AllRecvData, CurrentRecvLength, realLength);
        CurrentRecvLength += realLength;

    }


    public delegate void CallBackRecvOver(byte[] allData);

    private CallBackRecvOver callBackRecv;


    private void RecvMsgOver()
    {
        if (callBackRecv != null)
        {
            callBackRecv(AllRecvData);
        }
        CurrentRecvLength = 0;
        AllDataLength = 0;
        AllRecvData = null;

    }
}
