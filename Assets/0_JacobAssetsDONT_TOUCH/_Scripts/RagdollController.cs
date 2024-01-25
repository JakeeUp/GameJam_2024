using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RagdollController : MonoBehaviour
{
    // Fields related to the Ragdoll
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Animator animator;

    public float respawnDelay = 2f;
    public GameObject respawnPoint;

    public GameObject playerBody; 
    public List<GameObject> disappearVFXPrefabs; 
    public List<Transform> vfxSpawnPositions; 
    public float disappearDuration = 2f; 

    private void Awake()
    {
        InitializeRagdollComponents();
        SetRagdollState(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRagdollState();
        }
    }

    private void InitializeRagdollComponents()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        animator = GetComponent<Animator>();
    }

    private void ToggleRagdollState()
    {
        bool isRagdollActive = !animator.enabled;
        SetRagdollState(!isRagdollActive);
    }

    public void TurnOnRagDoll()
    {
        // Start the fade to black
        UIManager.instance.fadeToBlack = true;
        StartCoroutine(FadeFromBlackAfterDelay());
        SetRagdollState(true);
    }
    public Respawner respawner; // Reference to the Respawner script

    public void TurnOnRagDollWithForce(Vector3 forceDirection, float forceMagnitude)
    {
        if (gameObject.activeSelf) // Check if the game object is active
        {
            // Start the fade to black
            UIManager.instance.fadeToBlack = true;
            StartCoroutine(FadeFromBlackAfterDelay());
            SetRagdollState(true);

            // Delay by 2 seconds before disabling player body and spawning VFX
            StartCoroutine(DelayedActions(1f));

            if (respawner != null)
            {
                respawner.RespawnPlayer(gameObject, respawnPoint, respawnDelay);
            }
        }
    }

    private IEnumerator DelayedActions(float delayInSeconds)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        // Perform actions after the delay
        DisablePlayerBody();
        SpawnVFX();
    }
   
    private IEnumerator FadeFromBlackAfterDelay()
    {
        // Wait for 2 seconds before starting to fade from black
        yield return new WaitForSeconds(2f);
        UIManager.instance.fadeFromBlack = true;
    }


    private void DisablePlayerBody()
    {
        if (playerBody != null)
        {
            playerBody.SetActive(false);
        }
    }

    private void SpawnVFX()
    {
        for (int i = 0; i < disappearVFXPrefabs.Count; i++)
        {
            if (disappearVFXPrefabs[i] != null && i < vfxSpawnPositions.Count)
            {
                Instantiate(disappearVFXPrefabs[i], vfxSpawnPositions[i].position, Quaternion.identity);
            }
        }
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

    public void Respawn()
    {
        Debug.Log("Respawning player");

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.transform.position;
            EnablePlayerBody();
            ResetPlayerState();
        }
    }

    private void EnablePlayerBody()
    {
        if (playerBody != null)
        {
            playerBody.SetActive(true);
        }
    }

    private void ResetPlayerState()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }





}
