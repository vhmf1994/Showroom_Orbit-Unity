using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PainelAreaInterna : MonoBehaviour
{
    [Header("Geral")]
    private ControladorCameras controladorCameras;

    private Transform areaInternaDecorada;
    private Transform areaInternaNaoDecorada;

    [SerializeField] private Transform baseAreaInterna;
    [SerializeField] private Transform baseAreaExterna;
    [Space(5)]
    [SerializeField] private Transform controladorCamerasPrefab;
    [SerializeField] private Transform areaInternaDecoradaPrefab;
    [SerializeField] private Transform areaInternaNaoDecoradaPrefab;
    [SerializeField] private Transform areaExternaPrefab;

    [Header("Apartamentos")]
    [SerializeField] private List<Bloco> blocos;
    [SerializeField] private Apartamento apartamentoAtual;
    [Space(5)]
    [SerializeField] private int idBlocoAtual;
    [SerializeField] private int idAndarAtual;
    [SerializeField] private int idAptoAtual;

    [Header("Áreas")]
    [SerializeField] private Button botaoAreaExterna;
    [SerializeField] private Button botaoMaquete;
    [Space(10)]
    [SerializeField] private Button botaoComDecoracao;
    [SerializeField] private Button botaoSemDecoracao;
    [Space(10)]
    [SerializeField] private Button botaoVistaJanela;

    [Header("Cômodos")]
    [SerializeField] private Transform grupoBotoesComodos;
    [Space(5)]
    [SerializeField] private Transform prefabBotaoComodo;

    [Header("Feedback")]
    [SerializeField] private Sprite botaoSelecionado;
    [SerializeField] private Sprite botaoDesselecionado;

    private Button botaoSelecionadoAtual;

    private float tempoParaSelecionar = 0f;

    private bool loaded = false;

    private void Validate()
    {
        blocos = FindObjectsOfType<Bloco>().ToList();
        blocos.Sort((b1, b2) => b1.IdBloco < b2.IdBloco ? -1 : 1);

#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        // TODO mudar para valor selecionado na tela de maquete
        idBlocoAtual = GameController.Instance.IdBlocoAtual;
        idAndarAtual = GameController.Instance.IdAndarAtual;
        idAptoAtual = GameController.Instance.IdAptoAtual;

        // Verificando objetos de cena
        if (controladorCameras == null)
            controladorCameras = FindObjectOfType<ControladorCameras>();

        if (areaInternaDecorada == null && baseAreaInterna.childCount > 0)
            areaInternaDecorada = baseAreaInterna.GetChild(0);

        if (areaInternaNaoDecorada == null && baseAreaInterna.childCount > 1)
        {
            areaInternaNaoDecorada = baseAreaInterna.GetChild(1);
            areaInternaNaoDecorada?.gameObject.SetActive(false);
        }
        //

        ConfigurarApartamento();
    }

    private void Awake()
    {
        loaded = false;

        SpawnarAreas(() =>
        {
            Validate();

            ConfigurarBotoes();
            CriarBotoesComodos();
            OnBotaoComDecoracaoClick();
            ConfigurarApartamento();

            loaded = true;
        });
    }
    private void FixedUpdate()
    {
        if (!loaded) return;

        grupoBotoesComodos.GetComponentsInChildren<Button>().All((b) =>
        {
            b.interactable = tempoParaSelecionar <= 0;

            return true;
        });

        botaoVistaJanela.image.color = controladorCameras.CameraAtual.temVistaJanela ? botaoVistaJanela.colors.normalColor : botaoVistaJanela.colors.disabledColor;

        if (tempoParaSelecionar <= 0) return;

        tempoParaSelecionar -= Time.deltaTime;
    }

    private void SpawnarAreas(Action onLoadComplete)
    {
        Empreendimento empreendimento = AddressablesController.Instance.GetEmpreendimento();

        StartCoroutine(empreendimento.LoadMaquetePrefab((go) =>
        {
            StartCoroutine(empreendimento.LoadAreaInternaDecoradoPrefab((go) =>
            {
                areaInternaDecorada = go.transform;

                StartCoroutine(empreendimento.LoadAreaInternaNaoDecoradoPrefab((go) =>
                {
                    areaInternaNaoDecorada = go.transform;

                    StartCoroutine(empreendimento.LoadConfigCameraAreaInterna((go) =>
                    {
                        controladorCameras = go.GetComponent<ControladorCameras>();

                        onLoadComplete?.Invoke();
                    }));
                }, baseAreaInterna));
            }, baseAreaInterna));
        }, baseAreaExterna));
    }

    private void ConfigurarBotoes()
    {
        botaoAreaExterna.onClick.RemoveAllListeners();
        botaoMaquete.onClick.RemoveAllListeners();
        botaoComDecoracao.onClick.RemoveAllListeners();
        botaoSemDecoracao.onClick.RemoveAllListeners();
        botaoVistaJanela.onClick.RemoveAllListeners();

        botaoAreaExterna.onClick.AddListener(OnBotaoAreaExternaClick);
        botaoMaquete.onClick.AddListener(OnBotaoMaqueteClick);
        botaoComDecoracao.onClick.AddListener(OnBotaoComDecoracaoClick);
        botaoSemDecoracao.onClick.AddListener(OnBotaoSemDecoracaoClick);
        botaoVistaJanela.onClick.AddListener(OnBotaoVistaJanelaClick);
    }
    private void ConfigurarApartamento()
    {
        apartamentoAtual = blocos[idBlocoAtual].Andares[idAndarAtual].Apartamentos[idAptoAtual];

        // Atualização de posição, rotação e escala
        controladorCameras.transform.position = apartamentoAtual.transform.position;
        areaInternaDecorada.position = apartamentoAtual.transform.position;
        areaInternaNaoDecorada.position = apartamentoAtual.transform.position;

        controladorCameras.transform.eulerAngles = apartamentoAtual.transform.eulerAngles;
        areaInternaDecorada.eulerAngles = apartamentoAtual.transform.eulerAngles;
        areaInternaNaoDecorada.eulerAngles = apartamentoAtual.transform.eulerAngles;

        controladorCameras.transform.localScale = apartamentoAtual.transform.localScale;
        areaInternaDecorada.localScale = apartamentoAtual.transform.localScale;
        areaInternaNaoDecorada.localScale = apartamentoAtual.transform.localScale;

        controladorCameras.ForceCameraPosition();
        UpdateReflectionProbes();
    }

    private void OnBotaoAreaExternaClick() { SceneController.Instance.LoadScene(Scenes._07_Area_Externa); }
    private void OnBotaoMaqueteClick() { SceneController.Instance.LoadScene(Scenes._06_Maquete); }
    private void OnBotaoComDecoracaoClick()
    {
        botaoComDecoracao.gameObject.SetActive(false);
        botaoSemDecoracao.gameObject.SetActive(true);

        areaInternaDecorada.gameObject.SetActive(true);
        areaInternaNaoDecorada.gameObject.SetActive(false);

        UpdateReflectionProbes();
    }
    private void OnBotaoSemDecoracaoClick()
    {
        botaoComDecoracao.gameObject.SetActive(true);
        botaoSemDecoracao.gameObject.SetActive(false);

        areaInternaDecorada.gameObject.SetActive(false);
        areaInternaNaoDecorada.gameObject.SetActive(true);

        UpdateReflectionProbes();
    }
    private void OnBotaoVistaJanelaClick()
    {
        if (tempoParaSelecionar > 0) return;

        if (!controladorCameras.CameraAtual.temVistaJanela) return;

        botaoVistaJanela.image.sprite = botaoVistaJanela.image.sprite == botaoSelecionado ? botaoDesselecionado : botaoSelecionado;
        controladorCameras.AlternarVistas(botaoVistaJanela.image.sprite == botaoSelecionado);

        tempoParaSelecionar = 1f;
    }

    private void CriarBotoesComodos()
    {
        for (int i = 0; i < controladorCameras.CamerasComodo.Count; i++)
        {
            Transform novoBotaoComodo = Instantiate(prefabBotaoComodo, grupoBotoesComodos);

            CameraComodo cameraComodo = controladorCameras.CamerasComodo[i];

            novoBotaoComodo.GetComponent<Button>().image.sprite = botaoDesselecionado;

            novoBotaoComodo.GetChild(0).GetComponent<TMP_Text>().SetText(cameraComodo.NomeComodo);
            novoBotaoComodo.GetChild(1).GetComponent<Image>().sprite = (cameraComodo.SpriteComodo);

            novoBotaoComodo.GetComponent<Button>().onClick.RemoveAllListeners();
            novoBotaoComodo.GetComponent<Button>().onClick.AddListener(() => SelecionarCamera(novoBotaoComodo.GetComponent<Button>(), cameraComodo));

            if (botaoSelecionadoAtual == null)
            {
                botaoSelecionadoAtual = novoBotaoComodo.GetComponent<Button>();
                SelecionarCamera(novoBotaoComodo.GetComponent<Button>(), cameraComodo);
            }
        }
    }

    private void SelecionarCamera(Button botaoSelecionadoAtual, CameraComodo cameraComodo)
    {
        if (tempoParaSelecionar > 0) return;

        if (this.botaoSelecionadoAtual != null)
            this.botaoSelecionadoAtual.image.sprite = botaoDesselecionado;

        this.botaoSelecionadoAtual = botaoSelecionadoAtual;

        this.botaoSelecionadoAtual.image.sprite = botaoSelecionado;

        controladorCameras.SelecionarCamera(cameraComodo);

        botaoVistaJanela.image.sprite = botaoDesselecionado;

        tempoParaSelecionar = 1f;
    }

    private void UpdateReflectionProbes()
    {
        baseAreaInterna.GetComponentsInChildren<ReflectionProbe>().All((rp) =>
        {
            rp.RenderProbe();
            return true;
        });
    }
}