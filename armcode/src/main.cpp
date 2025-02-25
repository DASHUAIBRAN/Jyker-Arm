#include <Arduino.h>
#include <led.h>
#include <claw.h>
#include "config.h"
char buffer[100];
int bufferIndex = 0;
bool receivingData = false;

char recFormC[20];
int recCount = 0;






void setup() {
     Serial.begin(115200);
     //em_motor_init();
     ledsetup();
     while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
}



void loop() {

        
}




