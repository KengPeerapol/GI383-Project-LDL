using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [Header("ลาก FPSText (TMP) มาใส่ตรงนี้")]
    public TextMeshProUGUI fpsText;

    [Header("ความถี่ในการอัปเดตตัวเลข (วินาที)")]
    public float refreshRate = 0.25f; // ค่าเริ่มต้นคืออัปเดต 4 ครั้งต่อวินาที

    private float timer;
    private int frameCount;

    void Update()
    {
        // 1. นับเวลาที่ผ่านไป (ใช้ unscaledDeltaTime เพื่อให้ตัวเลขทำงานแม้ตอนกด Pause เกม)
        timer += Time.unscaledDeltaTime;

        // 2. นับจำนวนเฟรมที่เรนเดอร์ได้
        frameCount++;

        // 3. ถ้าเวลาผ่านไปถึงรอบที่กำหนด (เช่น 0.25 วิ) ค่อยคำนวณและแสดงผล
        if (timer >= refreshRate)
        {
            // คำนวณ FPS = จำนวนเฟรม / เวลา
            int fps = Mathf.RoundToInt(frameCount / timer);

            if (fpsText != null)
            {
                fpsText.text = "FPS: " + fps.ToString();
            }

            // รีเซ็ตตัวนับเพื่อเริ่มรอบใหม่
            timer = 0f;
            frameCount = 0;
        }
    }
}