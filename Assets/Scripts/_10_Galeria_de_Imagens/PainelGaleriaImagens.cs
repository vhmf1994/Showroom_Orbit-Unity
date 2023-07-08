using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PainelGaleriaImagens : MonoBehaviour
{
    [SerializeField] private Transform grupoGaleriaImagens;
    [SerializeField] private Image imagemAtual;

    [SerializeField] private Button imagemPrefab;

    private Button imagemSelecionada;

    private List<Sprite> imagensSlide;

    private void Start()
    {
        Empreendimento empreendimento = AddressablesController.Instance.GetEmpreendimento();

        StartCoroutine(empreendimento.LoadGaleriaImagens((imagens) =>
        {
            imagensSlide = imagens;

            imagemAtual.sprite = imagensSlide[0];
            ConfigurarBotoesImagens();
        }));
    }

    private void ConfigurarBotoesImagens()
    {
        for (int i = 0; i < imagensSlide.Count; i++)
        {
            Button novaImagem = Instantiate(imagemPrefab, grupoGaleriaImagens);
            Sprite novaImageSprite = imagensSlide[i];

            novaImagem.name = $"[Button] - Imagem ({i})";

            novaImagem.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = novaImageSprite;
            novaImagem.transform.GetChild(1).gameObject.SetActive(false);

            novaImagem.GetComponent<Button>().onClick.AddListener(() => SelecionarImagem(novaImagem));

            if (imagemSelecionada == null)
            {
                SelecionarImagem(novaImagem);
            }
        }
    }

    private void SelecionarImagem(Button imagem)
    {
        if (this.imagemSelecionada != null)
            this.imagemSelecionada.transform.GetChild(1).gameObject.SetActive(false);

        this.imagemSelecionada = imagem;

        this.imagemSelecionada.transform.GetChild(1).gameObject.SetActive(true);

        imagemAtual.sprite = this.imagemSelecionada.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite;
    }
}