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

        public string customEventName = "";



		public Sprite playerSprite;
		private Vector2 movement;

        public List<KeyCode> upKey;
        public List<KeyCode> downKey;
        public List<KeyCode> leftKey;
        public List<KeyCode> rightKey;
        public List<KeyCode> fireKey;

	    //public Weapon Weapon;

		public float moveSpeed = 6f;  //remove?      
		public Rect movementBounds;

        // Movement modifier applied to directional movement.
        public float playerSpeed = 2.0f;

        // What the current speed of our player is
        private float currentSpeed = 0.0f;
        // The last movement that we've made
        private Vector3 lastMovement = new Vector3();

		private DanmakuContext context;



		private Vector3 direction = new Vector3();

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
            context = DanmakuController.Instance.GetContext("Player");
	    }

	    private void Update()
		{
            HandleRotation();
			Movement();
			HandleShooting();

		}





        //private void HandleMovement()
        //{
        //    direction.Set(0f, 0f, 0f);

        //    if (Input.GetKey(upKey))
        //    {
        //        direction.y = 1f;
        //    }
        //    else if (Input.GetKey(downKey))
        //    {
        //        direction.y = -1f;
        //    }

        //    if (Input.GetKey(leftKey))
        //    {
        //        direction.x = -1f;
        //    }
        //    else if (Input.GetKey(rightKey))
        //    {
        //        direction.x = 1f;
        //    }

        //    Vector2 newPosition = _transform.position +
        //                         (moveSpeed * direction.normalized * context.DeltaTime);

        //    if (newPosition.x < movementBounds.xMin)
        //    {
        //        newPosition.x = movementBounds.xMin;
        //    }
        //    else if (newPosition.x > movementBounds.xMax)
        //    {
        //        newPosition.x = movementBounds.xMax;
        //    }

        //    if (newPosition.y < movementBounds.yMin)
        //    {
        //        newPosition.y = movementBounds.yMin;
        //    }
        //    else if (newPosition.y > movementBounds.yMax)
        //    {
        //        newPosition.y = movementBounds.yMax;
        //    }

        //    _transform.position = newPosition;

        //}

		private void HandleShooting()
		{
		    this.GetComponent<Weapon>().Fire(fireKey);
		}
        // Will move the player based off of keys pressed
        void Movement()
        {
            // The movement that needs to occur this frame
            Vector3 movement = new Vector3();

            // Check for input
            movement += MoveIfPressed(upKey, Vector3.up);
            movement += MoveIfPressed(downKey, Vector3.down);
            movement += MoveIfPressed(leftKey, Vector3.left);
            movement += MoveIfPressed(rightKey, Vector3.right);

            /*
               * If we pressed multiple buttons, make sure we're only
               * moving the same length.
            */
            movement.Normalize();

            // Check if we pressed anything
            if (movement.magnitude > 0)
            {
                // If we did, move in that direction
                currentSpeed = playerSpeed;
                this.transform.Translate(movement * Time.deltaTime *
                                        playerSpeed, Space.World);
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
        /*
* Will return the movement if any of the keys are pressed,
* otherwise it will return (0,0,0)
*/
        Vector3 MoveIfPressed(List<KeyCode> keyList, Vector3 Movement)
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
                    return Movement;
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

            context.SetVariable("PlayerRotation", -angle - 90);

           // Debug.Log("#PlayerRotation = " + context.GetVariable("PlayerRotation"));


	    }
    }
}
