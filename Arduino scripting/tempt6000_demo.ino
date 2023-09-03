#define LightsensorPin A0

void setup() {
  pinMode(LightsensorPin, INPUT);
  Serial.begin(19200);
}

void loop() {
  delay(3000);
  float v = analogRead(LightsensorPin);
  float volts = analogRead(LightsensorPin) * 5.0 /1024.0;
  float amps = volts/ 10000.0;
  float microamps = amps * 1000000;
  float lux = microamps * 2.0;

  // Serial.print("Lux = ");
  // Serial.println(v);
}
