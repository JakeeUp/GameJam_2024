using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
    [SerializeField] private GameObject stompObject; // Reference to the stomp object
    [SerializeField] private LayerMask enemyLayer;   // Layer that represents the enemy
    PlayerController controller;
    [SerializeField] private float playerStompForce = 5f;
    [SerializeField]private bool isFalling = false;
    [SerializeField] private AudioClip stompSound;
    private AudioSource audioSource;


    private void Awake()
    {
        
        controller = GetComponent<PlayerController>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        // Check if the player is falling (you can use your own condition here)
        isFalling = controller.isFalling; 

        // Enable or disable the stomp object based on the player's falling state
        stompObject.SetActive(isFalling);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is falling and collided with an enemy
        if (isFalling && other.CompareTag("Enemy"))
        {
            // Call the StompEnemy method on the enemy
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.StompEnemy(); // This will play the VFX and destroy the enemy
                if (stompSound != null)
                {
                    audioSource.PlayOneShot(stompSound);
                }
                // Add an upward force to the player's Rigidbody
                Rigidbody playerRigidbody = GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    playerRigidbody.AddForce(Vector3.up * playerStompForce, ForceMode.Impulse);
                }
            }
        }
    }

  
    private bool IsEnemyHit(GameObject other)
    {
        // Check if the collided object is on the enemyLayer
        Debug.Log("enemy has been hit");
        return (enemyLayer.value & 1 << other.layer) != 0;
    }
}
