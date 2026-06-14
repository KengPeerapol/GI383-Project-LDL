using UnityEngine;

public class WallBounce : MonoBehaviour
{
    [Header("ความแรงในการเด้ง")]
    public float bounceForce = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ⭐ จุด Optimize 1: ใช้ TryGetComponent เช็กว่าเป็น Player ไหม (ทำงานเร็วกว่า GetComponent)
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            // ถ้าเป็น Player แน่ๆ ค่อยดึงกล่องฟิสิกส์ของมันมาใช้งานด้วย TryGetComponent อีกรอบ
            if (collision.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                // ⭐ จุด Optimize 2: ใช้ GetContact(0) แทนคำสั่ง contacts[0] แบบเก่า
                // การใช้ contacts[0] Unity จะแอบสร้าง Array ขึ้นมาใหม่ทำให้เปลือง RAM ชั่วคราว
                // GetContact(0) จะดึงจุดที่ชนจุดแรกมาให้เลยทันทีแบบเพียวๆ ทำงานเร็วกว่ามากครับ
                Vector2 bounceDirection = collision.GetContact(0).normal;

                // ⭐ จุด Optimize 3: เปลี่ยนจาก velocity เป็น linearVelocity 
                // เพื่อให้โค้ดทันสมัยและเข้ากับระบบฟิสิกส์ของ Unity 6 มากที่สุด
                rb.linearVelocity = bounceDirection * bounceForce;
            }
        }
    }
}