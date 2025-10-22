using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public Transform doorTransform;     // Префаб двери
    public Handle handleScript;         // Ссылка на скрипт ручки
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isPlayerInTrigger = false;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isRotating = false;
    private bool hasRotatedHandle = false;

    void Start()
    {
        closedRotation = doorTransform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, -openAngle, 0f);
    }

    void Update()
    {
        if (isPlayerInTrigger)
            Debug.Log("Player is in trigger zone");

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.R))
            Debug.Log("R pressed inside trigger");


        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.R) && !isRotating)
        {
            isRotating = true;
            isOpen = !isOpen;
            hasRotatedHandle = false; // Сброс перед новым вращением
        }

        if (isRotating)
        {
            Quaternion targetRotation = isOpen ? openRotation : closedRotation;
            doorTransform.rotation = Quaternion.RotateTowards(doorTransform.rotation, targetRotation, openSpeed * Time.deltaTime * 100f);

            // Ручка вращается только один раз
            if (!hasRotatedHandle)
            {
                handleScript.RotateHandle(isOpen);  // Поворачиваем ручку
                hasRotatedHandle = true;
            }

            if (Quaternion.Angle(doorTransform.rotation, targetRotation) < 0.1f)
            {
                doorTransform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger");
            isPlayerInTrigger = false;
        }
    }

    

}
