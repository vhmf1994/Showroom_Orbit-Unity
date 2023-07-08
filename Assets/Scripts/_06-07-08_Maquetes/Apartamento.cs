using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apartamento : MonoBehaviour
{
    [SerializeField] private int idApartamento;

    public int IdApartamento => idApartamento;

    private void OnValidate()
    {
        idApartamento = transform.GetSiblingIndex() + 1;
        gameObject.name = $"{transform.parent.name}_Apt_0{idApartamento}";

        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);

        int idBloco = transform.parent.parent.GetComponent<Bloco>().IdBloco;

        if(idBloco % 2 == 1)
        {
            switch (idApartamento)
            {
                case 1:
                    transform.localScale = Vector3.one;
                    transform.localRotation = Quaternion.Euler(transform.localRotation.x, 0, transform.localRotation.z);
                    break;
                case 2:
                    transform.localScale = new Vector3(1,1,-1);
                    break;
                case 3:
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 4:
                    transform.localScale = Vector3.one;
                    transform.localRotation = Quaternion.Euler(transform.localRotation.x, 180, transform.localRotation.z);
                    break;
            }
        }
        else
        {
            switch (idApartamento)
            {
                case 1:
                    transform.localScale = Vector3.one;
                    transform.localRotation = Quaternion.Euler(transform.localRotation.x, 180, transform.localRotation.z);
                    break;
                case 2:
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 3:
                    transform.localScale = new Vector3(1, 1, -1);
                    break;
                case 4:
                    transform.localScale = Vector3.one;
                    transform.localRotation = Quaternion.Euler(transform.localRotation.x, 0, transform.localRotation.z);
                    break;
            }
        }
    }
}
