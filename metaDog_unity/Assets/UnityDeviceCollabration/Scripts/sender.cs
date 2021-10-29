using System.Collections;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

// シリアル通信用ハンドラ
public class sender : MonoBehaviour {
    // String(シリアルで受け取った文字列)を引数とするデリゲート型の定義
    public delegate void SerialDataReceivedEventHandler (string message);
    // データを受け取った時に呼び出されるデリゲート
    public event SerialDataReceivedEventHandler OnDataReceived;

    // シリアルポート(Windows)
    public string portName;

    // ボーレート
    public int baudRate = 115200;

    private SerialPort serialPort_;
    private Thread thread_;

    // 実行中かどうかのフラグ
    private bool isRunning_ = false;

    // 受け取ったデータ
    private string message_;
    // 新しいデータを受け取ったかのフラグ
    private bool isNewMessageReceived_ = false;

    // public ReadData readDataText;
    public DogController dog;
    public ReadData textPressure;
    private string sendtext="";

    // 起動時処理
    void Awake () {
        Open ();
    }

    public void SendText(){
        string tmp="";
        if(dog.status == 0 || dog.status == 3||dog.radioControllerIsWaiting)
            tmp="s";
        if(dog.status == 1 ||dog.status == 4)
            tmp="g";
        if(sendtext != tmp){
            sendtext = tmp;
            Write(sendtext);
        }
    }

    void Update () {
        // 新しいデータを受け取ったらデリゲートを介してメソッドを実行
        if (isNewMessageReceived_) {
            OnDataReceived (message_);
        }
        SendText();
    }

    // 終了時処理
    void OnDestroy () {
        Close ();
    }

    // ポート開放処理
    private void Open () {
        // ポートの設定と開放
        serialPort_ = new SerialPort (portName, baudRate, Parity.None, 8, StopBits.One);
        serialPort_.ReadTimeout = 500;
        serialPort_.Open ();

        // 実行中フラグをオン
        isRunning_ = true;

        // データの読み込みは別スレッドで動かす
        thread_ = new Thread (Read);
        thread_.Start ();
    }

    // ボート閉鎖処理
    private void Close () {
        isRunning_ = false;

        // スレッドを止める
        if (thread_ != null && thread_.IsAlive) {
            thread_.Join ();
        }

        // ポート閉鎖
        if (serialPort_ != null && serialPort_.IsOpen) {
            serialPort_.Close ();
            serialPort_.Dispose ();
        }
    }

    // データ受信処理
    private void Read () {

        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen) {
            // データを受け取っていたら値を保存する
            try {
                if (serialPort_.BytesToRead > 0) {
                    message_ = serialPort_.ReadLine ();
                    isNewMessageReceived_ = true;
                }
            } catch (System.Exception e) {
                Debug.LogWarning (e.Message);
            }
        }
    }

    // データ送信処理
    public void Write (string message) {
        try {
            serialPort_.Write (message);
        } catch (System.Exception e) {
            Debug.LogWarning (e.Message);
        }
    }
}