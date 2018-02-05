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




public abstract class FSMState
{

    private ushort StateId;

    public virtual void OnBeforeEnter()
    {

    }

    public virtual void CopyState(FSMState state)
    {

    }

    //派生类必须实现的方法
    public abstract void OnEnter();

    public virtual void Update()
    {

    }

    public virtual void OnLeave()
    {

    }

}

public class WalkState:FSMState
{
    public override void OnEnter()
    {
       
    }
}

public class AttackState : FSMState
{
    public override void OnEnter()
    {

    }
}
