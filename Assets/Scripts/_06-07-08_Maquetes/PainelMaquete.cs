using Hessburg;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PainelMaquete : MonoBehaviour
{
    [Header("Geral")]
    [SerializeField] private ControladorCameras controladorCameras;
    [Space(5)]
    [SerializeField] private Transform baseMaquete;
    [SerializeField] private Transform cuboSelecao;
    [SerializeField] private Transform setaSelecao;
    [Space(5)]
    [SerializeField] private Transform cameraApartamento;
    [Space(5)]
    [SerializeField] private Transform controladorCamerasPrefab;
    [SerializeField] private Transform maquetePrefab;

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

    [Header("Apartamentos")]
    [SerializeField] private List<Bloco> blocos;
    [SerializeField] private Apartamento apartamentoAtual;
    [Space(5)]
    //[SerializeField] private Button botaoPainelAjustes;
    [SerializeField] private Transform painelApartamentos;
    [Space(5)]
    [SerializeField] private TMP_Text displayBlocos;
    [SerializeField] private TMP_Text displayAndares;
    [SerializeField] private TMP_Text displayAptos;
    [Space(5)]
    [SerializeField] private Button blocoMais;
    [SerializeField] private Button blocoMenos;
    [Space(5)]
    [SerializeField] private Button andarMais;
    [SerializeField] private Button andarMenos;
    [Space(5)]
    [SerializeField] private Button aptoMais;
    [SerializeField] private Button aptoMenos;
    [Space(5)]
    [SerializeField] private int idBlocoAtual;
    [SerializeField] private int idAndarAtual;
    [SerializeField] private int idAptoAtual;

    [Header("Estação")]
    [SerializeField] private SunLight controladorSol;
    [SerializeField] private Button botaoEstacao;
    [Space(5)]
    [SerializeField] private Transform painelEstacao;
    [Space(5)]
    [SerializeField] private TMP_Text displayHora;
    [SerializeField] private TMP_Text displayEstacao;
    [Space(5)]
    [SerializeField] private Button horaMais;
    [SerializeField] private Button horaMenos;
    [Space(5)]
    [SerializeField] private Button estacaoMais;
    [SerializeField] private Button estacaoMenos;
    [Space(5)]
    [SerializeField] private int horaAtual;
    [SerializeField] private int idEstacaoAtual;

    private Button botaoSelecionadoAtual;

    private float tempoParaSelecionar = 0f;

    private readonly int[] horas = new int[] { 8, 12, 16 };
    private readonly string[] estacoes = new string[] { "PRIMAVERA", "VERÃO", "OUTONO", "INVERNO" };
    private readonly DateTime[] dataEstacoes = new DateTime[] { new DateTime(2023, 11, 22),
                                               new DateTime(2023, 02, 21),
                                               new DateTime(2023, 5, 21),
                                               new DateTime(2023, 8, 22) };

    private bool loaded = false;

    private void Validate()
    {
        blocos = FindObjectsOfType<Bloco>().ToList();
        blocos.Sort((b1, b2) => b1.IdBloco < b2.IdBloco ? -1 : 1);

        idBlocoAtual = Mathf.Clamp(idBlocoAtual, 0, blocos.Count - 1);
        idAndarAtual = Mathf.Clamp(idAndarAtual, 0, blocos[idBlocoAtual].Andares.Count - 1);
        idAptoAtual = Mathf.Clamp(idAptoAtual, 0, blocos[idBlocoAtual].Andares[idAndarAtual].Apartamentos.Count - 1);

        // Verificando objetos de cena
        if (controladorCameras == null)
            controladorCameras = FindObjectOfType<ControladorCameras>();

        if (cameraApartamento == null)
            cameraApartamento = controladorCameras.transform.GetChild(1);

        if (maquetePrefab == null)
            maquetePrefab = baseMaquete.GetChild(0);
        //

        ConfigurarApartamento();
    }

    private void Awake()
    {
        loaded = false;

        idBlocoAtual = GameController.Instance.IdBlocoAtual;
        idAndarAtual = GameController.Instance.IdAndarAtual;
        idAptoAtual = GameController.Instance.IdAptoAtual;

        SpawnarAreas(() =>
        {
            Validate();

            ConfigurarBotoes();
            CriarBotoesCameras();
            ConfigurarApartamento();

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

        StartCoroutine(empreendimento.LoadMaquetePrefab(go =>
        {
            StartCoroutine(empreendimento.LoadConfigCameraMaquete(go =>
            {
                StartCoroutine(empreendimento.LoadSolMaquete(go =>
                {
                    controladorCameras = go.GetComponent<ControladorCameras>();

                    onLoadComplete?.Invoke();
                }));
            }));
        }, baseMaquete));
    }

    private void ConfigurarBotoes()
    {
        //botaoPainelAjustes.onClick.RemoveAllListeners();
        blocoMais.onClick.RemoveAllListeners();
        blocoMenos.onClick.RemoveAllListeners();
        andarMais.onClick.RemoveAllListeners();
        andarMenos.onClick.RemoveAllListeners();
        aptoMais.onClick.RemoveAllListeners();
        aptoMenos.onClick.RemoveAllListeners();

        botaoAreaExterna.onClick.RemoveAllListeners();
        botaoAreaInterna.onClick.RemoveAllListeners();

        botaoEstacao.onClick.RemoveAllListeners();

        horaMais.onClick.RemoveAllListeners();
        horaMenos.onClick.RemoveAllListeners();
        estacaoMais.onClick.RemoveAllListeners();
        estacaoMenos.onClick.RemoveAllListeners();

        //botaoPainelAjustes.onClick.AddListener(OnBotaoPainelAjustes);
        blocoMais.onClick.AddListener(OnBlocoMais);
        blocoMenos.onClick.AddListener(OnBlocoMenos);
        andarMais.onClick.AddListener(OnAndarMais);
        andarMenos.onClick.AddListener(OnAndarMenos);
        aptoMais.onClick.AddListener(OnAptoMais);
        aptoMenos.onClick.AddListener(OnAptoMenos);

        botaoAreaExterna.onClick.AddListener(OnBotaoAreaExternaClick);
        botaoAreaInterna.onClick.AddListener(OnBotaoAreaInternaClick);

        botaoEstacao.onClick.AddListener(OnBotaoEstacaoClick);

        horaMais.onClick.AddListener(OnHoraMais);
        horaMenos.onClick.AddListener(OnHoraMenos);
        estacaoMais.onClick.AddListener(OnEstacaoMais);
        estacaoMenos.onClick.AddListener(OnEstacaoMenos);

        AtualizarDisplays();

        displayEstacao.SetText(estacoes[idEstacaoAtual]);
    }
    private void CriarBotoesCameras()
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

        cameraApartamento = controladorCameras.transform.GetChild(1);

        cuboSelecao.gameObject.SetActive(false);
        setaSelecao.gameObject.SetActive(false);
    }
    private void SelecionarCamera(Button botaoSelecionadoAtual, CameraComodo cameraComodo)
    {
        if (tempoParaSelecionar > 0) return;

        if (this.botaoSelecionadoAtual != null)
            this.botaoSelecionadoAtual.image.sprite = botaoDesselecionado;

        this.botaoSelecionadoAtual = botaoSelecionadoAtual;

        this.botaoSelecionadoAtual.image.sprite = botaoSelecionado;

        controladorCameras.SelecionarCamera(cameraComodo);

        bool vistaApto = (cameraComodo.NomeComodo == "Vista Apartamento");

        if (painelEstacao.gameObject.activeSelf)
            OnBotaoEstacaoClick();

        painelApartamentos.gameObject.SetActive(vistaApto);

        cuboSelecao.gameObject.SetActive(vistaApto);
        setaSelecao.gameObject.SetActive(vistaApto);

        tempoParaSelecionar = 1f;
    }

    private void OnBotaoAreaExternaClick() { SceneController.Instance.LoadScene(Scenes._07_Area_Externa); }
    private void OnBotaoAreaInternaClick() { SceneController.Instance.LoadScene(Scenes._08_Area_Interna); }
    private void OnBotaoEstacaoClick()
    {
        if (painelApartamentos.gameObject.activeSelf)
            SelecionarCamera(grupoBotoesAreas.GetChild(grupoBotoesAreas.childCount - 2).GetComponent<Button>(), controladorCameras.CamerasComodo[0]);

        painelEstacao.gameObject.SetActive(!painelEstacao.gameObject.activeSelf);

        botaoEstacao.image.sprite = painelEstacao.gameObject.activeSelf ? botaoSelecionado : botaoDesselecionado;

        AtualizarDisplays();
    }
    private void OnHoraMais()
    {
        horaAtual++;

        if (horaAtual > horas.Length - 1)
            horaAtual = 0;

        AtualizarDisplays();
    }
    private void OnHoraMenos()
    {
        horaAtual--;

        if (horaAtual < 0)
            horaAtual = horas.Length - 1;

        AtualizarDisplays();
    }
    private void OnEstacaoMais()
    {
        idEstacaoAtual++;

        if (idEstacaoAtual > 3)
            idEstacaoAtual = 0;

        AtualizarDisplays();
    }
    private void OnEstacaoMenos()
    {
        idEstacaoAtual--;

        if (idEstacaoAtual < 0)
            idEstacaoAtual = 3;

        AtualizarDisplays();
    }
    private void OnBotaoPainelAjustes()
    {
        if (tempoParaSelecionar > 0) return;

        painelApartamentos.gameObject.SetActive(!painelApartamentos.gameObject.activeSelf);

        //botaoPainelAjustes.image.sprite = painelAjustes.gameObject.activeSelf ? botaoSelecionado : botaoDesselecionado;

        SelecionarCamera(painelApartamentos.gameObject.activeSelf ? grupoBotoesAreas.GetChild(grupoBotoesAreas.childCount - 1).GetComponent<Button>() : grupoBotoesAreas.GetChild(grupoBotoesAreas.childCount - 2).GetComponent<Button>(),
                         painelApartamentos.gameObject.activeSelf ? controladorCameras.CamerasComodo[1] : controladorCameras.CamerasComodo[0]);

        cuboSelecao.gameObject.SetActive(painelApartamentos.gameObject.activeSelf);
        setaSelecao.gameObject.SetActive(painelApartamentos.gameObject.activeSelf);

        tempoParaSelecionar = 1f;
    }
    private void OnBlocoMais()
    {
        idBlocoAtual++;

        if (idBlocoAtual > blocos.Count - 1)
            idBlocoAtual = 0;

        if (idAndarAtual > blocos[idBlocoAtual].Andares.Count - 1)
            idAndarAtual = 0;

        AtualizarDisplays();
        ConfigurarApartamento();
    }
    private void OnBlocoMenos()
    {
        idBlocoAtual--;

        if (idBlocoAtual < 0)
            idBlocoAtual = blocos.Count - 1;

        if (idAndarAtual > blocos[idBlocoAtual].Andares.Count - 1)
            idAndarAtual = 0;

        AtualizarDisplays();
        ConfigurarApartamento();
    }

    private void OnAndarMais()
    {
        idAndarAtual++;

        if (idAndarAtual > blocos[idBlocoAtual].Andares.Count - 1)
            idAndarAtual = 0;

        if (idAptoAtual > blocos[idBlocoAtual].Andares[idAndarAtual].Apartamentos.Count - 1)
            idAptoAtual = 0;

        AtualizarDisplays();
        ConfigurarApartamento();
    }
    private void OnAndarMenos()
    {
        idAndarAtual--;

        if (idAndarAtual < 0)
            idAndarAtual = blocos[idBlocoAtual].Andares.Count - 1;

        if (idAptoAtual > blocos[idBlocoAtual].Andares[idAndarAtual].Apartamentos.Count - 1)
            idAptoAtual = 0;

        AtualizarDisplays();
        ConfigurarApartamento();
    }

    private void OnAptoMais()
    {
        idAptoAtual++;

        if (idAptoAtual > blocos[idBlocoAtual].Andares[idAndarAtual].Apartamentos.Count - 1)
            idAptoAtual = 0;

        AtualizarDisplays();
        ConfigurarApartamento();
    }
    private void OnAptoMenos()
    {
        idAptoAtual--;

        if (idAptoAtual < 0)
            idAptoAtual = blocos[idBlocoAtual].Andares[idAndarAtual].Apartamentos.Count - 1;

        AtualizarDisplays();
        ConfigurarApartamento();
    }

    private void AtualizarDisplays()
    {
        displayBlocos.SetText(blocos[idBlocoAtual].IdBloco.ToString("00"));
        displayAndares.SetText(blocos[idBlocoAtual].Andares[idAndarAtual].IdAndar.ToString("00"));
        displayAptos.SetText(blocos[idBlocoAtual].Andares[idAndarAtual].Apartamentos[idAptoAtual].IdApartamento.ToString("000"));

        displayHora.SetText($"{horas[horaAtual]} h");
        displayEstacao.SetText(estacoes[idEstacaoAtual]);

        controladorSol.timeInHours = horas[horaAtual];
        controladorSol.dayOfYear = dataEstacoes[idEstacaoAtual].DayOfYear;

        // TODO Salvar Apartamento atual
        GameController.Instance.IdBlocoAtual = idBlocoAtual;
        GameController.Instance.IdAndarAtual = idAndarAtual;
        GameController.Instance.IdAptoAtual = idAptoAtual;
    }

    private void ConfigurarApartamento()
    {
        apartamentoAtual = blocos[idBlocoAtual].Andares[idAndarAtual].Apartamentos[idAptoAtual];

        // Atualização de posição, rotação e escala da camera
        cameraApartamento.transform.forward = idBlocoAtual % 2 == 0 ? (idAptoAtual <= 1 ? -blocos[idBlocoAtual].Andares[idAndarAtual].transform.right : blocos[idBlocoAtual].Andares[idAndarAtual].transform.right) : (idAptoAtual > 1 ? -blocos[idBlocoAtual].Andares[idAndarAtual].transform.right : blocos[idBlocoAtual].Andares[idAndarAtual].transform.right);
        cameraApartamento.transform.position = blocos[idBlocoAtual].Andares[idAndarAtual].transform.position + (blocos[idBlocoAtual].transform.forward * 5f * (idBlocoAtual % 2 == 0 ? 1 : -1));

        // Atualização da posição da seta
        Vector3 positionToGo = blocos[idBlocoAtual].transform.position + (blocos[idBlocoAtual].transform.forward * 7.5f * (idBlocoAtual % 2 == 0 ? 1 : -1));
        setaSelecao.transform.position = positionToGo + (Vector3.up * 15);

        // Atualização de posição do cubo
        cuboSelecao.eulerAngles = apartamentoAtual.transform.eulerAngles;
        cuboSelecao.localScale = apartamentoAtual.transform.localScale;
        cuboSelecao.position = apartamentoAtual.transform.position;

        // Resetar a camera
        controladorCameras.CameraAtual?.Inicializar();
        controladorCameras.CameraAtual?.SetPriorityValue(99);
    }
}
