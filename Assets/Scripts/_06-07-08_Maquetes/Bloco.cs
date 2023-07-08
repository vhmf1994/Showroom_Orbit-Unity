using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bloco : MonoBehaviour
{
    [SerializeField] private int idBloco;
    [SerializeField] private List<Andar> andares;

    public int IdBloco => idBloco;
    public List<Andar> Andares => andares;

    private void OnValidate()
    {
        gameObject.name = $"Bloco_{idBloco}"; 

        andares = GetComponentsInChildren<Andar>().ToList();
    }
}
