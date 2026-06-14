using UnityEngine;

// ⭐ จุด Optimize 1: ป้องกันบั๊ก! บังคับให้ Unity ใส่ Rigidbody2D ให้ทันทีที่แปะสคริปต์นี้
// (หมดปัญหาผู้เล่นลืมใส่ Rigidbody2D แล้วเกมพังตอนกด Play)
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("ความเร็วของศัตรู")]
    public float speed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        // ดึงคอมโพเนนต์ Rigidbody2D ในตัวมันเองมาใช้งาน
        rb = GetComponent<Rigidbody2D>();

        // 1. สุ่มมุมเอียงขึ้น-ลง ระหว่าง -30 ถึง 30 องศา
        float randomAngle = Random.Range(-30f, 30f);

        // 2. คำนวณทิศทาง: เอาลูกศรชี้ซ้าย (Vector2.left) มาบิดองศาตามมุมที่สุ่มได้
        Vector2 moveDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.left;

        // 3. ออกแรงผลักให้ศัตรูพุ่งไปตามทิศทางนั้นๆ ด้วยความเร็วที่ตั้งไว้
        // (ใช้ linearVelocity ถูกต้องแล้วสำหรับ Unity เวอร์ชันใหม่)
        rb.linearVelocity = moveDirection * speed;
    }

    void Update()
    {
        // ⭐ จุด Optimize 2: ลดภาระเครื่อง (Caching)
        // การเรียกใช้ rb.linearVelocity จะมีการคำนวณหลังบ้านของ Unity
        // เราจึงดึงค่ามาเก็บไว้ในตัวแปร 'currentVelocity' ครั้งเดียวก่อน 
        // เพื่อจะได้ไม่ต้องสั่งให้ Unity ไปดึงข้อมูลถึง 3 รอบใน 1 เฟรม
        Vector2 currentVelocity = rb.linearVelocity;

        // เช็กว่าศัตรูกำลังขยับอยู่หรือไม่
        if (currentVelocity != Vector2.zero)
        {
            // ⭐ จุด Optimize 3: ใช้ตัวแปรที่เก็บไว้มาคำนวณ
            // Mathf.Atan2 จะช่วยแปลงความเร็วแกน X, Y ออกมาเป็น "มุมเรเดียน"
            // แล้วคูณด้วย Mathf.Rad2Deg เพื่อแปลงกลับเป็น "องศา" ที่คนเข้าใจได้
            float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;

            // สั่งบิดองศาภาพ (หันหน้า) ไปตามทิศทางที่กำลังพุ่งไปตลอดเวลา
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}