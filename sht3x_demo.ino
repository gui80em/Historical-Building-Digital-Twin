//includes the libraries 
#include <Wire.h>
#include <Arduino.h>
#include "Adafruit_SHT31.h"

//creates an instance of sht31 object
Adafruit_SHT31 sht31 = Adafruit_SHT31();

//setup the ports, 0x44 address to comunicate with sht31
void setup() {
  Serial.begin(9600);
  while (!Serial)
    delay(10); 
  if (! sht31.begin(0x44))        
  { 
    Serial.println("Couldn't find SHT31");
    while (1) delay(1);
  }
}

void loop() {
  //reads temperature and humidity
  float temperature = sht31.readTemperature();
  float humidity = sht31.readHumidity(); 
  //checks the temp. value readed
  if (! isnan(temperature))      
  {
    Serial.print("Temperature [*C] = "); 
    Serial.println(temperature);
  } 
  else 
  {
    Serial.println("Error while reading temperature");
  }
 //checks the hum. value readed
  if (! isnan(humidity))      
  { 
    Serial.print("RH % = "); 
    Serial.println(humidity);
  } 
  else 
  {
    Serial.println("Error while reading humidity.");
  }
  delay(2000);
  
}
