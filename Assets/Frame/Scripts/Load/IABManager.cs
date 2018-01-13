/***
 * 
 *    Title: "" XXX项目
 *           主题：一个场景的所有bundle包管理
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
using System.IO;

/// <summary>
/// 每个Asset存在多个objs
/// </summary>
public class AssetObj
{
    public List<UnityEngine.Object> objs;

    public AssetObj(params UnityEngine.Object[] temObjs)
    {
        objs = new List<UnityEngine.Object>();
        objs.AddRange(temObjs);
    }

    public void ReleaseObj()
    {
        for (int i = 0; i <objs.Count; i++)
        {
            Resources.UnloadAsset(objs[i]);
        }
    }
}

/// <summary>
/// 一个bundle里存在多个Asset
/// </summary>
public class AssetObjs
{
    public Dictionary<string, AssetObj> objsTree;

    public AssetObjs(string name,AssetObj temObjs)
    {
        objsTree = new Dictionary<string, AssetObj>();
        objsTree.Add(name,temObjs);
    }

    public void AddAssetObj(string name, AssetObj temObjs)
    {
        objsTree.Add(name,temObjs);
    }

    public void ReleaseAllAssetObjs()
    {
        List<string> keys = new List<string>();
        keys.AddRange(objsTree.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            ReleaseAssetObj(keys[i]);
        }
    }

    public void ReleaseAssetObj(string name)
    {
        if (objsTree.ContainsKey(name))
        {
            objsTree[name].ReleaseObj();
        }
        else
        {
            Debug.LogError("tree not contain asset===="+name);
        }
    }

    public List<UnityEngine.Object> GetAssetObjs(string name)
    {
        if (objsTree.ContainsKey(name))
        {
            return objsTree[name].objs;
        }
        else
        {
            Debug.LogError("tree not contain asset====" + name);
            return null;
        }
    }
}

public delegate void LoadAssetBundleCallBack(string sceneName,string bundleName);
public class IABManager
{

    private Dictionary<string, IABRelationManager> LoadHelp = new Dictionary<string, IABRelationManager>();
    private Dictionary<string, AssetObjs> LoadObjs = new Dictionary<string, AssetObjs>();
    private string sceneName;

    public IABManager(string temSceneName)
    {
        sceneName = temSceneName;
    }

    public bool IsLoadingAssetBundle(string bundleName)
    {
        if (LoadHelp.ContainsKey(bundleName))
        {
            return true;
        }
        else
        {
            Debug.LogError("LoadHelp not contain bundle=====" + bundleName);
            return false;
        }
    }

    private string[] GetDependences(string bundleName)
    {
        return IABManifestLoader.Instance.GetAllDependence(bundleName);
    }

    #region 加载

    public void LoadAssetBundle(string bundleName,LoadProgress progress,LoadAssetBundleCallBack callBack)
    {
        if (!LoadHelp.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager();
            loader.Init(bundleName, progress);
            LoadHelp.Add(bundleName, loader);
            callBack(sceneName, bundleName);
        }
        else
        {
            Debug.Log("Has contain bundle========="+bundleName);
        }
    }
    public IEnumerator LoadAssetBundles(string bundleName)
    {
        while (!IABManifestLoader.Instance.IsLoadFinish())
        {
            yield return null;
        }
        IABRelationManager loader = LoadHelp[bundleName];
        string[] dependences = GetDependences(bundleName);
        loader.SetDependences(dependences);
        for (int i = 0; i <dependences.Length; i++)
        {
            yield return LoadAssetDependences(dependences[i], bundleName, loader.GetProgress());
        }
        yield return loader.LoadAssetBundle();
    }

    public IEnumerator LoadAssetDependences(string bundleName, string refName, LoadProgress progress)
    {
        if (!LoadHelp.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager();
            loader.Init(bundleName, progress);
            if (refName != null)
            {
                loader.AddReference(refName);
            }
            LoadHelp.Add(bundleName, loader);
            yield return LoadAssetBundles(bundleName);
        }
        else
        {
            if (refName!=null)
            {
                IABRelationManager loader = LoadHelp[bundleName];
                loader.AddReference(refName);
            }
        }
    }

    #endregion
    #region 卸载
    public void DisposeObjs(string bundleName, string resName)
    {
        if (LoadObjs.ContainsKey(bundleName))
        {
            AssetObjs temObjs = LoadObjs[bundleName];
            temObjs.ReleaseAssetObj(resName);
        }
    }

    public void DisposeSingleBundleObjs(string bundleName)
    {
        if (LoadObjs.ContainsKey(bundleName))
        {
            AssetObjs temObjs = LoadObjs[bundleName];
            temObjs.ReleaseAllAssetObjs();
        }
        Resources.UnloadUnusedAssets();
    }

    public void DisposeAllObjs()
    {
        List<string> keys = new List<string>();
        keys.AddRange(LoadObjs.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            DisposeSingleBundleObjs(keys[i]);
        }
        LoadObjs.Clear();
    }

    public void DisposeBundle(string bundleName)
    {
        if (LoadHelp.ContainsKey(bundleName))
        {
            IABRelationManager loader = LoadHelp[bundleName];
            List<string> dependeces = loader.GetDependences();
            for (int i = 0; i < dependeces.Count; i++)
            {
                if (LoadHelp.ContainsKey(dependeces[i]))
                {
                    IABRelationManager dependence = LoadHelp[dependeces[i]];
                    if (dependence.RemoveReference(bundleName))
                    {
                       DisposeBundle(dependence.GetBundleName()) ;
                    }
                }
            }
            if (loader.GetReference().Count <= 0)
            {
                loader.Dispose();
                LoadHelp.Remove(bundleName);
            }
        }
    }

    public void DisposeAllAssetBundleAndRes()
    {
        DisposeAllObjs();
        DisposeAllBundle();

    }

    public void DisposeAllBundle()
    {
        List<string> keys = new List<string>();
        keys.AddRange(LoadHelp.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            IABRelationManager loader = LoadHelp[keys[i]];
            loader.Dispose();
        }
        LoadHelp.Clear();
    }

    #endregion

    #region 由下层提供API

    public UnityEngine.Object GetSingleRes(string bundleName,string resName)
    {
        if (LoadObjs.ContainsKey(bundleName))
        {
            AssetObjs assetObjs = LoadObjs[bundleName];
            List<UnityEngine.Object> objs = assetObjs.GetAssetObjs(resName);
            return objs[0];
        }
        if (LoadHelp.ContainsKey(bundleName))
        {
            IABRelationManager relationManager = LoadHelp[bundleName];
            UnityEngine.Object obj = relationManager.GetSingleRes(bundleName);
            AssetObj temObj = new AssetObj(obj);
            if (LoadObjs.ContainsKey(bundleName))
            {
                AssetObjs assetObjs = LoadObjs[bundleName];
                assetObjs.AddAssetObj(resName, temObj);
            }
            else
            {
                AssetObjs temObjs = new AssetObjs(resName, temObj);
                LoadObjs.Add(bundleName, temObjs);
            }
            return obj;
        }
        else
        {
            return null;
        }

    }

    public UnityEngine.Object[] GetMutiRes(string bundleName, string resName)
    {
        if (LoadObjs.ContainsKey(bundleName))
        {
            AssetObjs assetObjs = LoadObjs[bundleName];
            List<UnityEngine.Object> objs = assetObjs.GetAssetObjs(resName);
            return objs.ToArray();
        }
        if (LoadHelp.ContainsKey(bundleName))
        {
            IABRelationManager relationManager = LoadHelp[bundleName];
            UnityEngine.Object[] obj = relationManager.GetMutiRes(bundleName);
            AssetObj temObj = new AssetObj(obj);
            if (LoadObjs.ContainsKey(bundleName))
            {
                AssetObjs assetObjs = LoadObjs[bundleName];
                assetObjs.AddAssetObj(resName, temObj);
            }
            else
            {
                AssetObjs temObjs = new AssetObjs(resName, temObj);
                LoadObjs.Add(bundleName, temObjs);
            }
            return obj;
        }
        else
        {
            return null;
        }
    }

    public void DebugAssetBundle(string bundleName)
    {
        if (LoadHelp.ContainsKey(bundleName))
        {
            LoadHelp[bundleName].DebugAsset();
        }
    }

    public bool IsLoadingFinish(string bundleName)
    {
        if (LoadHelp.ContainsKey(bundleName))
        {
            return LoadHelp[bundleName].IsLoadFinish();
        }
        else
        {
            Debug.LogError("LoadHelp not contain bundle====="+bundleName);
            return false;
        }
    }
    #endregion
}



