// Define the analog pin connected to A2 (IO34)
const int analogPin = A2;

// Define the reference voltage of the Arduino (typically 3.3V or 5V)
const float referenceVoltage = 3.3;

// Define the resistors in the voltage divider
const float resistor1 = 1000000.0; // 1M ohm
const float resistor2 = 1000000.0; // 1M ohm

// Full charge voltage threshold
const float fullChargeVoltage = 4.2;

// Define ADC resolution
const int adcResolution = 4095;

void setup() {
    // Initialize serial communication at 9600 baud rate
    Serial.begin(9600);
}

void loop() {
    // Read the analog value from the analog pin (0-4095)
    int analogValue = analogRead(analogPin);

    // Convert the analog value to a voltage (0-3.3V or 0-5V depending on the reference voltage)
    float measuredVoltage = analogValue * (referenceVoltage / adcResolution);

    // Calculate the actual battery voltage using the voltage divider formula
    // Vbat = measuredVoltage * (resistor1 + resistor2) / resistor2
    float batteryVoltage = measuredVoltage * (resistor1 + resistor2) / resistor2;

    // Print the measured voltage and battery voltage to the serial monitor
    Serial.print("Measured Voltage: ");
    Serial.print(measuredVoltage);
    Serial.print(" V, Battery Voltage: ");
    Serial.print(batteryVoltage);
    Serial.println(" V");

    // Check if the battery is fully charged
    if (batteryVoltage >= fullChargeVoltage) {
        Serial.println("Battery is fully charged.");
    } else {
        Serial.println("Battery is not fully charged.");
    }

    // Wait for a second before taking another reading
    delay(1000);
}
