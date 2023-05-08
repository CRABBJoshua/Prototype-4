using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * TODO LIST
 * Add a timer
 * Work on fine tuning the Movement
 */

public class TopDownCharacterController : MonoBehaviour
{
	//WeaponTimer
	public float WeaponTimer;

	public WeaponDelay wd;

	//ShotCheck
	private bool HasShot = false;

	//RigidBody
	private Rigidbody2D rb;

	//Reference to attached animator
	private Animator animator;

    //The direction the player is moving in
    private Vector2 playerDirection;

    //The speed at which they're moving
    private float playerSpeed = 1f;

	Camera cam;

	[Header("Movement parameters")]

    //The maximum speed the player can move
    [SerializeField] private float playerMaxSpeed = 100f;
    [SerializeField] private GameObject Projectile;


	/// <summary>
	/// When the script first initialises
	/// </summary>
	private void Awake()
    {
        //Get the attached components so we can use them later
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		cam = Camera.main;
    }

	/// <summary>
	/// When a fixed update cycle is called
	/// </summary>
	private void FixedUpdate()
    {
        //Set the velocity to the direction they're moving in, multiplied
        //by the speed they're moving
        rb.velocity = playerDirection * (playerSpeed * playerMaxSpeed) * Time.fixedDeltaTime;
		wd.SetDelay(WeaponTimer);
    }

    public void OnPlayerInputShoot(InputAction.CallbackContext context)
    {
        //Not performed? Don't run any other code
        if (!context.performed)
            return;

		//Otherwise:
		SpawnProjectile();
		//SpawnPlayer();
        Debug.Log($"Shoot! {Time.time}", gameObject);
    }

    /// <summary>
    /// Called when the player wants to move in a certain direction
    /// </summary>
    /// <param name="context"></param>
    public void OnPlayerInputMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            //Was the action just cancelled (released)? If so, set
            //speed to 0
            playerSpeed = 0f;

            //Update the animator too, and return
            animator.SetFloat("Speed", 0);
            return;
        }

        //Otherwise, if the context wasn't performed, don't run
        //the code below
        if (!context.performed)
            return;

        //Read the direction that the player wants to move, from the
        //keys that have been pressed
        Vector2 direction = context.ReadValue<Vector2>();

        //Set the player's direction to whatever it is
        playerDirection = direction;

        //Set animator parameters
        animator.SetFloat("Horizontal", playerDirection.x);
        animator.SetFloat("Vertical", playerDirection.y);
        animator.SetFloat("Speed", playerDirection.magnitude);

        //And set the speed to 1, so they move!
        playerSpeed = 1f;
    }

	public void SpawnProjectile()
	{
		if(Mouse.current.leftButton.wasPressedThisFrame)
		{
			if(!HasShot)
			{
				//Get mouse position
				Vector2 mousePosition = Mouse.current.position.ReadValue();

				//Get mouse position in the world
				Vector3 worldMousePos = cam.ScreenToWorldPoint(mousePosition);
				worldMousePos.z = transform.position.z;

				//Get a vector looking towards that
				Vector3 bulletDir = (worldMousePos - transform.position).normalized;

				//Spawn the bullet
				GameObject bullet = Instantiate(Projectile, transform.position, Quaternion.identity);
				bullet.GetComponent<Projectile>().Force = bulletDir * 1000;

				//Set the rotation to look along bulletDir
				bullet.transform.rotation = Quaternion.FromToRotation(Vector2.up, bulletDir);
				HasShot = true;
				StartCoroutine(WeaponCoolDown());
			}
			//Start Weapon Call Down
			else if(HasShot)
			{
				Debug.Log(HasShot);
				
			}
			
		}
	}

	public void SpawnPlayer()
	{
		if (Mouse.current.leftButton.wasPressedThisFrame)
		{
			Vector3 MousePos = Mouse.current.position.ReadValue();
			MousePos.z = Camera.main.nearClipPlane;
			Vector3 WorldPos = Camera.main.ScreenToWorldPoint(MousePos);
			Vector2 WorldPos2D = new Vector2(WorldPos.x, WorldPos.y);


			Instantiate(this, WorldPos2D, Quaternion.identity);
		}
	}

	IEnumerator WeaponCoolDown()
	{
		yield return new WaitForSeconds(WeaponTimer);
		HasShot = false;
		Debug.Log("CoolDown!");
	}
}
