using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Coin Spawn Settings")]
    public GameObject coinPrefab; // Prefab ของเหรียญ
    public Transform player; // ตัวละครของผู้เล่น
    public float spawnDistance = 30f; // ระยะที่เหรียญเกิดข้างหน้าผู้เล่น
    public int coinsPerRow = 5; // จำนวนเหรียญต่อแถว
    public float coinSpacing = 2f; // ระยะห่างของเหรียญ
    public float spawnInterval = 2f; // เวลาที่เกิดแถวเหรียญใหม่

    [Header("Coin Collection Settings")]
    public int coinValue = 1; // ค่าคะแนนของเหรียญ
    public AudioClip collectSound; // เสียงเก็บเหรียญ

    private float lastSpawnZ; // ตำแหน่ง Z ล่าสุดที่เหรียญเกิด

    void Start()
    {
        lastSpawnZ = player.position.z;
        InvokeRepeating(nameof(SpawnCoinRow), 0f, spawnInterval);
    }

    void SpawnCoinRow()
    {
        float spawnZ = lastSpawnZ + spawnDistance; // คำนวณตำแหน่ง Z
        float startX = -((coinsPerRow - 1) * coinSpacing) / 2; // จุดเริ่มต้นแถวเหรียญ

        for (int i = 0; i < coinsPerRow; i++)
        {
            Vector3 spawnPos = new Vector3(startX + (i * coinSpacing), 1f, spawnZ);
            GameObject newCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
            newCoin.AddComponent<CoinCollision>().Setup(coinValue, collectSound);
        }

        lastSpawnZ = spawnZ;
    }
}

public class CoinCollision : MonoBehaviour
{
    private int coinValue;
    private AudioClip collectSound;

    public void Setup(int value, AudioClip sound)
    {
        coinValue = value;
        collectSound = sound;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }
            Destroy(gameObject); // ทำลายเหรียญ
        }
    }
}
