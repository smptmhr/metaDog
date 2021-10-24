#include <VL53L0X.h>
#include<Wire.h>
#define Hand_Pin 13

volatile bool isWait; //true:ラジコン待機状態
float hand_sound; //クラップ音を検出すると0になる

VL53L0X sensor;

void setup() {
  Serial.begin(115200);
  //Serial.begin(9600);

  Wire.begin(SDA, SCL);
  sensor.setTimeout(500);
  if(!sensor.init()){
    Serial.println("Failed to detect and initialize sensor");
    while(1){}
  }
  sensor.startContinuous();
  
  //VM-CLAP1は1回目のanalogReadで小さい値が出るのでダミー処理
  hand_sound = analogRead(Hand_Pin);
  isWait = true;
}

void loop() {

  /*
  //Serial.print(sensor.readRangeContinuousMillimeters());
  int d = sensor.readRangeContinuousMillimeters();
  //if (sensor.timeoutOccurred()) { Serial.print(" TIMEOUT"); }
  //Serial.print(d);
  //Serial.println();
  if(d < 50 && !isWait){
    Serial.println("back");
    isWait = true;
    delay(1000);
    Serial.println("stop");
    delay(500);
  }
  //delay(500);
*/
  
  hand_sound = analogRead(Hand_Pin);  
  Serial.println(hand_sound);
  if (isWait && hand_sound < 5.0) { //待機状態でクラップ
    Serial.println("go");
    isWait = false;
    //delay(3000);
  }
  delay(1);
}
