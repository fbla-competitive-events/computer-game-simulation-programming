using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portal
{
    public class Teleport : MonoBehaviour
    {
        GameObject NPC;
        public Spawner s;

        public int count = 0;
        public int val = 0;

        // Use this for initialization
        void Start()
        {
            NPC = this.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (count >= 6)
            {
                val = Random.Range(0, 12);
                for (int i = 0; i < 12; i++)
                {
                    if (s.occupied[val] == false)
                    {
                        NPC.transform.position = s.spawn[val].transform.position;
                    }
                    else
                        val = Random.Range(0, 12);
                }
                count = 0;
            }
        }

        public void Participate()
        {
            
            count++;
        }


    }
}
