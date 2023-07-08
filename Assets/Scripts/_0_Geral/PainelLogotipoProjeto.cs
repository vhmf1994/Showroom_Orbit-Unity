using TMPro;
using UnityEngine;

public class PainelLogotipoProjeto : MonoBehaviour
{
    [SerializeField] private TMP_Text textoProjeto;

    private void Start()
    {
        AtualizarTexto();
    }

    private void AtualizarTexto()
    {
        Empreendimento empreendimento = AddressablesController.Instance.GetEmpreendimento();

        string nomeProjeto = empreendimento.Nome;

        textoProjeto.SetText($"Voc� est� em <color=#333333><b>{nomeProjeto}</b></color>");
    }
}
