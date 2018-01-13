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
using System;

public delegate void NativeResCallBackDelegate(NativeResCallBackNode temNode);

public class NativeResCallBackNode
{
    public string SceneName;
    public string BundleName;
    public string ResName;
    public ushort BackMsgId;
    public bool IsSingle;
    public NativeResCallBackNode Next;
    public NativeResCallBackDelegate CallBack;

    public NativeResCallBackNode(bool temSingle, string temSceneName, string temBundleName, string temResName, ushort temBackMsgId,NativeResCallBackDelegate temCallBack,NativeResCallBackNode temNext)
    {
        SceneName = temSceneName;
        BundleName = temBundleName;
        ResName = temResName;
        BackMsgId = temBackMsgId;
        IsSingle = temSingle;
        Next = temNext;
        CallBack = temCallBack;
    }

    public void Dispose()
    {
        CallBack = null;
        Next = null;
    }
}

public class NativeResCallBackNodeManager
{
    private Dictionary<string, NativeResCallBackNode> NodeManager = null;

    public NativeResCallBackNodeManager()
    {
        NodeManager = new Dictionary<string, NativeResCallBackNode>();
    }

    public void AddBundle(string bundleName, NativeResCallBackNode temNode)
    {
        if (NodeManager.ContainsKey(bundleName))
        {
            NativeResCallBackNode node = NodeManager[bundleName];
            while (node.Next != null)
            {
                node = node.Next;
            }
            node.Next = temNode;
        }
        else
        {
            NodeManager.Add(bundleName,temNode);
        }
    }

    public void Dispose(string bundleName)
    {
        if (NodeManager.ContainsKey(bundleName))
        {
           
            NativeResCallBackNode node = NodeManager[bundleName];
            while (node.Next != null)
            {
                NativeResCallBackNode temNode = node;
                node = node.Next;
                temNode.Dispose();
            }
            node.Dispose();
            NodeManager.Remove(bundleName);
        }
    }

    public void BundleCallBack(string bundleName)
    {
        if (NodeManager.ContainsKey(bundleName))
        {
            NativeResCallBackNode node = NodeManager[bundleName];
            do
            {
                node.CallBack(node);
                node = node.Next;
            } while (node!=null);
        }
    }
}

public class NativeResLoader :AssetBase
{


    private HunkAssetResBack _HunkBackMsg = null;

    public HunkAssetResBack HunkBackMsg
    {
        get {
            if (_HunkBackMsg == null)
            {
                _HunkBackMsg = new HunkAssetResBack();
            }
            return _HunkBackMsg; }
    }

    public NativeResCallBackNodeManager CallbackManager
    {
        get
        {
            if (_CallbackManager == null)
            {
                _CallbackManager = new NativeResCallBackNodeManager();
            }
            return _CallbackManager;
        }
    }

    private NativeResCallBackNodeManager _CallbackManager = null;

