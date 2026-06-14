using UnityEngine;

public class WallDamage : MonoBehaviour
{
    [Header("ความเสียหายเมื่อชนกำแพง")]
    public float damage = 20f;

    [Header("ตั้งค่าการเสียการควบคุม (Stun)")]
    public bool canStunPlayer = false;   // ติ๊กถูกเพื่อเปิดใช้งานระบบสตัน
    public float stunDuration = 0.5f;    // ระยะเวลาที่คุมไม่ได้

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. จัดการเรื่องลดเลือด
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        // 2. จัดการเรื่องเสียการควบคุม
        if (canStunPlayer) // ถ้ากำแพงนี้ตั้งค่าให้สตันได้
        {
            PlayerController playerCtrl = collision.gameObject.GetComponent<PlayerController>();
            if (playerCtrl != null)
            {
                // สั่งให้ Player เสียการควบคุมตามเวลาที่ตั้งไว้
                playerCtrl.ApplyStun(stunDuration);
            }
        }
    }
}