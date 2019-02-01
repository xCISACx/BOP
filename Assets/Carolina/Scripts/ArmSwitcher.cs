using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSwitcher : MonoBehaviour
{
	public GameObject RegularArmGameObject;
	public GameObject SlidingArmGameObject;
	public GameObject JumpingArmGameObject1;
	public GameObject JumpingArmGameObject2;
	public GameObject JumpingArmGameObject3;
	public GameObject JumpingArmGameObject4;
	public ParticleSystem PrSS;
	public ParticleSystem PrS1;
	public ParticleSystem PrS2;
	public ParticleSystem PrS3;
	public ParticleSystem PrS4;

	void SetSlidingArm()
	{
		RegularArmGameObject.SetActive(false);
		SlidingArmGameObject.SetActive(true);
		PrSS.Stop();
	}

	void SetJumpingArm1()
	{
		RegularArmGameObject.SetActive(false);
		SlidingArmGameObject.SetActive(false);
		JumpingArmGameObject1.SetActive(true);
		PrS1.Stop();
	}
	
	void SetJumpingArm2()
	{
		RegularArmGameObject.SetActive(false);
		SlidingArmGameObject.SetActive(false);
		if (JumpingArmGameObject1.active)
		{
			JumpingArmGameObject1.SetActive(false);
		}
		JumpingArmGameObject2.SetActive(true);
		PrS2.Stop();
	}
	
	void SetJumpingArm3()
	{
		RegularArmGameObject.SetActive(false);
		if (JumpingArmGameObject1.active || JumpingArmGameObject2.active)
		{
			JumpingArmGameObject1.SetActive(false);
			JumpingArmGameObject2.SetActive(false);
		}
		JumpingArmGameObject3.SetActive(true);
		PrS3.Stop();
	}
	
	void SetJumpingArm4()
	{
		RegularArmGameObject.SetActive(false);
		SlidingArmGameObject.SetActive(false);
		if (JumpingArmGameObject1.active || JumpingArmGameObject2.active || JumpingArmGameObject3.active)
		{
			JumpingArmGameObject1.SetActive(false);
			JumpingArmGameObject2.SetActive(false);
			JumpingArmGameObject3.SetActive(false);
		}
		JumpingArmGameObject4.SetActive(true);
		PrS4.Stop();
	}
	
	void SetJumpingArm5()
	{
		RegularArmGameObject.SetActive(false);
		SlidingArmGameObject.SetActive(false);
		if (JumpingArmGameObject1.active || JumpingArmGameObject2.active || JumpingArmGameObject4.active)
		{
			JumpingArmGameObject1.SetActive(false);
			JumpingArmGameObject2.SetActive(false);
			JumpingArmGameObject4.SetActive(false);
		}
		JumpingArmGameObject3.SetActive(true);
		PrS3.Stop();
	}
	
	void SetJumpingArm6()
	{
		RegularArmGameObject.SetActive(false);
		SlidingArmGameObject.SetActive(false);
		if (JumpingArmGameObject1.active || JumpingArmGameObject3.active || JumpingArmGameObject4.active) 
		{
			JumpingArmGameObject1.SetActive(false);
			JumpingArmGameObject3.SetActive(false);
			JumpingArmGameObject4.SetActive(false);
		}
		JumpingArmGameObject2.SetActive(true);
		PrS2.Stop();
	}
	
}