    public override void ProcessEvent(MsgBase msg)
    {
        switch (msg.MsgId)
        {
                 case (ushort)AssetEvent.HunkRes:
                 {
                    HunkAssetRes hunkResMsg = (HunkAssetRes)msg;
                     GetResources(hunkResMsg.SceneName,hunkResMsg.BundleName,hunkResMsg.ResName,hunkResMsg.IsSingle,hunkResMsg.BackMsgId);
                 }
                break;
            case (ushort)AssetEvent.ReleaseBundleObj:
                {
                    HunkAssetRes hunkResMsg = (HunkAssetRes)msg;
                    ILoaderManager._Instance.UnLoadBundleRes(hunkResMsg.SceneName,hunkResMsg.BundleName);
                }
                break;
            case (ushort)AssetEvent.ReleaseScenesBundle:
                {
                    HunkAssetRes hunkResMsg = (HunkAssetRes)msg;
                    ILoaderManager._Instance.UnLoadAllBundles(hunkResMsg.SceneName);
                }
                break;
            case (ushort)(ushort)AssetEvent.ReleaseScenesBundleAndRes:
                {
                    HunkAssetRes hunkResMsg = (HunkAssetRes)msg;
                    ILoaderManager._Instance.UnLoadAllBundleAndRes(hunkResMsg.SceneName);
                }
                break;
            case (ushort)(ushort)AssetEvent.ReleaseScenesObj:
                {
                    HunkAssetRes hunkResMsg = (HunkAssetRes)msg;
                    ILoaderManager._Instance.UnLoadAllRes(hunkResMsg.SceneName);
                }
                break;
            case (ushort)(ushort)AssetEvent.ReleaseSingleBundle:
                {
                    HunkAssetRes hunkResMsg = (HunkAssetRes)msg;
                    ILoaderManager._Instance.UnLoadAeestBundle(hunkResMsg.SceneName,hunkResMsg.BundleName);
                }
                break;
            case (ushort)(ushort)AssetEvent.ReleaseSingleObj:
                {
                    HunkAssetRes hunkResMsg = (HunkAssetRes)msg;
                    ILoaderManager._Instance.UnloadResObj(hunkResMsg.SceneName, hunkResMsg.BundleName, hunkResMsg.ResName);
                }
                break;
        }
    }

    
    void Awake()
    {
        MsgIds = new ushort[]
        {
          (ushort)AssetEvent.HunkRes,
          (ushort)AssetEvent.ReleaseBundleObj,
          (ushort)AssetEvent.ReleaseScenesBundle,
          (ushort)AssetEvent.ReleaseScenesBundleAndRes,
          (ushort)AssetEvent.ReleaseScenesObj,
          (ushort)AssetEvent.ReleaseSingleBundle,
          (ushort)AssetEvent.ReleaseSingleObj
        };
        RigestSelf(this,MsgIds);
    }

    public void SendMsgBack(NativeResCallBackNode temNode)
    {
        if (temNode.IsSingle)
        {
            UnityEngine.Object temObj = ILoaderManager._Instance.GetSingleRes(temNode.SceneName, temNode.BundleName, temNode.ResName);
            this.HunkBackMsg.Change(temNode.BackMsgId, temObj);
            SendMsg(HunkBackMsg);
        }
        else
        {
            UnityEngine.Object[] temObj = ILoaderManager._Instance.GetMutiRes(temNode.SceneName, temNode.BundleName,temNode.ResName);
            this.HunkBackMsg.Change(temNode.BackMsgId, temObj);
            SendMsg(HunkBackMsg);
        }
    }

    private void LoadProgress(string bundleName, float progress)
    {
        if (progress >= 1.0f)
        {
            CallbackManager.BundleCallBack(bundleName);
            CallbackManager.Dispose(bundleName);
        }
    }

    public void GetResources(string sceneName, string bundleName, string resName, bool isSingle, ushort backMsgId)
    {
        if (!ILoaderManager._Instance.IsLoadingBundle(sceneName, bundleName))
        {
            ILoaderManager._Instance.LoadAsset(sceneName, bundleName, LoadProgress);
            string fullName = ILoaderManager._Instance.GetBundleName(sceneName, bundleName);
            if (fullName != null)
            {
                NativeResCallBackNode node = new NativeResCallBackNode(isSingle, sceneName, fullName, resName, backMsgId, SendMsgBack, null);
                CallbackManager.AddBundle(fullName, node);
            }
            else
            {
                Debug.Log("not contain bundle=======" + bundleName);
            }
        }
        else if (ILoaderManager._Instance.IsLoadBundleFinish(sceneName, bundleName))
        {
            if (isSingle)
            {
                UnityEngine.Object temObj = ILoaderManager._Instance.GetSingleRes(sceneName, bundleName, resName);
                this.HunkBackMsg.Change(backMsgId, temObj);
                SendMsg(HunkBackMsg);
            }
            else
            {
                UnityEngine.Object[] temObj = ILoaderManager._Instance.GetMutiRes(sceneName, bundleName, resName);
                this.HunkBackMsg.Change(backMsgId, temObj);
                SendMsg(HunkBackMsg);
            }
        }
        else
        {
            string fullName = ILoaderManager._Instance.GetBundleName(sceneName, bundleName);
            if (fullName != null)
            {
                NativeResCallBackNode node = new NativeResCallBackNode(isSingle, sceneName, fullName, resName, backMsgId, SendMsgBack, null);
                CallbackManager.AddBundle(fullName, node);
            }
            else
            {
                Debug.Log("not contain bundle=======" + bundleName);
            }
        }


    }
}
