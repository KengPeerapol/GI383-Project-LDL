using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("ใส่ PauseMenuPanel ตรงนี้")]
    public GameObject pauseMenuUI;

    // เพิ่มตัวแปรนี้เพื่อเช็กว่า "ตอนนี้เกมหยุดอยู่หรือไม่?"
    private bool isPaused = false;

    // ฟังก์ชัน Update จะคอยตรวจสอบการกดปุ่มตลอดเวลา
    void Update()
    {
        // ถ้าผู้เล่นกดปุ่ม Esc (Escape) บนคีย์บอร์ด
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ถ้าเกมหยุดอยู่ -> ให้เล่นต่อ
            if (isPaused)
            {
                ResumeGame();
            }
            // ถ้าเกมเล่นปกติอยู่ -> ให้หยุดเกม
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        Time.timeScale = 0f;
        isPaused = true; // อัปเดตสถานะว่า "เกมหยุดแล้ว"
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false; // อัปเดตสถานะว่า "เกมกำลังเล่นอยู่"
    }
}