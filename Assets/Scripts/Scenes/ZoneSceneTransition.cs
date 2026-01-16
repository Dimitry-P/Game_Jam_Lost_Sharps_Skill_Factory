using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor;

public class ZoneSceneTransition : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Сцена для загрузки")]
    [SerializeField] private SceneReference targetScene;

    [Header("Transition Settings")]
    [Tooltip("Задержка перед переходом")]
    [SerializeField] private float transitionDelay = 0.5f;

    [Tooltip("Эффект затемнения")]
    [SerializeField] private bool useFadeEffect = true;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Color fadeColor = Color.black;

    [Header("Trigger Settings")]
    [SerializeField] private bool requireKeyPress = false;
    [SerializeField] private KeyCode activationKey = KeyCode.E;
    [SerializeField] private bool oneTimeUse = true;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject promptUI;
    [SerializeField] private string interactText = "Press E to enter";

    private bool playerInZone;
    private bool used;

    private void Awake()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void Update()
    {
        if (playerInZone && !used)
        {
            if (requireKeyPress)
            {
                if (Input.GetKeyDown(activationKey))
                {
                    StartTransition();
                }
            }
            else
            {
                StartTransition();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (!oneTimeUse || !used))
        {
            playerInZone = true;

            if (requireKeyPress && promptUI != null)
            {
                promptUI.SetActive(true);
                // Установите текст через TextMeshPro или другой компонент
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    private void StartTransition()
    {
        if (used && oneTimeUse) return;

        used = true;
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        // Подготовка игрока
        PlayerInputToggle(false);

        // Визуальная обратная связь
        if (promptUI != null)
            promptUI.SetActive(false);

        // Эффект затемнения
        if (useFadeEffect)
        {
            yield return StartCoroutine(FadeScreen(0, 1));
            yield return new WaitForSeconds(transitionDelay);
        }

        // Загрузка сцены
        if (targetScene != null && !string.IsNullOrEmpty(targetScene.SceneName))
        {
            SceneManager.LoadScene(targetScene.SceneName);
        }
        else
        {
            Debug.LogError("Target scene not set!");
        }
    }

    private IEnumerator FadeScreen(float from, float to)
    {
        // Реализация эффекта затемнения через UI Image или Post Processing
        // Здесь должен быть ваш код для плавного затемнения
        yield return null;
    }

    private void PlayerInputToggle(bool state)
    {
        // Отключение управления игроком
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var controller = player.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = state;

            var movement = player.GetComponent<PlayerMovement>();
            if (movement != null) movement.enabled = state;
        }
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(ZoneSceneTransition))]
public class ZoneSceneTransitionEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (ZoneSceneTransition)target;
        if (script.requireKeyPress)
        {
            script.promptUI = (GameObject)EditorGUILayout.ObjectField(
                "Prompt UI",
                script.promptUI,
                typeof(GameObject),
                true);

            script.interactText = EditorGUILayout.TextField("Interact Text", script.interactText);
        }
    }
}
#endif