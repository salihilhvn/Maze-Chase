using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    
    // 65-70 derece açıya uygun bir offset. İhtiyacına göre Unity Inspector'dan değiştirebilirsin.
    public Vector3 offset = new Vector3(0f, 12f, -6f); 
    
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Kameranın gitmesi gereken hedef pozisyon
        Vector3 desiredPosition = target.position + offset;
        
        // Yumuşak geçiş (Lerp) ile kamerayı hareket ettir
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // Kamera sürekli karaktere baksın (Açıyı otomatik ayarlar)
        transform.LookAt(target);
    }
}
