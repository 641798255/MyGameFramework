/***
 * 
 *    Title: "" 游戏框架项目
 *           主题： 工具类
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

public enum ManagerId
{
    GameManager = 0,
    UIManager = FrameTools.MsgSpan,
    AssetManager = FrameTools.MsgSpan * 2,
    AudioManager = FrameTools.MsgSpan * 3,
    NPCManager = FrameTools.MsgSpan * 4,
    CharactorManager = FrameTools.MsgSpan * 5,
    NetManager = FrameTools.MsgSpan * 6,
    UIManager2 = FrameTools.MsgSpan * 7
}
public class FrameTools
{
    public const int MsgSpan=3000;

}
