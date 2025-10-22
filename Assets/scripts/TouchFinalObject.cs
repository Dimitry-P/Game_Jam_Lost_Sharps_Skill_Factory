// Â AddScore.cs
using SimpleFPS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TouchFinalObject : MonoBehaviour
{
    public static TouchFinalObject Instance { get; private set; }

    private bool enteredCollider;
    public bool EnteredCollider => enteredCollider;



    private bool playerInZone = false;
    private bool pickedUp = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    private void Update()
    {
        if (playerInZone && !pickedUp && Input.GetKeyDown(KeyCode.E))
        {
            pickedUp = true;
            enteredCollider = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FirstPersonController>() != null)
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FirstPersonController>() != null)
        {
            playerInZone = false;
        }
    }
}