using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public class PainelPlantas : MonoBehaviour
{
    [Header("Grupos")]
    [SerializeField] private Transform grupoBotoesPlantas;

    [Header("Prefab")]
    [SerializeField] private BotaoPlanta prefabBotaoPlanta;

    [Header("Painel Planta Selecionada")]
    [SerializeField] private PainelPlanta plantaSelecionada_1;
    [SerializeField] private PainelPlanta plantaSelecionada_2;

    [Header("Feedback")]
    [SerializeField] private Sprite botaoSelecionado;
    [SerializeField] private Sprite botaoDesselecionado;

    private void Start()
    {
        ConfigurarPainelSuperior();
    }

    private void ConfigurarPainelSuperior()
    {
        Empreendimento empreendimento = AddressablesController.Instance.GetEmpreendimento();

        ///
        StartCoroutine(empreendimento.LoadNomesPlantas((ta) =>
        {
            JSONNode nodePlantas = JSON.Parse(ta.text)["plantas"];

            StartCoroutine(empreendimento.LoadGaleriaPlantas((plantas) =>
            {
                for (int i = 0; i < nodePlantas.Count; i++)
                {
                    int idPlanta = i;

                    BotaoPlanta novoBotaoPlanta = Instantiate(prefabBotaoPlanta, grupoBotoesPlantas);

                    novoBotaoPlanta.ConfigurarBotaoPlanta(
                        nodePlantas[idPlanta],
                        plantas[idPlanta],
                        (n) => SelecionarPlanta(n)
                        );

                    if (plantaSelecionada_1.PlantaSelecionada == null)
                        SelecionarPlanta(novoBotaoPlanta);
                }
            }));
        }));
        ///
    }

    private void SelecionarPlanta(BotaoPlanta plantaSelecionada)
    {
        if (plantaSelecionada_1.PlantaSelecionada == null)
        {
            Debug.Log("Selecionando " + plantaSelecionada.NomePlanta);

            plantaSelecionada_1.MudarSelecao(plantaSelecionada);
            return;
        }

        if (plantaSelecionada.ancorado)
        {
            if (plantaSelecionada_1.PlantaSelecionada == plantaSelecionada ||
                plantaSelecionada_2.PlantaSelecionada == plantaSelecionada)
                return;

            plantaSelecionada_1.PlantaSelecionada.ancorado = true;
            plantaSelecionada_1.MudarSelecao(plantaSelecionada_1.PlantaSelecionada);

            plantaSelecionada_2.MudarSelecao(plantaSelecionada);
            plantaSelecionada_2.Ativar(true);
        }
        else
        {
            plantaSelecionada_1.PlantaSelecionada.ancorado = false;

            if (plantaSelecionada_2.PlantaSelecionada != null)
                plantaSelecionada_2.PlantaSelecionada.ancorado = false;

            plantaSelecionada_1.MudarSelecao(plantaSelecionada);

            plantaSelecionada_2.Ativar(false);
            plantaSelecionada_2.MudarSelecao(null);
        }
    }
}
