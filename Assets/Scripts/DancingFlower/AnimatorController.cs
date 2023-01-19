using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResDesUX
{
    public class AnimatorController : MonoBehaviour
    {
        Animator animator;
        public int state = 0;
        public int stateMax = 4;
        public int transitionFrames = 5;
        private float inBetweenState;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
          
          
          if(inBetweenState + 0.1 < state || inBetweenState - 0.1 > state)
          {
                inBetweenState += (state - Mathf.Round(inBetweenState)) / transitionFrames;
          }

          animator.speed = 1 + 1 * inBetweenState / stateMax;
          animator.SetFloat("State", inBetweenState / stateMax);
            
        }
    }
}
