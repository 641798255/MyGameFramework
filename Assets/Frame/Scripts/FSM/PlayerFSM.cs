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


public enum PlayerFSMStateId
{
    Idle,
    Attack,
    Walk,
    Run,
    MaxValue
}

public class PlayerFSM : MonoBehaviour
{

    private FSMManager manager;
	// Use this for initialization
	void Start ()
	{
	    WalkState walkState = new WalkState();
	    AttackState attackState = new AttackState();
	    manager = new FSMManager(2);
	    manager.AddState(walkState);
	    manager.AddState(attackState);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    manager.Update();
	}

    public void PlayAttack()
    {
        manager.ChangeState((ushort)PlayerFSMStateId.Attack);
    }
}
