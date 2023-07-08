using SimpleJSON;
using TMPro;
using UnityEngine;

public class PainelIntro : MonoBehaviour
{
    [SerializeField] private TMP_Text textoTitulo;
    [SerializeField] private TMP_Text textoSubtitulo;

    [SerializeField] private TMP_Text textoDescricao;

    private void Awake()
    {
        ConfigurarPainel();
    }

    private void ConfigurarPainel()
    {
        Empreendimento empreendimento = AddressablesController.Instance.GetEmpreendimento();

        StartCoroutine(empreendimento.LoadFichaTecnica((ta) =>
        {
            JSONNode nodeFicha = JSON.Parse(ta.text);

            string slogan = nodeFicha[nameof(slogan)];
            string resumo = nodeFicha[nameof(resumo)];

            textoTitulo.SetText(empreendimento.Nome);

            textoSubtitulo.SetText(slogan);
            textoDescricao.SetText(resumo);
        }));
    }
}
