using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{


    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Animator animator;

    private void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        animator = GetComponent<Animator>();

        SetRagdollState(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            bool isRagdollActive = !animator.enabled; 
            SetRagdollState(!isRagdollActive);
        }
    }
    public void TurnOnRagDoll()
    {
        SetRagdollState(true);
    }

    public void TurnOnRagDoll(Vector3 forceDirection, float forceMagnitude)
    {
        SetRagdollState(true);

        Rigidbody mainRigidbody = FindMainRigidbody();
        if (mainRigidbody != null)
        {
            Debug.Log($"Applying force: {forceDirection.normalized * forceMagnitude}");
            mainRigidbody.AddForce(forceDirection.normalized * forceMagnitude, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("Main Rigidbody not found.");
        }
    }

    private Rigidbody FindMainRigidbody()
    {
        Rigidbody mainRb = null;
        float maxMass = 0f;
        foreach (var rb in ragdollRigidbodies)
        {
            if (rb.mass > maxMass)
            {
                mainRb = rb;
                maxMass = rb.mass;
            }
        }
        return mainRb;
    }
    private void SetRagdollState(bool state)
    {
        foreach (var rb in ragdollRigidbodies)
        {
            if (rb != this.GetComponent<Rigidbody>()) 
            {
                rb.isKinematic = !state;
            }
        }

        foreach (var col in ragdollColliders)
        {
            if (col != this.GetComponent<Collider>()) 
            {
                col.enabled = state;
            }
        }

      
        animator.enabled = !state;
    }
}
