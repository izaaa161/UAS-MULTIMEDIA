using UnityEngine;

// Semua rintanganmu harus punya Collider2D
[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour
{
    // Batas kiri layar untuk menghancurkan diri
    private float leftEdge;

    void Start()
    {
        // Hitung batas kiri sekali saja saat muncul
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    void Update()
    {
        // Bergerak ke kiri berdasarkan kecepatan global dari GameManager
        if (GameManager.Instance != null) {
            transform.position += GameManager.Instance.GameSpeed * Time.deltaTime * Vector3.left;
        }

        // Hancurkan diri jika sudah melewati batas
        if (transform.position.x < leftEdge) {
            Destroy(gameObject);
        }
    }
    
    // Logika tabrakan yang bersih
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Hanya cek tabrakan dengan Player
        if (other.CompareTag("Player")) 
        {
            // Laporkan ke GameManager bahwa permainan selesai
            if (GameManager.Instance != null) {
                GameManager.Instance.GameOver();
            }
        }
    }
}