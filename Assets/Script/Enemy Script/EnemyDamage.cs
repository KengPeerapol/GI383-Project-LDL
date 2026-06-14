using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("ความเสียหายที่ทำกับ Player")]
    public float damage = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            // ⭐ เพิ่มเงื่อนไขเช็ก: ถ้าเวลาหมด (ชนะแล้ว)
            if (FinishTime.IsGameWon)
            {
                Destroy(gameObject); // ศัตรูหายไปเลย ไม่ลดเลือด
            }
            else // แต่ถ้าเวลายังไม่หมด (กำลังเล่นปกติ)
            {
                player.TakeDamage(damage); // ลดเลือดปกติ
                Destroy(gameObject);
            }
        }
    }
}