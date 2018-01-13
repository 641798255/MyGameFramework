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

public class IABResLoader:IDisposable
{

    private AssetBundle ABRes;

    public IABResLoader(AssetBundle abRes)
    {
        ABRes = abRes;
    }

    public UnityEngine.Object this[string resName]
    {
        get
        {
            if (ABRes!=null&&ABRes.Contains(resName))
            {
                Debug.LogError("ABRes dont contain-------"+resName);
                return null;
            }
            return ABRes.LoadAsset(resName);
        }
    }

    public UnityEngine.Object[] LoadResources(string resName)
    {
        if (ABRes != null && ABRes.Contains(resName))
        {
            Debug.LogError("ABRes dont contain-------" + resName);
            return null;
        }
        return ABRes.LoadAssetWithSubAssets(resName);
    }

    public void UnLoadRes(UnityEngine.Object res)
    {
        Resources.UnloadAsset(res);
    }

    public void Dispose()
    {
        if (ABRes==null)
        {
            return;
        }
        ABRes.Unload(false);
    }

    public void DebugResloader()
    {
        if (ABRes==null)
        {
            return;
        }
        string[] resNames = ABRes.GetAllAssetNames();
        for (int i = 0; i <resNames.Length; i++)
        {
            Debug.Log("ABRes contain res======="+resNames[i]);
        }
    }

}
