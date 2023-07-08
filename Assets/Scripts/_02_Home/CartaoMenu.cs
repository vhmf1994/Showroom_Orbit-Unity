using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartaoMenu : MonoBehaviour
{
    [Header("Ficha Cartao")]
    [SerializeField] private int idFichaTecnica;

    [Header("Configuracao Visual")]
    [SerializeField] private Image logotipo;
    [SerializeField] private Image thumbnail;

    [SerializeField] private TMP_Text textosResumo;

    [Header("Componentes")]
    [SerializeField] private RectTransform cartaoRectTransform;
    [SerializeField] private Button botaoEntrar;

    private readonly Vector2 tamanhoFixo = new Vector2(400, 500);

    private void Start()
    {
        // TEMP
        idFichaTecnica = GameController.Instance.IdEmpreendimentoSelecionado;
        // TEMP
    }

    public void DefinirFichaTecnica(int fichaTecnica)
    {
        this.idFichaTecnica = fichaTecnica;

        //ConfigurarCartao(GameController.Instance.FichaTecnicaSelecionada);
    }

    private void ConfigurarCartao(FichaTecnica fichaTecnica)
    {
        logotipo.sprite = fichaTecnica.Logotipo;
        thumbnail.sprite = fichaTecnica.GaleriaImagens[0];

        textosResumo.SetText(fichaTecnica.Resumo);

        botaoEntrar.onClick.RemoveAllListeners();
        botaoEntrar.onClick.AddListener(() => OnBotaoEntrarClick(this.idFichaTecnica));
    }

    private void OnBotaoEntrarClick(int fichaTecnica)
    {
        GameController.Instance.DefinirFichaTecnica(fichaTecnica);

        SceneController.Instance.LoadScene(Scenes._03_Intro);
    }

    public void ScaleUp()
    {
        botaoEntrar.gameObject.SetActive(true);
        botaoEntrar.interactable = false;

        cartaoRectTransform.sizeDelta = Vector2.Lerp(cartaoRectTransform.sizeDelta, 100 * Vector2.up + tamanhoFixo, .1f);
        botaoEntrar.interactable = true;
    }

    public void ScaleDown()
    {
        botaoEntrar.gameObject.SetActive(false);

        cartaoRectTransform.sizeDelta = Vector2.Lerp(cartaoRectTransform.sizeDelta, tamanhoFixo, .1f);
    }
}
