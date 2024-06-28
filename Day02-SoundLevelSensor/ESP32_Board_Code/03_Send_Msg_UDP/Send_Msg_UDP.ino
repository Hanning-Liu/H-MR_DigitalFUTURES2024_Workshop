#include <WiFi.h>
#include <WiFiUdp.h>

// Replace with your network credentials
const char* ssid = "刘函宁的iPhone";
const char* password = "1234511111";

// IP address and port of the destination
const char* udpAddress = "172.20.10.4";
const int udpPort = 12355;

//Sound Sensor Settings
int sensorPin = A0;
int sensorValue = 0;

// UDP instance
WiFiUDP udp;

void setup() {
  //Initialize sound sensor
  pinMode(sensorPin, INPUT);
  
  // Initialize Serial for debugging
  Serial.begin(115200);

  // Connect to Wi-Fi
  WiFi.begin(ssid, password);
  Serial.print("Connecting to Wi-Fi...");
  
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  
  Serial.println("Connected to Wi-Fi");
}

void loop() {

  sensorValue=analogRead(sensorPin);   //connect mic sensor to Analog 0
  //Serial.printf("sensor value: %d\n", sensorValue);  //Print the read sensor value
  Serial.println(sensorValue,DEC);//print the sound value to serial
  delay(100);

  // Message to send
  const char* message = sensorValue;

  // Send a UDP packet
  udp.beginPacket(udpAddress, udpPort);
  udp.write((const uint8_t*)message, strlen(message));
  udp.endPacket();

  Serial.println("UDP packet sent");
}
