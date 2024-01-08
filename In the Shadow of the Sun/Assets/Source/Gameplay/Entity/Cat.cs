using System;
using System.Collections;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public Animator animator;
    public string interactBool;
    public float delay;

    public void OnMouseDown()
    {
        animator.SetBool(interactBool, true);
        StartCoroutine(DoAnimTick());
    }

    private IEnumerator DoAnimTick()
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(interactBool, false);   
        yield return null;
    }
}
