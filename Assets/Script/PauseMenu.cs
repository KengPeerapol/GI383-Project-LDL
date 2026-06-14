using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("หน้าต่าง Pause Menu")]
    public GameObject pauseMenuUI;

    [Header("ปุ่ม Pause (หน้าจอปกติ)")]
    public GameObject pauseButtonUI; // ⭐ เอาไว้สั่งซ่อนปุ่มตอนเกมจบ

    private bool isPaused = false;

    // ⭐ ตัวแปรกลางที่เปิดให้สคริปต์อื่น (เลือด, เวลา) สั่งปิดระบบ Pause ได้
    public static bool isGameOver = false;

    void Start()
    {
        // รีเซ็ตค่าให้กด Pause ได้ปกติเมื่อเริ่มฉากใหม่
        isGameOver = false;
    }

    void Update()
    {
        // ถ้าเกมจบแล้ว (ชนะหรือแพ้)
        if (isGameOver)
        {
            // ซ่อนปุ่ม Pause บนหน้าจอทิ้งไปเลย ผู้เล่นจะได้กดไม่ได้
            if (pauseButtonUI != null && pauseButtonUI.activeSelf)
            {
                pauseButtonUI.SetActive(false);
            }
            return; // ❌ หยุดการทำงานของโค้ดตรงนี้ (ทำให้กด Esc ไม่ได้)
        }

        // ระบบกดปุ่ม Esc ตามปกติ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isGameOver) return; // กันเหนียวเผื่อผู้เล่นมือไวกดตอนตายพอดี

        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        if (pauseButtonUI != null) pauseButtonUI.SetActive(false); // ซ่อนปุ่มตอนเปิดเมนู

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (isGameOver) return;

        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (pauseButtonUI != null) pauseButtonUI.SetActive(true); // โชว์ปุ่มตอนเล่นปกติ

        Time.timeScale = 1f;
        isPaused = false;
    }
}