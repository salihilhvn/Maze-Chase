using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    [Header("Hedef ve UI (Ayarla)")]
    [Tooltip("Takip edilecek güvenlik görevlisi objesi")]
    public Transform target; 
    
    [Tooltip("Ekranın kenarında çıkacak uyarı ikonu (UI Image)")]
    public RectTransform indicatorUI; 
    
    [Tooltip("İkon ekran kenarından ne kadar içeride dursun?")]
    public float edgePadding = 50f; 

    private Camera mainCam;
    private Image indicatorImage;

    private void Start()
    {
        mainCam = Camera.main;
        if (indicatorUI != null)
        {
            // UI ikonunu gizlemek/açmak için Image componentini alıyoruz
            indicatorImage = indicatorUI.GetComponent<Image>();
        }
    }

    private void LateUpdate()
    {
        if (target == null || indicatorUI == null || mainCam == null) return;

        // 1. Hedefin (Görevlinin) dünyadaki 3D pozisyonunu ekranın 2D X,Y koordinatlarına çeviriyoruz
        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position);

        // Hedef kameranın arkasında kaldı mı? (Örn: biz ileri koşarken adam arkada kaldıysa Z değeri negatif olur)
        bool isBehind = screenPos.z < 0;

        // 2. Hedef ekranın tamamen dışına çıktı mı? (Sağ, Sol, Üst, Alt veya Arkada)
        bool isOffScreen = isBehind || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        if (isOffScreen)
        {
            // Adam kadrajda değil! Uyarı ikonunu ekranda GÖSTER.
            if (indicatorImage != null) indicatorImage.enabled = true;

            // Kamera arkasındaysa koordinatları aynala (yön şaşmaması için)
            if (isBehind)
            {
                screenPos.x = Screen.width - screenPos.x;
                screenPos.y = Screen.height - screenPos.y;
            }

            // 3. İkonu ekranın en kenarlarına kelepçele (Clamp)
            // Böylece dışarı taşmaz, ekranın sınırlarında yapışık kalır.
            screenPos.x = Mathf.Clamp(screenPos.x, edgePadding, Screen.width - edgePadding);
            screenPos.y = Mathf.Clamp(screenPos.y, edgePadding, Screen.height - edgePadding);

            // İkonun pozisyonunu güncelle
            indicatorUI.position = new Vector3(screenPos.x, screenPos.y, 0f);

            // 4. (Opsiyonel Görsellik) İkonu adamın olduğu yöne doğru döndür
            // Eğer bir ok görseli kullanırsan çok işe yarar
            Vector3 center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Vector3 dir = (screenPos - center).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            // indicatorUI.rotation = Quaternion.Euler(0, 0, angle - 90f); 
        }
        else
        {
            // Adam zaten kadrajda! İkonu GİZLE.
            if (indicatorImage != null) indicatorImage.enabled = false;
        }
    }
}
