#ifndef LEDWS2812_H_
#define LEDWS2812_H_

#include <FastLED.h>
// How many leds in your strip?
#define NUM_LEDS 12

// For led chips like WS2812, which have a data line, ground, and power, you just
// need to define DATA_PIN.  For led chipsets that are SPI based (four wires - data, clock,
// ground, and power), like the LPD8806 define both DATA_PIN and CLOCK_PIN
// Clock pin only needed for SPI based chipsets when not using hardware SPI
#define DATA_PIN 2
#define CLOCK_PIN 13

void ledsetup();
void ledloop();
void setBrightness(int brightness);
#endif