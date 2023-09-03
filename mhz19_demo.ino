#include <Arduino.h>
#include <SoftwareSerial.h>
#include <Mhz19.h>

#define TX_PIN D3
#define RX_PIN D4

//sensor object
Mhz19 mhz19;
//serial for MHZ19
SoftwareSerial mySerial(RX_PIN, TX_PIN);

void setup() {
  Serial.begin(9600);
  mySerial.begin(9600);

  mhz19.begin(&mySerial);
  mhz19.setMeasuringRange(Mhz19MeasuringRange::Ppm_5000);
  mhz19.enableAutoBaseCalibration();
  
  Serial.println("Preheating...");  // Preheating, 3 minutes
  while (!mhz19.isReady()) {
    delay(50);
  }

  Serial.println("Ready...");
}

void loop() {
  auto carbonDioxide = mhz19.getCarbonDioxide();
  if (carbonDioxide >= 0) {
    Serial.println(String(carbonDioxide) + " ppm");
  }
  delay(2000);
}
