#include <ThingSpeak.h> //nubol
#include <ESP8266WiFi.h>//wifi

// per accedir al servidor del nubol de thingSpeak
unsigned long numeroDelChannel=1870014 ;
const char * writeAPIKey= "5PJ18X1W8T9CSRWD";
const char * host="api.thingspeak.com";
const int httPort=80;
const char * ip;

// definim la nostre wifi
const char* ssid = "QUATTRE_FIBRA-4411";       
const char* password = "4s57%gf7";   
WiFiServer server(80); 
void setup() {
  Serial.begin(19200);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) { 
    delay(500);
  }
  ip=WiFi.localIP().toString().c_str();
  delay(2000);
}

void loop() {
  float lux  = float(random(1,1000));
  float temp = float(random(5,40));
  float hum  = float(random(15,90));
  float co2 = float(random(100,5000));
  WiFiClient client;
  if(!client.connect(host,httPort)){
    return; 
  }
  String url="/update?key=";
  url+=writeAPIKey;
  url+="&field1=";
  url+=String(lux);
  url+="&field2=";
  url+=String(temp);
  url+="&field3=";
  url+=String(hum);
  url+="&field4=";
  url+=String(co2);
  url+="\r\n";
  //peticio al servidor thingSpeak
  client.print(String("GET ")+url+" HTTP/1.1\r\n"+
              "Host: "+host+"\r\n"+
              "Connection: close\r\n\r\n");
  client.stop();
  delay(15);
  //pugem al nubol
  ThingSpeak.setField(1,lux);
  ThingSpeak.setField(2,temp);
  ThingSpeak.setField(3,hum);
  ThingSpeak.setField(4,co2);
  ThingSpeak.writeFields(numeroDelChannel,writeAPIKey);
  //delay entre mesures 20s, el nubol s'actalitza cada 15s
  delay(20000); 
}

