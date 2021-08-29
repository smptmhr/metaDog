using System.Collections;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

// シリアル通信用ハンドラ
public class SerialHandler : MonoBehaviour {
    // String(シリアルで受け取った文字列)を引数とするデリゲート型の定義
    public delegate void SerialDataReceivedEventHandler (string message);
    // データを受け取った時に呼び出されるデリゲート
    public event SerialDataReceivedEventHandler OnDataReceived;

    // シリアルポート(Windows)
    public string portName = "COM8";
    // シリアルポート(Mac)
    //public string portName = "/dev/cu.ESP32test-ESP32SPP";
    // シリアルポート(BlueTooth)
    //public string portName = "/dev/cu.ESP32test-ESP32SPP";

    // ボーレート
    public int baudRate = 9600;

    private SerialPort serialPort_;
    private Thread thread_;

    // 実行中かどうかのフラグ
    private bool isRunning_ = false;

    // 受け取ったデータ
    private string message_;
    // 新しいデータを受け取ったかのフラグ
    private bool isNewMessageReceived_ = false;

    // 起動時処理
    void Awake () {
        Open ();
    }

    void Update () {
        // 新しいデータを受け取ったらデリゲートを介してメソッドを実行
        if (isNewMessageReceived_) {
            OnDataReceived (message_);
        }
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