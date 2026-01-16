using TMPro;
using UnityEngine;

public class DialogueZoneTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueData dialogue;
    public bool requireKeyPress = true;
    public KeyCode activationKey = KeyCode.E;
    public string sceneToLoadAfterDialogue;

    [Header("Visual Feedback")]
    public GameObject interactionPrompt;
    public string promptText = "Press E to interact";

    private bool playerInTrigger;

    private void Update()
    {
        if (playerInTrigger && (!requireKeyPress || Input.GetKeyDown(activationKey)))
        {
            DialogueManager.Instance.StartDialogue(dialogue, sceneToLoadAfterDialogue);
            TogglePrompt(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            TogglePrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            TogglePrompt(false);
        }
    }

    private void TogglePrompt(bool show)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(show);

            var textComponent = interactionPrompt.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = promptText;
            }
        }
    }
}