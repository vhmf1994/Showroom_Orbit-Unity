using SimpleJSON;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PainelFichaTecnica : MonoBehaviour
{
    [Header("Cabeçalho")]
    [SerializeField] private TMP_Text textoTitulo;
    [SerializeField] private TMP_Text textoProduto;
    [SerializeField] private TMP_Text textoEndereco;
    [SerializeField] private TMP_Text textoAreaTerreno;

    [Header("Área de Lazer")]
    [SerializeField] private Transform gridArea;
    [SerializeField] private Transform prefabArea;

    [Header("Tipologia das Unidades")]
    [SerializeField] private Transform gridTipologia;
    [SerializeField] private Transform prefabTipologia;

    [Header("Proximidades")]
    [SerializeField] private Transform gridProximidades;
    [SerializeField] private Transform prefabProximidades;

    [Header("Vias de Acesso")]
    [SerializeField] private Transform gridViasAcesso;
    [SerializeField] private Transform prefabViasAcesso;

    private void Start()
    {
        ConfigurarPainel();
    }

    /*private void ConfigurarPainel()
    {
        FichaTecnica fichaTecnica = GameController.Instance.FichaTecnicaSelecionada;

        textoTitulo.SetText($"Ficha técnica - {fichaTecnica.Nome}".ToUpper());
        textoProduto.SetText($"Produto: {fichaTecnica.Produto}");
        textoEndereco.SetText(fichaTecnica.Endereco);
        textoAreaTerreno.SetText(fichaTecnica.AreaTerreno);

        for (int i = 0; i < fichaTecnica.AreasLazer.Count; i++)
        {
            Transform objetoArea = Instantiate(prefabArea, gridArea);

            objetoArea.GetChild(1).GetComponent<TMP_Text>().SetText(fichaTecnica.AreasLazer[i]);
            objetoArea.GetChild(0).GetComponent<Image>().sprite = ResourcesController.Instance.IconesAreaExterna[fichaTecnica.IconesAreasLazer[i]];
        }

        for (int i = 0; i < fichaTecnica.Tipologia.Count; i++)
        {
            Transform objetoTipo = Instantiate(prefabTipologia, gridTipologia);

            objetoTipo.GetChild(1).GetComponent<TMP_Text>().SetText(fichaTecnica.Tipologia[i]);
        }

        for (int i = 0; i < fichaTecnica.Proximidade.Count; i++)
        {
            Transform objetoProximidade = Instantiate(prefabProximidades, gridProximidades);

            objetoProximidade.GetChild(1).GetComponent<TMP_Text>().SetText(fichaTecnica.Proximidade[i]);
        }

        for (int i = 0; i < fichaTecnica.ViasAcesso.Count; i++)
        {
            Transform objetoViasAcesso = Instantiate(prefabViasAcesso, gridViasAcesso);

            objetoViasAcesso.GetChild(1).GetComponent<TMP_Text>().SetText(fichaTecnica.ViasAcesso[i]);
        }
    }*/

    private void ConfigurarPainel()
    {
        Empreendimento empreendimento = AddressablesController.Instance.GetEmpreendimento();

        textoTitulo.SetText($"Ficha técnica - {empreendimento.Nome}".ToUpper());

        StartCoroutine(empreendimento.LoadFichaTecnica( (ta) =>
        {
            #region Conversão Json
            JSONNode nodeFicha = JSON.Parse(ta.text);

            string nome = nodeFicha[nameof(nome)];
            string slogan = nodeFicha[nameof(slogan)];
            string resumo = nodeFicha[nameof(resumo)];
            string produto = nodeFicha[nameof(produto)];
            string endereco = nodeFicha[nameof(endereco)];
            string area = nodeFicha[nameof(area)];

            List<string> areasLazer = new List<string>();
            List<string> tipologia = new List<string>();
            List<string> proximidades = new List<string>();
            List<string> viasAcesso = new List<string>();

            List<int> iconesAreaLazer = new List<int>();

            JSONNode nodeAreasLazer = nodeFicha[nameof(areasLazer)];
            JSONNode nodeTipologia = nodeFicha[nameof(tipologia)];
            JSONNode nodeProximidades = nodeFicha[nameof(proximidades)];
            JSONNode nodeViasAcesso = nodeFicha[nameof(viasAcesso)];

            for (int j = 0; j < nodeAreasLazer.Count; j++)
                areasLazer.Add(nodeAreasLazer[j]);

            for (int j = 0; j < nodeTipologia.Count; j++)
                tipologia.Add(nodeTipologia[j]);

            for (int j = 0; j < nodeProximidades.Count; j++)
                proximidades.Add(nodeProximidades[j]);

            for (int j = 0; j < nodeViasAcesso.Count; j++)
                viasAcesso.Add(nodeViasAcesso[j]);

            foreach (string key in nodeAreasLazer.Keys)
                iconesAreaLazer.Add(int.Parse(key));
            #endregion

            textoProduto.SetText($"Produto: {produto}");
            textoEndereco.SetText(endereco);
            textoAreaTerreno.SetText(area);

            StartCoroutine(AddressablesController.Instance.LoadIconesAreaExterna((icones) =>
            {
                for (int i = 0; i < areasLazer.Count; i++)
                {
                    Transform objetoArea = Instantiate(prefabArea, gridArea);

                    objetoArea.GetChild(1).GetComponent<TMP_Text>().SetText(areasLazer[i]);
                    objetoArea.GetChild(0).GetComponent<Image>().sprite = icones[iconesAreaLazer[i]];

                    Canvas.ForceUpdateCanvases();
                }
                for (int i = 0; i < tipologia.Count; i++)
                {
                    Transform objetoTipo = Instantiate(prefabTipologia, gridTipologia);

                    objetoTipo.GetChild(1).GetComponent<TMP_Text>().SetText(tipologia[i]);

                    Canvas.ForceUpdateCanvases();
                }
                for (int i = 0; i < proximidades.Count; i++)
                {
                    Transform objetoProximidade = Instantiate(prefabProximidades, gridProximidades);

                    objetoProximidade.GetChild(1).GetComponent<TMP_Text>().SetText(proximidades[i]);

                    Canvas.ForceUpdateCanvases();
                }
                for (int i = 0; i < viasAcesso.Count; i++)
                {
                    Transform objetoViasAcesso = Instantiate(prefabViasAcesso, gridViasAcesso);

                    objetoViasAcesso.GetChild(1).GetComponent<TMP_Text>().SetText(viasAcesso[i]);

                    Canvas.ForceUpdateCanvases();
                }
            }));
        }));
    }
}
