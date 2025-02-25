#include <led.h>
// Time scaling factors for each component
#define TIME_FACTOR_HUE 60
#define TIME_FACTOR_SAT 100
#define TIME_FACTOR_VAL 100
// Define the array of leds
CRGB leds[NUM_LEDS];
void ledsetup() { 
    FastLED.addLeds<WS2812, DATA_PIN, RGB>(leds, NUM_LEDS);  // GRB ordering is typical
    FastLED.setBrightness(10);  // Set global brightness to 50%
    for (size_t i = 0; i < NUM_LEDS; i++)
    {
      /* code */
      leds[i] = CRGB::White;
    }
    FastLED.show();
}


void ledloop() { 
    // for (size_t i = 0; i < 255; i++)
    // {
    //   /* code */
    //   FastLED.setBrightness(i);
      
    //   FastLED.show();
    //   delay(100);
    // }
    // for (size_t i = 255; i > 0; i--)
    // {
    //   /* code */
    //   FastLED.setBrightness(i);
    //   FastLED.show();
    //   delay(100);
    // }
    
    
}

void setBrightness(int brightness)
{
  if(brightness==0)
  {
    FastLED.setBrightness(0);
    for (size_t i = 0; i < NUM_LEDS; i++)
    {
      /* code */
      leds[i] = CRGB::Black;
    }
    FastLED.show();
  }
  else
  {
    FastLED.setBrightness(brightness);
    for (size_t i = 0; i < NUM_LEDS; i++)
    {
      /* code */
      leds[i] = CRGB::White;
    }
    FastLED.show();
    
  }
}

