using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class ResourcesController : MonoSingleton<ResourcesController>
{
    #region Constantes Diretorios
    // Nome dos diretórios GERAL
    internal const string diretorioFichasTecnicas = "Geral/Fichas Tecnicas";

    internal const string diretorioIconesAreaExterna = "Geral/Icones Area Externa";
    internal const string diretorioIconesAreaInterna = "Geral/Icones Area Interna";

    internal const string diretorioSlide = "Geral/Background Slide";

    #region GALERIAS
    internal const string diretorioLogotipo = "/Logotipo/Logotipo";

    internal const string diretorioGaleriaImagens = "/Galeria de Imagens";
    internal const string diretorioGaleriaVideos = "/Galeria de Videos";
    internal const string diretorioGaleriaPlantas = "/Galeria de Plantas";

    // TEMP Até conseguir vincular algum mapa
    internal const string diretorioGaleriaLocalizacao = "/Localizacao";
    // TEMP

    internal const string diretorioPrefabs = "/Empreendimento/Prefabs/";
    internal const string diretorioConfiguracaoCameras = "/Empreendimento/Cameras/";

    #endregion

    #endregion

    protected override void InitializeBehaviour()
    {
    }

    protected override void FinishBehaviour()
    {
    }

    /*[Header("Configurações do App")]
    [SerializeField] private List<FichaTecnica> fichasTecnicas;
    [Space(10)]
    [SerializeField] private List<Sprite> iconesAreaExterna;
    [SerializeField] private List<Sprite> iconesAreaInterna;
    [Space(10)]
    [SerializeField] private List<Sprite> imagensSlide;

    public List<FichaTecnica> FichasTecnicas => fichasTecnicas;
    public List<Sprite> ImagensSlide => imagensSlide;
    public List<Sprite> IconesAreaExterna => iconesAreaExterna;

#if UNITY_EDITOR
    private void OnValidate()
    {
        CarregarFichasTecnicas();
        CarregarIcones();
    }
#endif

    private void CarregarFichasTecnicas()
    {
        fichasTecnicas = new List<FichaTecnica>();

        TextAsset[] fichasTecnicasAsset = Resources.LoadAll<TextAsset>(diretorioFichasTecnicas);

        for (int i = 0; i < fichasTecnicasAsset.Length; i++)
        {
            JSONNode nodeFicha = JSON.Parse(fichasTecnicasAsset[i].text);

            string nome = nodeFicha[nameof(nome)];
            string slogan = nodeFicha[nameof(slogan)];
            string resumo = nodeFicha[nameof(resumo)];
            string produto = nodeFicha[nameof(produto)];
            string endereco = nodeFicha[nameof(endereco)];
            string area = nodeFicha[nameof(area)];

            List<string> areasLazer = new List<string>();
            List<string> tipologia = new List<string>();
            List<string> proximidades = new List<string>();
            List<string> viasAcesso = new List<string>();

            List<int> iconesAreaLazer = new List<int>();

            JSONNode nodeAreasLazer = nodeFicha[nameof(areasLazer)];
            JSONNode nodeTipologia = nodeFicha[nameof(tipologia)];
            JSONNode nodeProximidades = nodeFicha[nameof(proximidades)];
            JSONNode nodeViasAcesso = nodeFicha[nameof(viasAcesso)];

            for (int j = 0; j < nodeAreasLazer.Count; j++)
                areasLazer.Add(nodeAreasLazer[j]);

            for (int j = 0; j < nodeTipologia.Count; j++)
                tipologia.Add(nodeTipologia[j]);

            for (int j = 0; j < nodeProximidades.Count; j++)
                proximidades.Add(nodeProximidades[j]);

            for (int j = 0; j < nodeViasAcesso.Count; j++)
                viasAcesso.Add(nodeViasAcesso[j]);

            foreach (string key in nodeAreasLazer.Keys)
                iconesAreaLazer.Add(int.Parse(key));

            FichaTecnica novaFichaTecnica = new FichaTecnica
            (
                nome,
                slogan,
                resumo,
                produto,
                endereco,
                area,
                areasLazer,
                tipologia,
                proximidades,
                viasAcesso,
                iconesAreaLazer
            );
            novaFichaTecnica.CarregarGalerias();
            novaFichaTecnica.CarregarModelos3D();

            fichasTecnicas.Add(novaFichaTecnica);
        }
    }
    private void CarregarIcones()
    {
        iconesAreaExterna = Resources.LoadAll<Sprite>(diretorioIconesAreaExterna).ToList();
        iconesAreaInterna = Resources.LoadAll<Sprite>(diretorioIconesAreaInterna).ToList();

        imagensSlide = Resources.LoadAll<Sprite>(diretorioSlide).ToList();
    }

    [SerializeField] private AssetLabelReference fichasTecnicasLabelReference;

    [SerializeField] private AssetLabelReference iconesAreaExternaLabelReference;
    [SerializeField] private AssetLabelReference iconesAreaInternaLabelReference;

    [SerializeField] private AssetLabelReference backgroundSlidesLabelReference;

    private void CarregarFichasTecnicasAddressables()
    {
        Addressables.LoadAssetsAsync<TextAsset>(fichasTecnicasLabelReference, ta => { Debug.Log($"Consegui carregar {ta.name}"); }).Completed += OnFichasTecnicasLoaded;
    }
    private void OnFichasTecnicasLoaded(AsyncOperationHandle<IList<TextAsset>> asyncOperationHandle)
    {
        fichasTecnicas = new List<FichaTecnica>();

        List<TextAsset> fichasTecnicasAsset = new List<TextAsset>(asyncOperationHandle.Result.Cast<TextAsset>().ToList());

        for (int i = 0; i < fichasTecnicasAsset.Count; i++)
        {
            JSONNode nodeFicha = JSON.Parse(fichasTecnicasAsset[i].text);

            string nome = nodeFicha[nameof(nome)];
            string slogan = nodeFicha[nameof(slogan)];
            string resumo = nodeFicha[nameof(resumo)];
            string produto = nodeFicha[nameof(produto)];
            string endereco = nodeFicha[nameof(endereco)];
            string area = nodeFicha[nameof(area)];

            List<string> areasLazer = new List<string>();
            List<string> tipologia = new List<string>();
            List<string> proximidades = new List<string>();
            List<string> viasAcesso = new List<string>();

            List<int> iconesAreaLazer = new List<int>();

            JSONNode nodeAreasLazer = nodeFicha[nameof(areasLazer)];
            JSONNode nodeTipologia = nodeFicha[nameof(tipologia)];
            JSONNode nodeProximidades = nodeFicha[nameof(proximidades)];
            JSONNode nodeViasAcesso = nodeFicha[nameof(viasAcesso)];

            for (int j = 0; j < nodeAreasLazer.Count; j++)
                areasLazer.Add(nodeAreasLazer[j]);

            for (int j = 0; j < nodeTipologia.Count; j++)
                tipologia.Add(nodeTipologia[j]);

            for (int j = 0; j < nodeProximidades.Count; j++)
                proximidades.Add(nodeProximidades[j]);

            for (int j = 0; j < nodeViasAcesso.Count; j++)
                viasAcesso.Add(nodeViasAcesso[j]);

            foreach (string key in nodeAreasLazer.Keys)
                iconesAreaLazer.Add(int.Parse(key));

            FichaTecnica novaFichaTecnica = new FichaTecnica
            (
                nome,
                slogan,
                resumo,
                produto,
                endereco,
                area,
                areasLazer,
                tipologia,
                proximidades,
                viasAcesso,
                iconesAreaLazer
            );
            novaFichaTecnica.CarregarGalerias();
            novaFichaTecnica.CarregarModelos3D();

            fichasTecnicas.Add(novaFichaTecnica);
        }
    }

    [Obsolete]
    private void CarregarIconesAddressables()
    {
        iconesAreaExterna = GetIconesExternosSprites();
        iconesAreaInterna = GetIconesInternosSprites();

        imagensSlide = GetBackgroundSlideSprites();
    }

    public List<Sprite> GetIconesExternosSprites()
    {
        List<Sprite> iconesExternosSprites = new List<Sprite>();

        Addressables.LoadAssetsAsync<Sprite>(backgroundSlidesLabelReference, s => { }).Completed += (ao) => iconesAreaExterna = ao.Result.Cast<Sprite>().ToList();

        return iconesExternosSprites;
    }
    public List<Sprite> GetIconesInternosSprites()
    {
        List<Sprite> iconesInternosSprites = new List<Sprite>();

        Addressables.LoadAssetsAsync<Sprite>(backgroundSlidesLabelReference, s => { }).Completed += (ao) => iconesAreaInterna = ao.Result.Cast<Sprite>().ToList();

        return iconesInternosSprites;
    }

    public List<Sprite> GetBackgroundSlideSprites()
    {
        List<Sprite> backgroundSprites = new List<Sprite>();

        Addressables.LoadAssetsAsync<Sprite>(backgroundSlidesLabelReference, s => { }).Completed += (ao) => backgroundSprites = ao.Result.Cast<Sprite>().ToList();

        return backgroundSprites;
    }*/
}

