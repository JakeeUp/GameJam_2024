using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorOpenScript : MonoBehaviour
{

    private Animator anim;

    [SerializeField]private bool IsAtDoor = false;

    [SerializeField] private TextMeshProUGUI CodeText;
    string CodeTextValue = "";
    public string safeCode;
    public GameObject CodePanel;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CodeText.text = CodeTextValue;

        if (Input.GetKey(KeyCode.E) && IsAtDoor)
        {
            CodePanel.SetActive(true);
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
        }
        else if (CodeTextValue.Length >= 8) 
        {
            CodeTextValue = "";
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IsAtDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsAtDoor = false;
        CodePanel.SetActive(false);
    }
  
}
