using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeedX = 0f;
    public float rotationSpeedY = 150f; // Kendi etrafında fırıl fırıl dönmesi için
    public float rotationSpeedZ = 0f;  // İstersen Unity içinden bu değerleri değiştirebilirsin

    private void Start()
    {
        // Sahne başladığında bu obje kendini LevelManager'a kaydeder.
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RegisterCollectible();
        }
    }

    private void Update()
    {
        // Objeyi her saniye belirlediğimiz hızda döndürür. (Time.deltaTime oyunun kasması/hızlanmasından bağımsız stabil dönmesini sağlar)
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Eğer çarpan kişi "Player" etiketine sahipse çalışır
        if (other.CompareTag("Player"))
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.CollectItem();
            }
            
            // Objeyi (altın/anahtar vb.) yokederek ekrandan kaldır
            Destroy(gameObject);
        }
    }
}
