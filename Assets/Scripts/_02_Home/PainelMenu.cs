using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PainelMenu : MonoBehaviour
{
    [SerializeField] private List<CartaoMenu> cartoesMenu;

    [SerializeField] private Transform grupoCartoes;
    [SerializeField] private CartaoMenu cartaoPrefab;

    [SerializeField] private ScrollRect grupoCartoesScrollRect;
    [SerializeField] private Scrollbar grupoCartoesScrollBar;

    [SerializeField] private List<float> valoresScroll;

    [SerializeField] private float distancia;
    [SerializeField] private float scroll_Pos;

    private void Start()
    {
        ConfigurarCartoes();

        // TODO fazer efeito carrossel no grupo
        ConfigurarScroll();
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            scroll_Pos = grupoCartoesScrollBar.value;
        }
        else
        {
            for (int i = 0; i < valoresScroll.Count; i++)
            {
                if (scroll_Pos <= valoresScroll[i] + (distancia / 2) && scroll_Pos >= valoresScroll[i] - (distancia / 2))
                {
                    scroll_Pos = Mathf.Lerp(grupoCartoesScrollBar.value, valoresScroll[i], 0.1f);
                    grupoCartoesScrollBar.value = scroll_Pos;

                    cartoesMenu[i].ScaleUp();
                }
                else
                {
                    cartoesMenu[i].ScaleDown();
                }
            }
        }
    }

    private void ConfigurarCartoes()
    {
        cartoesMenu = new List<CartaoMenu>();

        /*for (int i = 0; i < ResourcesController.Instance.FichasTecnicas.Count; i++)
        {
            int idFicha = i;

            CartaoMenu novoCartao = Instantiate(cartaoPrefab, grupoCartoes);

            novoCartao.DefinirFichaTecnica(idFicha);

            cartoesMenu.Add(novoCartao);
        }*/
    }

    private void ConfigurarScroll()
    {
        if(grupoCartoes.childCount == 1)
        {
            distancia = 0;
        }
        else
        {
            distancia = 1f / (grupoCartoes.childCount - 1);
        }

        valoresScroll = new List<float>();

        for (float i = 0; i < grupoCartoes.childCount; i++)
        {
            float novoValor = i * distancia;

            valoresScroll.Add(novoValor);
        }
    }

    private void OnScrollValueChanged(Vector2 v)
    {
    }
}
