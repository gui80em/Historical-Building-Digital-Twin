using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft;
using System;
using UnityEngine.UI;
using TMPro;

public class ScreenData : MonoBehaviour
{
    // Referencia al objeto "UI Text"
    public TMP_Text textUI;
    private long numeroDelChannel=1870014;
    private string readAPIKey = "5PJ18X1W8T9CSRWD";
    private int num_entries = 1;
    private string inputText = "Loading...";
    private string nouText;
    void Start()
    {
        textUI = gameObject.GetComponent<TMP_Text>();
        textUI.text = inputText;
        StartCoroutine(ReadData());
    }
    IEnumerator ReadData(){
        while(true){
            string url = "https://api.thingspeak.com/channels/"+numeroDelChannel+"/feeds.json?api_key="+readAPIKey+"&results="+num_entries;

            UnityWebRequest request = UnityWebRequest.Get(url);  // Crear petición
            yield return request.SendWebRequest(); 
            if(request.error != null) {
                Debug.Log(request.error+'a');
            } else {
                //Debug.Log(request.downloadHandler.text);
                var data = JsonConvert.DeserializeObject<SensorResponse>(request.downloadHandler.text); 
                /*Debug.Log(data.channel.field1+": "+data.feeds[0].field1);
                Debug.Log(data.channel.field2+": "+data.feeds[0].field2);
                Debug.Log(data.channel.field3+": "+data.feeds[0].field3);
                Debug.Log(data.channel.field4+": "+data.feeds[0].field4);*/
                ActualitzarText(data);

            }
            // Actualizar el texto del objeto "UI Text" cada minuto
            //InvokeRepeating("ActualitzarText", 0f, 60f);
            yield return new WaitForSeconds(10);
        }
    }
    void ActualitzarText(SensorResponse data)
    {
        // Generar un texto aleatorio
        nouText = data.channel.field1+": "+data.feeds[0].field1+"\n"+
                  data.channel.field2+": "+data.feeds[0].field2+"\n"+
                  data.channel.field3+": "+data.feeds[0].field3+"\n"+
                  data.channel.field4+": "+data.feeds[0].field4;

        // Actualizar el texto del objeto "UI Text"
        textUI.text = nouText;
    }
}