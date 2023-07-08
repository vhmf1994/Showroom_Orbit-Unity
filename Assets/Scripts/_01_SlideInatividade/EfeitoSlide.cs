using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EfeitoSlide : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Scenes sceneToGo;

    internal enum TipoEfeito
    {
        Pan_Horizontal,
        Pan_Diagonal,
        Zoom_In,
        Zoom_Out,
    }
    internal enum TipoSlide
    {
        Galeria,
        Empreendimentos
    }

    private RectTransform slideRectTransform;
    private Image slideImage;

    private List<Sprite> slideSprites;
    private List<Sprite> slideSpritesUsadas;

    [SerializeField] private List<TipoEfeito> efeitosDisponiveis;
    private List<TipoEfeito> efeitosUsados;

    private TipoEfeito tipoEfeitoAtual;
    [SerializeField] private TipoSlide tipoSlideAtual;

    private Vector2 posicaoFixa;

    private Vector2 posicaoInicial;
    private Vector2 posicaoFinal;

    private Vector2 escalaInicial;
    private Vector2 escalaFinal;

    [SerializeField] private float slideSpeed = 7;
    [SerializeField] private float zoomSpeed = 15;

    private UnityAction onClickEvent;

    private void Awake()
    {
        slideRectTransform = GetComponent<RectTransform>();
        slideImage = GetComponent<Image>();

        onClickEvent = () => StartCoroutine(OnClickEffect());

        posicaoFixa = slideRectTransform.anchoredPosition;
    }

    private void Start()
    {
        if(tipoSlideAtual == TipoSlide.Empreendimentos)
        {
            StartCoroutine(AddressablesController.Instance.LoadBackgroundSlides((s) =>
            {
                slideSprites = s;

                Debug.Log(slideSprites.Count);

                slideSpritesUsadas = new List<Sprite>(slideSprites);
                efeitosUsados = new List<TipoEfeito>(efeitosDisponiveis);

                NovoSlide();
            }));
        }
        else
        {
            Empreendimento empreendimento = AddressablesController.Instance.GetEmpreendimento();

            StartCoroutine(empreendimento.LoadGaleriaImagens((s) =>
            {
                slideSprites = s;

                Debug.Log(slideSprites.Count);

                slideSpritesUsadas = new List<Sprite>(slideSprites);
                efeitosUsados = new List<TipoEfeito>(efeitosDisponiveis);

                NovoSlide();
            }));
        }
    }

    private void NovoSlide()
    {
        ///////// Imagem nova
        int spriteID = Random.Range(0, slideSpritesUsadas.Count);

        slideImage.sprite = slideSpritesUsadas[spriteID];

        slideSpritesUsadas.RemoveAt(spriteID);

        if (slideSpritesUsadas.Count == 0)
            slideSpritesUsadas = new List<Sprite>(slideSprites);
        ///////// Imagem nova

        ///////// Tipo de efeito
        if (efeitosUsados.Count == 0)
            efeitosUsados = new List<TipoEfeito>(efeitosDisponiveis);

        do
        {
            int efeitoRandom = Random.Range(0, efeitosDisponiveis.Count);
            tipoEfeitoAtual = efeitosDisponiveis[efeitoRandom];
        }
        while (!efeitosUsados.Contains(tipoEfeitoAtual));

        efeitosUsados.Remove(tipoEfeitoAtual);
        ///////// Tipo de efeito

        StartCoroutine(EfeitoSlideCoroutine(tipoEfeitoAtual));
    }

    private void FixedUpdate()
    {
        slideRectTransform.anchoredPosition = Vector2.MoveTowards(slideRectTransform.anchoredPosition, posicaoFinal + posicaoFixa, Time.fixedDeltaTime * slideSpeed);
        slideRectTransform.sizeDelta = Vector2.MoveTowards(slideRectTransform.sizeDelta, escalaFinal, Time.fixedDeltaTime * zoomSpeed);
    }

    private IEnumerator EfeitoSlideCoroutine(TipoEfeito tipoEfeito)
    {
        // GERAR EFEITO
        switch (tipoEfeito)
        {
            case TipoEfeito.Pan_Horizontal:
                PanHorizontal();
                break;
            case TipoEfeito.Pan_Diagonal:
                PanDiagonal();
                break;
            case TipoEfeito.Zoom_In:
                ZoomIn();
                break;
            case TipoEfeito.Zoom_Out:
                ZoomOut();
                break;
            default:
                break;
        }

        slideRectTransform.anchoredPosition = posicaoFixa + posicaoInicial;
        slideRectTransform.sizeDelta = escalaInicial;

        slideImage.CrossFadeColor(Color.white, .5f, false, false);

        ///////// Efeito
        yield return new WaitForSeconds(10f);
        ///////// Efeito

        slideImage.CrossFadeColor(Color.black, .5f, false, false);

        yield return new WaitForSeconds(.5f);

        NovoSlide();
    }

    private void PanHorizontal()
    {
        posicaoInicial = 240 * ((Random.Range(0, 2) == 0) ? -1 : 1) * Vector2.right;
        posicaoFinal = posicaoInicial * -1;

        escalaInicial = new Vector2(2400, 1350);
        escalaFinal = new Vector2(2400, 1350);
    }
    private void PanDiagonal()
    {
        posicaoInicial = (240 * ((Random.Range(0, 2) == 0) ? -1 : 1) * Vector2.right) + 135 * ((Random.Range(0, 2) == 0) ? -1 : 1) * Vector2.up;
        posicaoFinal = posicaoInicial * -1;

        escalaInicial = new Vector2(2400, 1350);
        escalaFinal = new Vector2(2400, 1350);
    }

    private void ZoomOut()
    {
        posicaoInicial = 240 * (Random.Range(-1, 2) * Vector2.right) + 135 * (Random.Range(-1, 2) * Vector2.up);
        posicaoFinal = 360 * (Random.Range(-1, 2) * Vector2.right) + 202.5f * (Random.Range(-1, 2) * Vector2.up);

        escalaInicial = new Vector2(2400, 1350);
        escalaFinal = new Vector2(2640, 1485);
    }

    private void ZoomIn()
    {
        posicaoInicial = 360 * (Random.Range(-1, 2) * Vector2.right) + 202.5f * (Random.Range(-1, 2) * Vector2.up);
        posicaoFinal = 240 * (Random.Range(-1, 2) * Vector2.right) + 135 * (Random.Range(-1, 2) * Vector2.up);

        escalaInicial = new Vector2(2640, 1485);
        escalaFinal = new Vector2(2400, 1350);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent.Invoke();
    }

    private IEnumerator OnClickEffect()
    {
        slideImage.CrossFadeColor(Color.black, 1f, false, false);
        slideImage.raycastTarget = false;

        yield return null;

        SceneController.Instance.LoadScene(sceneToGo);
        GameController.Instance.SetInativo(false);
    }
}