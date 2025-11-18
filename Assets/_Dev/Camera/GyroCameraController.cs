using UnityEngine;

public class GyroCameraController : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;

    private Quaternion rotationFix = new Quaternion(0, 0, 1, 0);
    private Quaternion targetRotation;
    private float smoothSpeed = 5f;

    void Start()
    {
        gyroEnabled = EnableGyro();
    }

    void Update()
    {
        if (gyroEnabled)
        {
            // Aplica a rotação suavemente
            targetRotation = gyro.attitude * rotationFix;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothSpeed * Time.deltaTime);
        }
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }
        return false;
    }
}

