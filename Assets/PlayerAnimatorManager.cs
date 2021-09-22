using System;
using UnityEngine;
using System.Collections;

namespace Com.MyCompany.MyGame
{
    public class PlayerAnimatorManager : MonoBehaviour
    {
        #region Private Fields

        private Animator animator;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }

        private void Update()
        {
            if (!animator)
            {
                return;
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (v < 0)
            {
                v = 0;
            }
            
            animator.SetFloat("Speed", h * h + v * v);
        }

        #endregion
    }
}