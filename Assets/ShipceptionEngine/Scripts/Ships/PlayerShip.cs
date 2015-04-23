using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using VirtualDropkick.DanmakuEngine;
using VirtualDropkick.DanmakuEngine.Unity;

namespace Shipception
{

	[RequireComponent(typeof(DanmakuOrigin))]
	public class PlayerShip : MonoBehaviour
	{

		public ShipComponent Wings;
		public ShipComponent Cannon;
		public ShipComponent Engine;
		public ShipComponent Cockpit;

        public string CustomEventName = "";



		public Sprite PlayerSprite;
		//private Vector2 _movement;

        public List<KeyCode> UpKey;
        public List<KeyCode> DownKey;
        public List<KeyCode> LeftKey;
        public List<KeyCode> RightKey;
        public List<KeyCode> FireKey;

	    //public Weapon Weapon;

		public float MoveSpeed = 6f;     
		public Rect MovementBounds;

        // Movement modifier applied to directional movement.
        public float PlayerSpeed = 2.0f;

        // What the current speed of our player is
        private float currentSpeed = 0.0f;
        // The last movement that we've made
        private Vector3 lastMovement = new Vector3();

		private DanmakuContext _context;



		//private Vector3 direction = new Vector3();

		private Transform _transform;

		private void Awake()
		{
            AwakeOrSpawned();
		}


	    private void OnSpawned()
	    {
	        AwakeOrSpawned();
	    }

	    private void AwakeOrSpawned()
	    {
	        _transform = transform;
            _context = DanmakuController.Instance.GetContext("Player");
	    }

	    private void Update()
		{
            //HandleRotation();
			HandleMovement();
			HandleShooting();

		}

        private void HandleMovement()
        {

            // The movement that needs to occur this frame
            Vector3 movement = new Vector3();

            // Check for input
            movement += MoveIfPressed(UpKey, Vector3.up);
            movement += MoveIfPressed(DownKey, Vector3.down);
            movement += MoveIfPressed(LeftKey, Vector3.left);
            movement += MoveIfPressed(RightKey, Vector3.right);

            movement.Normalize();

            Vector2 newPosition = _transform.position +
                                 (MoveSpeed * movement * _context.DeltaTime);

            //Constrain to In-Editor defined ScreenBounds
            if (newPosition.x < MovementBounds.xMin)
            {
                newPosition.x = MovementBounds.xMin;
            }
            else if (newPosition.x > MovementBounds.xMax)
            {
                newPosition.x = MovementBounds.xMax;
            }

            if (newPosition.y < MovementBounds.yMin)
            {
                newPosition.y = MovementBounds.yMin;
            }
            else if (newPosition.y > MovementBounds.yMax)
            {
                newPosition.y = MovementBounds.yMax;
            }

            _transform.position = newPosition;

            // Check if we pressed anything
            if (movement.magnitude > 0)
            {
                // If we did, move in that direction
                currentSpeed = PlayerSpeed;
                this.transform.Translate(movement * Time.deltaTime *
                                        PlayerSpeed, Space.World);
                lastMovement = movement;
            }
            else
            {
                // Otherwise, move in the direction we were going
                this.transform.Translate(lastMovement * Time.deltaTime
                                        * currentSpeed, Space.World);
                // Slow down over time
                currentSpeed *= .9f;
            }

        }

		private void HandleShooting()
		{
		    this.GetComponent<Weapon>().Fire(FireKey);
		}
        // Will move the player based off of keys pressed
        //private void Movement()
        //{
        //    // The movement that needs to occur this frame
        //    Vector3 movement = new Vector3();

        //    // Check for input
        //    movement += MoveIfPressed(UpKey, Vector3.up);
        //    movement += MoveIfPressed(DownKey, Vector3.down);
        //    movement += MoveIfPressed(LeftKey, Vector3.left);
        //    movement += MoveIfPressed(RightKey, Vector3.right);

        //    /*
        //       * If we pressed multiple buttons, make sure we're only
        //       * moving the same length.
        //    */
        //    movement.Normalize();

        //    // Check if we pressed anything
        //    if (movement.magnitude > 0)
        //    {
        //        // If we did, move in that direction
        //        currentSpeed = PlayerSpeed;
        //        this.transform.Translate(movement * Time.deltaTime *
        //                                PlayerSpeed, Space.World);
        //        lastMovement = movement;
        //    }
        //    else
        //    {
        //        // Otherwise, move in the direction we were going
        //        this.transform.Translate(lastMovement * Time.deltaTime
        //                                * currentSpeed, Space.World);
        //        // Slow down over time
        //        currentSpeed *= .9f;
        //    }
        //}
        /*
* Will return the movement if any of the keys are pressed,
* otherwise it will return (0,0,0)
*/
        Vector3 MoveIfPressed(List<KeyCode> keyList, Vector3 movement)
        {
            // Check each key in our list
            foreach (KeyCode element in keyList)
            {
                if (Input.GetKey(element))
                {
                    /*
                      * It was pressed so we leave the function
                      * with the movement applied.
                    */
                    return movement;
                }
            }

            // None of the keys were pressed, so don't need to move
            return Vector3.zero;
        }

	    private void HandleRotation()
	    {
            // We need to tell where the mouse is relative to the   
            // player  
            Vector3 worldPos = Input.mousePosition;  
            worldPos = Camera.main.ScreenToWorldPoint(worldPos);

            /*   
             *  Get the differences from each axis (stands for    
             *  deltaX and deltaY)   
             */
            float dx = this.transform.position.x - worldPos.x; 
            float dy = this.transform.position.y - worldPos.y;
            // Get the angle between the two objects  
            float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
            /*     
             * * The transform's rotation property uses a Quaternion,     
             * * so we need to convert the angle in a Vector     
             * * (The Z axis is for rotation for 2D).  */
            Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            
            // Assign the ship's rotation  
	        this.transform.rotation = rot;

            _context.SetVariable("PlayerRotation", -angle - 90);

           // Debug.Log("#PlayerRotation = " + _context.GetVariable("PlayerRotation"));


	    }
    }
}
