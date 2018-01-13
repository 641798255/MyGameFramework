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

public delegate void LoadProgress(string bundle,float  progress);

public delegate void loadFinish(string bundle);
public class IABLoader
{
    private string BundleName;
    private string CommonBundlePath;
    private WWW CommonLoader;
    private float CommonResLoadProcess;
    private LoadProgress _LoadProgress;
    private loadFinish _LoadFinish;
    private IABResLoader abResLoader;

    public IABLoader(LoadProgress progress,loadFinish finish)
    {
        BundleName = "";
        CommonBundlePath = "";
        CommonResLoadProcess = 0;
        _LoadFinish = finish;
        _LoadProgress = progress;
        abResLoader = null;
    }

    public IEnumerator CommonLoad()
    {
        CommonLoader = new WWW(CommonBundlePath);
        while (CommonLoader.isDone)
        {
            CommonResLoadProcess = CommonLoader.progress;
            if (_LoadProgress!=null)
            {
                _LoadProgress(BundleName,CommonResLoadProcess);
            }
            yield return CommonResLoadProcess;
            CommonResLoadProcess = CommonLoader.progress;
        }
        if (CommonResLoadProcess >= 1.0f)
        {
            abResLoader = new IABResLoader(CommonLoader.assetBundle);
            if (_LoadProgress != null)
            {
                _LoadProgress(BundleName, CommonResLoadProcess);
            }
            if (_LoadFinish!=null)
            {
                _LoadFinish(BundleName);
            }
        }
        else
        {
            Debug.LogError("load Error=="+BundleName);
        }
        CommonLoader = null;

    }

    public void LoadRes(string path)
    {
        CommonBundlePath = path;
    }

    public void SetBundleName(string bundleName)
    {
        BundleName = bundleName;
    }

    public void DebugLoader()
    {
        if (CommonLoader != null)
        {
           abResLoader.DebugResloader();
        }
    }
    #region 下层提供功能

    public UnityEngine.Object GetRes(string name)
    {
        if (abResLoader != null)
        {
            return abResLoader[name];
        }
        return null;
    }

    public UnityEngine.Object[] GetMutiRes(string name)
    {
        if (abResLoader != null)
        {
            return abResLoader.LoadResources(name);
        }
        return null;
    }

    public void UnLoadRes(UnityEngine.Object res)
    {
        if (abResLoader!=null)
        {
        abResLoader.UnLoadRes(res);

        }
    }

    public void Dispose()
    {
        if (abResLoader!=null)
        {
            abResLoader.Dispose();
            abResLoader = null;
        }
    }

    #endregion
}
