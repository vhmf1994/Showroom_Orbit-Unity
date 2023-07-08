using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoFlutuante : MonoBehaviour
{
    public bool inverter = false;
    [Header("Configuracao da forma de onda")]
    public float amplitude = 0.01f;
    public float frequency = 0.5f;

    [Header("Offset para rotacionar nos eixos")]
    [Range(0.01f, 5f)] public float offsetX = 1f;
    [Range(0.01f, 5f)] public float offsetZ = 1f;


    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();
    private Vector3 tempRot = new Vector3();

    void Start()
    {
        posOffset = transform.position;
        tempRot = transform.eulerAngles;
    }

    void Update()
    {
        float formaDeOnda = Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        if(inverter == true) formaDeOnda *= -1f;

        tempPos = posOffset;
        tempPos.y += formaDeOnda;

        tempRot = transform.eulerAngles;
        tempRot.x += formaDeOnda * offsetX;
        
        tempRot.z += formaDeOnda * offsetZ;
        

        transform.position = tempPos;
        transform.rotation = Quaternion.Euler(tempRot);
    }
}
