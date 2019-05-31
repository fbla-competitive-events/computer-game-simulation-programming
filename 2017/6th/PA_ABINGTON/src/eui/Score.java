package eui;

import java.util.Arrays;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

public class Score {

	private float Score;
	private String name;
	
	public Score(Float s, String n){
		Score = s;
		name = n;	
	}
	
	public static void main(String[] args) {
		Score[] sc = new Score[]{
		new Score(0.35f,"one"),
		new Score(2.1f,"two"),
		new Score(3.4f,"three"),
		new Score(0.25f,"four"),
		};
		
		sc = sc[0].sortScoreList(Arrays.asList(sc));
		System.err.println("SZ: "+sc.length+"\n");
		for(Score f: sc){
			System.out.println(f.getName());
		}
		
	}
	
	public Score[] sortScoreList(List<Score> s){
		
		Collections.sort(s, new Comparator<Score>() {
			public int compare(Score o1, Score o2) {
				return Float.compare(o1.getScore() , o2.getScore());
			}
		});
		
		return (Score[]) s.toArray();
		
	}

	public float getScore() {
		return Score;
	}

	public void setScore(float score) {
		Score = score;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}
	
	
	
}
