using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Video;

public class AddressablesController : MonoSingleton<AddressablesController>
{
    [Header("Geral")]
    [SerializeField] private List<AssetReferenceSprite> backgroundSlides;
    [Space]
    [SerializeField] private List<AssetReferenceSprite> iconesAreaExterna;
    [SerializeField] private List<AssetReferenceSprite> iconesAreaInterna;
    [Space]
    [SerializeField] private List<Empreendimento> empreendimentos;

    protected override void InitializeBehaviour() { }
    protected override void FinishBehaviour() { }

    public Empreendimento GetEmpreendimento()
    {
        return empreendimentos[GameController.Instance.IdEmpreendimentoSelecionado];
    }

    public IEnumerator LoadBackgroundSlides(Action<List<Sprite>> onLoadComplete)
    {
        List<Sprite> loadedSprites = new List<Sprite>();

        AsyncOperationHandle asyncOperationHandle = Addressables.LoadAssetsAsync<Sprite>(backgroundSlides, (ao) =>
        {
            loadedSprites.Add(ao);
        }, Addressables.MergeMode.Union);

        yield return asyncOperationHandle;

        onLoadComplete?.Invoke(loadedSprites);
    }
    public IEnumerator LoadIconesAreaExterna(Action<List<Sprite>> onLoadComplete)
    {
        List<Sprite> loadedSprites = new List<Sprite>();

        AsyncOperationHandle asyncOperationHandle = Addressables.LoadAssetsAsync<Sprite>(iconesAreaExterna, (ao) =>
        {
            loadedSprites.Add(ao);
        }, Addressables.MergeMode.Union);

        yield return asyncOperationHandle;

        onLoadComplete?.Invoke(loadedSprites);
    }
    public IEnumerator LoadIconesAreaInterna(Action<List<Sprite>> onLoadComplete)
    {
        List<Sprite> loadedSprites = new List<Sprite>();

        AsyncOperationHandle asyncOperationHandle = Addressables.LoadAssetsAsync<Sprite>(iconesAreaInterna, (ao) =>
        {
            loadedSprites.Add(ao);
        }, Addressables.MergeMode.Union);

        yield return asyncOperationHandle;

        onLoadComplete?.Invoke(loadedSprites);
    }
}

[Serializable]
public class Empreendimento
{
    [SerializeField] private string m_nomeEmpreendimento;
    public string Nome => m_nomeEmpreendimento;
    [Space]
    [SerializeField] private AssetReferenceTextAsset m_fichaTecnicaReferenceTextAsset;
    [Space]
    [SerializeField] private AssetReferenceSprite m_logotipo;

    public IEnumerator LoadFichaTecnica(Action<TextAsset> onLoadComplete)
    {
        TextAsset loadedFichaTecnica = null;

        AsyncOperationHandle<TextAsset> asyncOperationHandle = Addressables.LoadAssetAsync<TextAsset>(m_fichaTecnicaReferenceTextAsset);

        yield return asyncOperationHandle;

        loadedFichaTecnica = asyncOperationHandle.Result;

        onLoadComplete?.Invoke(loadedFichaTecnica);
    }
    public IEnumerator LoadLogotipo(Action<Sprite> onLoadComplete)
    {
        Sprite loadedLogotipo = null;

        AsyncOperationHandle<Sprite> asyncOperationHandle = Addressables.LoadAssetAsync<Sprite>(m_logotipo);

        yield return asyncOperationHandle;

        loadedLogotipo = asyncOperationHandle.Result;
    }

    [Header("Cameras")]
    [SerializeField] private AssetReferenceGameObject m_configCameraMaquete;
    [Space]
    [SerializeField] private AssetReferenceGameObject m_configCameraAreaExterna;
    [SerializeField] private AssetReferenceGameObject m_configCameraAreaInterna;

    public IEnumerator LoadConfigCameraMaquete(Action<GameObject> onLoadComplete, Transform parent = null)
    {
        GameObject loadedCameraMaquete = null;

        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(m_configCameraMaquete, parent);

        yield return asyncOperationHandle;

        loadedCameraMaquete = asyncOperationHandle.Result;

        onLoadComplete?.Invoke(loadedCameraMaquete);
    }
    public IEnumerator LoadConfigCameraAreaExterna(Action<GameObject> onLoadComplete, Transform parent = null)
    {
        GameObject loadedCameraAreaExterna = null;

        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(m_configCameraAreaExterna, parent);

        yield return asyncOperationHandle;

        loadedCameraAreaExterna = asyncOperationHandle.Result;

        onLoadComplete?.Invoke(loadedCameraAreaExterna);
    }
    public IEnumerator LoadConfigCameraAreaInterna(Action<GameObject> onLoadComplete, Transform parent = null)
    {
        GameObject loadedCameraAreaInterna = null;

        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(m_configCameraAreaInterna, parent);

        yield return asyncOperationHandle;

        loadedCameraAreaInterna = asyncOperationHandle.Result;

        onLoadComplete?.Invoke(loadedCameraAreaInterna);
    }

    [Header("Empreendimentos")]
    [SerializeField] private AssetReferenceGameObject m_maquetePrefab;
    [Space]
    [SerializeField] private AssetReferenceGameObject m_areaExternaPrefab;
    [Space]
    [SerializeField] private AssetReferenceGameObject m_areaInternaDecoradaPrefab;
    [SerializeField] private AssetReferenceGameObject m_areaInternaNaoDecoradaPrefab;
    [Space]
    [SerializeField] private AssetReferenceGameObject m_solMaquete;

