using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour 
{
    public static GameManager Instance { get; private set; }

    [Header("Pengaturan Kecepatan")]
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float GameSpeed { get; private set; }

    [Header("Referensi Scene")]
    [SerializeField] private Player player;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Parallax parallax; 

    [Header("Referensi UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject gameOverScreen;

    private float score;

    private void Awake() 
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }
    
    private void Start() 
    {
        NewGame();
    }

    public void NewGame() 
    {
        // Reset semua state
        score = 0f;
        GameSpeed = initialGameSpeed;
        
        // Aktifkan semua komponen game
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        parallax.enabled = true;

        // Sembunyikan UI yang tidak perlu
        gameOverScreen.SetActive(false);
        retryButton.SetActive(false);

        // Hancurkan semua rintangan sisa dari permainan sebelumnya
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (var obstacle in obstacles) {
            Destroy(obstacle.gameObject);
        }

        enabled = true; // Aktifkan Update() di GameManager ini
    }

    public void GameOver() 
    {
        // Matikan semua komponen game
        GameSpeed = 0f;
        spawner.gameObject.SetActive(false);
        parallax.enabled = false;
        
        // Tampilkan UI yang relevan
        gameOverScreen.SetActive(true);
        retryButton.SetActive(true);

        enabled = false; // Matikan Update() di GameManager ini

        // Matikan Player di akhir agar tidak ada error aneh
        player.gameObject.SetActive(false);
    }

    private void Update() 
    {
        // Tingkatkan kecepatan dan skor seiring waktu
        GameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += GameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
    }
}