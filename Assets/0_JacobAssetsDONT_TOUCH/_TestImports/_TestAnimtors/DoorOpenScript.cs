using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorOpenScript : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private bool IsAtDoor = false;

    [SerializeField] private TextMeshProUGUI CodeText;
    private string CodeTextValue = "";
    public string safeCode;
    public GameObject CodePanel;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CodeText.text = CodeTextValue;

        if (Input.GetKeyDown(KeyCode.E) && IsAtDoor) // Use GetKeyDown to prevent multiple activations in one press
        {
            CodePanel.SetActive(true);
            UnlockAndShowCursor(); // Call function to unlock and show cursor
        }

        // If the code panel is active and the user presses Escape, hide the panel and lock the cursor
        if (CodePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CodePanel.SetActive(false);
            LockAndHideCursor(); // Call function to lock and hide cursor
        }
    }

    public void AddString(string String)
    {
        if (CodeTextValue.Length < 8)
        {
            CodeTextValue += String;
        }

        if (CodeTextValue.Equals(safeCode, StringComparison.OrdinalIgnoreCase))
        {
            anim.SetTrigger("OpenDoor");
            CodePanel.SetActive(false);
            LockAndHideCursor(); // Lock and hide the cursor again after correct input
        }
        else if (CodeTextValue.Length >= 8)
        {
            CodeTextValue = "";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // It's better to use CompareTag instead of checking the tag property directly
        {
            IsAtDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAtDoor = false;
            CodePanel.SetActive(false);
            LockAndHideCursor(); // Lock and hide the cursor when the player exits the trigger area
        }
    }

    private void UnlockAndShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
