using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RealisticSun : MonoBehaviour
{
    [SerializeField][Range(-90, 90)] private float latitude;
    [SerializeField][Range(-180, 180)] private float longitude;

    [SerializeField] private int year = 2023; // Ano
    [SerializeField][Range(1, 12)] private int month = 6; // M�s
    [SerializeField][Range(1, 31)] private int day = 14; // Dia

    [SerializeField][Range(0, 23)] private int hour = 12; // Hora

    private void OnValidate()
    {
        day = Mathf.Clamp(day, 1, System.DateTime.DaysInMonth(year, month));

        Debug.Log(DateTime.Now.DayOfYear);

        // Configura a posi��o inicial do sol
        CalculateSunRotation();
    }

    private void CalculateSunRotation()
    {
        // Obter a data e hora atual
        DateTime currentDate = new DateTime(year, month, day, hour, 0, 0);

        // Calcular o n�mero de dias desde o in�cio do ano
        int dayOfYear = currentDate.DayOfYear;

        // Obter o deslocamento do fuso hor�rio atual em horas
        int timeZoneOffset = TimeZoneInfo.Local.BaseUtcOffset.Hours;

        // Calcular a declina��o solar
        float declination = 23.45f * Mathf.Sin(Mathf.Deg2Rad * (360f / 365f * (dayOfYear - 81)));

        // Calcular o �ngulo hor�rio
        float hourAngle = (currentDate.Hour - 12) * 15f + (longitude - timeZoneOffset * 15f);

        // Converter os �ngulos para radianos
        float latitudeRad = Mathf.Deg2Rad * latitude;
        float declinationRad = Mathf.Deg2Rad * declination;
        float hourAngleRad = Mathf.Deg2Rad * hourAngle;

        // Calcular a altitude e o azimute do sol
        float sinAltitude = Mathf.Sin(latitudeRad) * Mathf.Sin(declinationRad) + Mathf.Cos(latitudeRad) * Mathf.Cos(declinationRad) * Mathf.Cos(hourAngleRad);
        float altitude = Mathf.Asin(sinAltitude);

        float cosAzimuth = (Mathf.Sin(declinationRad) * Mathf.Cos(latitudeRad) - Mathf.Cos(declinationRad) * Mathf.Sin(latitudeRad) * Mathf.Cos(hourAngleRad)) / Mathf.Cos(altitude);
        float azimuth = Mathf.Acos(cosAzimuth);

        // Converter os �ngulos de volta para graus
        altitude = Mathf.Rad2Deg * altitude;
        azimuth = Mathf.Rad2Deg * azimuth;

        transform.rotation = Quaternion.Euler(altitude, azimuth, 0f);
    }
}