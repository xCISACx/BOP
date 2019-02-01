using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBehaviour : MonoBehaviour {
	
	public bool canReduceAmmo = true;
	public float ammoSpendingCooldownTime;
    public GameManager gameManager;
    public SpawnInk spawnInk;
    
	[Header("Ammo")]
	public int bouncyAmmoPercent = 1;
	public int speedyAmmoPercent = 1;
	public int stickyAmmoPercent = 1;
	public int clearAmmoPercent = 1;
	
	public int currentBouncyAmmo = 20;
	public int currentSpeedyAmmo = 20;
	public int currentStickyAmmo = 20;
	public int currentClearAmmo = 20;
	
	public int initialBouncyAmmo = 5;
	public int initialSpeedyAmmo = 5;
	public int initialStickyAmmo = 5;
	public int initialClearAmmo = 5;
	
	public int maxBouncyAmmo = 20;
	public int maxSpeedyAmmo = 20;
	public int maxStickyAmmo = 20;
	public int maxClearAmmo = 20;

	// Use this for initialization
	void Start () {
		
		canReduceAmmo = true;
	    gameManager = FindObjectOfType<GameManager>();
	    spawnInk = FindObjectOfType<SpawnInk>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SpendAmmo(int ammo)
    {
        switch (spawnInk.ammoType)
        {
            case SpawnInk.AmmoType.Bouncy:
                if (canReduceAmmo)
                {
                    currentBouncyAmmo -= ammo;
                    if (currentBouncyAmmo != 0)
                    {
                        gameManager.ReduceAmmoBar();   
                    }
                    StartCoroutine(JustSpentAmmo());
                }
                break;
            case SpawnInk.AmmoType.Speedy:
                if (canReduceAmmo)
                {
                    currentSpeedyAmmo -= ammo;
                    if (currentSpeedyAmmo != 0)
                    {
                        gameManager.ReduceAmmoBar();   
                    }
                    StartCoroutine(JustSpentAmmo());
                }
                break;
            case SpawnInk.AmmoType.Sticky:
                if (canReduceAmmo)
                {
                    currentStickyAmmo -= ammo;
                    if (currentStickyAmmo != 0)
                    {
                        gameManager.ReduceAmmoBar();   
                    }
                    StartCoroutine(JustSpentAmmo());
                }
                break;
            case SpawnInk.AmmoType.Clear:
                if (canReduceAmmo)
                {
                    currentClearAmmo -= ammo;
                    if (currentClearAmmo != 0)
                    {
                        gameManager.ReduceAmmoBar();   
                    }
                    StartCoroutine(JustSpentAmmo());
                }
                break;
        }
    }
						
    IEnumerator JustSpentAmmo()
    {
        canReduceAmmo = false;
        yield return new WaitForSeconds(ammoSpendingCooldownTime);
        canReduceAmmo = true;
    }

    public void AmmoLimiter()
    {
        if (currentBouncyAmmo < 0)
            currentBouncyAmmo = 0;
        if (currentSpeedyAmmo < 0)
            currentSpeedyAmmo = 0;
        if (currentStickyAmmo < 0)
            currentStickyAmmo = 0;
        if (currentClearAmmo < 0)
            currentClearAmmo = 0;
        if (currentBouncyAmmo > maxBouncyAmmo)
            currentBouncyAmmo = maxBouncyAmmo;
        if (currentSpeedyAmmo > maxBouncyAmmo)
            currentSpeedyAmmo = maxBouncyAmmo;
        if (currentStickyAmmo > maxBouncyAmmo)
            currentStickyAmmo = maxBouncyAmmo;
        if (currentClearAmmo > maxBouncyAmmo)
            currentClearAmmo = maxBouncyAmmo;
    }

    public void NoAmmo()
    {
        if (currentBouncyAmmo == 0)
        {
            Debug.Log("No bouncy ammo left");
            return;
        }
        if (currentSpeedyAmmo == 0)
        {
            Debug.Log("No speedy ammo left");
            return;
        }
        if (currentStickyAmmo == 0)
        {
            Debug.Log("No sticky ammo left");
            return;
        }
        if (currentClearAmmo == 0)
        {
            Debug.Log("No clear ammo left");
            return;
            
        }
    }
}
