using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
	
	public float BouncyAmmoPercent = 0;
	public float SpeedyAmmoPercent = 0;
	public float StickyAmmoPercent = 0;
	public float CurrentBouncyAmmo = 5;
	public int CurrentSpeedyAmmo = 5;
	public int CurrentStickyAmmo = 5;
	public int InitialBouncyAmmo = 5;
	public int InitialSpeedyAmmo = 5;
	public int InitialStickyAmmo = 5;
	public int MaxBouncyAmmo = 5;
	public int MaxSpeedyAmmo = 5;
	public int MaxStickyAmmo = 5;
	public Texture2D AmmoBottleBackground;
	public Texture2D BouncyAmmoBarTexture;
	public float BouncyAmmoBarLength;
	public bool playerHasUnlockedBouncy;
	public bool playerHasUnlockedSpeedy;
	public bool playerHasUnlockedSticky;
	public TextMeshProUGUI AmmoAmountText;
	public GunInteraction GI;
	public bool UIisActive;
	
	//public GUIStyle guiStyle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (CurrentBouncyAmmo <= MaxBouncyAmmo)
		{
			BouncyAmmoPercent = CurrentBouncyAmmo/MaxBouncyAmmo;
			BouncyAmmoBarLength = (BouncyAmmoPercent * 50);
			AmmoAmountText.text = "" + CurrentBouncyAmmo + " / " + MaxBouncyAmmo;
		}
		
		if (CurrentBouncyAmmo > MaxBouncyAmmo)
		{
			CurrentBouncyAmmo = MaxBouncyAmmo;
		}

		if (CurrentBouncyAmmo < 0)
		{
			CurrentBouncyAmmo = 0;
		}
		
		
		

	}
	
	private void OnGUI()
	{
		if (GI.UI.activeSelf)
		{
			//guiStyle.fontSize = 50;
			GUI.DrawTexture(new Rect(450,-10,80,110), AmmoBottleBackground);
			GUI.DrawTexture(new Rect(475,75,30,-BouncyAmmoBarLength), BouncyAmmoBarTexture);
			//GUI.Label(new Rect(550,20,100000,100000), "" + CurrentBouncyAmmo + " / " + MaxBouncyAmmo, guiStyle);
		}
	}
}
