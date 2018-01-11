/***
 * 
 *    Title: "" 游戏框架项目
 *           主题： 
 *    Description: 
 *           功能：注册物体到UImanager
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
using UnityEngine.UI;
using UnityEngine.Events;

public class UIBehavior : MonoBehaviour {

    public void Awake()
    {
        UIManager.Instance.RigestObj(this.gameObject.name, this.gameObject);
    }
    public void AddButtonClickListener(UnityAction action)
    {
        Button btn = GetComponent<Button>();
        if (btn)
        {
            btn.onClick.AddListener(action);
        }
    }

    public void RemoveButtonClickListener(UnityAction action)
    {
        Button btn = GetComponent<Button>();
        if (btn)
        {
            btn.onClick.RemoveListener(action);
        }
    }

    public void AddSliderListener(UnityAction<float> action)
    {
        Slider slider = GetComponent<Slider>();
        if (slider)
        {
            slider.onValueChanged.AddListener(action);
        }
    }

    public void RemoveSliderListener(UnityAction<float> action)
    {
        Slider slider = GetComponent<Slider>();
        if (slider)
        {
            slider.onValueChanged.RemoveListener(action);
        }
    }

    public void AddIuputFieldListener(UnityAction<string> action)
    {
        InputField input = GetComponent<InputField>();
        if (input)
        {
            input.onValueChanged.AddListener(action);
        }
    }

    public void RemoveIuputFieldListener(UnityAction<string> action)
    {
        InputField input = GetComponent<InputField>();
        if (input)
        {
            input.onValueChanged.RemoveListener(action);
        }
    }
}
