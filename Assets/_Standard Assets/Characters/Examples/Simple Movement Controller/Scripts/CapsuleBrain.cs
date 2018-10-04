﻿using StandardAssets.Characters.CharacterInput;
using StandardAssets.Characters.Common;
using StandardAssets.Characters.Effects;
using StandardAssets.Characters.Physics;
using UnityEngine;

namespace StandardAssets.Characters.Examples.SimpleMovementController
{
	[RequireComponent(typeof(CharacterPhysics))]
	[RequireComponent(typeof(ICharacterInput))]
	public class CapsuleBrain : CharacterBrain
	{
		[SerializeField]
		protected float maxSpeed = 5f;

		[SerializeField]
		protected float timeToMaxSpeed = 0.5f;
		
		[SerializeField] 
		protected float turnSpeed = 300f;

		[SerializeField]
		protected float jumpSpeed = 5f;
		
		/// <summary>
		/// Manages movement events
		/// </summary>
		[SerializeField, Tooltip("The management of movement events e.g. footsteps")]
		protected CapsuleMovementEventHandler capsuleMovementEventHandler;
	   
		/// <summary>
		/// The current movement properties
		/// </summary>
		private float currentSpeed;

		/// <summary>
		/// The current movement properties
		/// </summary>
		private float movementTime;

		/// <summary>
		/// A check to see if input was previous being applied
		/// </summary>
		private bool previouslyHasInput;

		/// <summary>
		/// The main camera's transform, used for calculating look direction.
		/// </summary>
		private Transform mainCameraTransform;

		/// <inheritdoc/>
		public override float normalizedForwardSpeed
		{
			get
			{
				return currentSpeed / maxSpeed;
			}
		}

		/// <inheritdoc/>
		public override MovementEventHandler movementEventHandler
		{
			get { return capsuleMovementEventHandler; }
		}

		public override float targetYRotation { get; set; }

		protected override void Awake()
		{
			base.Awake();
			capsuleMovementEventHandler.Init(this, transform, characterPhysics);
			mainCameraTransform = Camera.main.transform;
		}

		private void OnEnable()
		{
			characterInput.jumpPressed += OnJumpPressed;
			capsuleMovementEventHandler.Subscribe();
		}
		
		/// <summary>
		/// Unsubscribe
		/// </summary>
		private void OnDisable()
		{
			capsuleMovementEventHandler.Unsubscribe();
			if (characterInput == null)
			{
				return;
			}
			
			characterInput.jumpPressed -= OnJumpPressed;
		}
		
		/// <summary>
		/// Handles camera rotation
		/// </summary>
		protected override void Update()
		{
			base.Update();
			Quaternion targetRotation = CalculateTargetRotation();
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

			targetYRotation = targetRotation.eulerAngles.y;
		}
		
		
		protected virtual Quaternion CalculateTargetRotation()
		{
			Vector3 flatForward = mainCameraTransform.forward;
			flatForward.y = 0f;
			flatForward.Normalize();

			Vector3 localMovementDirection =
				new Vector3(characterInput.moveInput.x, 0f, characterInput.moveInput.y);
			Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward, localMovementDirection);
			cameraToInputOffset.eulerAngles = new Vector3(0f, cameraToInputOffset.eulerAngles.y, 0f);

			return Quaternion.LookRotation(cameraToInputOffset * flatForward);
		}

		/// <summary>
		/// Handles jumping
		/// </summary>
		private void OnJumpPressed()
		{
			if (characterPhysics.isGrounded)
			{
				characterPhysics.SetJumpVelocity(jumpSpeed);
			}	
		}

		/// <summary>
		/// Handles movement on Physics update
		/// </summary>
		private void FixedUpdate()
		{
			Move();
			capsuleMovementEventHandler.Tick();
		}

		/// <summary>
		/// State based movement
		/// </summary>
		private void Move()
		{
			if (characterInput.hasMovementInput)
			{
				if (!previouslyHasInput)
				{
					movementTime = 0f;
				}
				Accelerate();
			}
			else
			{
				if (previouslyHasInput)
				{
					movementTime = 0f;
				}

				Stop();
			}

			Vector2 input = characterInput.moveInput;
			if (input.sqrMagnitude > 1)
			{
				input.Normalize();
			}
		
			Vector3 forward = transform.forward * input.magnitude;
			Vector3 sideways = Vector3.zero;
			
			characterPhysics.Move((forward + sideways) * currentSpeed * Time.fixedDeltaTime, Time.fixedDeltaTime);

			previouslyHasInput = characterInput.hasMovementInput;
		}	

		/// <summary>
		/// Calculates current speed based on acceleration anim curve
		/// </summary>
		private void Accelerate()
		{
			movementTime += Time.fixedDeltaTime;
			movementTime = Mathf.Clamp(movementTime, 0f, timeToMaxSpeed);
			currentSpeed = movementTime / timeToMaxSpeed * maxSpeed;
		}
		
		/// <summary>
		/// Stops the movement
		/// </summary>
		private void Stop()
		{
			currentSpeed = 0f;
		}
	}
}