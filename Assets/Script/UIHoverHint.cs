using UnityEngine;
using UnityEngine.EventSystems; // ⭐ สำคัญมาก: ต้องมีบรรทัดนี้เพื่อใช้งานระบบ UI Events

// ต้องสืบทอด IPointerEnterHandler (ตอนชี้) และ IPointerExitHandler (ตอนเอาออก)
public class UIHoverHint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("ลากข้อความ HintText มาใส่ตรงนี้")]
    public GameObject hintText;

    void Start()
    {
        // เริ่มเกมมา ให้ซ่อนข้อความไว้ก่อน
        if (hintText != null)
        {
            hintText.SetActive(false);
        }
    }

    // ฟังก์ชันนี้จะทำงานอัตโนมัติเมื่อ "เอาเมาส์ชี้เข้า" ปุ่ม
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hintText != null)
        {
            hintText.SetActive(true);
        }
    }

    // ฟังก์ชันนี้จะทำงานอัตโนมัติเมื่อ "เอาเมาส์ออก" จากปุ่ม
    public void OnPointerExit(PointerEventData eventData)
    {
        if (hintText != null)
        {
            hintText.SetActive(false);
        }
    }
}