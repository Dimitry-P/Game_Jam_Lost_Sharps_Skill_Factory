using UnityEngine;
using TMPro;
using Echoes_At_The_Last_Station;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI Elements")]
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textContent;

    [Header("Player Controls")]
    public GameObject player;
    private MouseLook mouseLook;
    private FPSInput fpsInput;

    [Header("Dialogue Settings")]
    public LookAtSpeaker cameraLookAt;
    public DialogueUIController dialogueUIController;
    public float continueCooldown = 0.3f;

    [Header("Scene Transition")]
    public Image fadeOverlay;
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;

    private DialogueLine[] currentDialogueLines;
    private int currentLineIndex;
    private bool isDialogueActive;
    private bool canContinue = false;
    public System.Action OnDialogueContinue;
    private bool wasKeyPressedBeforeDialogue = false;
    private string sceneToLoadAfterDialogue;

    public bool IsDialogueActive => isDialogueActive;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (player != null)
        {
            mouseLook = player.GetComponent<MouseLook>();
            fpsInput = player.GetComponent<FPSInput>();
        }

        if (fadeOverlay != null)
        {
            fadeOverlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
            fadeOverlay.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isDialogueActive) return;

        if (!wasKeyPressedBeforeDialogue)
        {
            wasKeyPressedBeforeDialogue = true;
            Input.ResetInputAxes();
            return;
        }

        if (!canContinue) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ProcessDialogueInput();
        }
    }

    public void StartDialogue(DialogueData dialogue, string sceneToLoad = "")
    {
        if (isDialogueActive || dialogue == null || dialogue.lines == null || dialogue.lines.Length == 0)
        {
            Debug.LogWarning("Failed to start dialogue");
            return;
        }

        sceneToLoadAfterDialogue = sceneToLoad;
        wasKeyPressedBeforeDialogue = false;
        currentDialogueLines = dialogue.lines;
        currentLineIndex = 0;
        isDialogueActive = true;
        canContinue = true;

        TogglePlayerControls(false);
        ShowLine(currentLineIndex);
    }

    private void ProcessDialogueInput()
    {
        canContinue = false;
        ShowNextLine();
        StartCoroutine(ContinueCooldown());
    }

    public void ShowNextLine()
    {
        if (!isDialogueActive || currentDialogueLines == null) return;

        currentLineIndex++;

        if (currentLineIndex < currentDialogueLines.Length)
        {
            ShowLine(currentLineIndex);
            OnDialogueContinue?.Invoke();
        }
        else
        {
            if (!string.IsNullOrEmpty(sceneToLoadAfterDialogue))
            {
                StartCoroutine(TransitionToScene());
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private IEnumerator TransitionToScene()
    {
        // Fade out effect
        if (fadeOverlay != null)
        {
            yield return StartCoroutine(FadeScreen(0, 1));
            yield return new WaitForSeconds(0.5f);
        }

        EndDialogue();

        if (!string.IsNullOrEmpty(sceneToLoadAfterDialogue))
        {
            SceneManager.LoadScene(sceneToLoadAfterDialogue);
        }
    }

    private IEnumerator FadeScreen(float startAlpha, float endAlpha)
    {
        fadeOverlay.gameObject.SetActive(true);
        float elapsed = 0f;
        Color color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, startAlpha);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeOverlay.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeOverlay.color = color;
    }

    void ShowLine(int index)
    {
        if (index < 0 || index >= currentDialogueLines.Length)
        {
            Debug.LogError($"Invalid dialogue index: {index}");
            return;
        }

        var line = currentDialogueLines[index];

        if (textName != null) textName.text = line.speakerName;
        if (textContent != null) textContent.text = line.content;

        if (line.lookTarget != null && cameraLookAt != null)
        {
            cameraLookAt.SetTarget(line.lookTarget.transform);
        }

        if (dialogueUIController != null && line.lookTarget != null)
        {
            dialogueUIController.ShowDialogue(line.speakerName, line.content, line.lookTarget.transform);
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        currentDialogueLines = null;
        sceneToLoadAfterDialogue = null;

        if (cameraLookAt != null)
            cameraLookAt.ClearTarget();

        TogglePlayerControls(true);

        if (dialogueUIController != null)
            dialogueUIController.HideDialogue();

        OnDialogueContinue = null;
    }

    public void TogglePlayerControls(bool enable)
    {
        if (mouseLook != null) mouseLook.enabled = enable;
        if (fpsInput != null) fpsInput.enabled = enable;
    }

    IEnumerator ContinueCooldown()
    {
        yield return new WaitForSeconds(continueCooldown);
        canContinue = true;
    }
}