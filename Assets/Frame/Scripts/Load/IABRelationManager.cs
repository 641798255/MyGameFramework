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

public class IABRelationManager
{
    private List<string> DependencyBundle=null;
    private List<string> ReferenceBundle = null;
    IABLoader AssetLoader = null;
    private bool _IsLoadFinish = false;
    private string TheBundleName;
    private LoadProgress _LoaderProgress;

    public IABRelationManager()
    {
        DependencyBundle = new List<string>();
        ReferenceBundle = new List<string>();
    }

    public string GetBundleName()
    {
        return TheBundleName;
    }

    public void Init(string bundleName,LoadProgress progress)
    {
        _IsLoadFinish = false;
        TheBundleName = bundleName;
        _LoaderProgress = progress;
        AssetLoader = new IABLoader(progress, BundleLoadFinish);
        AssetLoader.SetBundleName(bundleName);
        string bundlePath = IPathTools.GetWWWAssetBundlePath()+"/"+bundleName;
        AssetLoader.LoadRes(bundlePath);
    }

    public void BundleLoadFinish(string bundleName)
    {
        _IsLoadFinish = true;
    }

    public bool IsLoadFinish()
    {
        return _IsLoadFinish;
    }

    public LoadProgress GetProgress()
    {
        return _LoaderProgress;
    }
    public void AddDependence(string bundleName)
    {
        DependencyBundle.Add(bundleName);
    }

    public void SetDependences(string[] dependencies)
    {
        if (dependencies.Length > 0)
        {
            DependencyBundle.AddRange(dependencies);
        }
    }

    public List<string> GetDependences()
    {
        return DependencyBundle;
    }

    public void RemoveDependences(string bundleName)
    {
        for (int i = 0; i < DependencyBundle.Count; i++)
        {
            if (bundleName.Equals(DependencyBundle[i]))
            {
                DependencyBundle.RemoveAt(i);
            }
        }
    }
    public void AddReference(string bundleName)
    {
        ReferenceBundle.Add(bundleName);
    }

    public List<string> GetReference()
    {
        return ReferenceBundle;
    }

    public bool RemoveReference(string bundleName)
    {
        for (int i = 0; i < ReferenceBundle.Count; i++)
        {
            if (bundleName.Equals(ReferenceBundle[i]))
            {
                ReferenceBundle.RemoveAt(i);
            }
        }
        if (ReferenceBundle.Count<=0)
        {
            Dispose();
            return true;
        }
        return false;
    }

    #region 由下层提供API

    public IEnumerator LoadAssetBundle()
    {
        yield return AssetLoader.CommonLoad();
    }

    public void Dispose()
    {
        if (AssetLoader != null)
        {
            AssetLoader.Dispose();
        }
    }

    public UnityEngine.Object GetSingleRes(string resName)
    {
        if (AssetLoader != null)
        {
            return AssetLoader.GetRes(resName);
        }
        return null;
    }
    public UnityEngine.Object[] GetMutiRes(string resName)
    {
        if (AssetLoader != null)
        {
            return AssetLoader.GetMutiRes(resName);
        }
        return null;
    }

    public void DebugAsset()
    {
        if (AssetLoader != null)
        {
            AssetLoader.DebugLoader();
        }
        else
        {
            Debug.LogError("AssetLoader不存在=====");
        }
    }
    #endregion


}
