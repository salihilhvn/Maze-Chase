using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; // Her yerden kolayca ulaşmak için Singleton
    
    [Header("Level Info")]
    public int totalCollectibles = 0;
    public int collectedCount = 0;

    private void Awake()
    {
        // Singleton kurulumu (Sahnede sadece 1 tane LevelManager olmasını garanti eder)
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Haritadaki objeler (altın/anahtar) oyuna başladığında kendilerini bu fonskyionla kaydeder
    public void RegisterCollectible()
    {
        totalCollectibles++;
    }

    // Karakter bir obje aldığında bu fonksiyon çağrılır
    public void CollectItem()
    {
        collectedCount++;
        Debug.Log("Obje Toplandı! Toplanan: " + collectedCount + "/" + totalCollectibles);
        
        // TODO: UI (Ekranda yazan skor) güncellenecek
    }

    // Karakter Çıkış Kapısına (Exit) geldiğinde bu fonksiyon çalışır
    public void CheckLevelComplete()
    {
        if (collectedCount >= totalCollectibles)
        {
            Debug.Log("BÖLÜM BİTTİ! Sonraki bölüme geçiliyor...");
            LoadNextLevel();
        }
        else
        {
            Debug.Log("Çıkış kapısı kilitli! Daha toplaman gereken " + (totalCollectibles - collectedCount) + " obje var!");
        }
    }

    private void LoadNextLevel()
    {
        // Mevcut sahnenin indeksini alıp 1 ekliyoruz
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Eğer File -> Build Settings'de sırada başka bir bölüm (sahne) varsa onu yükle
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Oyun Bitti! Ana Menüye dönülüyor...");
            // Oynanacak bölüm kalmadıysa başa sar
            SceneManager.LoadScene(0); 
        }
    }
}
