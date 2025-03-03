using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Camera cam;
    public Gamepad gamepad;
    public InputSystem_Actions actions;
    public float speed = 1; // how many units per sencond
    public float shiftMultiplier = 2; //the multiplier of using the shift key
    // Start is called once before the first execution of Update after the MonoBehaviour is created
	public float scrollMultiplier = 1;

	bool walking		=	false;
	Vector2 walkingN	= 	new Vector2(0, 0);
	bool UpDown			= 	false;
	float UpDownN		= 	0;
	bool IsShifting 	=	false;
	
	bool looking		=	false;
	Vector2 lookingN	= 	new Vector2(0, 0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//print(scrollMultiplier);
		if (walking)
		{
			Quaternion rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
			Vector3 newVec = new Vector3(walkingN.x, 0, walkingN.y);
			newVec = rot * newVec;
			transform.Translate(newVec * speed * Time.deltaTime * (IsShifting ? shiftMultiplier : 1));
		}
		if (looking)
		{
			//transform.rotation = Quaternion.Euler(transform.rotation.x , transform.rotation.y + lookingN.x*0.1f, transform.rotation.z);
			transform.eulerAngles +=  new Vector3(-lookingN.y*0.1f, lookingN.x*0.1f, 0);
			//transform.Rotate(-lookingN.y*0.1f, 0, 0);
			//transform.Rotate(0, lookingN.x*0.1f, 0);
		}
		if (UpDown)
		{
			transform.Translate(0, UpDownN, 0);
		}
    }

    void move(float x, float y, float z)
    {
        print("x = " + x + ", y = " + y + ", z = " + z);
        gameObject.transform.Translate(x, y, z);
	}
    //duplicates here
	public void isWalking(InputAction.CallbackContext context)
	{
		walking = context.performed;
		walkingN = context.ReadValue<Vector2>();
		print("is walking");
	}
	
	public void isUpDown(InputAction.CallbackContext context)
	{
		UpDown = context.performed;
		UpDownN = context.ReadValue<float>();
	}
	public void isShifting(InputAction.CallbackContext context)
	{
		IsShifting = context.performed;
		
	}
	
	public void isLooking(InputAction.CallbackContext context)
	{
		looking = context.performed;
		lookingN = context.ReadValue<Vector2>();
	}
	public void isScrolling(InputAction.CallbackContext context)
	{
		scrollMultiplier += context.ReadValue<float>();
	}

}
