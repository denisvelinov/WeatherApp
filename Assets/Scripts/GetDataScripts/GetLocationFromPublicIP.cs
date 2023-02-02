using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GetLocationFromPublicIP : MonoBehaviour
{
    public class Root
    {
        public string geoplugin_request { get; set; }
        public int geoplugin_status { get; set; }
        public string geoplugin_delay { get; set; }
        public string geoplugin_credit { get; set; }
        public string geoplugin_city { get; set; }
        public string geoplugin_region { get; set; }
        public string geoplugin_regionCode { get; set; }
        public string geoplugin_regionName { get; set; }
        public string geoplugin_areaCode { get; set; }
        public string geoplugin_dmaCode { get; set; }
        public string geoplugin_countryCode { get; set; }
        public string geoplugin_countryName { get; set; }
        public int geoplugin_inEU { get; set; }
        public int geoplugin_euVATrate { get; set; }
        public string geoplugin_continentCode { get; set; }
        public string geoplugin_continentName { get; set; }
        public string geoplugin_latitude { get; set; }
        public string geoplugin_longitude { get; set; }
        public string geoplugin_locationAccuracyRadius { get; set; }
        public string geoplugin_timezone { get; set; }
        public string geoplugin_currencyCode { get; set; }
        public string geoplugin_currencySymbol { get; set; }
        public string geoplugin_currencySymbol_UTF8 { get; set; }
        public double geoplugin_currencyConverter { get; set; }
    }

    const string URL_GetPublicIP = "https://api.ipify.org/";
    const string URL_GetGeographicData = "http://www.geoplugin.net/json.gp?ip=";

    private string PublicIP;
    private Root root;
    public string IPLocationResponce;

    public IEnumerator GetPublicIP() 
    {
        // attempt to retrieve our public IP address
        string resultHolder = null;
        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetPublicIP))
        {
            request.timeout = 3;
            yield return request.SendWebRequest();

            // did the request succeed?
            if (request.result == UnityWebRequest.Result.Success)
            {
                PublicIP = request.downloadHandler.text.Trim();
                StartCoroutine(GetGeographicInformation());
            }
            else
            {
                resultHolder = $"Failed to get public IP: {request.downloadHandler.text}";
            }
        }
        IPLocationResponce = resultHolder;
        yield return null;
    }

    IEnumerator GetGeographicInformation()
    {
        string resultHolder;
        // attempt to retrieve the geographic data
        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetGeographicData + PublicIP))
        {
            request.timeout = 1;
            yield return request.SendWebRequest();

            // did the request succeed?
            if (request.result == UnityWebRequest.Result.Success)
            {
                root = JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);

                string[] tz = root.geoplugin_timezone.Split('\\');
                resultHolder = $"latitude={root.geoplugin_latitude}&longitude={root.geoplugin_longitude}&timezone={string.Join("", tz)}";
            }
            else
            {
                resultHolder = $"Failed to get geographic data: {request.downloadHandler.text}";
            }
        }
        IPLocationResponce = resultHolder;
        yield return null;
    }
}
