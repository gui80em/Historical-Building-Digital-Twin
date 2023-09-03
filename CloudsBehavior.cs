using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class CloudsBehavior : MonoBehaviour
{
    
    public Material cloudMat;
    private string weatherURL = "https://api.openweathermap.org/data/2.5/weather?lat=39&lon=-0&appid=d3ca2c4bef322734354f3f5bd1f548f7";
    public int cloud_weather;
    private float cloudiness_adapt;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        cloudMat = renderer.material;
        StartCoroutine(GetWeather());
    }

    IEnumerator GetWeather()
    {
        while(true){
            //Debug.Log("Comença rut");
            UnityWebRequest request = UnityWebRequest.Get(weatherURL);  // Crear petició
            yield return request.SendWebRequest(); // Llança petició

            if(request.error != null) {
                Debug.Log(request.error);
            } else {

                var weather = JsonConvert.DeserializeObject<WeatherResponse>(request.downloadHandler.text); // Descerializar json

                cloud_weather = weather.clouds.all;
                ChangeCloudiness(cloud_weather);

            }
            yield return new WaitForSeconds(10);

        }  
    }

    private void ChangeCloudiness(int cloudiness)
    {
        cloudiness_adapt = (float) (-0.0185*cloudiness + 2.965);
        cloudMat.SetFloat("_CloudsDensity", cloudiness_adapt);
    }
}
