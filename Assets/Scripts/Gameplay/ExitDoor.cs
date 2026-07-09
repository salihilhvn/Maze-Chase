using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Karakter (Player) çıkış kapısına çarptığında
        if (other.CompareTag("Player"))
        {
            if (LevelManager.Instance != null)
            {
                // Bölümün bitip bitmediğini (yeterince obje toplanıp toplanmadığını) kontrol et
                LevelManager.Instance.CheckLevelComplete();
            }
        }
    }
}
