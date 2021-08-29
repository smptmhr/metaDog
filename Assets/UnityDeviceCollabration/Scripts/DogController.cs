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
	
	bool isWait = true;
	bool isStop = true;

	void Start(){
		dogController = GetComponent<CharacterController>();
		dogAnimator = GetComponent<Animator>();
	}

	void Update(){
		RotateDog();
		RunDog();
		DogMoveStart();
		DogBackHome();
		DogStop();
	}

	void DogBackHome(){
		if(readDataText.getText() == "back" && !isWait){
			Debug.Log("帰宅");
			gameObject.transform.position =　new Vector3(-7.14f,1.0f,8.9f);
			gameObject.transform.rotation= Quaternion.Euler(0.0f,-263.439f,0.0f);
			isWait=true;
		}
	}

	void DogMoveStart(){
		if(readDataText.getText() == "go" && isWait){
			Debug.Log("出発");
			//gameObject.transform.position =　new Vector3(3.0f,1.0f,10.0f);
			isWait=false;
			isStop=false;
		}
	}

	void DogStop(){
		if(readDataText.getText() == "stop"){
			dogAnimator.SetFloat("DogSpeed", 0);
			isStop = true;
		}
	}
	void RotateDog(){
		if (Input.GetKey(KeyCode.D)){
			transform.Rotate(Vector3.up * 0.5f);
			isDogStopAndTurn = false;
			if (dogSpeed < MINSPEED + ACCEL_DOG){
				dogAnimator.SetBool("isTurnRight", true);
				isDogStopAndTurn = true;
			}
		}
		else if (Input.GetKey(KeyCode.A)){
			transform.Rotate(Vector3.up * -0.5f);
			isDogStopAndTurn = false;
			if (dogSpeed < MINSPEED + ACCEL_DOG){
				dogAnimator.SetBool("isTurnLeft", true);
				isDogStopAndTurn = true;
			}
		}
		else{
			dogAnimator.SetBool("isTurnRight", false);
			dogAnimator.SetBool("isTurnLeft", false);
			isDogStopAndTurn = false;
		}
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
