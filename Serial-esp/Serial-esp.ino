#define HAND_PIN 13
#define START_PIN 32
#define STOP_PIN 35
#define BACK_PIN 34

bool isWait, isStop, isBack;
const int th = 50;

void setup() {
  Serial.begin(115200);
  delay(100);
}

void loop() {
  int startPress = analogRead(START_PIN);
  int stopPress = analogRead(STOP_PIN);
  int backPress = analogRead(BACK_PIN);

  float handSound = analogRead(HAND_PIN);

  //Serial.println(handSound);

  /*
  Serial.print(startPress);
  Serial.print(",");
  Serial.print(stopPress);
  Serial.print(",");
  Serial.println(backPress);
  */
  
  if(handSound < 5.0)
    Serial.println("clap");
  else if(stopPress > th)   //ストップ用圧力センサを踏んだ
    Serial.println("stop");
  else if(backPress > th)   //バック用圧力センサを踏んだ
    Serial.println("back"); 
  else if(startPress > th)  //スタート地点に戻ってきたので待機
    Serial.println("wait");
  else
    Serial.println(""); 
  
  delay(1);
}
