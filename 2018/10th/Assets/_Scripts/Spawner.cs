using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portal
{
    public class Spawner : MonoBehaviour
    {
        public GameObject[] spawn;
        public bool[] occupied;
        public int val = 0;
        public Teleport[] t;
    }
}
