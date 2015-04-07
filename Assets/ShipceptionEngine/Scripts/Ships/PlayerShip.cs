using UnityEngine;
using System.Collections;
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

		public Sprite playerSprite;
		private Vector2 movement;

		public KeyCode upKey;
		public KeyCode downKey;
		public KeyCode leftKey;
		public KeyCode rightKey;
		public KeyCode fireKey;
		public float moveSpeed = 6f;        
		public Rect movementBounds;

		private DanmakuContext context;

		private DanmakuOrigin origin;

		private Vector3 direction = new Vector3();

		private Transform _transform;

		private void Awake()
		{
			_transform = transform;
			origin = GetComponent<DanmakuOrigin>();
		}

		private void Start()
		{
			context = origin.Context;
		}
	
		private void Update()
		{
			HandleMovement();
			HandleShooting();

		}

		private void HandleMovement()
		{
			direction.Set(0f, 0f, 0f);

			if (Input.GetKey(upKey))
			{
				direction.y = 1f;
			}
			else if (Input.GetKey(downKey))
			{
				direction.y = -1f;
			}

			if (Input.GetKey(leftKey))
			{
				direction.x = -1f;
			}
			else if (Input.GetKey(rightKey))
			{
				direction.x = 1f;
			}

			Vector2 newPosition = _transform.position +
								 (moveSpeed * direction.normalized * context.DeltaTime);

			if (newPosition.x < movementBounds.xMin)
			{
				newPosition.x = movementBounds.xMin;
			}
			else if (newPosition.x > movementBounds.xMax)
			{
				newPosition.x = movementBounds.xMax;
			}

			if (newPosition.y < movementBounds.yMin)
			{
				newPosition.y = movementBounds.yMin;
			}
			else if (newPosition.y > movementBounds.yMax)
			{
				newPosition.y = movementBounds.yMax;
			}

			_transform.position = newPosition;

		}

		private void HandleShooting()
		{
			// handle shooting
			if (Input.GetKey(fireKey))
			{
				if (!origin.IsRunningBulletPattern)
				{
					origin.StartBulletPattern();
				}

				if (!origin.RootEmitter.enabled)
				{
					origin.RootEmitter.enabled = true;
				}
			}
			else if (origin.IsRunningBulletPattern && origin.RootEmitter.enabled)
			{
				origin.RootEmitter.enabled = false;
				origin.ResetRootEmitter();
			}
			

		}
	}
}
