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

public enum AssetEvent
{
    HunkRes=ManagerId.AssetManager+1,
    ReleaseSingleObj,
    ReleaseBundleObj,
    ReleaseScenesObj,
    ReleaseSingleBundle,
    ReleaseScenesBundle,
    ReleaseScenesBundleAndRes
}
public class HunkAssetRes:MsgBase
{
    public string SceneName;
    public string BundleName;
    public string ResName;
    public ushort BackMsgId;
    public bool IsSingle;

    public HunkAssetRes(ushort msgId,bool temSingle,string temSceneName,string  temBundleName,string temResName,ushort temBackMsgId)
    {
        MsgId = msgId;
        SceneName = temSceneName;
        BundleName = temBundleName;
        ResName = temResName;
        BackMsgId = temBackMsgId;
        IsSingle = temSingle;
    }
}

public class HunkAssetResBack : MsgBase
{
    public UnityEngine.Object[] values;

    public HunkAssetResBack()
    {
        MsgId = 0;
        values = null;
    }

    public void Change(ushort msgId, params UnityEngine.Object[] temValues)
    {
        MsgId = msgId;
        values = temValues;
    }

    public void Change(ushort msgId)
    {
        MsgId = msgId;
    }

    public void Change(params UnityEngine.Object[] temValues)
    {
        values = temValues;
    }

}

public class AssetEventMsg : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
