using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;



    public class Platformer2DUserControl : MonoBehaviour
    {
		private PlatformerCharacter2D character;
        private bool jump;

		public bool isMovementEnabled = true;
        private void Awake()
        {
			character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
		if (isMovementEnabled) {
			if (!jump)
            // Read the jump input in Update so button presses aren't missed.
            jump = CrossPlatformInputManager.GetButtonDown ("Jump");
		} else {
			character.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			character.anim.SetFloat ("Speed", 0);

		}
        }

        private void FixedUpdate()
		{
			if (isMovementEnabled) {
				// Read the inputs.
				bool crouch = Input.GetKey (KeyCode.LeftControl);
				float h = CrossPlatformInputManager.GetAxis ("Horizontal");
				// Pass all parameters to the character control script.
				if (GameControl.control.isCharFlipCorrect) {
					character.Move (h, crouch, jump);
				}
				jump = false;
			}
	
		}
    }
