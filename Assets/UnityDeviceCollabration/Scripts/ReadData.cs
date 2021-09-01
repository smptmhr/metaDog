using UnityEngine;
using UnityEngine.UI;

// データ受信の処理
public class ReadData : MonoBehaviour {
    // シリアルハンドラ
    // 外から指定するためpublic
    public SerialHandler serial;

    // 受信データ表示用Text
    Text text;

    // 初期化処理
    void Start () {
        // アタッチされているTextコンポーネントを受け取る
        text = GetComponent<Text> ();
        // データを受け取った時に呼び出される関数の設定
        serial.OnDataReceived += DataReceived;
    }

    // 受信時処理
    private void DataReceived (string data) {
        // Textに書き込む
        text.text = data;
    }

    public string getText(){
        Debug.Log(text.text);
        return text.text;
    }

}