package smWorld;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.NoSuchElementException;
import java.util.Scanner;

import javax.swing.ImageIcon;

public class Sprite {

	private ImageIcon image;
	private double x, y, modx, mody;
	private String name;
	private int id, tileid;
	private boolean followScreen;
	private collisionType collision;
	
	public enum collisionType {
		TOP,
		LEFT,
		FULL,
		RIGHT,
		BOTTOM,
		TOPLEFTD,
		TOPRIGHTD,
		BOTTOMLEFTD,
		BOTTOMRIGHTD
	}
	
	public int compareToFile(int i){
		
		Scanner scan;
		try {
			scan = new Scanner(new File("collisions.txt"));
			
			do{
				int l = Integer.parseInt(scan.nextLine());
				if(i==l){
					int s = Integer.parseInt(scan.nextLine());
					scan.close();
					return s;
				}
				
			} while(scan.hasNextLine());
			
			scan.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (NoSuchElementException f){
			// NO COLLISION
		}
		
		return -1;
	}

	public collisionType setCollisionData(int i){
		
		i = compareToFile(i);
		
		if(i == 0){
			this.setCollision(collisionType.TOP);
		}if(i == 1){
			this.setCollision(collisionType.LEFT);
		}if(i == 2){
			this.setCollision(collisionType.FULL);
		}if(i == 3){
			this.setCollision(collisionType.RIGHT);
		}if(i == 4){
			this.setCollision(collisionType.BOTTOM);
		}if(i == 5){
			this.setCollision(collisionType.TOPLEFTD);
		}if(i == 6){
			this.setCollision(collisionType.TOPRIGHTD);
		}if(i == 7){
			this.setCollision(collisionType.BOTTOMLEFTD);
		}if(i == 8){
			this.setCollision(collisionType.BOTTOMRIGHTD);
		}
		
		return this.getCollision();
		
	}
	
	public Sprite collisionImage(){
		if(getCollision() == collisionType.TOP){
			return new Sprite(new ImageIcon("Tiles\\top.png"),x,y);
		}if(getCollision() == collisionType.LEFT){
			return new Sprite(new ImageIcon("Tiles\\left.png"),x,y);
		}if(getCollision() == collisionType.FULL){
			return new Sprite(new ImageIcon("Tiles\\full.png"),x,y);
		}if(getCollision() == collisionType.RIGHT){
			return new Sprite(new ImageIcon("Tiles\\right.png"),x,y);
		}if(getCollision() == collisionType.BOTTOM){
			return new Sprite(new ImageIcon("Tiles\\bottom.png"),x,y);
		}if(getCollision() == collisionType.TOPLEFTD){
			return new Sprite(new ImageIcon("Tiles\\topleft.png"),x,y);
		}if(getCollision() == collisionType.TOPRIGHTD){
			return new Sprite(new ImageIcon("Tiles\\topright.png"),x,y);
		}if(getCollision() == collisionType.BOTTOMLEFTD){
			return new Sprite(new ImageIcon("Tiles\\bottomleft.png"),x,y);
		}if(getCollision() == collisionType.BOTTOMRIGHTD){
			return new Sprite(new ImageIcon("Tiles\\bottomright.png"),x,y);
		}
		return null;
	}
	
	public Sprite(){
		x = 0; 
		y = 0;
		modx=0;
		mody=0;
	}
	
	public Sprite(ImageIcon i){
		this();
		this.setImage(i);
	}
	
	public Sprite(ImageIcon i, double x, double y){
		this();
		this.setImage(i);
		this.setX(x);
		this.setY(y);
	}

	public ImageIcon getImage() {
		return image;
	}

	public void setImage(ImageIcon image) {
		this.image = image;
		File f = new File(image.getDescription());
		this.setName((f.getName()).substring(0, f.getName().length()-4));
	}

	public double getX() {
		return x+modx;
	}

	public void setX(double x) {
		this.x = x;
	}

	public double getY() {
		return y+mody;
	}

	public void setY(double y) {
		this.y = y;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public boolean isFollowScreen() {
		return followScreen;
	}

	public void setFollowScreen(boolean followScreen) {
		this.followScreen = followScreen;
	}

	public double getModx() {
		return modx;
	}

	public void setModx(double modx) {
		this.modx = modx;
	}

	public double getMody() {
		return mody;
	}

	public void setMody(double mody) {
		this.mody = mody;
	}

	public collisionType getCollision() {
		return collision;
	}

	public void setCollision(collisionType collision) {
		this.collision = collision;
	}

	public int getTileid() {
		return tileid;
	}

	public void setTileid(int tileid) {
		this.tileid = tileid;
	}
	
}
