package loz;

import javax.swing.JButton;

@SuppressWarnings("serial")
public class Button extends JButton{

	private int x, y;
	private Runnable execute;
	private String text;
	private int r, g, b;
	
	public Button(int x, int y, String text, Runnable execute){
		this.x = x;
		this.y = y;
		this.text = text;
		this.execute = execute;
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

	public Runnable getExecute() {
		return execute;
	}

	public void setExecute(Runnable execute) {
		this.execute = execute;
	}

	public String getText() {
		return text;
	}

	public void setText(String text) {
		this.text = text;
	}

	public int getR() {
		return r;
	}

	public void setR(int r) {
		this.r = r;
	}

	public int getG() {
		return g;
	}

	public void setG(int g) {
		this.g = g;
	}

	public int getB() {
		return b;
	}

	public void setB(int b) {
		this.b = b;
	}
	
}
