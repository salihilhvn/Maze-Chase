using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCamera;

    private void Start()
    {
        // Sahnede "MainCamera" etiketine sahip kamerayı bulur
        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (mainCamera == null) return;

        // UI elemanının (Canvas) yüzünü her zaman kameraya doğru düz bir şekilde çevir
        transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward,
                         mainCamera.rotation * Vector3.up);
    }
}
