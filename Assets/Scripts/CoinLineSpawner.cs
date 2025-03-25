using UnityEngine;

public class CoinLineSpawner : MonoBehaviour
{
    public GameObject coinPrefab;     // Prefab เหรียญ
    public int coinCount = 10;        // จำนวนเหรียญ
    public float spacing = 2f;        // ระยะห่างระหว่างเหรียญ
    public Vector3 direction = Vector3.forward; // ทิศทางการเรียง
    public float heightOffset = 0.5f; // ความสูงจากพื้น

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnPosition = transform.position + direction.normalized * spacing * i;
            spawnPosition.y += heightOffset; // ยกเหรียญลอยจากพื้นเล็กน้อย

            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }
}