using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoSingleton<SceneController>
{
    private Func<bool> beforeCurrentSceneUnload;
    private Func<bool> afterNextSceneLoad;

    [SerializeField] private Scenes currentScene;
    [SerializeField] private List<Scenes> activeScenes;

    public Scenes CurrentScene => currentScene;

    protected override void InitializeBehaviour()
    {
        activeScenes = new List<Scenes>();
        activeScenes.Add(Scenes._00_Splash);

        currentScene = activeScenes.First();
    }
    protected override void FinishBehaviour() { }

    public void LoadScene(Scenes nextScene)
    {
        LoadScene(nextScene, LoadSceneMode.Single);
    }
    public void LoadScene(Scenes nextScene, LoadSceneMode loadSceneMode)
    {
        LoadScene(nextScene, loadSceneMode, null, null);
    }
    public void LoadScene(Scenes nextScene, LoadSceneMode loadSceneMode, Func<bool> beforeCurrentSceneUnload, Func<bool> afterNextSceneLoad)
    {
        if (activeScenes.Contains(nextScene)) return;

        currentScene = nextScene;

        if (loadSceneMode == LoadSceneMode.Single)
            activeScenes.Clear();

        activeScenes.Add(currentScene);

        this.beforeCurrentSceneUnload = beforeCurrentSceneUnload;
        this.afterNextSceneLoad = afterNextSceneLoad;

        StartCoroutine(LoadSceneAsync(nextScene, loadSceneMode));
    }

    public void UnloadScene()
    {
        if (activeScenes.Count <= 1) return;

        StartCoroutine(UnloadSceneAsync(currentScene));

        activeScenes.Remove(activeScenes.Last());

        currentScene = activeScenes.Last();

        Canvas.ForceUpdateCanvases();
    }

    private IEnumerator LoadSceneAsync(Scenes nextScene, LoadSceneMode loadSceneMode)
    {
        if (loadSceneMode == LoadSceneMode.Single)
        {
            yield return FadeController.Instance.FadeOut(.25f);
        }

        if (beforeCurrentSceneUnload != null)
        {
            yield return new WaitUntil(beforeCurrentSceneUnload.Invoke);
            beforeCurrentSceneUnload = null;
        }

        yield return SceneManager.LoadSceneAsync(nextScene.ToString(), loadSceneMode);

        if (afterNextSceneLoad != null)
        {
            yield return new WaitUntil(afterNextSceneLoad.Invoke);
            afterNextSceneLoad = null;
        }

        yield return new WaitForSeconds(0.1f);

        if (loadSceneMode == LoadSceneMode.Single)
        {
            yield return FadeController.Instance.FadeIn(.25f);
        }

        Canvas.ForceUpdateCanvases();
    }
    private static IEnumerator UnloadSceneAsync(Scenes scene)
    {
        yield return SceneManager.UnloadSceneAsync(scene.ToString());
    }
}

public enum Scenes
{
    _00_Splash,
    _01_Inatividade,
    _02_Home,
    _03_Intro,
    _04_Localizacao,
    _05_Ficha_Tecnica,
    _06_Maquete,
    _07_Area_Externa,
    _08_Area_Interna,
    _09_Plantas,
    _10_Galeira_de_Imagens,
    _11_Galeira_de_Videos,
}