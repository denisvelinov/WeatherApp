//using Newtonsoft.Json;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//public class GetLocationFromGPS : MonoBehaviour
//{
//    private string PublicIP;
//    private geoPluginResponse GeographicData;
//    private string IPResponce;
//    private string GeographicDataResponce;

//    private IEnumerator GetPublicIP()
//    {
//        // attempt to retrieve our public IP address
//        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetPublicIP))
//        {
//            request.timeout = 1;
//            yield return request.SendWebRequest();

//            // did the request succeed?
//            if (request.result == UnityWebRequest.Result.Success)
//            {
//                PublicIP = request.downloadHandler.text.Trim();
//                IPResponce = "Success";
//                StartCoroutine(GetGeographicInformation());
//            }
//            else
//            {
//                //Return responce $"Failed to get public IP: {request.downloadHandler.text}";
//                IPResponce = $"Failed to get public IP: {request.downloadHandler.text}";
//            }
//        }
//        yield return null;
//    }

//    private IEnumerator GetGeographicInformation()
//    {
//        // attempt to retrieve the geographic data
//        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetGeographicData + PublicIP))
//        {
//            request.timeout = 1;
//            yield return request.SendWebRequest();

//            // did the request succeed?
//            if (request.result == UnityWebRequest.Result.Success)
//            {
//                GeographicData = JsonConvert.DeserializeObject<geoPluginResponse>(request.downloadHandler.text);
//                //Return $"?latitude={GeographicData.Latitude}&longitude={GeographicData.Longitude}&timezone={GeographicData.TimeZone}"
//                GeographicDataResponce = $"latitude={GeographicData.Latitude}&longitude={GeographicData.Longitude}&timezone={GeographicData.TimeZone}";
//            }
//            else
//            {
//                //Return responce $"Failed to get geographic data: {request.downloadHandler.text}";
//                GeographicDataResponce = $"Failed to get geographic data: {request.downloadHandler.text}";
//            }
//        }
//        yield return null;
//    }

//    public string GetLocationResponce()
//    {
//        StartCoroutine(GetPublicIP());
//        if (IPResponce == "Success")
//        {
//            return GeographicDataResponce;
//        }
//        else
//        {
//            return IPResponce;
//        }
//    }
//}
