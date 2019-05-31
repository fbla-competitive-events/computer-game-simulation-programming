package loz;

import java.util.Timer;
import java.util.TimerTask;

public class Light {
	
	private float radius, strength, apparentRadius, red = 1f, green = 1f, blue = 1f;
	private int x, y;
	private boolean flickering;
	
	public Light(float r, float s, boolean ficker){
		this.setRadius(r);
		this.setApparentRadius(r);
		this.setStrength(s);
		this.setFlickering(ficker);
		if(flickering){
			Timer f = new Timer();
			f.schedule(new TimerTask(){
				@Override
				public void run() {
					setApparentRadius((float)(getRadius()+(Math.random()*8f)));
				}
			}, 75, 75);
		}
	}

	public float getRadiusDifference(){
		return Math.abs(radius-apparentRadius);
	}
	
	public float getRadius() {
		return radius;
	}

	public void setRadius(float radius) {
		this.radius = radius;
	}

	public float getStrength() {
		return strength;
	}

	public void setStrength(float strength) {
		this.strength = strength;
	}

	public float getRed() {
		return red;
	}

	public void setRed(float red) {
		this.red = red;
	}

	public float getGreen() {
		return green;
	}

	public void setGreen(float green) {
		this.green = green;
	}

	public float getBlue() {
		return blue;
	}

	public void setBlue(float blue) {
		this.blue = blue;
	}

	public boolean isFlickering() {
		return flickering;
	}

	public void setFlickering(boolean flickering) {
		this.flickering = flickering;
	}

	public float getApparentRadius() {
		return apparentRadius;
	}

	public void setApparentRadius(float apparentRadius) {
		this.apparentRadius = apparentRadius;
	}

	public int getX() {
		return x;
	}

	public void setX(int x) {
		this.x = x;
	}

	public int getY() {
		return y;
	}

	public void setY(int y) {
		this.y = y;
	}

}
