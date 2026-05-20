using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;

public class animcontorolle : MonoBehaviour
{
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {  
        animator = GetComponent<Animator>();
        int randamNumber = Random.Range(0,3);
        animator.SetInteger("animNumber",randamNumber);
    }

}
