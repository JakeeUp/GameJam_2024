using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapyAnimationManager : MonoBehaviour
{
    private Animator animator;
    private float timer = 30f; // 30 seconds timer
    private bool playerInRoom = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(IdleBreakerCoroutine());
    }

    IEnumerator IdleBreakerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timer);
            animator.SetTrigger("ToIdleBreaker");
        }
    }

    void Update()
    {
        if (!playerInRoom && animator.GetCurrentAnimatorStateInfo(0).IsName("surpriseIdle"))
        {
            animator.SetBool("ExitSurpriseIdle", true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRoom = true;
            animator.SetBool("IsPlayerInRoom", true);
        }
        else
        {
            animator.SetBool("ExitSurpriseIdle", false);
        }
    }

    void OnTriggerExit(Collider other)
    {
            animator.SetBool("ExitSurpriseIdle", false);
        if (other.CompareTag("Player"))
        {
            playerInRoom = false;
            animator.SetBool("IsPlayerInRoom", false);

        }
    }
}
