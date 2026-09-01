using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [Header("Sayfa Panelleri")]
    public RectTransform homePanel;
    public RectTransform leaderboardPanel;
    public RectTransform storePanel;

    [Header("Alt Butonlar ve Seçili Kare")]
    public RectTransform activeSquare;
    public RectTransform homeBtn;
    public RectTransform leaderboardBtn;
    public RectTransform storeBtn;

    [Header("Animasyon Hızı")]
    public float kaymaHizi = 15f;

    // Her panelin gitmesi gereken hedef X koordinatları
    private float homeTargetX;
    private float leaderboardTargetX;
    private float storeTargetX;
    
    private RectTransform activeBtn;

    private float merkezX;
    private float ekranGenisligi;

    void Start()
    {
        // Ekranın KESİN fiziksel genişliği
        ekranGenisligi = Screen.width;
        
        // Ekranın KESİN ortasının X koordinatı (Canvas Overlay modunda)
        merkezX = Screen.width / 2f;
        
        // Başlangıç hedeflerini ayarla (Home ortada)
        SetTargets(homePanel);
        
        // Panelleri anında yerlerine oturt. (Eğer Unity'de tasarlarken sahnede yanlışlıkla 
        // kaydırdıysan bile oyun başladığı an tam olması gereken mükemmel yerlerine geçerler)
        homePanel.position = new Vector3(homeTargetX, homePanel.position.y, homePanel.position.z);
        leaderboardPanel.position = new Vector3(leaderboardTargetX, leaderboardPanel.position.y, leaderboardPanel.position.z);
        storePanel.position = new Vector3(storeTargetX, storePanel.position.y, storePanel.position.z);
        
        // Karenin başlangıç ayarı
        activeBtn = homeBtn;
        activeSquare.position = new Vector3(homeBtn.position.x, activeSquare.position.y, activeSquare.position.z);
    }

    void Update()
    {
        // Tüm panelleri her saniye kendi hedefine doğru yumuşakça kaydır.
        // Bu sistem sayesinde butonlara art arda hızlıca bassan bile paneller yolda kalmaz, şaşmaz!
        homePanel.position = Vector3.Lerp(homePanel.position, new Vector3(homeTargetX, homePanel.position.y, homePanel.position.z), Time.deltaTime * kaymaHizi);
        leaderboardPanel.position = Vector3.Lerp(leaderboardPanel.position, new Vector3(leaderboardTargetX, leaderboardPanel.position.y, leaderboardPanel.position.z), Time.deltaTime * kaymaHizi);
        storePanel.position = Vector3.Lerp(storePanel.position, new Vector3(storeTargetX, storePanel.position.y, storePanel.position.z), Time.deltaTime * kaymaHizi);

        // Seçili kareyi de aktif butonun üzerine kaydır
        if (activeBtn != null)
        {
            activeSquare.position = Vector3.Lerp(activeSquare.position, new Vector3(activeBtn.position.x, activeSquare.position.y, activeSquare.position.z), Time.deltaTime * kaymaHizi);
        }
    }

    public void LeaderboardaGit()
    {
        SetTargets(leaderboardPanel);
        activeBtn = leaderboardBtn;
    }

    public void HomeaGit()
    {
        SetTargets(homePanel);
        activeBtn = homeBtn;
    }

    public void StoreaGit()
    {
        SetTargets(storePanel);
        activeBtn = storeBtn;
    }

    private void SetTargets(RectTransform merkezdekiPanel)
    {
        // Burası sihrin koptuğu yer. Panellerin sırasını (Leaderboard -> Home -> Store) asla bozmuyoruz.
        if (merkezdekiPanel == homePanel)
        {
            homeTargetX = merkezX;
            leaderboardTargetX = merkezX - ekranGenisligi;
            storeTargetX = merkezX + ekranGenisligi;
        }
        else if (merkezdekiPanel == leaderboardPanel)
        {
            leaderboardTargetX = merkezX;
            homeTargetX = merkezX + ekranGenisligi; // Home sağa kayar
            storeTargetX = merkezX + (ekranGenisligi * 2); // Store daha da sağa kayar (arkada bekler)
        }
        else if (merkezdekiPanel == storePanel)
        {
            storeTargetX = merkezX;
            homeTargetX = merkezX - ekranGenisligi; // Home sola kayar
            leaderboardTargetX = merkezX - (ekranGenisligi * 2); // Leaderboard daha da sola kayar (arkada bekler)
        }
    }

    public void StartGame()
    {
        // "Gameplay" adlı sahneyi yükler
        SceneManager.LoadScene("Gameplay");
    }
}
