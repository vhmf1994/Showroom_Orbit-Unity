using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Andar : MonoBehaviour
{
    [SerializeField] private int idAndar;
    [SerializeField] private List<Apartamento> apartamentos;

    public int IdAndar => idAndar;
    public List<Apartamento> Apartamentos => apartamentos;

    private void OnValidate()
    {
        gameObject.name = $"{transform.parent.name}_FL_0{transform.GetSiblingIndex() + 1}";

        apartamentos = GetComponentsInChildren<Apartamento>().ToList();

        float y = 0;

        switch (idAndar)
        {
            case 1:
                y = -1.244527f;
                break;
            case 2:
                y = 1.385472f;
                break;
            case 3:
                y = 4.015472f;
                break;
            case 4:
                y = 6.645472f;
                break;
            case 5:
                y = 9.275472f;
                break;
        }
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
}
