using UnityEngine;
using UnityEngine.UI;

// データ送信の処理
public class SendData : MonoBehaviour {
    // シリアルハンドラ
    // 外から指定するためpublic
    public SerialHandler serial;

    // 送信データ用Text
    // 外から指定するためpublic
    public Text sendText;

    // テキストの送信
    public void SendText () {
        // シリアル通信で送信
        serial.Write (sendText.text);
    }

}