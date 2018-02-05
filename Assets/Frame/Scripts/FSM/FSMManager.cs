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

public class FSMManager
{
    private FSMState[] Arr_FSMState;
    private ushort CurAdd;
    public ushort CurStateId;
    public FSMManager(ushort stateNum)
    {
        Arr_FSMState = new FSMState[stateNum];
    }

    public void AddState(FSMState temState)
    {
        if (CurAdd < Arr_FSMState.Length)
        {
            Arr_FSMState[CurAdd] = temState;
            CurAdd++;
        }
    }

    public void ChangeState(ushort stateId)
    {
        Arr_FSMState[CurStateId].OnLeave();
        CurStateId = stateId;
        Arr_FSMState[CurStateId].CopyState(Arr_FSMState[stateId]);
        Arr_FSMState[CurStateId].OnBeforeEnter();
        Arr_FSMState[CurStateId].OnEnter();
    }

    public void Update()
    {
        Arr_FSMState[CurStateId].Update();
    }
}
