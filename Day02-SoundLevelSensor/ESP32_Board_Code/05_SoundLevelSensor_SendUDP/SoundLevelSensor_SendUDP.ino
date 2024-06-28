#include <WiFi.h>
#include <WiFiUdp.h>

#define SoundSensorPin A0  //this pin read the analog voltage from the sound level meter
#define VREF  3.3  //voltage on AREF pin,default:operating voltage

// Replace with your network credentials
const char* ssid = "刘函宁的iPhone";
const char* password = "1234511111";

// IP address and port of the destination
const char* udpAddress = "172.20.10.4";
// Port should be 12315, 12325, 12335
const int udpPort = 12335;

// UDP instance
WiFiUDP udp;

void setup() {
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
  float voltageValue, dbValue;
  voltageValue = analogRead(SoundSensorPin) / 4096.0 * VREF;
  dbValue = voltageValue * 50.0;  //convert voltage to decibel value
  Serial.print(dbValue, 1);
  Serial.println(" dBA");
  delay(125);

  // Message to send
  String message = String(dbValue);

  // Send a UDP packet
  udp.beginPacket(udpAddress, udpPort);
  //udp.write((uint8_t*)message, strlen(message));
  udp.print(message);
  udp.endPacket();

  Serial.println("UDP packet sent");
}
