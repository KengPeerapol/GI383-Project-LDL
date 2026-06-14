using UnityEngine;
using TMPro;

public class FinishLine : MonoBehaviour
{
    [Header("ตั้งค่าการเคลื่อนที่")]
    public float moveSpeed = 5f;

    [Header("อ้างอิง (ลากมาใส่จาก Hierarchy)")]
    public Transform player;
    public TextMeshProUGUI distanceText;

    void Update()
    {
        // 1. ทำให้เส้นชัยเคลื่อนที่ไปทางซ้ายเรื่อยๆ
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // 2. คำนวณระยะทาง
        if (player != null && distanceText != null)
        {
            float distance = transform.position.x - player.position.x;

            if (distance > 0)
            {
                // เปลี่ยนคำว่า "ระยะทาง: " เป็น "Distance: "
                distanceText.text = "Distance : " + distance.ToString("F0") + " m";
            }
            else
            {
                // เปลี่ยนคำว่า "เข้าเส้นชัยแล้ว!" เป็น "Finish!"
                distanceText.text = "Finish!";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Player reached the finish line!");

            Time.timeScale = 0f;
        }
    }
}