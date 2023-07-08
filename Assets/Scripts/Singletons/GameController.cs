using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class GameController : MonoSingleton<GameController>
{
    [Header("Ficha Técnica Selecionada")]
    [SerializeField] private int idEmpreendimentoSelecionada;

    [Header("Seleção de Apartamentos")]
    [SerializeField, ReadOnly] private int idBlocoAtual;
    [SerializeField, ReadOnly] private int idAndarAtual;
    [SerializeField, ReadOnly] private int idAptoAtual;

    public int IdBlocoAtual { get { return idBlocoAtual; } set { idBlocoAtual = value; } }
    public int IdAndarAtual { get { return idAndarAtual; } set { idAndarAtual = value; } }
    public int IdAptoAtual { get { return idAptoAtual; } set { idAptoAtual = value; } }

    //public FichaTecnica FichaTecnicaSelecionada => ResourcesController.Instance.FichasTecnicas[fichaTecnicaSelecionada];
    public int IdEmpreendimentoSelecionado => idEmpreendimentoSelecionada;

    [Header("Inatividade")]
    [SerializeField] private float tempoMaximoInativo;
    private float tempoInativo;

    private bool inativo;

    protected override void InitializeBehaviour()
    {
        QualitySettings.vSyncCount = 1;

        StartCoroutine(WaitToStart());
    }

    private IEnumerator WaitToStart()
    {
        yield return FadeController.Instance.FadeIn(2f);

        yield return new WaitForSeconds(1f);

        SceneController.Instance.LoadScene(Scenes._01_Inatividade);
    }

    protected override void FinishBehaviour()
    {

    }

    protected override void UpdateBehaviour()
    {
        if (Input.anyKeyDown)
            tempoInativo = 0;

        if (inativo) return;

        tempoInativo += Time.deltaTime;

        if (tempoInativo >= tempoMaximoInativo)
        {
            SetInativo(true);
            SceneController.Instance.LoadScene(Scenes._01_Inatividade);
        }
    }

    public void SetInativo(bool inativo)
    {
        this.inativo = inativo;
        tempoInativo = this.inativo ? 0 : tempoInativo;
    }

    public void DefinirFichaTecnica(int fichaTecnica) => idEmpreendimentoSelecionada = fichaTecnica;
}