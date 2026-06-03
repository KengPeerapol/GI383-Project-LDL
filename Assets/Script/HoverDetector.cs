using UnityEngine;

public class HoverDetector : MonoBehaviour
{
    [Header("ลากออบเจกต์ HintText มาใส่ตรงนี้")]
    public GameObject hintText;

    // ตัวแปรเก็บกล่องรับการชนของสามเหลี่ยม
    private Collider2D myCollider;

    void Start()
    {
        // ให้โค้ดดึงเอา Polygon Collider 2D ในตัวสามเหลี่ยมมาใช้งานอัตโนมัติ
        myCollider = GetComponent<Collider2D>();

        // ปิดการแสดงผลข้อความไว้ตอนเริ่มเกม
        if (hintText != null)
        {
            hintText.SetActive(false);
        }
    }

    void Update()
    {
        if (myCollider == null || hintText == null) return;

        // 1. ดึงตำแหน่งเมาส์ปัจจุบัน แล้วแปลงเป็นพิกัดในโลกของเกม 2D
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2. เช็คแบบคณิตศาสตร์ชัวร์ๆ ว่าพิกัดเมาส์ เข้ามาอยู่ในพื้นที่ Collider หรือยัง
        if (myCollider.OverlapPoint(mousePosition))
        {
            // ถ้าเมาส์อยู่ข้างใน -> เปิดข้อความ
            hintText.SetActive(true);
        }
        else
        {
            // ถ้าเมาส์อยู่ข้างนอก -> ปิดข้อความ
            hintText.SetActive(false);
        }
    }
}