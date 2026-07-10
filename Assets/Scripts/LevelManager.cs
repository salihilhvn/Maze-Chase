using UnityEngine;
using TMPro; // Ekrana yazı yazdırmak için ekledik
using System.Collections; // Bekletme süresi (Coroutine) için ekledik

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [Header("Level Info")]
    public int totalCollectibles = 0;
    public int collectedCount = 0;

    [Header("Level System")]
    public GameObject[] levelPrefabs; // Tüm level prefablerini buraya koyacağız
    public Transform playerTransform; // Yeni level başlayınca oyuncuyu sıfırlamak için
    public Vector3 playerSpawnPosition = Vector3.zero; // Oyuncunun başlangıç pozisyonu
    private int currentLevelIndex = 0;
    private GameObject currentLevelInstance;

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
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        // 1. Varsa eski bölümü sil
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        // 2. Altın sayaçlarını sıfırla
        totalCollectibles = 0;
        collectedCount = 0;
        
        // UI'ı gizle ve yazıyı güncelle
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
        UpdateUI();

        // 3. Yeni bölümü yarat (Eğer index dizinin dışına çıkmadıysa)
        if (levelPrefabs != null && levelPrefabs.Length > 0)
        {
            if (levelIndex < levelPrefabs.Length)
            {
                currentLevelInstance = Instantiate(levelPrefabs[levelIndex]);
            }
            else
            {
                Debug.Log("Tüm bölümler bitti, başa dönülüyor!");
                currentLevelIndex = 0; // Başa sar
                currentLevelInstance = Instantiate(levelPrefabs[currentLevelIndex]);
            }
        }
        else
        {
            Debug.LogWarning("LevelManager içinde hiç Prefab yok! Lütfen Editörden ekleyin.");
        }

        // 4. Oyuncuyu başlangıç noktasına ışınla
        if (playerTransform != null)
        {
            playerTransform.position = playerSpawnPosition;
            // Eğer oyuncunun bir Rigidbody'si varsa hareketlerini sıfırlamak iyi olabilir
            Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
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
        
        currentLevelIndex++; // Sonraki bölüme geç
        LoadLevel(currentLevelIndex);
    }
}
