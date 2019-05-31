using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LeaderboardEntry {
	public new string name;
	public double rating;
	public int rank;
	public LeaderboardEntry(string name, double rating)
	{
		this.name = name;
		this.rating = rating;
	}
	public string getName()
	{
		return name;
	}
	public double getRating()
	{
		return rating;
	}
	public int getRank()
	{
		return rank;
	}
	public void setRating(double r)
	{
		rating = r;
	}
	public void setName(string s){
		name = s;
	}
	public void setRank(int r)
	{
		rank = r;
	}
}
