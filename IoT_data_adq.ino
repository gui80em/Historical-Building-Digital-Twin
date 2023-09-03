#include <Arduino.h>
#include <SoftwareSerial.h>
#include <Mhz19.h>
#include <Wire.h>
#include <Arduino.h>
#include "Adafruit_SHT31.h"
#include <ThingSpeak.h> //nubol
#include <ESP8266WiFi.h>//wifi

#define LightsensorPin A0
#define TX_PIN D3
#define RX_PIN D4

Adafruit_SHT31 sht31 = Adafruit_SHT31();
Mhz19 mhz19;
//serial for MHZ19
SoftwareSerial mySerial(RX_PIN, TX_PIN);

unsigned long numeroDelChannel=1870014 ;
const char * writeAPIKey= "5PJ18X1W8T9CSRWD";
const char * host="api.thingspeak.com";
const int httPort=80;
const char * ip;

const char* ssid = "QUATTRE_FIBRA-4411";       
const char* password = "4s57%gf7"; 
WiFiServer server(80); 
float t= 19.81;
void setup() {
  pinMode(LightsensorPin, INPUT);
  Serial.begin(9600);
  mySerial.begin(9600);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) { 
    delay(500);
  }
  ip=WiFi.localIP().toString().c_str();
  delay(2000);

  mhz19.begin(&mySerial);
  mhz19.setMeasuringRange(Mhz19MeasuringRange::Ppm_5000);
  mhz19.enableAutoBaseCalibration();


  if (! sht31.begin(0x44))        
  { 
    Serial.println("Couldn't find SHT31");
    while (1) delay(1);
  }


  Serial.println("Preheating...");  
  while (!mhz19.isReady()) {
    delay(50);
  }

  Serial.println("Ready...");
}


void loop() {
  float v = analogRead(LightsensorPin);
  float volts = analogRead(LightsensorPin) * 5.0 /1024.0;
  float amps = volts/ 10000.0;
  float microamps = amps * 1000000;
  float lux = microamps * 2.0;

  Serial.print("Lux = ");
  Serial.println(v);

  float temperature = sht31.readTemperature();
  float humidity = sht31.readHumidity(); 
  Serial.print("Temperature [*C] = "); 
  Serial.println(temperature);
  Serial.print("RH % = "); 
  Serial.println(humidity);

  auto carbonDioxide = mhz19.getCarbonDioxide();
  Serial.println(String(carbonDioxide) + " ppm");
  
  WiFiClient client;
  if(!client.connect(host,httPort)){
    return; 
  }
  
  String url="/update?key=";
  url+=writeAPIKey;
  url+="&field1=";
  url+=String(lux);
  url+="&field2=";
  url+=String(t);
  url+="&field3=";
  url+=String(66.4);
  url+="&field4=";
  url+=String(carbonDioxide);
  url+="\r\n";
  t=t+0.01;
  client.print(String("GET ")+url+" HTTP/1.1\r\n"+
              "Host: "+host+"\r\n"+
              "Connection: close\r\n\r\n");
  client.stop(); delay(10000);
  ThingSpeak.setField(1,lux);
  ThingSpeak.setField(2,temperature);
  ThingSpeak.setField(3,humidity);
  ThingSpeak.setField(4,carbonDioxide);
  ThingSpeak.writeFields(numeroDelChannel,writeAPIKey);

  Serial.println("Escriptura completada");
  delay(10000);
}
