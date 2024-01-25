using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RagdollController : MonoBehaviour
{
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Animator animator;
    [SerializeField] private GameObject playerCharacter;

    public float respawnDelay = 2f;
    public GameObject respawnPoint;

    private GameObject playerBody; 
      public List<GameObject> disappearVFXPrefabs; 
     public List<Transform> vfxSpawnPositions; 
    private float disappearDuration = 2f;

    [SerializeField]private AudioSource audioSource; 
    [SerializeField]private AudioClip ragdollSound;

    private void Awake()
    {
        InitializeRagdollComponents();
        SetRagdollState(false);
        playerCharacter.SetActive(true);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on the game object.");
            }
        }
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
        UIManager.instance.fadeToBlack = true;
        StartCoroutine(FadeFromBlackAfterDelay());
        SetRagdollState(true);
    }
    public Respawner respawner;  

    public void TurnOnRagDollWithForce(Vector3 forceDirection, float forceMagnitude)
    {
        if (gameObject.activeSelf)
        {
            SetRagdollState(true);

            if (audioSource != null && ragdollSound != null)
            {
                audioSource.PlayOneShot(ragdollSound);
            }

            StartCoroutine(DelayedActions(2f));

            if (respawner != null)
            {
                respawner.RespawnPlayer(gameObject, respawnPoint, respawnDelay);
            }
        }
    }

    private IEnumerator DelayedActions(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        if (playerCharacter != null)
        {
            playerCharacter.SetActive(false);
        }

        SpawnVFX();

        StartCoroutine(FadeToBlackAfterDelay(1f));
    }

    private IEnumerator FadeToBlackAfterDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        UIManager.instance.fadeToBlack = true;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(1);
    }
   
    private void EnablePlayerCharacter()
    {
        if (playerCharacter != null)
        {
            playerCharacter.SetActive(true);
        }
    }
    private IEnumerator RagdollSequence(Vector3 forceDirection, float forceMagnitude)
    {
        SetRagdollState(true);

        yield return new WaitForSeconds(1f);

        DisablePlayerBody();
        SpawnVFX();

        yield return new WaitForSeconds(1f);

        UIManager.instance.fadeToBlack = true;

        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1f);
        UIManager.instance.fadeFromBlack = true;
    }

    
    
    private IEnumerator FadeFromBlackAfterDelay()
    {
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
