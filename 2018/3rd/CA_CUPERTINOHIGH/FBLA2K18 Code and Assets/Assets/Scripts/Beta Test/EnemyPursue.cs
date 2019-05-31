using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPursue : MonoBehaviour
{

    public float MoveSpeed = 2;
    public Transform Player;
    public float MaxDist = 10f;
    public float MinDist = 5f;
    private Vector3 gravity = Vector3.zero;
    public Material TriggerMaterial;
    private bool EndGame = false;

    public Texture backgroundTexture;
    public Texture bgText;
    void Update()
    {
        transform.LookAt(Player);



        if (Vector3.Distance(transform.position, Player.position) >= MinDist && !GameOptions.pause)
        {
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            GetComponent<Animator>().SetFloat("MoveSpeed", 1f);
        }

        if (Vector3.Distance(transform.position, Player.position) < MinDist)
        {
            
            SkinnedMeshRenderer mesh = transform.Find("mesh").GetComponent<SkinnedMeshRenderer>();
            Material[] mats = new Material[] { mesh.material, TriggerMaterial };
            mesh.materials = mats;

            GameOptions.pause = true;

            StartCoroutine(Pause());
           
        }

        if (GetComponent<CharacterController>().isGrounded)
        {
            gravity = Vector3.zero;
        } else
        {
            gravity += Physics.gravity * Time.deltaTime;
        }
        gravity = transform.TransformDirection(gravity);
        GetComponent<CharacterController>().Move(gravity);
    }

    IEnumerator Pause()
    {
        
        yield return new WaitForSeconds(0.3f);
        EndGame = true;
        yield return new WaitForSeconds(0.3f);
        Application.Quit();
        
    }



    void OnGUI()
    {
        if (EndGame)
        {
            
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture, ScaleMode.StretchToFill);
            GUI.DrawTexture(new Rect(Screen.width / 8, Screen.height / 8, Screen.width/4 * 3, Screen.height/4 * 3), bgText);
        }
    }
}
