using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ControladorCameras : MonoBehaviour
{
    [Header("Componentes")]

    [SerializeField] private TouchField cameraTouchField;

    [Header("Lembre de colocar a camera geral primeiro na hierarquia")]
    [SerializeField] private List<CameraComodo> camerasComodo;
    public List<CameraComodo> CamerasComodo => camerasComodo;

    private CameraComodo cameraAtual;
    public CameraComodo CameraAtual => cameraAtual;

    private void OnValidate()
    {
        camerasComodo = GetComponentsInChildren<CameraComodo>().ToList();
    }

    private void Start()
    {
        cameraTouchField = FindObjectOfType<TouchField>();

        camerasComodo = GetComponentsInChildren<CameraComodo>().ToList();

        SelecionarCamera(camerasComodo.First());
    }

    public void SelecionarCamera(CameraComodo proximaCamera)
    {
        if (cameraAtual == proximaCamera) return;

        CameraComodo cameraAnterior = cameraAtual;

        StartCoroutine(ResetarUltimaCamera(cameraAnterior));

        cameraAtual = proximaCamera;

        cameraAtual.Inicializar();
        cameraAtual.SetPriorityValue(99);
    }

    public void AlternarVistas(bool vistaJanela)
    {
        if (cameraAtual == null) return;

        cameraAtual.AlternarVistas(vistaJanela);
    }

    public void ForceCameraPosition()
    {
        if (cameraAtual == null) return;

        cameraAtual.ForceCameraPosition();
    }

    private IEnumerator ResetarUltimaCamera(CameraComodo cameraAnterior)
    {
        cameraAnterior?.SetPriorityValue(1);

        yield return new WaitForSeconds(1f);

        cameraAnterior?.Inicializar();
    }

    private void LateUpdate()
    {
        cameraAtual.AtualizarCamera(cameraTouchField.Horizontal(), cameraTouchField.Vertical(), cameraTouchField.PinchDistance);
    }
}
