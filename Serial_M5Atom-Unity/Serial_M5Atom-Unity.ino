#include <M5Atom.h>
#include "BluetoothSerial.h"
#include "LedArray.h"

#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif

BluetoothSerial SerialBT;

float accX = 0.0F;
float accY = 0.0F;
float accZ = 0.0F;

float oldAccX = 0.0F;

float thH = 0.5;
float thL = -0.5;

bool highPos = false;
bool lowPos = false;
bool isStart = false;
bool isGood = false;

int start_t = 0;
int clapCount = 0;

float numCheck = 0.0F;


void led_circle(){
  for(int i=0; i<sizeof(arrayCircle_Red)/sizeof(int); i++){
    M5.dis.drawpix(arrayCircle_Red[i],COLOR_RED);
  }
  for(int i=0; i<sizeof(arrayCircle_Black)/sizeof(int); i++){
    M5.dis.drawpix(arrayCircle_Black[i],COLOR_BLACK);
  }
}

void led_cross(){
  for(int i=0; i<sizeof(arrayCross_Blue)/sizeof(int); i++){
    M5.dis.drawpix(arrayCross_Blue[i],COLOR_BLUE);
  }
  for(int i=0; i<sizeof(arrayCross_Black)/sizeof(int); i++){
    M5.dis.drawpix(arrayCross_Black[i],COLOR_BLACK);
  }
}

/*
void led_light(int arrayData1[], int arrayData2[], int color){
  for(int i=0; i<sizeof(arrayData1)/sizeof(int); i++){
    M5.dis.drawpix(arrayData1[i],color);
  }
  for(int i=0; i<sizeof(arrayData2)/sizeof(int); i++){
    M5.dis.drawpix(arrayData2[i],COLOR_BLACK);
  }
}*/

void led_reset(){
  for(int i=0; i<25; i++){
    M5.dis.drawpix(i,COLOR_WHITE);
  }
}


void setup() {
  SerialBT.begin("M5Atom Matrix");
  M5.begin(true, false, true);
  M5.IMU.Init();
  led_reset();
}

void loop() {
  M5.IMU.getAccelData(&accX, &accY, &accZ);

  int numStroke = 0;
  int time_ms = millis();

  if(oldAccX < thH && accX >= thH){
    highPos = true;
  }
  if(oldAccX > thL && accX <= thL){
    lowPos = true;
  }

  if(highPos && lowPos){
    numStroke = 1;
    highPos = false;
    lowPos = false;

    if(!isStart){
      isStart = true;
      start_t = time_ms;
    }
    else{
      clapCount++;
    }
  }

  if(isStart){
    if(time_ms - start_t > 2000){
      if(clapCount <=2 && isGood){
        SerialBT.println("go");
        isGood = false;
        delay(2000);
        led_reset();
        SerialBT.println("");
      }
      else if(clapCount > 2&&clapCount < 6){
        SerialBT.println("good");
        led_circle();
        //led_light(arrayCircle_Red, arrayCircle_Black, COLOR_RED);
        isGood = true;
        delay(1000);
      }
      else if(clapCount >= 6){
        SerialBT.println("go");
        led_cross();
        //led_light(arrayCross_Blue, arrayCross_Black, COLOR_BLUE);
        isGood = false;
        delay(2000);
        led_reset();
        SerialBT.println("");
      }
      clapCount = 0;
      isStart = false;
    }
  }
  else if(isGood){
    if(time_ms -start_t > 4000){
      SerialBT.println("go");
      isGood = false;
      delay(1000);
      led_reset();
      SerialBT.println("");
    }
  }

  //SerialBT.print(accX);

  oldAccX = accX;
  
  delay(10);
}
