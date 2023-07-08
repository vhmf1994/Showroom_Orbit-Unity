using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PainelGaleiraVideos : MonoBehaviour
{
    [SerializeField] private Transform grupoGaleriaVideos;

    [Space(10)]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource videoAudioSource;

    [Space(10)]
    [SerializeField] private RawImage videoAtual;
    [SerializeField] private Image imagemThumbnail;
    [SerializeField] private DadosVideo videoClipAtual;

    [Space(10)]
    [SerializeField] private Button botaoPlay;
    [SerializeField] private Slider sliderTempo;

    [Space(10)]
    [SerializeField] private Button videoPrefab;

    private Button videoSelecionado;

    private bool isPlaying;

    private void Start()
    {
        videoPlayer.loopPointReached += (vp) =>
        {
            vp.Stop();

            ResetUI();

            vp.Prepare();
        };

        videoPlayer.prepareCompleted += OnVideoPrepare;
        videoPlayer.loopPointReached += OnVideoEnded;

        ConfigurarBotoesVideos();

        botaoPlay.onClick.RemoveAllListeners();
        botaoPlay.onClick.AddListener(() => { if (isPlaying) Pause(); else Play(); });

        sliderTempo.onValueChanged.AddListener((v) =>
        {
            Pause();
            videoPlayer.frame = (long)sliderTempo.value;
        });
    }

    private void Update()
    {
        if (!isPlaying) return;

        // Slider - Usando essa função pra poder usar o slider sem afetar em nada o "onValueChanged" do slider
        sliderTempo.SetValueWithoutNotify(videoPlayer.frame);
    }

    List<Sprite> thumbsSprites;
    List<VideoClip> galeriaVideos;

    private void ConfigurarBotoesVideos()
    {
        Empreendimento empreendimento = AddressablesController.Instance.GetEmpreendimento();

        ///
        StartCoroutine(empreendimento.LoadGaleriaVideos((videos) =>
        {
            galeriaVideos = videos;

            StartCoroutine(empreendimento.LoadGaleriaVideosThumb((thumbs) =>
            {
                thumbsSprites = thumbs;

                for (int i = 0; i < galeriaVideos.Count; i++)
                {
                    Button novoVideo = Instantiate(videoPrefab, grupoGaleriaVideos);
                    VideoClip novoVideoClip = galeriaVideos[i];

                    novoVideo.name = $"[Button] - Video ({i})";

                    novoVideo.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = thumbsSprites[i];
                    novoVideo.transform.GetChild(1).gameObject.SetActive(false);

                    novoVideo.GetComponent<Button>().onClick.AddListener(() => SelecionarImagem(novoVideo));

                    if (videoSelecionado == null)
                    {
                        SelecionarImagem(novoVideo);
                    }
                }
            }));
        }));
    }

    private void SelecionarImagem(Button video)
    {
        if (this.videoSelecionado != null)
            this.videoSelecionado.transform.GetChild(1).gameObject.SetActive(false);

        this.videoSelecionado = video;

        this.videoSelecionado.transform.GetChild(1).gameObject.SetActive(true);

        videoPlayer.clip = galeriaVideos[thumbsSprites.IndexOf(this.videoSelecionado.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite)];
        videoPlayer.Prepare();

        imagemThumbnail.sprite = this.videoSelecionado.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite;

        ResetUI();
    }

    private void ResetUI()
    {
        // Play
        botaoPlay.interactable = false;

        isPlaying = false;
        botaoPlay.transform.GetChild(0).gameObject.SetActive(!isPlaying);
        botaoPlay.transform.GetChild(1).gameObject.SetActive(isPlaying);

        // Slider de Tempo
        sliderTempo.value = 0;

        // Thumb
        //imagemThumbnail.gameObject.SetActive(true);
    }

    private void OnVideoPrepare(VideoPlayer vp)
    {
        Pause();

        // Play
        botaoPlay.interactable = true;

        // Slider de Tempo
        sliderTempo.maxValue = vp.frameCount;

        OnVideoEnded(vp);
    }
    private void OnVideoEnded(VideoPlayer vp)
    {
        videoPlayer.frame = 0;

        // Thumbnail
        imagemThumbnail.gameObject.SetActive(true);
    }

    private void Play()
    {
        isPlaying = true;

        UpdateVideoDisplay();

        videoPlayer.Play();

        // Thumbnail
        imagemThumbnail.gameObject.SetActive(false);
    }

    private void Pause()
    {
        isPlaying = false;

        UpdateVideoDisplay();

        videoPlayer.Pause();
    }

    private void UpdateVideoDisplay()
    {
        // Play
        botaoPlay.transform.GetChild(0).gameObject.SetActive(!isPlaying);
        botaoPlay.transform.GetChild(1).gameObject.SetActive(isPlaying);
    }
}

public class DadosVideo
{
    [SerializeField] private int idVideo;

    [SerializeField] private Sprite videoThumbnail;
    [SerializeField] private VideoClip videoClip;
}