using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BotaoPlanta : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text textoNomePlanta;
    [SerializeField] private Image imagemPlanta;

    [SerializeField] private string nomePlantaAtual;
    [SerializeField] private Sprite plantaAtual;

    [SerializeField] private Button ancorarComparacao;

    private Action<BotaoPlanta> onClick;

    public string NomePlanta => nomePlantaAtual;
    public Sprite Planta => plantaAtual;

    public bool ancorado;

    public void ConfigurarBotaoPlanta(string nomePlantaAtual, Sprite plantaAtual, Action<BotaoPlanta> onClick)
    {
        this.nomePlantaAtual = nomePlantaAtual;
        this.plantaAtual = plantaAtual;

        this.onClick = onClick;

        textoNomePlanta.SetText(this.nomePlantaAtual);
        imagemPlanta.sprite = this.plantaAtual;

        // TODO mudar ícone ancorado;

        ancorarComparacao.onClick.RemoveAllListeners();
        ancorarComparacao.onClick.AddListener(() => ancorado = !ancorado);

        this.ancorado = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Selecionar Planta
        onClick.Invoke(this);
    }
}
