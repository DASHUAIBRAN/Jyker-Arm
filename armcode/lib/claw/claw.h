#ifndef CLAW_H_
#define CLAW_H_


#include <Arduino.h>
#include <ESP32Servo.h>
#include <Ticker.h>
 
void em_motor_init();
 
void em_motor_run(uint8_t *angle);
 
void em_motor_run_by_angle(uint8_t angle1, uint8_t angle2, uint8_t angle3);

#endif