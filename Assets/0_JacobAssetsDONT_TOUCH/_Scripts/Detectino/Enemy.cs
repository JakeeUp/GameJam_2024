using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public LayerMask visionLayerMask;
    public float fieldOfViewAngle = 110f;
    public float visionRange = 10f;
    private RagdollController ragdollController; 
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            ragdollController = player.GetComponent<RagdollController>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 directionToPlayer = other.transform.position - transform.position;
            float angle = Vector3.Angle(directionToPlayer, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, visionRange, visionLayerMask))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("Player has been seen!");
                        if (ragdollController != null)
                        {
                            Vector3 forceDirection = new Vector3(300, 100, 100);
                            float forceMagnitude = 1000f;
                            Debug.Log("Sending player flying");
                            //ragdollController.TurnOnRagDoll(new Vector3(0, 1, 0), 20f);
                            // ragdollController.TurnOnRagDoll(forceDirection, forceMagnitude);
                            ragdollController.TurnOnRagDoll(Vector3.up, 500f);
                        }
                    }
                }
            }
        }
    }
}
