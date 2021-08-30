using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour{
	private float dogSpeed;
	private CharacterController dogController;
	private Animator dogAnimator;
	private bool isDogStopAndTurn;
	public ReadData readDataText;

	private const float MAXSPEED = 1.0f;
	private const float MINSPEED = 0.0f;
	private const float ACCEL_DOG = 0.005f;
	private const float ROTATESPEED = 1.0f;
	
	public bool isWait = true;
	public bool isGo = false;
	public bool isStop = true;
	public bool isBackRotatioin = false;

	Vector3 target = new Vector3(-7.27f,1.0f,7.75f);
	Vector3 defaltDirection = new Vector3(0.0f,0.0f,0.0f);

	void Start(){
		dogController = GetComponent<CharacterController>();
		dogAnimator = GetComponent<Animator>();
	}

	void Update(){
		RunDog();
		DogMoveStart();
		DogBackHome();
		DogMoveStop();
		dogAnimator.SetInteger("Idle", UnityEngine.Random.Range(1, 5));
	}

	

	void DogBackHome(){
		if(readDataText.getText() == "back" && !isWait){
			gameObject.transform.position =　new Vector3(-8.02f,1.0f,8.78f);
			gameObject.transform.localEulerAngles= new Vector3(0.0f,96.561f,0.0f);
			isWait=true;
			isGo = false;
		}
	}

	void DogMoveStart(){
		if(readDataText.getText() == "go" && isWait){
			isWait=false;
			isStop=false;
			isGo = true;
         	transform.eulerAngles= new Vector3(0,310,0);
			Debug.Log(transform.eulerAngles.y);
        }
		if(isGo){
			GoRotate();
			if(transform.position.x<-7)
				isGo = false;
		}
	}
	
	void GoRotate(){
        Vector3 targetDirection = target - transform.position;
        float singleStep = ROTATESPEED * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
	}

	void DogMoveStop(){
		if(readDataText.getText() == "stop"){
			dogAnimator.SetFloat("DogSpeed", 0);
			isStop = true;
			isBackRotatioin = true;
		}
		if(isBackRotatioin){
			GoRotate();
			dogAnimator.SetBool("isTurnRight", true);
			isDogStopAndTurn  = true;
			Vector3 targetDirection = target - transform.position;
			targetDirection.Normalize();
			Vector3 tmp = transform.forward - targetDirection;
			 if(tmp.magnitude<0.3){
			 	isBackRotatioin = false;
				dogAnimator.SetBool("isTurnRight", false);
			}
		}
	}
	void RotateRight(){
			transform.Rotate(Vector3.up * 0.5f);
			isDogStopAndTurn = false;
			if (dogSpeed < MINSPEED + ACCEL_DOG){
				dogAnimator.SetBool("isTurnRight", true);
				isDogStopAndTurn  = true;
			}
	}

	void RotateLeft(){
			transform.Rotate(Vector3.up * -0.5f);
			isDogStopAndTurn = false;
			if (dogSpeed < MINSPEED + ACCEL_DOG){
				dogAnimator.SetBool("isTurnLeft", true);
				isDogStopAndTurn = true;
			}
	}

	void RotateStop(){
		dogAnimator.SetBool("isTurnRight", false);
		dogAnimator.SetBool("isTurnLeft", false);
		isDogStopAndTurn = false;
	}

	void RunDog(){
		if (!isDogStopAndTurn && !isStop){
			if (dogSpeed < MAXSPEED - ACCEL_DOG){
				dogSpeed += ACCEL_DOG;
			}else if (dogSpeed > MINSPEED + ACCEL_DOG){
				dogSpeed -= ACCEL_DOG;
			}

			gameObject.transform.position += dogSpeed * 2 * transform.forward * Time.deltaTime;
			dogAnimator.SetFloat("DogSpeed", dogSpeed);
		}
	}
}
