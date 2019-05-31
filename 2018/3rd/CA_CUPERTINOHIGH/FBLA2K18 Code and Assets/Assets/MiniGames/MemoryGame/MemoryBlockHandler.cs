using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MemoryBlockHandler : MonoBehaviour {

    public GameObject[] MemoryBlocks;
    public GameObject CurrBlock;
    public int currBlockIdx;

    public bool PlayRythm = false;
    public List<int> Indexes;

    public int Phase = 0;

    public GameObject HUD;
    public TextMeshProUGUI Instruct;
    public Button PlayRythmButton;

	void Start () {
        
        Phase = 0;
        currBlockIdx = 0;
        Indexes = new List<int>();
    }
    private bool start = true;
	
    public void StartGame()
    {
        HUD.SetActive(true);
        Instruct.gameObject.SetActive(true);
        Phase = 0;
        currBlockIdx = 0;
        Indexes = new List<int>();
        PlayRythmButton.gameObject.SetActive(true);
        MemoryGameController.Pause = false;
        PlayRythmButton.gameObject.SetActive(true);
        PlayRythmButton.interactable = true;
        PlayRythmButton.Select();

        foreach(GameObject g in MemoryBlocks)
        {
            g.GetComponent<MemoryBlockObject>().UserDeselect();
        }
    }

	void Update () {
        Debug.Log(MemoryGameController.Pause);
        if (start)
        {
            //SelectCurrentBlock();
            start = false;
            
        }
        if (MemoryGameController.Pause)
        {
            //PlayRythmButton.enabled = false;
        } else
        {
            //PlayRythmButton.enabled = true;
        }

        if (!MemoryGameController.Pause)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PrevBlock();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextBlock();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log(PlayRythm);
                if (Indexes.Count > 0) SelectBlockChoice();
            }
        }
        if (PlayRythm)
        {
            PlayRythm = false;
            //Debug.Log("Playing");
            MemoryGameController.Pause = true;
            DeselectCurrentBlock();
            MemoryGameController.PlayingRythm = true;
            StartCoroutine(PlayRythmPattern());
           
        }
        
    }

    IEnumerator PlayRythmPattern()
    {
        foreach (int i in Indexes)
        {
            MemoryBlocks[i].GetComponent<MemoryBlockObject>().Select();
            yield return new WaitForSeconds(0.5f);
            MemoryBlocks[i].GetComponent<MemoryBlockObject>().Deselect();
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.5f);
        MemoryGameController.Pause = false;
        MemoryGameController.PlayingRythm = false;
        SelectCurrentBlock();
    }

    public void SelectBlockChoice()
    {
        if (currBlockIdx == Indexes[Phase])
        {
            Phase++;
            MemoryBlocks[currBlockIdx].GetComponent<MemoryBlockObject>().Pressed();
            Debug.Log("Correct");
            
            if (Phase == Indexes.Count)
            {
                MemoryGameController.Score++;
                MemoryGameController.Pause = false;
                PlayRythmButton.interactable = true;
                PlayRythmButton.Select();
            }
        } else
        {
            MemoryGameController.Wrong++;
            //Debug.Log("Err");
            MemoryBlocks[currBlockIdx].GetComponent<MemoryBlockObject>().Shake();
        }
    }

    public void PlayGame()
    {
        Indexes.Add(Random.Range(0, MemoryBlocks.Length));
        PlayRythm = true;
        Phase = 0;
    }


    public void DeselectCurrentBlock()
    {
        MemoryBlocks[currBlockIdx].GetComponent<MemoryBlockObject>().UserDeselect();
    }

    public void SelectCurrentBlock()
    {
        MemoryBlocks[currBlockIdx].GetComponent<MemoryBlockObject>().UserSelect();
    }

    public void NextBlock()
    {
        if (Phase == Indexes.Count)
        {
            MemoryGameController.Pause = true;
            PlayRythmButton.interactable = true;
            return;
        }
        MemoryBlocks[currBlockIdx].GetComponent<MemoryBlockObject>().UserDeselect();
        currBlockIdx++;
        if (currBlockIdx >= MemoryBlocks.Length)
        {
            currBlockIdx = 0;
        }

        MemoryBlocks[currBlockIdx].GetComponent<MemoryBlockObject>().UserSelect();

    }

    public void PrevBlock()
    {
        if (Phase == Indexes.Count)
        {
            MemoryGameController.Pause = true;
            PlayRythmButton.interactable = true;
            return;
        }
        MemoryBlocks[currBlockIdx].GetComponent<MemoryBlockObject>().UserDeselect();
        currBlockIdx--;
        if (currBlockIdx < 0)
        {
            currBlockIdx = MemoryBlocks.Length-1;
        }

        MemoryBlocks[currBlockIdx].GetComponent<MemoryBlockObject>().UserSelect();

    }
}
