#include <driver/adc.h>

// Define the ADC pin where the battery is connected
#define BATTERY_ADC_PIN ADC1_CHANNEL_0  // GPIO36 for example

// Define the reference voltage and the maximum ADC value
#define REF_VOLTAGE 3.3
#define ADC_MAX 4095

// Voltage divider resistors values
#define R1 10000  // 10k ohms
#define R2 10000  // 10k ohms

float readBatteryVoltage() {
  // Read the ADC value
  int adcValue = adc1_get_raw(BATTERY_ADC_PIN);

  // Convert ADC value to voltage
  float voltage = (adcValue * REF_VOLTAGE) / ADC_MAX;

  // Adjust for voltage divider
  voltage = voltage * (R1 + R2) / R2;

  return voltage;
}

float getBatteryPercentage(float voltage) {
  // Assuming a LiPo battery with a voltage range of 3.0V (0%) to 4.2V (100%)
  float percentage = (voltage - 3.0) / (4.2 - 3.0) * 100.0;

  // Ensure percentage is within 0-100%
  percentage = max(0.0, min(100.0, percentage));

  return percentage;
}

void setup() {
  Serial.begin(115200);

  // Initialize ADC
  adc1_config_width(ADC_WIDTH_BIT_12);
  adc1_config_channel_atten(BATTERY_ADC_PIN, ADC_ATTEN_DB_11);
}

void loop() {
  float voltage = readBatteryVoltage();
  float percentage = getBatteryPercentage(voltage);

  Serial.print("Battery Voltage: ");
  Serial.print(voltage);
  Serial.println(" V");

  Serial.print("Battery Percentage: ");
  Serial.print(percentage);
  Serial.println(" %");

  delay(5000); // Read every 5 seconds
}
