using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.Android;
using UnityEngine.UI;

public class WeatherManager : MonoBehaviour
{


    [Header("UI")]
    public Text statusText;
    public Text location;
    public Text country;

    // public Text mainWeather;
    public Text description;
    public Text temp;
    // public Text feels_like;
    public Text temp_min;
    public Text temp_max;
    public Text pressure;
    public Text humidity;
    public Text windspeed;

    private LocationInfo lastLocation;

    //  public TextMeshProUGUI text;
    void Start()
    {
        StartCoroutine(FetchLocationData());
    }
    private IEnumerator FetchLocationData()
    {
        Debug.Log("Started");
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        // First, check if user has location service enabled 
        if (!Input.location.isEnabledByUser) yield break;
        // Start service before querying location 
        Input.location.Start();
        // Wait until service initializes 
        int maxWait = 20; 
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        // Service didn't initialize in 20 seconds 
        if (maxWait < 1)
        {
            statusText.text = "Location Timed out";
            yield break;
        }
        // Connection has failed 
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            statusText.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            lastLocation = Input.location.lastData;
            //text.text = lastLocation.latitude.ToString() + "   " + lastLocation.longitude.ToString();
            UpdateWeatherData();
        }
        //Input.location.Stop();
    }
    private void UpdateWeatherData()
    {
        StartCoroutine(FetchWeatherDataFromApi(lastLocation.latitude.ToString(), lastLocation.longitude.ToString()));
    }
    private IEnumerator FetchWeatherDataFromApi(string latitude, string longitude)
    {
        string url = "http://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&appid=b69f62a5b739dca9d266de061632db9f&units=metric";
        UnityWebRequest fetchWeatherRequest = UnityWebRequest.Get(url);
        yield return fetchWeatherRequest.SendWebRequest();
        if (fetchWeatherRequest.isNetworkError || fetchWeatherRequest.isHttpError)
        {
            //Check and print error 
            statusText.text = fetchWeatherRequest.error;
        }
        else
        {
            Debug.Log(fetchWeatherRequest.downloadHandler.text);
            var response = JSON.Parse(fetchWeatherRequest.downloadHandler.text);
            location.text = response["name"];
            country.text = response["country"];

            // mainWeather.text = response["weather"][0]["main"];
            description.text = response["weather"][0]["description"];
            temp.text = response["main"]["temp"] + " C";
            //feels_like.text = "Feels like " + response["main"]["feels_like"] + " C";
            temp_min.text = "Min is " + response["main"]["temp_min"] + " C";
            temp_max.text = "Max is " + response["main"]["temp_max"] + " C";
            pressure.text = "Pressure is " + response["main"]["pressure"] + " Pa";
            humidity.text = response["main"]["humidity"] + " % Humidity";
            windspeed.text = "Windspeed is " + response["wind"]["speed"] + " Km/h";

        }
    }
}