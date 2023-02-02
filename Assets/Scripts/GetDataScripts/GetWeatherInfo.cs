using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetWeatherInfo : MonoBehaviour
{
    public class Daily
    {
        public List<string> time { get; set; }
        public List<double> temperature_2m_max { get; set; }
    }

    public class DailyUnits
    {
        public string time { get; set; }
        public string temperature_2m_max { get; set; }
    }

    public class Root
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public double elevation { get; set; }
        public DailyUnits daily_units { get; set; }
        public Daily daily { get; set; }
    }

    const string URL_GetWeatherData = "https://api.open-meteo.com/v1/forecast?";

    public Root weatherInfo;
    public string WeatherResponce;

    public IEnumerator GetWeatherInformation(string geographicData)
    {
        string weatherURL = URL_GetWeatherData;
        weatherURL += geographicData;
        weatherURL += "&daily=temperature_2m_max";

        string resultHolder = null;
        // attempt to retrieve the geographic data
        using (UnityWebRequest request = UnityWebRequest.Get(weatherURL))
        {
            request.timeout = 1;
            yield return request.SendWebRequest();

            // did the request succeed?
            if (request.result == UnityWebRequest.Result.Success)
            {
                weatherInfo = JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);

                resultHolder = "{" + $"{'"'}Latitude{'"'}: {weatherInfo.latitude}" +
                    $",{'"'}Longitude{'"'}: {weatherInfo.longitude}" +
                    $",{'"'}Timezone{'"'}: {'"'}{weatherInfo.timezone}{'"'}" +
                    $",{'"'}TimezoneAbbreviation{'"'}: {'"'}{weatherInfo.timezone_abbreviation}{'"'}" +
                    $",{'"'}Elevation{'"'}: {weatherInfo.elevation}" +
                    $",{'"'}Days{'"'}: [{'"'}{string.Join($"{'"'},{'"'}", weatherInfo.daily.time)}{'"'}]" +
                    $",{'"'}TemperaturesForcastDayMax{'"'}: [{string.Join(",", weatherInfo.daily.temperature_2m_max)}]" + "}";
            }
            else
            {
                WeatherResponce = $"Failed to get geographic data: {request.downloadHandler.text}";
            }
        }
        WeatherResponce = resultHolder;
        yield return null;
    }
}
