using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayObjects : MonoBehaviour
{
    public Button bntL;
    public Button bntR;
    public GameObject positionPrefab;
    public int numOfGOs;
    public int paddingSpace = 2;
    public float changeSpeed = 0.3f;
    public GameObject[] PositionArr = new GameObject[3];
    public GameObject[] goHolders = new GameObject[3];

    int[] positionNumber = new int[5];
    int goStationary;

    void Awake()
    {
        CheckNumOfObjects();
    }

    void CheckNumOfObjects()
    {
        numOfGOs = goHolders.Length;
        positionNumber = new int[goHolders.Length];
        if (numOfGOs != PositionArr.Length)
        {
            PositionArr = new GameObject[numOfGOs];
        }
        CreatePositionsGos();
    }

    void CreatePositionsGos()
    {
        int xStartPos = -(PositionArr.Length - 1) / 2;
        int xEndPos = -xStartPos;

        float spacing = ((float)xEndPos - (float)xStartPos) / ((float)PositionArr.Length - 1);

        GameObject tempPos;
        tempPos = Instantiate(positionPrefab, transform.position, transform.rotation) as GameObject;
        tempPos.transform.parent = gameObject.transform;
        PositionArr[0] = tempPos;
        PositionSubObjs(0, 0);
        PositionItems(0);

        for (int i = 1; i < PositionArr.Length; i++)
        {
            GameObject tempPos3;
            tempPos3 = Instantiate(positionPrefab, transform.position, transform.rotation) as GameObject;

            tempPos3.transform.parent = gameObject.transform;
            PositionArr[i] = tempPos3;
            PositionSubObjs(i, spacing);
            PositionItems(i);
        }
    }

    void PositionSubObjs(int n, float space)
    {
        int xStartPos = -PositionArr.Length + 2;
        float currentPosition = ((float)xStartPos + ((n * 2) * space));

        PositionArr[n].transform.position = new Vector3(currentPosition, transform.position.y, transform.position.z);
        positionNumber[n] = n;
    }

    void PositionItems(int n)
    {
        goHolders[n].transform.position = new Vector3(PositionArr[n].transform.position.x, PositionArr[n].transform.position.y, PositionArr[n].transform.position.z);
    }

    void ClearOldPosObjs()
    {
        DestroyImmediate(GameObject.Find("Position(Clone)"));
    }

    public void MoveItemToNextPositionRight(int pos)
    {
        int tempInt = positionNumber[positionNumber.Length - 1];

        for (int i = positionNumber.Length - 1; i > 0; i--)
        {
            positionNumber[i] = positionNumber[i - pos];
        }
        positionNumber[0] = tempInt;
        MoveItemsToNewPosition();
        ButtonsActive(false);
    }

    public void MoveItemToNextPositionLeft(int pos)
    {
        int tempInt = positionNumber[0];

        for (int i = 0; i < positionNumber.Length - 1; i++)
        {
            positionNumber[i] = positionNumber[i + pos];
        }
        positionNumber[positionNumber.Length - 1] = tempInt;
        MoveItemsToNewPosition();
        ButtonsActive(false);
    }

    void MoveItemsToNewPosition()
    {
        for (int i = 0; i < PositionArr.Length; i++)
        {
            goStationary = 0;

            StartCoroutine(MoveGOToNewPos(goHolders[positionNumber[i]], PositionArr[i], positionNumber[i]));
        }
    }

    IEnumerator MoveGOToNewPos(GameObject displayGO, GameObject positionGO, int posN)
    {
        if (Vector3.Distance(displayGO.transform.position, positionGO.transform.position) >= 3.0f)
        {
            displayGO.transform.position = new Vector3(positionGO.transform.position.x, positionGO.transform.position.y, positionGO.transform.position.z);
            yield return new WaitForSeconds(0.1f);
        }

        if (posN != 0 || posN != goHolders.Length)
        {
            while (Vector3.Distance(displayGO.transform.position, positionGO.transform.position) > 0.01f)
            {
                displayGO.transform.position = Vector3.MoveTowards(displayGO.transform.position, positionGO.transform.position, changeSpeed);
                yield return new WaitForSeconds(0.01f);
            }
        }

        goStationary++;

        if (goStationary == goHolders.Length)
        {
            ButtonsActive(true);
        }
    }

    void ButtonsActive(bool active)
    {
        bntL.interactable = active;
        bntR.interactable = active;
    }
}
