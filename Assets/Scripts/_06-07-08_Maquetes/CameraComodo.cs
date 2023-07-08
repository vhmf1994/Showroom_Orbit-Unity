using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Camera Body = Framing Transposer | Aim = POV
 */

public class CameraComodo : MonoBehaviour
{
    private CinemachineBrain camera;
    private CinemachineVirtualCamera cameraComodo;
    private CinemachineVirtualCamera cameraJanela;

    public CinemachineVirtualCamera CameraComodoAtual => cameraComodo;
    public bool temVistaJanela => cameraJanela != null;

    private CinemachineComponentBase componentAim;
    private CinemachineComponentBase componentBody;

    [Header("Alguns valores tem informação ao passar o mouse por cima")]
    [Header("Geral")]
    [SerializeField][Tooltip("Nome dado ao cômodo equivalente à câmera atual")] private string nomeComodo;
    [SerializeField][Tooltip("Ícone de refência para ser usado nos botões do painel à direita")] private Sprite spriteComodo;

    [SerializeField][Range(40, 90)] private int cameraFOV = 60;

    [Header("Rotação X")]
    [SerializeField][Tooltip("Valor inicial que a câmera vai ficar rotacionada")] private int valorInicialX;
    [Space(10)]
    [SerializeField][Tooltip("Caso queira configurar um mínimo que essa câmera pode rotacionar em torno do ponto central")] private int valorMinimoX = 0;
    [SerializeField][Tooltip("Caso queira configurar um máximo que essa câmera pode rotacionar em torno do ponto central")] private int valorMaximoX = 360;

    [Header("Rotação Y")]
    [SerializeField][Tooltip("Valor inicial que a câmera vai ficar rotacionada")] private int valorInicialY;
    [Space(10)]
    [SerializeField][Tooltip("Caso queira configurar um mínimo que essa câmera pode rotacionar em torno do ponto central")] private int valorMinimoY = -90;
    [SerializeField][Tooltip("Caso queira configurar um máximo que essa câmera pode rotacionar em torno do ponto central")] private int valorMaximoY = 90;

    [Header("Zoom")]
    [SerializeField][Tooltip("Valor inicial  da distância que a câmera vai ficar do ponto central")] private float distanciaZoomInicial;
    [Space(10)]
    [SerializeField][Tooltip("Caso queira configurar um mínimo que essa câmera pode dar zoom")] private float valorZoomMinimo;
    [SerializeField][Tooltip("Caso queira configurar um máximo que essa câmera pode dar zoom")] private float valorZoomMaximo;
    [Space(10)]
    [SerializeField][Tooltip("Valor da sensibilidade do zoom (maior = mais rápido)")][Range(1, 300)] private int sensibilidade = 100;
    [SerializeField][Tooltip("Para camera geral: 'Input Value Gain' | camera em primeira pessoa: 'Max Speed'")] private AxisState.SpeedMode modoSensibilidade;

    //Controle
    private float x_Value;
    private float y_Value;

    private float camDistance_Value;

    public string NomeComodo => nomeComodo;
    public Sprite SpriteComodo => spriteComodo;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!string.IsNullOrEmpty(nomeComodo))
            gameObject.name = $"Câmera {nomeComodo}";

        if (valorMaximoX - valorMinimoX == 360)
        {
            if (valorInicialX > valorMaximoX)
                valorInicialX = valorMinimoX;

            if (valorInicialX < valorMinimoX)
                valorInicialX = valorMaximoX;
        }
        else
        {
            valorInicialX = Mathf.Clamp(valorInicialX, valorMinimoX, valorMaximoX);
        }

        //valorInicialX = valorInicialX > Valor ? valorInicialX - 360 : (valorInicialX < 0 ? valorInicialX + 360 : valorInicialX);

        //valorInicialX = Mathf.Clamp(valorInicialX, valorMinimoX, valorMaximoX);
        valorInicialY = Mathf.Clamp(valorInicialY, valorMinimoY, valorMaximoY);

        distanciaZoomInicial = Mathf.Clamp(distanciaZoomInicial, valorZoomMinimo, valorZoomMaximo);

        Inicializar();
    }
