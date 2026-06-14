using UnityEngine;

public class WallBounce : MonoBehaviour
{
    [Header("ความแรงในการเด้ง")]
    public float bounceForce = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // เช็กว่าคนที่มาชนคือ Player ใช่หรือไม่ (ดูจากสคริปต์ PlayerController)
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // หาเวกเตอร์ทิศทางการชน (เช่น ถ้าชนกำแพงบน ทิศจะชี้ลงด้านล่างอัตโนมัติ)
                Vector2 bounceDirection = collision.contacts[0].normal;

                // สั่งให้ Player เด้งไปในทิศทางนั้นด้วยความแรงที่กำหนด
                rb.velocity = bounceDirection * bounceForce;
            }
        }
    }
}