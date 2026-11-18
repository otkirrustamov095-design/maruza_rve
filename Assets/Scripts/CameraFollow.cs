using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 5f, -10f);
    public float smooth = 5f;

    void LateUpdate()
    {
        if (!target) return;
        // Faqat Y o'qi bo'yicha aylanishni hisobga olamiz (mashina ag'darilsa kamera aylanib ketmasin)
        Quaternion yaw = Quaternion.Euler(0f, target.eulerAngles.y, 0f);
        Vector3 desired = target.position + yaw * offset;
        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}
