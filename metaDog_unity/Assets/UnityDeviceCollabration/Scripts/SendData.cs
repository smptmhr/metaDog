using UnityEngine;
using UnityEngine.UI;

// データ送信の処理
public class SendData : MonoBehaviour {
    // シリアルハンドラ
    // 外から指定するためpublic
    public SerialHandler serial;
    public ReadData readDataText;

    // 外から指定するためpublic
    private string sendText;

    void Start(){
    }

    void Update(){
            sendText=readDataText.getText();
            if(sendText != "back"  || sendText !="")
                serial.Write (sendText);
            Debug.Log(sendText);
    }

}