using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

enum AnimationStates
{
	Idle,
	Move,
	Crouch,
	Attack,
	Death
}

enum AIState
{
	Patrol,
	Chase,
	Attack
}

public class AI : MonoBehaviour 
{
	public float speed =  0.5f;
	public Transform waypointsParent;
	public Transform groundDetection;
	public float proximityBeforeChangingPoints = 0.5f;
	public float proximityBeforeAttacking = 1f;
	public float distanceBeforeAbandoning = 3.5f; // SHOULD ALWAYS BE EQUAL TO OR GREATER THAN CIRCLE COLLIDER
	public float timeToWaitBeforeChangingPoints = 0.1f;
	public Vector2 velocity;

	private bool canShoot;
	public float cooldownTime;

	private bool playerInRange;

	public Transform firePoint;
	public LayerMask notToHit;
	public GameObject inkPrefab;
	public GameObject meh;

	private Animator animator;
	private Transform[] waypoints;
	private int targetWaypointIndex = 0;
	private Transform targetDestination;

	private Transform playerTransform;

	private Coroutine delayRoutine;

	private AIState aiState = AIState.Patrol;

    [SerializeField]
    private PolygonCollider2D[] colliders;
    private int currentColliderIndex = 0;

	private void Start()
	{
		animator = GetComponent<Animator>();

		// for what ever reason this is including the parent so we skip it.
		waypoints = waypointsParent.GetComponentsInChildren<Transform> ().Skip(1).ToArray();

		if (waypoints.Length == 0)
		{
			Debug.LogError("WHY NO WAYPOINTS SET!? REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
		}

		GetNextPoint ();
	}

	private void Update()
	{
		if (aiState == AIState.Patrol)
		{
			if (waypoints.Length == 0 || delayRoutine != null)
				return;

			if (Vector3.Distance(transform.position, targetDestination.position) <= proximityBeforeChangingPoints)
			{
				Debug.Log("switching");
				delayRoutine = StartCoroutine(DelayBeforeGetNextPoint());
				return;
			}
		}


		if (aiState == AIState.Chase)
		{
			// CONSTANTLY CHECK DELTA BECAUSE NO EFFORT REQUIRED
			float deltaX = (transform.position - playerTransform.position).x;
			transform.eulerAngles = new Vector3(0, (deltaX > 0 ? 180 : 0), 0);

			float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);
			Debug.Log("Distance from the player: " + distanceFromPlayer);

			// Check if we are close enough to attack.
			if (distanceFromPlayer <= proximityBeforeAttacking)
			{
					Debug.Log("Can attack");
					speed = 0;
					Debug.Log("Switching to Attacking");
					animator.SetInteger("State", (int)AnimationStates.Attack);
					StartCoroutine(Shoot());
			}

			else if (distanceFromPlayer > proximityBeforeAttacking)
			{
					// low effort coding ftw.
					targetDestination = playerTransform;
					// get closer
			}

			if (distanceFromPlayer >= distanceBeforeAbandoning)
			{
				aiState = AIState.Patrol;
				Debug.Log("Patrolling...");

				// REVERT THE SPEED
				speed /= 2;

				GetNextPoint();
				return;
			}

			if (aiState == AIState.Attack)
			{
				if (canShoot)
				{
					animator.SetInteger("State", (int)AnimationStates.Attack);
					Debug.Log("Attacking");
				}
			}
		}

		// Animation stuff
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn")) return;

		transform.position = Vector2.MoveTowards(transform.position, targetDestination.position, speed * Time.deltaTime);

		animator.SetInteger("State", (int)AnimationStates.Move);

	}

	public IEnumerator Shoot()
	{
		Debug.Log("Shooting");
		firePoint = transform.Find("FirePoint");
		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
		float deltaX = (transform.position - playerTransform.position).x;
		int directionMultiplier = (deltaX > 0) ? -1 : 1;
		GameObject ink = (GameObject)Instantiate(meh, (Vector2)firePointPosition, Quaternion.identity);
		ink.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * directionMultiplier, velocity.y);
		canShoot = false;
		yield return new WaitForSeconds(cooldownTime);
		canShoot = true;
	}

	private void GetNextPoint()
	{
		Transform previousDestination = targetDestination;
		targetDestination = waypoints [targetWaypointIndex];

		if (previousDestination != null)
		{
			float deltaX = (previousDestination.position - targetDestination.position).x;
			Debug.Log("DeltaX: " + deltaX);
			// +ve = forward, -ve = backwards

			transform.eulerAngles = new Vector3 (0, (deltaX > 0 ? 180 : 0), 0);
		}

		// Add one on to the current waypoint.
		// Use the modulo operator to get the remainder (it'll return zero when x + 1 % l == 0)
		targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
	}

	private IEnumerator DelayBeforeGetNextPoint()
	{
		animator.SetInteger("State", (int)AnimationStates.Idle);
		yield return new WaitForSeconds (timeToWaitBeforeChangingPoints);
		GetNextPoint ();

		// unity apparently doesn't set the coroutine to null.
		// what is it even doing?
		delayRoutine = null;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("OnTriggerEnter2D(" + other + ")");

        if (other.CompareTag("Player"))
        {
            if (delayRoutine != null)
            {
                StopCoroutine(delayRoutine);
                delayRoutine = null;
            }


            // Switch to chasing!
			aiState = AIState.Chase;


            // DOUBLE THE SPEED
			//TOFIX: If the Circle Collider is set to 3, the speed won't double.
            speed *= 2;
			Debug.Log("Gotta go fast");

            // Update player transform - this is pointless and could be cached :^).
            playerTransform = other.gameObject.transform;
            // chase player
            // shoot stuff
       }
    }

    private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag ("Player"))
		{
			// Switch to patrolling!
			aiState = AIState.Patrol;

			// REVERT THE SPEED.
			speed /= 2;

			GetNextPoint ();
		}
	}

    public void SetColliderForSprite( int spriteNum )
    {
        //Debug.Log("switching from " + currentColliderIndex + " to " + spriteNum);
        colliders[currentColliderIndex].enabled = false;
        currentColliderIndex = spriteNum;
        colliders[currentColliderIndex].enabled = true;
    }
}