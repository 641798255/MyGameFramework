/***
 * 
 *    Title: "" 游戏框架项目
 *           主题：客户端panel
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

public class LoadPanel : UIBase {
    public override void ProcessEvent(MsgBase msg)
    {
        switch (msg.MsgId)
        {
            case (ushort)UIEventHu.Load:
                Debug.Log(111);
                break;
            case (ushort)UIEventHu.Rigest:
                Debug.Log(222);
                break;
            case (ushort)UIEventChen.Load:

                Debug.Log(333);
                break;
            default:
                break;
        }
    }
    // Use this for initialization
    private void Awake()
    {
        MsgIds = new ushort[] {
            (ushort)UIEventHu.Load,
            (ushort)UIEventHu.Rigest,
            (ushort)UIEventChen.Load,
            (ushort)UIEventChen.Rigest
        };
        RigestSelf(this, MsgIds);
    }

    void Start ()
	{
        UIManager.Instance.GetObj("BtnHaha").GetComponent<UIBehavior>().AddButtonClickListener(ButtonClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ButtonClick()
    {
        MsgBase loadRigest = new MsgBase((ushort)UIEventHu.Load);
        SendMsg(loadRigest);
        MsgBase rigestBase = new MsgBase((ushort)UIEventHu.Rigest);
        SendMsg(rigestBase);
        MsgBase msgTrans = new MsgBaseTransform((ushort)UIEventChen.Load,transform);
        SendMsg(msgTrans);
    }
}
