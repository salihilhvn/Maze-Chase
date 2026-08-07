using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
using System.Collections; 

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [Header("Level Info")]
    public int totalCollectibles = 0;
    public int collectedCount = 0;

    [Header("UI Elements (Arayüz)")]
    public TextMeshProUGUI scoreText; 
    public GameObject levelCompletePanel; 
    public GameObject levelFailedPanel; // YENI EKLENDI: Yakalanınca çıkacak ekran

    private bool isGameOver = false; // Oyunun bittiğini takip etmek için

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI(); 
        
        if (levelCompletePanel != null) levelCompletePanel.SetActive(false); 
        if (levelFailedPanel != null) levelFailedPanel.SetActive(false);
    }

    public void RegisterCollectible()
    {
        totalCollectibles++;
        UpdateUI(); 
    }

    public void CollectItem()
    {
        if (isGameOver) return; // Oyun bittiyse skor artmasın
        collectedCount++;
        UpdateUI(); 
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
        if (isGameOver) return;

        if (collectedCount >= totalCollectibles)
        {
            isGameOver = true;
            if (levelCompletePanel != null) levelCompletePanel.SetActive(true); 
            StartCoroutine(LoadNextLevelAfterDelay(2f));
        }
    }

    // YENI EKLENDI: Düşman bizi yakaladığında bu fonksiyon çalışacak
    public void LevelFailed()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("YAKALANDIN! BÖLÜM BAŞARISIZ.");
        
        if (levelFailedPanel != null) levelFailedPanel.SetActive(true); 
        
        // 2 saniye sonra aynı bölümü tekrar yükle
        StartCoroutine(RestartLevelAfterDelay(2f));
    }

    private IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0); 
        }
    }

    private IEnumerator RestartLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
