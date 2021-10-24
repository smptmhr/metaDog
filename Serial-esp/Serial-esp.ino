#define HAND_PIN 13
#define START_PIN 32
#define STOP_PIN 35
#define BACK_PIN 34

bool isWait, isStop, isBack;
const int th = 1000;

void setup() {
  Serial.begin(115200);
  delay(100);

   isWait = true;
   isStop = false;
   isBack = false;
}

void loop() {
  int startPress = analogRead(START_PIN);
  int stopPress = analogRead(STOP_PIN);
  int backPress = analogRead(BACK_PIN);

  float handSound = analogRead(HAND_PIN);

  //Serial.println(handSound);

  //待機状態かつクラップが認識
  if(isWait && handSound < 5.0){
    Serial.println("clap");
    isWait = false;
  }
  //ストップ用圧力センサを踏んだ
  if(!isWait && !isStop && stopPress > th){
    Serial.println("stop");
    isStop = true;
    isBack = false;
  }
  //バック用圧力センサを踏んだ
  if(!isWait && !isBack && backPress > th){
    Serial.println("back");
    isBack = true;
    isStop = false;
  }
  //スタート地点に戻ってきたので待機
  if(!isWait && isBack && startPress > th){
    Serial.println("wait");
    isWait = true;
    isBack = false;
  }
  
  delay(10);
}
