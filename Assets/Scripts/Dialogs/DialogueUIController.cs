using UnityEngine;
using TMPro;

public class DialogueUIController : MonoBehaviour
{
    public Canvas canvas;                 // Canvas в режиме World Space
    public RectTransform uiRoot;         // Корень UI панели
    public TextMeshProUGUI nameText;     // Текст имени
    public TextMeshProUGUI contentText;  // Текст диалога
    public Camera mainCamera;             // Камера для ориентации UI

    private Transform lookTarget;        // К кому прикреплён UI

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        HideDialogue();
    }

    void LateUpdate()
    {
        if (lookTarget != null && uiRoot != null)
        {
            Vector3 lookDirection = uiRoot.position - mainCamera.transform.position;
            uiRoot.rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
            Vector3 euler = uiRoot.rotation.eulerAngles;

            euler.x = 0;
            euler.z = 0;

            uiRoot.rotation = Quaternion.Euler(euler);
        }
    }

    public void ShowDialogue(string name, string content, Transform target)
    {
        lookTarget = target;
        nameText.text = name;
        contentText.text = content;
        canvas.enabled = true;
    }

    public void HideDialogue()
    {
        lookTarget = null;
        mainCamera.transform.localRotation = Quaternion.identity;
        canvas.enabled = false;
    }
}

