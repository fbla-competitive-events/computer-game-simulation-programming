using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portal
{
    public class Busy : MonoBehaviour
    {
        public Spawner s;

        public int Index;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Necro" || other.tag == "Jones" || other.tag == "Chad" || other.tag == "Debby" || other.tag == "Kevin")
            {
                s.occupied[Index] = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Necro" || other.tag == "Jones" || other.tag == "Chad" || other.tag == "Debby" || other.tag == "Kevin")
            {
                s.occupied[Index] = false;
            }
        }
    }
}
