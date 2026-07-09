using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void Start()
    {
        // Sahne başladığında bu obje kendini LevelManager'a kaydeder.
        // Böylece haritada kaç tane toplanması gereken obje olduğunu otomatik saymış oluruz.
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RegisterCollectible();
        }
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
