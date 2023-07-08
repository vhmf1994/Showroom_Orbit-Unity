using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PainelAreaExterna : MonoBehaviour
{
    [Header("Geral")]
    [SerializeField] private ControladorCameras controladorCameras;
    [Space(5)]
    [SerializeField] private Transform baseAreaExterna;

    [Header("Áreas")]
    [SerializeField] private Button botaoAreaExterna;
    [SerializeField] private Button botaoAreaInterna;
    [SerializeField] private Button botaoMaquete;

    [Header("Cômodos")]
    [SerializeField] private Transform grupoBotoesAreas;
    [Space(5)]
    [SerializeField] private Transform prefabBotaoAreas;

    [Header("Feedback")]
    [SerializeField] private Sprite botaoSelecionado;
    [SerializeField] private Sprite botaoDesselecionado;

    private Button botaoSelecionadoAtual;

    private float tempoParaSelecionar = 0f;

    private bool loaded = false;

    private void Awake()
    {
        loaded = false;

        SpawnarAreas(() =>
        {
            ConfigurarBotoes();
            CriarBotoesAreasExternas();

            loaded = true;
        });
    }
    private void FixedUpdate()
    {
        if (!loaded) return;

        grupoBotoesAreas.GetComponentsInChildren<Button>().All((b) =>
        {
            b.interactable = tempoParaSelecionar <= 0;
            return true;
        });

        if (tempoParaSelecionar <= 0) return;

        tempoParaSelecionar -= Time.deltaTime;
    }

    private void SpawnarAreas(Action onLoadComplete)
    {
        Empreendimento empreendimento = AddressablesController.Instance.GetEmpreendimento();

        string areaAtual = SceneController.Instance.CurrentScene == Scenes._06_Maquete ? "Maquete" : "AreaExterna";

        StartCoroutine(empreendimento.LoadAreaExternaPrefab((go) =>
        {
            StartCoroutine(empreendimento.LoadConfigCameraAreaExterna((go) =>
            {
                controladorCameras = go.GetComponent<ControladorCameras>();

                onLoadComplete?.Invoke();
            }));
        },baseAreaExterna));
    }

    private void ConfigurarBotoes()
    {
        botaoAreaExterna.onClick.RemoveAllListeners();
        botaoAreaInterna.onClick.RemoveAllListeners();
        botaoMaquete.onClick.RemoveAllListeners();

        botaoAreaExterna.onClick.AddListener(OnBotaoAreaExternaClick);
        botaoAreaInterna.onClick.AddListener(OnBotaoAreaInternaClick);
        botaoMaquete.onClick.AddListener(OnBotaoMaqueteClick);
    }

    private void OnBotaoAreaExternaClick() { SceneController.Instance.LoadScene(Scenes._07_Area_Externa); }
    private void OnBotaoAreaInternaClick() { SceneController.Instance.LoadScene(Scenes._08_Area_Interna); }
    private void OnBotaoMaqueteClick() { SceneController.Instance.LoadScene(Scenes._06_Maquete); }

    private void CriarBotoesAreasExternas()
    {
        if (controladorCameras == null)
            controladorCameras = FindObjectOfType<ControladorCameras>();

        for (int i = 0; i < controladorCameras.CamerasComodo.Count; i++)
        {
            Transform novoBotaoComodo = Instantiate(prefabBotaoAreas, grupoBotoesAreas);

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

        tempoParaSelecionar = 1f;
    }
}
