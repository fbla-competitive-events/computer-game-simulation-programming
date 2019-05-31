package loz;

public class Dice {

	public int myNumSides;
	
	public Dice(){
		myNumSides=6;	
	}
	
	public Dice(int sides){
	 myNumSides=sides;	
	}
	
	public Dice(double sides){
		int side = (int) Math.round(sides);
		 myNumSides=side;	
		}
	
	public int roll() {
		
		double result = (Math.random()*myNumSides)+1;
		
		return (int)result;
		
	}
	
	public int getNumSides(){
		return this.myNumSides;
	}
	
	public void setNumSides(int a){
		this.myNumSides=a;
	}
	
}
