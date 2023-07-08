using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PainelLocalizacao : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private TMP_Text textoLocalizacao;

    [SerializeField] private RawImage imagemLocalizacao;

    [Header("Configurações")]
    [SerializeField] private string API_KEY;
    [Space]
    [SerializeField] private float latitude;
    [SerializeField] private float longitude;
    [Space]
    [SerializeField, Range(1, 20)] private int zoom;
    [Space]
    [SerializeField, ReadOnly] private string url;
    [Space]
    [SerializeField, ReadOnly] private int mapWidth;
    [SerializeField, ReadOnly] private int mapHeight;
    [Space]
    [SerializeField, ReadOnly] private Rect rect;
    [Space]
    [SerializeField, ReadOnly] private float lastLatitude;
    [SerializeField, ReadOnly] private float lastLongitude;
    [Space]
    [SerializeField, ReadOnly] private int lastZoom;
    [Space]
    [SerializeField, ReadOnly] private bool updatable;
    [SerializeField, ReadOnly] private bool isUpdating;

    //-23.45856, -46.91995

    private void Awake()
    {
        imagemLocalizacao = FindObjectOfType<RawImage>();

        rect = imagemLocalizacao.rectTransform.rect;

        mapWidth = (int)Math.Round(rect.width);
        mapHeight = (int)Math.Round(rect.height);
    }

    private void Start()
    {
        isUpdating = false;

        Empreendimento empreendimento = AddressablesController.Instance.GetEmpreendimento();

        StartCoroutine(empreendimento.LoadFichaTecnica((ta) =>
        {
            JSONNode nodeFicha = JSON.Parse(ta.text);
            string endereco = nodeFicha[nameof(endereco)];

            latitude = nodeFicha[nameof(latitude)];
            longitude = nodeFicha[nameof(longitude)];

            isUpdating = true;

            textoLocalizacao.SetText($"{empreendimento.Nome}\n{endereco}");
        }));
    }

    private void Update()
    {
        updatable = !isUpdating && ((latitude != lastLatitude) || (longitude != lastLongitude) || (zoom != lastZoom));

        if (!updatable) return;

        StartCoroutine(GetLocationInGoogleMaps(latitude, longitude, zoom));
    }

    private IEnumerator GetLocationInGoogleMaps(float latitude, float longitude, int zoom)
    {
        isUpdating = true;

        url = $"https://maps.googleapis.com/maps/api/staticmap?center={latitude},{longitude}" +
                                                               $"&zoom={zoom}" +
                                                               $"&size={mapWidth}x{mapHeight}" +
                                                               $"&scale=4" +
                                                               $"&markers=color:red%7C{latitude},{longitude}" +
                                                               $"&key={API_KEY}";

        Debug.Log(url);

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Houve um erro ao tentar localizar o mapa: {www.error}");
        }
        else
        {
            imagemLocalizacao.texture = (www.downloadHandler as DownloadHandlerTexture).texture;

            lastLatitude = latitude;
            lastLongitude = longitude;
            lastZoom = zoom;

            RecentralizarMapa();

            Debug.Log($"Mapa localizado com sucesso!");
        }

        isUpdating = false;
    }

    private IEnumerator Recentralizar()
    {
        float distanceFromCenter;

        do
        {
            distanceFromCenter = Vector2.Distance(imagemLocalizacao.rectTransform.anchoredPosition, Vector2.zero);

            imagemLocalizacao.rectTransform.anchoredPosition = Vector2.Lerp(imagemLocalizacao.rectTransform.anchoredPosition, Vector2.zero, 0.1f);

            yield return null;
        }
        while (distanceFromCenter > 0.1f);

        imagemLocalizacao.rectTransform.anchoredPosition = Vector2.zero;
    }

    [Button]
    public void RecentralizarMapa()
    {
        StartCoroutine(Recentralizar());
    }
    [Button()]
    public void DecreaseZoom()
    {
        zoom--;

        zoom = Mathf.Max(zoom, 13);
    }
    [Button]
    public void IncreaseZoom()
    {
        zoom++;

        zoom = Mathf.Min(zoom, 16);
    }
}
