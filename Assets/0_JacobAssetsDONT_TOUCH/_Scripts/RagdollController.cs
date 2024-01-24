using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{


    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Animator animator;

    public float respawnDelay = 2f;

    public GameObject playerBody; // Reference to the player's visible body GameObject
    public List<GameObject> disappearVFXPrefabs; // List of disappearing VFX prefabs
    public List<Transform> vfxSpawnPositions; // List of positions where VFXs will be spawned
    public GameObject respawnPoint;

    public float disappearDuration = 2f; // Duration of the disappearing effect

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

        // Spawn disappearing VFXs at specified positions
        for (int i = 0; i < disappearVFXPrefabs.Count; i++)
        {
            if (disappearVFXPrefabs[i] != null && i < vfxSpawnPositions.Count)
            {
                Instantiate(disappearVFXPrefabs[i], vfxSpawnPositions[i].position, Quaternion.identity);
            }
        }

        // Wait for the specified duration before respawning
        StartCoroutine(RespawnWithDelay());
    }
    private IEnumerator RespawnWithDelay()
    {
        yield return new WaitForSeconds(disappearDuration);

        // Trigger the respawn logic after the delay
        Respawn();
    }
    private IEnumerator DisappearPlayerAndSpawnVFX(Vector3 forceDirection, float forceMagnitude, Action onDisappearComplete)
    {
        yield return new WaitForSeconds(disappearDuration);

        if (playerBody != null)
        {
            playerBody.SetActive(false);
        }

        // Spawn disappearing VFXs at specified positions
        for (int i = 0; i < disappearVFXPrefabs.Count; i++)
        {
            if (disappearVFXPrefabs[i] != null && i < vfxSpawnPositions.Count)
            {
                Instantiate(disappearVFXPrefabs[i], vfxSpawnPositions[i].position, Quaternion.identity);
            }
        }

        onDisappearComplete?.Invoke();
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Respawn the player
        Respawn();
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

    public void ApplyForce(Vector3 force)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply the force to the Rigidbody component
            rb.AddForce(force, ForceMode.VelocityChange);
        }
        else
        {
            // Optionally, find all Rigidbodies in the ragdoll and apply the force to each one
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                // Skip the main Rigidbody if it's meant to be static (e.g., if it's just a container for the character)
                if (!rigidbody.isKinematic)
                {
                    rigidbody.AddForce(force, ForceMode.VelocityChange);
                }
            }
        }
    }


    public void Respawn()
    {
        Debug.Log("Respawning player"); // Add this line for debugging

        if (respawnPoint != null)
        {
            // Teleport the player to the respawn point
            transform.position = respawnPoint.transform.position;

            // Enable the player's visible body
            if (playerBody != null)
            {
                playerBody.SetActive(true);
            }

            // Reset any other necessary player state or variables

            // Now, re-enable the player controller (if you have a separate script for it)
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
    }
}
