using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerEnemy : MonoBehaviour
{
    private Animator animator;
    private bool isTriggered = false;
    public float pushForce = 500f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;

            if (animator != null)
            {
                animator.SetTrigger("PlayerNear"); 
            }

                RagdollController ragdollController = other.GetComponent<RagdollController>();
                if (ragdollController != null)
                {
                    Vector3 forceDirection = -transform.forward;
                    ragdollController.TurnOnRagDollWithForce(forceDirection, pushForce);
                }
        }
    }
    
}
