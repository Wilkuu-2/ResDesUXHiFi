using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.speed = Random.Range(1.0f,3.0f);

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetBool("Phase1", true);
            animator.SetBool("Phase2", false);
            animator.SetBool("Phase3", false);
            animator.SetBool("Phase4", false);
            animator.SetBool("Phase5", false);
 
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetBool("Phase2", true);
            animator.SetBool("Phase1", false);
            animator.SetBool("Phase3", false);
            animator.SetBool("Phase4", false);
            animator.SetBool("Phase5", false);
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetBool("Phase3", true);
            animator.SetBool("Phase2", false);
            animator.SetBool("Phase1", false);
            animator.SetBool("Phase4", false);
            animator.SetBool("Phase5", false);
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            animator.SetBool("Phase4", true);
            animator.SetBool("Phase2", false);
            animator.SetBool("Phase3", false);
            animator.SetBool("Phase1", false);
            animator.SetBool("Phase5", false);
        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            animator.SetBool("Phase5", true);
            animator.SetBool("Phase2", false);
            animator.SetBool("Phase3", false);
            animator.SetBool("Phase4", false);
            animator.SetBool("Phase1", false);
        }
    }
}
