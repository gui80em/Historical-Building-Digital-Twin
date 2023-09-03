using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft;

public class DemoText : MonoBehaviour
{
    public string urlThingSpeak = "https://api.thingspeak.com/channels/1870014/feeds.json"; // Reemplaza xxxxxx con el ID de tu canal
    public string apiKey = "5PJ18X1W8T9CSRWD"; // Reemplaza xxxxxxxxxxxxxxxx con tu API Key de ThingSpeak

    public void CrearArchivo()
    {
        StartCoroutine(ObtenerYGuardarDatos());
    }

    private IEnumerator ObtenerYGuardarDatos()
    {
        UnityWebRequest request = UnityWebRequest.Get(urlThingSpeak + "?api_key=" + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string datosJson = request.downloadHandler.text;
            var data = JsonConvert.DeserializeObject<SensorResponse>(request.downloadHandler.text);
            int numberResults = int.Parse(data.channel.last_entry_id);
            UnityWebRequest comp_request = UnityWebRequest.Get(urlThingSpeak + "?api_key=" + apiKey + "&num_results=" + numberResults);
            yield return comp_request.SendWebRequest();
            if (comp_request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(comp_request.error);
            }
            else
            {
                string datosJsonComp = comp_request.downloadHandler.text;
                GuardarDatosEnArchivo(datosJsonComp);

                Debug.Log("Dades guardades a l¡arxiu");
            }
        }
    }

    private void GuardarDatosEnArchivo(string datosJson)
    {
        string contenido = ObtenerContenidoFormateado(datosJson);

        string rutaArchivo = Application.dataPath + "/dades.csv";

        File.WriteAllText(rutaArchivo, contenido);
        Debug.Log(rutaArchivo);
    }

    private string ObtenerContenidoFormateado(string datosJson)
    {
        // Convertir los datos JSON a un objeto de tipo JObject
        var datosJObject = Newtonsoft.Json.Linq.JObject.Parse(datosJson);

        string contenido = "instant de lectura;llum;temperatura;humitat;co2\n";

        // Obtener la propiedad "feeds" que contiene los datos
        var feeds = datosJObject["feeds"];

        // Recorrer los datos y formatearlos
        foreach (var feed in feeds)
        {
            var instant = feed["created_at"].ToObject<string>();
            var llum = feed["field1"].ToObject<string>();
            var temp = feed["field2"].ToObject<string>();
            var hum = feed["field3"].ToObject<string>();
            var co2 = feed["field4"].ToObject<string>();

            contenido += instant + ";" + llum + ";" + temp + ";" + hum + ";" + co2   + "\n";
        }

        return contenido;
    }

    /*private int ObtenerNumeroEntradas()
    {
        UnityWebRequest request = UnityWebRequest.Get(urlThingSpeak + "?api_key=" + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string datosJson = request.downloadHandler.text;
            var data = JsonConvert.DeserializeObject<SensorResponse>(request.downloadHandler.text);
            string numberResults = data.channel.last_entry_id;

            return numberResults;
        }

    }*/

}
