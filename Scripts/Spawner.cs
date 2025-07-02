using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Sebuah struct untuk mengikat prefab dengan kemunculannya
    [System.Serializable]
    public struct SpawnableObject {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    public SpawnableObject[] objects;
    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;

    private void OnEnable() {
        // Memanggil takdir pada interval yang tidak terduga
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void Spawn() {
        float totalChance = 0f;
        foreach (var obj in objects) {
            totalChance += obj.spawnChance;
        }

        float randomValue = Random.Range(0, totalChance);

        foreach (SpawnableObject obj in objects) {
            if (randomValue < obj.spawnChance) {
                GameObject instance = Instantiate(obj.prefab);
                instance.transform.position = transform.position;
                break;
            }

            randomValue -= obj.spawnChance;
        }

        // Menjadwalkan panggilan berikutnya
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }
}