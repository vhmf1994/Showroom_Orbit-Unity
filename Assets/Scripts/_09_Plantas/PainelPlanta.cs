using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PainelPlanta : MonoBehaviour
{
    [SerializeField] private BotaoPlanta plantaSelecionada;

    public BotaoPlanta PlantaSelecionada => plantaSelecionada;

    [SerializeField] private TMP_Text nomePlanta;
    [SerializeField] private Image imagemPlanta;

    [SerializeField] private LayoutElement painelLayoutElement;

    public void MudarSelecao(BotaoPlanta botaoPlanta)
    {
        plantaSelecionada = botaoPlanta;

        if (botaoPlanta == null) return;

        nomePlanta.SetText(botaoPlanta.NomePlanta);
        imagemPlanta.sprite = botaoPlanta.Planta;

        painelLayoutElement.preferredWidth = plantaSelecionada.ancorado ? 700 : 1000;
        painelLayoutElement.preferredHeight = plantaSelecionada.ancorado ? 700 : 1000;
    }

    public void Ativar(bool ativado) => gameObject.SetActive(ativado);
}
