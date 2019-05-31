using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class wordCount  {


	public int count;
	public string name;
	public  wordCount(int c, string n)
	{
		count = c;
		name = n;
	}
	public void addCount()
	{
		count++;
	}
}