[System.Serializable]
public class FichaTecnica
{
    public string Nome;
    public string Slogan;
    [TextArea(2, 8)] public string Resumo;
    public string Produto;
    public string Endereco;
    public string AreaTerreno;

    public Sprite Logotipo;
    public Sprite Localizacao;

    [Space(10)]
    public List<string> AreasLazer;
    public List<int> IconesAreasLazer;
    [Space(10)]
    public List<string> Tipologia;
    public List<string> Proximidade;
    public List<string> ViasAcesso;
    [Space(10)]
    public List<Sprite> GaleriaImagens;
    [Space(10)]
    public List<Sprite> ThumbVideos;
    public List<VideoClip> GaleriaVideos;
    [Space(10)]
    public List<string> NomePlantas;
    public List<Sprite> GaleriaPlantas;
    [Space(10)]
    [Header("Modelos 3D")]
    public GameObject MaquetePrefab;
    public GameObject AreaExternaPrefab;
    public GameObject AreaInternaDecoradaPrefab;
    public GameObject AreaInternaNaoDecoradaPrefab;
    [Header("Câmeras")]
    public GameObject CamerasMaquetePrefab;
    public GameObject CamerasAreaExternaPrefab;
    public GameObject CamerasAreaInternaPrefab;

    public int idBlocoAtual;
    public int idAndarAtual;
    public int idAptoAtual;