#endif

    public void Inicializar()
    {
        camera = FindObjectOfType<CinemachineBrain>();

        if (cameraComodo == null)
            cameraComodo = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();

        if (cameraJanela == null && transform.childCount > 1)
            cameraJanela = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();

        cameraComodo.m_Lens.FieldOfView = cameraFOV;

        if (cameraJanela != null)
            cameraJanela.m_Lens.FieldOfView = cameraFOV;

        cameraComodo.Follow = transform;
        cameraComodo.LookAt = transform;

        SetPriorityValue(transform.childCount - transform.GetSiblingIndex());

        componentAim = cameraComodo.GetCinemachineComponent(CinemachineCore.Stage.Aim);
        (componentAim as CinemachinePOV).m_HorizontalAxis.m_Wrap = (valorMaximoX - valorMinimoX == 360);
        (componentAim as CinemachinePOV).m_HorizontalAxis.m_MinValue = valorMinimoX;
        (componentAim as CinemachinePOV).m_HorizontalAxis.m_MaxValue = valorMaximoX;
        (componentAim as CinemachinePOV).m_VerticalAxis.m_MinValue = valorMinimoY;
        (componentAim as CinemachinePOV).m_VerticalAxis.m_MaxValue = valorMaximoY;
        SetRotation(valorInicialX, valorInicialY);

        componentBody = cameraComodo.GetCinemachineComponent(CinemachineCore.Stage.Body);
        SetZoom(distanciaZoomInicial);

        SetSensitivity(sensibilidade);

        AlternarVistas(false);
    }

    public void AlternarVistas(bool vistaJanela)
    {
        if (cameraJanela == null) return; // Caso consiga clicar no botão da vista por algum motivo

        cameraComodo.enabled = !vistaJanela;
        cameraJanela.enabled = vistaJanela;
    }

    public void AtualizarCamera(float x, float y, float zoom)
    {
        float zoomValue = camDistance_Value - zoom/* * Time.fixedDeltaTime*/;

        float xValue = (zoomValue < .2 ? -x : x) * Time.fixedDeltaTime;
        float yValue = (zoomValue < .2 ? -y : y) * Time.fixedDeltaTime;

        UpdatePosition(xValue, yValue);
        SetZoom(zoomValue);
    }

    public void SetPriorityValue(int priorityValue)
    {
        cameraComodo.Priority = priorityValue;

        if (cameraJanela != null)
            cameraJanela.Priority = priorityValue;
    }

    public void SetRotation(float x_newValue, float y_newValue)
    {
        x_Value = x_newValue;
        y_Value = Mathf.Clamp(y_newValue, valorMinimoY, valorMaximoY);

        if (componentAim is CinemachinePOV)
        {
            (componentAim as CinemachinePOV).m_HorizontalAxis.Value = x_Value;
            (componentAim as CinemachinePOV).m_VerticalAxis.Value = y_Value;
        }
    }
    public void ForceCameraPosition()
    {
        camera.m_UpdateMethod = CinemachineBrain.UpdateMethod.ManualUpdate;

        float x = (componentAim as CinemachinePOV).m_HorizontalAxis.Value;
        float y = (componentAim as CinemachinePOV).m_VerticalAxis.Value;

        cameraComodo.ForceCameraPosition(transform.position, Quaternion.identity);
        SetRotation(x, y);

        camera.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
    }
    public void UpdatePosition(float x_newValue, float y_newValue)
    {
        if (componentAim is CinemachinePOV)
        {
            (componentAim as CinemachinePOV).m_HorizontalAxis.m_InputAxisValue = x_newValue;
            (componentAim as CinemachinePOV).m_VerticalAxis.m_InputAxisValue = y_newValue;

            //SetRotation((componentAim as CinemachinePOV).m_HorizontalAxis.Value, (componentAim as CinemachinePOV).m_VerticalAxis.Value);
        }
    }
    public void SetZoom(float zoomNewValue)
    {
        camDistance_Value = Mathf.Clamp(zoomNewValue, valorZoomMinimo, valorZoomMaximo);

        if (componentBody is CinemachineFramingTransposer)
            (componentBody as CinemachineFramingTransposer).m_CameraDistance = camDistance_Value;
        else if (componentBody is Cinemachine3rdPersonFollow)
            (componentBody as Cinemachine3rdPersonFollow).CameraDistance = camDistance_Value;
    }
    public void SetSensitivity(float sensitivity)
    {
        if (componentAim is CinemachinePOV)
        {
            (componentAim as CinemachinePOV).m_HorizontalAxis.m_MaxSpeed = sensitivity;
            (componentAim as CinemachinePOV).m_VerticalAxis.m_MaxSpeed = sensitivity;

            (componentAim as CinemachinePOV).m_HorizontalAxis.m_SpeedMode = modoSensibilidade;
            (componentAim as CinemachinePOV).m_VerticalAxis.m_SpeedMode = modoSensibilidade;
        }
    }
}