using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            ScoreManager.instance.AddScore(coinValue); // เพิ่มคะแนน
            Destroy(other.gameObject);                 // ลบเหรียญ
        }
    }
}