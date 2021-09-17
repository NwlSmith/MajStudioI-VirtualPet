using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public Image overlay;
    private bool pressedSpace = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInEnum());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressedSpace = true;
            StartCoroutine(FadeOutEnum());
        }
    }

    private IEnumerator FadeInEnum()
    {
        float duration = .5f;
        float elapsedTime = 0f;
        Color clearWhite = new Color(1, 1, 1, 0);
        while (elapsedTime < duration && !pressedSpace)
        {
            elapsedTime += Time.deltaTime;
            overlay.color = Color.Lerp(Color.white, clearWhite, elapsedTime / duration);
            yield return null;
        }
        overlay.color = Color.clear;
    }

    private IEnumerator FadeOutEnum()
    {
        float duration = .5f;
        float elapsedTime = 0f;
        Color clearWhite = new Color(1, 1, 1, 0);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            overlay.color = Color.Lerp(clearWhite, Color.white, elapsedTime / duration);
            yield return null;
        }
        overlay.color = Color.white;

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
