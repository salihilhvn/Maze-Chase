using System.Collections;
using UnityEngine;

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

    private RectTransform currentPanel;
    private Coroutine panelCoroutine;
    private Coroutine squareCoroutine;
    
    private Vector3 merkezPos;
    private float ekranGenisligi;

    void Start()
    {
        // Ekranın GERÇEK pixel genişliğini alıyoruz (Anchor/Pivot kaynaklı hataları 0'a indirir)
        ekranGenisligi = Screen.width;
        
        // Home panelinin başlangıçtaki dünya pozisyonunu (position) MERKEZ kabul ediyoruz
        merkezPos = homePanel.position;

        currentPanel = homePanel;
        
        // Diğer panelleri baştan sağa ve sola fırlatıyoruz (Tamamen World Position)
        leaderboardPanel.position = merkezPos + new Vector3(-ekranGenisligi, 0, 0);
        storePanel.position = merkezPos + new Vector3(ekranGenisligi, 0, 0);
    }

    public void LeaderboardaGit()
    {
        if (currentPanel == leaderboardPanel) return;
        
        SlideYap(leaderboardPanel, -ekranGenisligi, ekranGenisligi);
        KareyiKaydir(leaderboardBtn);
    }

    public void HomeaGit()
    {
        if (currentPanel == homePanel) return;

        float baslangicX = (currentPanel == leaderboardPanel) ? ekranGenisligi : -ekranGenisligi;
        float cikisX = (currentPanel == leaderboardPanel) ? -ekranGenisligi : ekranGenisligi;

        SlideYap(homePanel, baslangicX, cikisX);
        KareyiKaydir(homeBtn);
    }

    public void StoreaGit()
    {
        if (currentPanel == storePanel) return;

        SlideYap(storePanel, ekranGenisligi, -ekranGenisligi);
        KareyiKaydir(storeBtn);
    }

    private void SlideYap(RectTransform yeniPanel, float girisXOffset, float cikisXOffset)
    {
        if (panelCoroutine != null) StopCoroutine(panelCoroutine);
        panelCoroutine = StartCoroutine(PanelKaydirma(currentPanel, yeniPanel, girisXOffset, cikisXOffset));
        currentPanel = yeniPanel;
    }

    private IEnumerator PanelKaydirma(RectTransform eski, RectTransform yeni, float girisOffset, float cikisOffset)
    {
        // Gidecekleri dünya (ekran) pozisyonlarını hesapla
        Vector3 girisPos = merkezPos + new Vector3(girisOffset, 0, 0);
        Vector3 cikisPos = merkezPos + new Vector3(cikisOffset, 0, 0);
        
        yeni.position = girisPos;
        
        // Hedefe varana kadar döngü
        while (Vector3.Distance(yeni.position, merkezPos) > 1f)
        {
            eski.position = Vector3.Lerp(eski.position, cikisPos, Time.deltaTime * kaymaHizi);
            yeni.position = Vector3.Lerp(yeni.position, merkezPos, Time.deltaTime * kaymaHizi);
            yield return null;
        }

        yeni.position = merkezPos;
        eski.position = cikisPos;
    }

    private void KareyiKaydir(RectTransform hedefBtn)
    {
        if (squareCoroutine != null) StopCoroutine(squareCoroutine);
        squareCoroutine = StartCoroutine(KareKaydirma(hedefBtn));
    }

    private IEnumerator KareKaydirma(RectTransform hedefBtn)
    {
        while (Mathf.Abs(activeSquare.position.x - hedefBtn.position.x) > 0.1f)
        {
            activeSquare.position = Vector3.Lerp(activeSquare.position, new Vector3(hedefBtn.position.x, activeSquare.position.y, activeSquare.position.z), Time.deltaTime * kaymaHizi);
            yield return null;
        }
        activeSquare.position = new Vector3(hedefBtn.position.x, activeSquare.position.y, activeSquare.position.z);
    }
}
