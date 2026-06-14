using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("ความเสียหายที่ทำกับ Player")]
    public float damage = 20f;

    // ฟังก์ชันนี้จะทำงานอัตโนมัติเมื่อมีวัตถุที่มี Collider (และติ๊ก Is Trigger) มาชน
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ⭐ จุด Optimize ที่ 1: ใช้ TryGetComponent แทน GetComponent
        // วิธีนี้โค้ดจะเช็กว่าสิ่งที่ชนมีสคริปต์ PlayerHealth ไหม 
        // ถ้ามี มันจะดึงมาเก็บไว้ในตัวแปร 'player' ให้พร้อมใช้งานทันที (ทำงานเร็วกว่าแบบเดิม)
        if (collision.TryGetComponent(out PlayerHealth player))
        {
            // ⭐ จุด Optimize ที่ 2: เช็กเฉพาะตอนที่ "ยังไม่ชนะ"
            // เครื่องหมายอัศเจรีย์ (!) แปลว่า "ไม่" -> ถ้า FinishTime.IsGameWon "ไม่ใช่ความจริง" (คือเวลายังไม่หมด)
            if (!FinishTime.IsGameWon)
            {
                // ให้ทำการหักเลือดผู้เล่นตามปกติ
                player.TakeDamage(damage);
            }

            // ⭐ จุด Optimize ที่ 3: ดึง Destroy ออกมาข้างนอก
            // ไม่ว่าจะอยู่ในสถานะไหน (เวลายังไม่หมด หรือ ชนะแล้วพุ่งมาชน) 
            // สุดท้ายศัตรูตัวนี้ก็ต้องระเบิดหายไปอยู่ดี เลยเขียนคำสั่งแค่รอบเดียวพอครับ
            Destroy(gameObject);
        }
    }
}