    public FichaTecnica(string nome, string slogan, string resumo, string produto, string endereco, string areaTerreno, List<string> areasLazer, List<string> tipologia, List<string> proximidade, List<string> viasAcesso, List<int> iconesAreasLazer)
    {
        Nome = nome;
        Slogan = slogan;
        Resumo = resumo;
        Produto = produto;
        Endereco = endereco;
        AreaTerreno = areaTerreno;
        AreasLazer = areasLazer;
        Tipologia = tipologia;
        Proximidade = proximidade;
        ViasAcesso = viasAcesso;
        IconesAreasLazer = iconesAreasLazer;
    }

    public void CarregarGalerias()
    {
        Logotipo = Resources.Load<Sprite>($"{Nome}{ResourcesController.diretorioLogotipo}/Logotipo");
        Localizacao = Resources.Load<Sprite>($"{Nome}{ResourcesController.diretorioGaleriaLocalizacao}/Imagem_Localizacao");

        GaleriaImagens = Resources.LoadAll<Sprite>($"{Nome}{ResourcesController.diretorioGaleriaImagens}").ToList();

        ThumbVideos = Resources.LoadAll<Sprite>($"{Nome}{ResourcesController.diretorioGaleriaVideos}").ToList();
        GaleriaVideos = Resources.LoadAll<VideoClip>($"{Nome}{ResourcesController.diretorioGaleriaVideos}").ToList();

        GaleriaPlantas = Resources.LoadAll<Sprite>($"{Nome}{ResourcesController.diretorioGaleriaPlantas}").ToList();

        NomePlantas = new List<string>();

        TextAsset Plantas = Resources.Load<TextAsset>($"{Nome}{ResourcesController.diretorioGaleriaPlantas}/InfoPlantaBaixa");

        JSONNode nodePlanta = JSON.Parse(Plantas.text);

        for (int i = 0; i < nodePlanta["plantas"].Count; i++)
            NomePlantas.Add(nodePlanta["plantas"][i]);
    }

    public void CarregarModelos3D()
    {
        MaquetePrefab = Resources.Load<GameObject>($"{Nome}{ResourcesController.diretorioPrefabs}Maquete");
        AreaExternaPrefab = Resources.Load<GameObject>($"{Nome}{ResourcesController.diretorioPrefabs}AreaExterna");
        AreaInternaDecoradaPrefab = Resources.Load<GameObject>($"{Nome}{ResourcesController.diretorioPrefabs}AreaInternaDecorada");
        AreaInternaNaoDecoradaPrefab = Resources.Load<GameObject>($"{Nome}{ResourcesController.diretorioPrefabs}AreaInternaNaoDecorada");

        CamerasMaquetePrefab = Resources.Load<GameObject>($"{Nome}{ResourcesController.diretorioConfiguracaoCameras}ConfiguracaoCameraMaquete");
        CamerasAreaExternaPrefab = Resources.Load<GameObject>($"{Nome}{ResourcesController.diretorioConfiguracaoCameras}ConfiguracaoCameraAreaExterna");
        CamerasAreaInternaPrefab = Resources.Load<GameObject>($"{Nome}{ResourcesController.diretorioConfiguracaoCameras}ConfiguracaoCameraAreaInterna");
    }
}

[System.Serializable]
public class PlantasBaixas
{
    public List<string> NomesApartamentos;

    public PlantasBaixas(List<string> nomesApartamentos)
    {
        NomesApartamentos = nomesApartamentos;
    }
}
