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
	public bool isDogStopFlag = false;

	Vector3 target = new Vector3(-7.34f,1.0f,8.06f);
	

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

	void DogMoveStart(){
		if(readDataText.getText() == "go" && isWait && !isBackRotatioin){
			isWait=false;
			isStop=false;
			isGo = true;
         	// transform.eulerAngles= new Vector3(0,310,0);
			// Debug.Log(transform.eulerAngles.y);
        }
		if(isGo){
			if(transform.position.x<-4.0f)
				isGo = false;
		}
	}
	

	void DogBackHome(){
		if(readDataText.getText() == "back" && !isWait){
			gameObject.transform.position =　new Vector3(-7.27f,1.0f,7.75f);
			gameObject.transform.localEulerAngles= new Vector3(0.0f,96.561f,0.0f);
			isWait=true;
			isGo = false;
		}
	}

	void DogMoveStop(){
		if(readDataText.getText() == "stop" && !isDogStopFlag){
			dogAnimator.SetFloat("DogSpeed", 0);
			isStop = true;
			isBackRotatioin = true;
			isDogStopFlag = true;
			dogAnimator.SetBool("isTurnLeft", true);
		}
		if(isBackRotatioin){
			RotateLookBack();
			isDogStopAndTurn  = true;
			Vector3 targetDirection = target - transform.position;
			targetDirection.Normalize();
			Vector3 tmp = transform.forward - targetDirection;
			if(tmp.magnitude < 0.01){
			 	isBackRotatioin = false;
				isDogStopAndTurn = false;
				isDogStopFlag = false;

			}else if(tmp.magnitude < 0.3){
				dogAnimator.SetBool("isTurnLeft", false);
			}
		}
	}

	
	void RotateLookBack(){
        Vector3 targetDirection = target - transform.position;
        float singleStep = ROTATESPEED * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
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


}
