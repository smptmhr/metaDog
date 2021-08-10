using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
	private float dogSpeed;
	private CharacterController dogController;
	private Animator dogAnimator;
	private bool isDogStopAndTurn;

	private const float MAXSPEED = 1.0f;
	private const float MINSPEED = 0.0f;
	private const float ACCEL_DOG = 0.005f;


	void Start()
	{
		dogController = GetComponent<CharacterController>();
		dogAnimator = GetComponent<Animator>();
	}

	void Update()
	{
		RotateDog();
		RunDog();
	}

	void RotateDog()
	{
		if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(Vector3.up * 0.5f);
			isDogStopAndTurn = false;
			if (dogSpeed < MINSPEED + ACCEL_DOG)
			{
				dogAnimator.SetBool("isTurnRight", true);
				isDogStopAndTurn = true;
			}
		}
		else if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.up * -0.5f);
			isDogStopAndTurn = false;
			if (dogSpeed < MINSPEED + ACCEL_DOG)
			{
				dogAnimator.SetBool("isTurnLeft", true);
				isDogStopAndTurn = true;
			}
		}
		else
		{
			dogAnimator.SetBool("isTurnRight", false);
			dogAnimator.SetBool("isTurnLeft", false);
			isDogStopAndTurn = false;
		}
	}

	void RunDog()
	{
		if (!isDogStopAndTurn)
		{
			if (Input.GetKey(KeyCode.W) && dogSpeed < MAXSPEED - ACCEL_DOG)
			{
				dogSpeed += ACCEL_DOG;
			}
			else if (dogSpeed > MINSPEED + ACCEL_DOG)
			{
				dogSpeed -= ACCEL_DOG;
			}

			gameObject.transform.position += dogSpeed * 2 * transform.forward * Time.deltaTime;

			dogAnimator.SetFloat("DogSpeed", dogSpeed);
		}
	}
}
