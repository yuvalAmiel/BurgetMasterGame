using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LevelLoader : MonoBehaviour
{

    public GameObject m_LoadingScreen;
    public Slider m_LoadingSlider;
    public TMP_Text m_Text, m_ProgressText;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        m_LoadingSlider.gameObject.SetActive(true);
        m_Text.gameObject.SetActive(false);


        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            m_LoadingSlider.value = progress;
            m_ProgressText.text = Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }
    }
}
