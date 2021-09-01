using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour{
    // Start is called before the first frame update
    public GameObject dog;
    DogController dogscript;
    float speed = 0.3f;
    float step;
    bool downFlg = false;

    Vector3 GoTarget = new Vector3(-8.05f,2.08f,8.5f);
    Vector3 BackTarget = new Vector3(-7.96f,2.77f,8.5f);
    
    void Start(){
        dogscript = dog.GetComponent<DogController>();
        step =  speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update(){
        if(dogscript.isGo && dog.transform.position.x<-3){
            downFlg=true;
        }
        if(downFlg){
            transform.position = Vector3.MoveTowards(transform.position, GoTarget, step);
        }
        if(dogscript.isWait){
            downFlg = false;
            transform.position = Vector3.MoveTowards(transform.position, BackTarget, step*0.7f);
        }
    }
}
