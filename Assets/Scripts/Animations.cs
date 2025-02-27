﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private bool isCrouched=false;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)){
            if(isCrouched==false){
                isCrouched=true;
                animator.SetBool("isCrouched", true);
             }
             else{
                isCrouched=false;
                animator.SetBool("isCrouched", false);
            }
        }
        if(Input.GetKey(KeyCode.Z)){
            animator.SetBool("isWalking",true);
        }
        else if(Input.GetKey(KeyCode.Q)){
            animator.SetBool("isWalking",true);
        }
        else if(Input.GetKey(KeyCode.S)){
            animator.SetBool("isWalking",true);
        }
        else if(Input.GetKey(KeyCode.D)){
            animator.SetBool("isWalking",true);
        }
        else{
            animator.SetBool("isWalking",false);
        }
        
    }
}
