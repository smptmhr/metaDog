using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour{
	private float dogSpeed;
	private CharacterController dogController;
	private Animator dogAnimator;

	public int status = 0;
	//0 : Dog&radio controler are waiting at (A)
	//1 : Dog is running foaward
	//2 : Dog is out of display & radio controller is moving
	//3 : radio controller is stopping in center of touch display (B)
	//4 : radio controller is moving toward (C) 
	//5 : radio controller is moving orbitally
	//6 : radio controller is going back to waiting space
	//7 : radio controller is moving toward waiting area(A)
	public ReadData textPressure;
	public ReadData textStroke;

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
		Debug.Log(status);
		StatusController();
		SwitchStatusByReadText();
		//DogMoveStop();
		dogAnimator.SetInteger("Idle", UnityEngine.Random.Range(1, 5));
	}

	void StatusController(){
		switch (status){
			case 0: // Dog&radio controler are waiting at (A)
				break;
			case 1: // Dog is running foaward
				RunDog();
				break;
			case 6: // radio controller is going back to waiting space
				DogBackHome();
				break;
			case 7: // radio controller is moving toward waiting area(A)
				RunDog();
				break;
			case 8:
				DogMoveStop();
				break;
			default:
				break;
		}
	}

	void SwitchStatusByReadText(){
		Debug.Log(textPressure.getText());
		if(status ==0 && textPressure.getText() == "clap"){
			status = 1;
        }
		if(status ==2 && textPressure.getText() == "stop"){
			status = 3;
		}
		if(status ==3 && textStroke.getText() == "go"){
				status = 4; 
		}
		if(status == 4 && textPressure.getText() == "back"){
			status = 6;
		}
		if(status == 9 && textPressure.getText() == "wait"){
			status = 0;
		}
	}

	void RunDog(){
		if (dogSpeed < MAXSPEED - ACCEL_DOG){
			dogSpeed += ACCEL_DOG;
		}else if (dogSpeed > MINSPEED + ACCEL_DOG){
			dogSpeed -= ACCEL_DOG;
		}

		gameObject.transform.position += dogSpeed * 2 * transform.forward * Time.deltaTime;
		dogAnimator.SetFloat("DogSpeed", dogSpeed);

		if(status == 1 && transform.position.x<-8.0f)
			status=2;
		if(status == 7 && transform.position.x>-2.0f)
			status = 8;
	}
	
	void DogBackHome(){
			gameObject.transform.position =　new Vector3(-7.27f,1.0f,7.75f);
			gameObject.transform.localEulerAngles= new Vector3(0.0f,96.561f,0.0f);
			
			status = 7;
	}

	void DogMoveStop(){
		if(!isDogStopFlag){
			dogAnimator.SetFloat("DogSpeed", 0);
			isStop = true;
			isBackRotatioin = true;
			isDogStopFlag = true;
			dogAnimator.SetBool("isTurnLeft", true);
		}
		if(isBackRotatioin){
			RotateLookBack();
			Vector3 targetDirection = target - transform.position;
			targetDirection.Normalize();
			Vector3 tmp = transform.forward - targetDirection;
			if(tmp.magnitude < 0.01){
			 	isBackRotatioin = false;
				isDogStopFlag = false;

			}else if(tmp.magnitude < 0.3){
				dogAnimator.SetBool("isTurnLeft", false);
				status = 9;
			}
		}
	}

	
	void RotateLookBack(){
        Vector3 targetDirection = target - transform.position;
        float singleStep = ROTATESPEED * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
	}
}
