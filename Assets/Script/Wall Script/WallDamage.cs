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
        // ⭐ จุด Optimize 1: ใช้ TryGetComponent เพื่อเช็กว่าเป็นผู้เล่นหรือไม่ (เร็วกว่าแบบเดิม)
        // ถ้าสิ่งที่พุ่งมาชน "มี" สคริปต์ PlayerHealth แปลว่าเป็นผู้เล่นแน่นอน โค้ดถึงจะยอมเข้าไปทำคำสั่งด้านใน
        if (collision.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        {
            // 1. จัดการเรื่องลดเลือดทันที
            playerHealth.TakeDamage(damage);

            // 2. จัดการเรื่องเสียการควบคุม
            // ⭐ จุด Optimize 2: เช็กตัวแปร bool ก่อนเลย! 
            // ถ้ากำแพงนี้ไม่ได้ติ๊กถูก canStunPlayer โค้ดก็จะไม่ต้องเสียเวลาไปดึงสคริปต์ PlayerController ให้เหนื่อยฟรีๆ
            if (canStunPlayer)
            {
                // ดึงสคริปต์บังคับตัวละคร (ดึงเฉพาะตอนที่จะสตันจริงๆ)
                if (collision.gameObject.TryGetComponent(out PlayerController playerCtrl))
                {
                    // สั่งให้ Player เสียการควบคุมตามเวลาที่ตั้งไว้
                    playerCtrl.ApplyStun(stunDuration);
                }
            }
        }
    }
}