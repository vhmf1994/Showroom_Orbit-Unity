using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PainelBotoes : MonoBehaviour
{
    [SerializeField] private List<Button> botoesPainel;

    private readonly Scenes[] cenasPainel = new Scenes[]
    {
        Scenes._03_Intro,
        Scenes._04_Localizacao,
        Scenes._05_Ficha_Tecnica,
        Scenes._06_Maquete,
        Scenes._09_Plantas,
        Scenes._10_Galeira_de_Imagens,
        Scenes._11_Galeira_de_Videos,
    };

    [SerializeField] private Scenes cenaAtual;

    [Header("Feedback")]
    [SerializeField] private Sprite botaoSelecionado;
    [SerializeField] private Sprite botaoDesselecionado;

    private void OnValidate()
    {
        AtualizarBotoes();
    }

    private void Start()
    {
        AtualizarBotoes();
        AtribuirEventos();

        BotoesExtras();
    }

    private void AtualizarBotoes()
    {
        botoesPainel = GetComponentsInChildren<Button>().ToList();

        for (int i = 0; i < botoesPainel.Count; i++)
        {
            int idCenaAtual = (int)cenaAtual;

            if (idCenaAtual != i)
            {
                botoesPainel[i].interactable = true;
                botoesPainel[i].image.sprite = botaoDesselecionado;
            }
            else
            {
                botoesPainel[i].interactable = false;
                botoesPainel[i].image.sprite = botaoSelecionado;
            }
        }
    }

    private void AtribuirEventos()
    {
        for (int i = 0; i < botoesPainel.Count; i++)
        {
            int idCena = i;

            botoesPainel[i].onClick.RemoveAllListeners();
            botoesPainel[i].onClick.AddListener(() =>
            {
                //Debug.Log($"Loading scene {cenasPainel[idCena]}");
                SceneController.Instance.LoadScene(cenasPainel[idCena]);
            });
        }
    }




    [Header("Botoes Extras")]
    [SerializeField] private GameObject logoMRV;
    [SerializeField] private GameObject logoEmpreendimento;

    int contMRV = 0;
    int contEmpreendimento = 0;

    // Extra
    private void BotoesExtras()
    {
        Button botaoMRV = logoMRV.AddComponent<Button>();
        Button botaoEmpreendimento = logoEmpreendimento.AddComponent<Button>();

        botaoMRV.transition = Selectable.Transition.None;
        botaoEmpreendimento.transition = Selectable.Transition.None;

        botaoMRV.onClick.AddListener(() =>
        {
            contMRV++;

            if (contMRV > 2)
                SceneController.Instance.LoadScene(Scenes._01_Inatividade);
        });
        botaoEmpreendimento.onClick.AddListener(() =>
        {
            contEmpreendimento++;

            if (contEmpreendimento > 2)
                Application.Quit();
        });
    }
}
