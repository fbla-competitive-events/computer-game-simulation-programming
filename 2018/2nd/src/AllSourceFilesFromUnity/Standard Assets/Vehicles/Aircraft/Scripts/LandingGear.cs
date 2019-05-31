using UnityEngine;

namespace UnitySampleAssets.Vehicles.Aeroplane
{

    public class LandingGear : MonoBehaviour
    {

        // The landing gear can be raised and lowered at differing altitudes.
        // The gear is only lowered when descending, and only raised when climbing.

        // this script detects the raise/lower condition and sets a parameter on
        // the animator to actually play the animation to raise or lower the gear.

        public float raiseAtAltitude = 40;
        public float lowerAtAltitude = 40;

        private GearState state = GearState.Lowered;
        private Animator animator;

        private enum GearState
        {
            Raised = -1,
            Lowered = 1
        }

        private AeroplaneController plane;

        // Use this for initialization
        private void Start()
        {
            plane = GetComponent<AeroplaneController>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {

            if (state == GearState.Lowered && plane.Altitude > raiseAtAltitude && GetComponent<Rigidbody>().velocity.y > 0)
            {
                state = GearState.Raised;
            }

            if (state == GearState.Raised && plane.Altitude < lowerAtAltitude && GetComponent<Rigidbody>().velocity.y < 0)
            {
                state = GearState.Lowered;
            }

            // set the parameter on the animator controller to trigger the appropriate animation
            animator.SetInteger("GearState", (int) state);

        }
    }
}