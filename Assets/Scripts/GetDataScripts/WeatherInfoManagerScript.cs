using System.Collections;
using UnityEngine;
using DV.MessageTools;

public class WeatherInfoManagerScript : MonoBehaviour
{
    public enum EPhase
    {
        NotStarted,
        GetLocationInfo,
        GetWeatherInfo,

        Failed,
        Succeeded
    }

    public enum ErrorMessage 
    {
        None,
        LocationError,
        WeatherError
    }

    public EPhase Phase { get; private set; } = EPhase.NotStarted;
    public ErrorMessage DataError { get; private set; } = ErrorMessage.None;

    bool ShownWeatherInfo = false;

    //bool GetIPLocation = true;                                                //Later Implementation: Use code to choose between IP Location & GPS Location
    //bool GetGPSLocation = false;                                              //Later Implementation: Use code to choose between IP Location & GPS Location

    //Reference AppManager
    //[SerializeField] AppManager appManager;                                   //Later Implementation
    //Reference GetLocationFromPublicIP
    [SerializeField] GetLocationFromPublicIP ipLocation;
    //Reference GetLocationFromGPS
    //[SerializeField] GetLocationFromGPS gpsLocation;                          //Later Implementation: Get GPS Location
    //Reference GetWeatherInfo
    [SerializeField] GetWeatherInfo weatherInfo;

    [SerializeField] MessagesManager messageManager;                            //Later Implementation: Need to remove this reference, use appManager instead
    [SerializeField] DisplayManager displayManager;                             //Later Implementation: Need to remove this reference, use appManager instead

    string locationResponce;
    string weatherResponce;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetLocationInfo());                                      //Just for test, Later Implementation: trigger StartCoroutine from and event, not at app start
    }

    // Update is called once per frame
    void Update()
    {
        if (Phase == EPhase.Succeeded && !ShownWeatherInfo)
        {
            ShownWeatherInfo = true;

            //Send Toast message "Weather Data retrieved successfuly"
            messageManager.EnqueToastMessage("Weather Data retrieved successfuly");
            //Send weather data to Display Manager to display
            displayManager.SetAllDisplayproperties(weatherResponce);
        }
        else if (Phase == EPhase.Failed && DataError == ErrorMessage.LocationError && !ShownWeatherInfo)
        {
            ShownWeatherInfo = true;

            //Send Snackbar message $"Could not retrieve Location Data + {locationResponce}"
            messageManager.EnqueSnackbarMessage($"Could not retrieve Location Data + {locationResponce}");
        }
        else if (Phase == EPhase.Failed && DataError == ErrorMessage.WeatherError && !ShownWeatherInfo)
        {
            ShownWeatherInfo = true;

            //Send Snackbar message $"Could not retrieve Weather Data + {weatherResponce}"
            messageManager.EnqueSnackbarMessage($"Could not retrieve Weather Data + {weatherResponce}");
        }

        if (locationResponce == null || locationResponce == "")
        {
            locationResponce = ipLocation.IPLocationResponce;
        }
        if (weatherResponce == null || weatherResponce == "")
        {
            weatherResponce = weatherInfo.WeatherResponce;
        }
    }

    IEnumerator GetLocationInfo() 
    {
        Phase = EPhase.GetLocationInfo;

        StartCoroutine(ipLocation.GetPublicIP());                                //Just for test

        //if (GetIPLocation && !GetGPSLocation)                                  //Later Implementation: Use code to choose between IP Location & GPS Location
        //{
        //    //Get IP location StartCoroutine(ipLocation.GetPublicIP());
        //}
        //else if (!GetIPLocation && GetGPSLocation)
        //{
        //    //Get GPS location
        //}

        while (locationResponce == "" || locationResponce == null)
        {
            yield return null;
        }

        if (!locationResponce.StartsWith("Failed")) //Get Responce from Location Info scripts
        {
            StartCoroutine(GetWeatherInfo(locationResponce));
            yield return null;
        }
        else
        {
            Phase = EPhase.Failed;
            DataError = ErrorMessage.LocationError;
        }
        yield return null;
    }

    IEnumerator GetWeatherInfo(string geographicDataInfo)
    {
        Phase = EPhase.GetWeatherInfo;

        StartCoroutine(weatherInfo.GetWeatherInformation(geographicDataInfo));

        while (weatherResponce == "" || weatherResponce == null)
        {
            yield return null;
        }

        if (!weatherResponce.StartsWith("Failed")) //Get Responce from Weather Info script
        {
            Phase = EPhase.Succeeded;
        }
        else
        {
            Phase = EPhase.Failed;
            DataError = ErrorMessage.WeatherError;
        }

        yield return null;
    }
}
