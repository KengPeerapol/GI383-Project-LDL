using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text loadingText;

    [Header("Loading Settings")]
    [SerializeField] private float loadingDuration = 5f; // เวลาที่ต้องการให้แถบวิ่งถึง 100%

    private void Start()
    {
        StartCoroutine(LoadMainSceneAsync());
    }

    private IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main Scene");
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (timer < loadingDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / loadingDuration;
            progress = Mathf.Clamp01(progress);

            progressBar.value = progress;
            loadingText.text = $"Loading... {(int)(progress * 100)}%";

            yield return null;
        }

        // ให้แถบเต็ม 100%
        progressBar.value = 1f;
        loadingText.text = "Loading... 100%";

        // รอจน Scene โหลดเสร็จจริง
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // เปลี่ยน Scene
        operation.allowSceneActivation = true;
    }
}