    public IEnumerator LoadMaquetePrefab(Action<GameObject> onLoadComplete, Transform parent = null)
    {
        GameObject loadedMaquete = null;

        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(m_maquetePrefab, parent);

        yield return asyncOperationHandle;

        loadedMaquete = asyncOperationHandle.Result;

        onLoadComplete?.Invoke(loadedMaquete);
    }
    public IEnumerator LoadAreaExternaPrefab(Action<GameObject> onLoadComplete, Transform parent = null)
    {
        GameObject loadedAreaExterna = null;

        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(m_areaExternaPrefab, parent);

        yield return asyncOperationHandle;

        loadedAreaExterna = asyncOperationHandle.Result;

        onLoadComplete?.Invoke(loadedAreaExterna);
    }
    public IEnumerator LoadAreaInternaDecoradoPrefab(Action<GameObject> onLoadComplete, Transform parent = null)
    {
        GameObject loadedDecorado = null;

        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(m_areaInternaDecoradaPrefab, parent);

        yield return asyncOperationHandle;

        loadedDecorado = asyncOperationHandle.Result;

        onLoadComplete?.Invoke(loadedDecorado);
    }
    public IEnumerator LoadAreaInternaNaoDecoradoPrefab(Action<GameObject> onLoadComplete, Transform parent = null)
    {
        GameObject loadedNaoDecorado = null;

        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(m_areaInternaNaoDecoradaPrefab, parent);

        yield return asyncOperationHandle;

        loadedNaoDecorado = asyncOperationHandle.Result;

        onLoadComplete?.Invoke(loadedNaoDecorado);
    }

    public IEnumerator LoadSolMaquete(Action<GameObject> onLoadComplete)
    {
        GameObject loadedSol = null;

        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(m_solMaquete);

        yield return asyncOperationHandle;

        loadedSol = asyncOperationHandle.Result;

        onLoadComplete?.Invoke(loadedSol);
    }

    [Header("Galerias")]
    [SerializeField] private List<AssetReferenceSprite> m_galeriaImagens;
    [Space]
    [SerializeField] private List<AssetReferenceSprite> m_galeriaPlantas;
    [SerializeField] private AssetReferenceTextAsset m_plantasTextAsset;
    [Space]
    [SerializeField] private List<AssetReferenceSprite> m_galeriaVideosThumb;
    [SerializeField] private List<AssetReferenceVideo> m_galeriaVideoes;

    public IEnumerator LoadGaleriaImagens(Action<List<Sprite>> onLoadComplete)
    {
        List<Sprite> loadedSprites = new List<Sprite>();

        AsyncOperationHandle asyncOperationHandle = Addressables.LoadAssetsAsync<Sprite>(m_galeriaImagens, (ao) =>
        {
            loadedSprites.Add(ao);
        }, Addressables.MergeMode.Union);

        yield return asyncOperationHandle;

        onLoadComplete?.Invoke(loadedSprites);
    }
    public IEnumerator LoadGaleriaPlantas(Action<List<Sprite>> onLoadComplete)
    {
        List<Sprite> loadedSprites = new List<Sprite>();

        AsyncOperationHandle asyncOperationHandle = Addressables.LoadAssetsAsync<Sprite>(m_galeriaPlantas, (ao) =>
        {
            loadedSprites.Add(ao);
        }, Addressables.MergeMode.Union);

        yield return asyncOperationHandle;

        onLoadComplete?.Invoke(loadedSprites);
    }
    public IEnumerator LoadGaleriaVideosThumb(Action<List<Sprite>> onLoadComplete)
    {
        List<Sprite> loadedSprites = new List<Sprite>();

        AsyncOperationHandle asyncOperationHandle = Addressables.LoadAssetsAsync<Sprite>(m_galeriaVideosThumb, (ao) =>
        {
            loadedSprites.Add(ao);
        }, Addressables.MergeMode.Union);

        yield return asyncOperationHandle;

        onLoadComplete?.Invoke(loadedSprites);
    }
    public IEnumerator LoadGaleriaVideos(Action<List<VideoClip>> onLoadComplete)
    {
        List<VideoClip> loadedVideoes = new List<VideoClip>();

        AsyncOperationHandle asyncOperationHandle = Addressables.LoadAssetsAsync<VideoClip>(m_galeriaVideoes, (ao) =>
        {
            loadedVideoes.Add(ao);
        }, Addressables.MergeMode.Union);

        yield return asyncOperationHandle;

        onLoadComplete?.Invoke(loadedVideoes);
    }
    public IEnumerator LoadNomesPlantas(Action<TextAsset> onLoadComplete)
    {
        TextAsset loadedFichaTecnica = null;

        AsyncOperationHandle<TextAsset> asyncOperationHandle = Addressables.LoadAssetAsync<TextAsset>(m_plantasTextAsset);

        yield return asyncOperationHandle;

        loadedFichaTecnica = asyncOperationHandle.Result;

        onLoadComplete?.Invoke(loadedFichaTecnica);
    }
}

[Serializable]
public class AssetReferenceTextAsset : AssetReferenceT<TextAsset>
{
    public AssetReferenceTextAsset(string guid) : base(guid) { }
}
[Serializable]
public class AssetReferenceVideo : AssetReferenceT<VideoClip>
{
    public AssetReferenceVideo(string guid) : base(guid) { }
}