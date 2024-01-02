using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private float _timeUntilIdleSwitch;

    [SerializeField]
    private int _numberOfIdles;
    

    private bool _switchIdle;
    public float _idleTime; 
    private int _idleAnimation;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_switchIdle == false) {
            _idleTime += Time.deltaTime;

            if (_idleTime > _timeUntilIdleSwitch && stateInfo.normalizedTime % 1 < 0.02f) {

                _switchIdle = true;
                _idleAnimation = UnityEngine.Random.Range(1, (_numberOfIdles + 1));
                Debug.Log("Idle is:" + _idleAnimation);
            }
        }

        else if (stateInfo.normalizedTime % 1 > 0.98) {
            ResetIdle();
        }

        animator.SetFloat("IdleNumber", _idleAnimation, 0.02f, Time.deltaTime);

    }

    private void ResetIdle() {
        
        if(_switchIdle) {
        _idleAnimation--;
        }
        _switchIdle = false;
        _idleTime = 0;
    }
}
