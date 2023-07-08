using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoSingleton<FadeController>
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject loadingEffect;

#if UNITY_EDITOR
    private void OnValidate()
    {
        GetComponentInChildren<TMP_Text>().SetText($"Showroom Orbit Viewer \nBuild Version: {DateTime.Now.ToString("yyyyMMdd.HHmm")}");
    }
#endif

    protected override void InitializeBehaviour()
    {
        GetComponentInChildren<TMP_Text>().SetText($"Showroom Orbit Viewer \nBuild Version: {Application.version}");
    }

    protected override void FinishBehaviour()
    {

    }

    public IEnumerator FadeIn(float fadeTime)
    {
        // 1 -> 0
        fadeImage.CrossFadeAlpha(0, fadeTime, false);

        yield return new WaitForSeconds(fadeTime);

        fadeImage.raycastTarget = false;
        loadingEffect.SetActive(false);
    }
    public IEnumerator FadeOut(float fadeTime)
    {
        // 0 -> 1
        loadingEffect.SetActive(true);
        fadeImage.raycastTarget = true;
        fadeImage.CrossFadeAlpha(1, fadeTime, false);

        yield return new WaitForSeconds(fadeTime);
    }
}