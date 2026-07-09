using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Ekrana yazı yazdırmak için ekledik
using System.Collections; // Bekletme süresi (Coroutine) için ekledik

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [Header("Level Info")]
    public int totalCollectibles = 0;
    public int collectedCount = 0;

    [Header("UI Elements (Arayüz)")]
    public TextMeshProUGUI scoreText; // Sol üstte yazacak "Altın: 1/3" yazısı
    public GameObject levelCompletePanel; // Kapıya gelince çıkacak "Tebrikler" ekranı

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI(); // Başlarken skoru 0/0 olarak yazdır
        
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false); // Başlangıçta tebrikler ekranını gizle
    }

    public void RegisterCollectible()
    {
        totalCollectibles++;
        UpdateUI(); // Yeni altın bulundukça yazıyı güncelle
    }

    public void CollectItem()
    {
        collectedCount++;
        UpdateUI(); // Altın toplandıkça yazıyı güncelle
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Altın: " + collectedCount + " / " + totalCollectibles;
        }
    }

    public void CheckLevelComplete()
    {
        if (collectedCount >= totalCollectibles)
        {
            // Paneli göster
            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true); 
            }
            
            // Bölümü pat diye değil, 2 saniye bekleyip değiştir (Oyuncu yazıyı görsün)
            StartCoroutine(LoadNextLevelAfterDelay(2f));
        }
    }

    private IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // delay saniye bekle
        
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Sırada sahne varsa yükle, yoksa (oyun bittiyse) en başa dön
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0); 
        }
    }
}
