using UnityEngine;

public class BlackHoleVisual : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float pulseAmplitude = 0.1f;
    public float pulseFrequency = 2f;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        float pulse = Mathf.Sin(Time.time * pulseFrequency) * pulseAmplitude;
        transform.localScale = initialScale + new Vector3(pulse, pulse, 0f);
    }
}
