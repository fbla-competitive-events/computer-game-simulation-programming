using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets.Vehicles.Aeroplane
{
    [RequireComponent(typeof (AeroplaneController))]
    public class AeroplaneUserControl4Axis : MonoBehaviour
    {

        // these max angles are only used on mobile, due to the way pitch and roll input are handled
        public float maxRollAngle = 80;
        public float maxPitchAngle = 80;

        // reference to the aeroplane that we're controlling
        private AeroplaneController aeroplane;
        private float throttle;
        private bool airBrakes;
        private float yaw;

        private void Awake()
        {
            // Set up the reference to the aeroplane controller.
            aeroplane = GetComponent<AeroplaneController>();
        }

        private void FixedUpdate()
        {
            // Read input for the pitch, yaw, roll and throttle of the aeroplane.
            float roll = CrossPlatformInputManager.GetAxis("Mouse X");
            float pitch = CrossPlatformInputManager.GetAxis("Mouse Y");
            airBrakes = CrossPlatformInputManager.GetButton("Fire1");
            yaw = CrossPlatformInputManager.GetAxis("Horizontal");
            throttle = CrossPlatformInputManager.GetAxis("Vertical");
#if MOBILE_INPUT
        AdjustInputForMobileControls(ref roll, ref pitch, ref throttle);
#endif
            //
            // Pass the input to the aeroplane
            aeroplane.Move(roll, pitch, yaw, throttle, airBrakes);
        }

        private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
        {

            // because mobile tilt is used for roll and pitch, we help out by
            // assuming that a centered level device means the user
            // wants to fly straight and level! 

            // this means on mobile, the input represents the *desired* roll angle of the aeroplane,
            // and the roll input is calculated to achieve that.
            // whereas on non-mobile, the input directly controls the roll of the aeroplane.

            float intendedRollAngle = roll*maxRollAngle*Mathf.Deg2Rad;
            float intendedPitchAngle = pitch*maxPitchAngle*Mathf.Deg2Rad;
            roll = Mathf.Clamp((intendedRollAngle - aeroplane.RollAngle), -1, 1);
            pitch = Mathf.Clamp((intendedPitchAngle - aeroplane.PitchAngle), -1, 1);
        }
    }
}