using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableLetter : MonoBehaviour
{
    public char letter; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            GameManager.instance.CollectLetter(letter);
            gameObject.SetActive(false); 
        }
    }
}
