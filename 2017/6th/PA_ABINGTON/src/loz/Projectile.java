package loz;

import java.awt.Point;
import java.awt.Rectangle;
import java.util.ConcurrentModificationException;
import java.util.Timer;
import java.util.TimerTask;
import javax.swing.ImageIcon;

public class Projectile extends Sprite{

	private ImageIcon animatedImage, shatter; // Should be 10 frames at 100 ms per frame = 1 sec fly time, unless indefinite until collision.
	private boolean dieOnCollide, visible = true, byplayer, concluded, bounceBack;
	private int fally, damage, modWidth;
	private Stimulus stimulus;
	private Point target;
	private Timer fly = new Timer();
	private String name;
	
	public enum Stimulus { // projectile stim types. Used for enemy weakness debuff / strength bonus
		fire,
		frost,
		arrow,
		boomerang,
		regular;
	}
	
	public Projectile(ImageIcon an, ImageIcon sh){
		super(an);
		this.setWidth(16);
		this.setHeight(16);
		concluded = false;
		visible = true;
		this.setStimulus(Stimulus.regular);
	}
	
	public void launch(Runnable r){
		target = new Point(WorldBuilder.link.getX()+8,WorldBuilder.link.getY()+22 - 128);
		setX(WorldBuilder.link.getX()+8);
		setY(WorldBuilder.link.getY()+22);
		WorldBuilder.screen.getCollisions().add(this);
		fly.schedule(new TimerTask(){
			@Override
			public void run() {
				
					if (getX()+getWidth()/2 < target.getX()) {
						setX(getX() + 4);
					}
					if (getX()+getWidth()/2 >= target.getX()) {
						setX(getX() - 4);
					}
					if (getY()+getHeight()/2 < target.getY()) {
						setY(getY() + 4);
					}
					if (getY()+getHeight()/2 >= target.getY()) {
						setY(getY() - 4);
					}
					
					if(Math.abs(getX()+getWidth()/2-target.getX()) < 8 && Math.abs(getY()+getHeight()/2-target.getY()) < 8){
						setImage(new ImageIcon(""));
						r.run();
						fly.cancel();
						fly.purge();
					}
			}
		}, 35, 35);
	}
	
	public void spawnPlayerTarget(String animation){
		
		this.setImage(new ImageIcon(""));
		
		target = new Point(WorldBuilder.link.getX()+8,WorldBuilder.link.getY()+22);
		WorldBuilder.screen.getCollisions().add(this);
		//Projectile t = this;
		
		fly.schedule(new TimerTask(){

			@Override
			public void run() {
				
					if (getX()+getWidth()/2 < target.getX()) {
						setX(getX() + 4);
					}
					if (getX()+getWidth()/2 >= target.getX()) {
						setX(getX() - 4);
					}
					if (getY()+getHeight()/2 < target.getY()) {
						setY(getY() + 4);
					}
					if (getY()+getHeight()/2 >= target.getY()) {
						setY(getY() - 4);
					}
					
					if(Math.abs((WorldBuilder.link.getX()+8)-(getX()+getWidth()/2)) < getWidth()/2 && 
							Math.abs((WorldBuilder.link.getY()+16)-(getY()+getHeight()/2)) < getHeight()/2){
						WorldBuilder.link.hurtme(-1);
						setX(-999);
						fly.cancel();
						fly.purge();
						getAnimate().cancel();
						getAnimate().purge();
					}
					
			}
		}, 35, 35);
		
		this.playAnimation(animation, 30, false);
		
	}
	
	public void spawnTarget(String animation, Point p){
		
		this.setImage(new ImageIcon(""));
		
		target = p;
		WorldBuilder.screen.getCollisions().add(this);
		Projectile t = this;
		
		fly.schedule(new TimerTask(){

			@Override
			public void run() {
				
					if (getX() < target.getX()) {
						setX(getX() + 4);
					}
					if (getX() >= target.getX()) {
						setX(getX() - 4);
					}
					if (getY() < target.getY()) {
						setY(getY() + 4);
					}
					if (getY() >= target.getY()) {
						setY(getY() - 4);
					}
					
					if(Math.abs(target.getX()-getX()) < getWidth() && Math.abs(target.getY()-getY()) < getHeight()){
						
						if(Math.abs((WorldBuilder.link.getX()+8)-(getX()+getWidth()/2)) < getWidth()/2 && 
								Math.abs((WorldBuilder.link.getY()+16)-(getY()+getHeight()/2)) < getHeight()/2){
							WorldBuilder.link.hurtme(-1);
							setX(-999);
							fly.cancel();
							fly.purge();
							getAnimate().cancel();
							getAnimate().purge();
						}
						
						fly.cancel();
						fly.purge();
						getAnimate().cancel();
						getAnimate().purge();
						WorldBuilder.screen.getCollisions().remove(t);
						setX(-999);
					}
					
					if(Math.abs((WorldBuilder.link.getX()+8)-(getX()+getWidth()/2+getOffSetX())) < getWidth()/2 && 
							Math.abs((WorldBuilder.link.getY()+16)-(getY()+getHeight()/2+getOffSetY())) < getHeight()/2){
						WorldBuilder.link.hurtme(-1);
						setX(-999);
						fly.cancel();
						fly.purge();
						getAnimate().cancel();
						getAnimate().purge();
					}
					
			}
		}, 35, 35);
		
		this.playAnimation(animation, 30, false);
		
	}
	
	public void spawnBoomerang(int dir, long distance, int dam){
		
		this.setDamage(dam);
		WorldBuilder.screen.getCollisions().add(this);
		Projectile t = this;
		Timer kill = new Timer();
		
		fly.schedule(new TimerTask(){

			@Override
			public void run() {
				
				if (!concluded) {
					if (dir == 1) {
						move(0, -4);
					}
					if (dir == 2) {
						move(0, 4);
					}
					if (dir == 3) {
						move(-4, 0);
					}
					if (dir == 4) {
						move(4, 0);
					}
					
					checkCollisionWithWorldObjects(fly,kill,t);
					
				} else {

					if (getX() < WorldBuilder.link.getX()) {
						setX(getX() + 4);
					}
					if (getX() >= WorldBuilder.link.getX()) {
						setX(getX() - 4);
					}
					if (getY() < WorldBuilder.link.getY()+16) {
						setY(getY() + 4);
					}
					if (getY() >= WorldBuilder.link.getY()+16) {
						setY(getY() - 4);
					}
					if (Math.abs(getX()-WorldBuilder.link.getX()) < 13 && Math.abs(getY()-WorldBuilder.link.getY()-12) < 17) {
						fly.cancel();
						fly.purge();
						WorldBuilder.screen.getCollisions().remove(t);
						WorldBuilder.link.setHasboomerang(true);
						SoundEffect.boomerang1.stop();
					}
				}
				
			}
			
		}, 35, 35);
		
		kill.schedule(new TimerTask(){
			@Override
			public void run() {
				concluded = true;
			}
		}, distance);
		
	}
	
	public void Spawn(int dir){ // add projectile to game room and determine move direction + speed.
		
		this.setWidth(16);
		this.setHeight(16);
		WorldBuilder.screen.getCollisions().add(this);
								
		Projectile t = this;
		
			Timer fly = new Timer();
			Timer kill = new Timer();
			fly.schedule(new TimerTask(){
				@Override
				public void run() {			
					if(dir == 1){
						move(0,-4);
					}
					if(dir == 2){
						move(0,4);
					}
					if(dir == 3){
						move(-4,0);
					}
					if(dir == 4){
						move(4,0);
					}
					try{
						
						checkCollisionWithWorldObjects(fly,kill,t);
						
					if (dir != -1) {
						for (Sprite s : WorldBuilder.screen.getCollisions()) {
							if (s != t && (!(s instanceof Projectile)
									|| (((s instanceof Projectile)) && !((Projectile) s).byplayer))) {
								Rectangle r1 = new Rectangle(getX(), getY()+8, getWidth(), getHeight()-8);
								Rectangle r2 = new Rectangle(s.getX(), s.getY(), s.getWidth(), s.getHeight());
								if (r1.intersects(r2)) {
									fly.cancel();
									kill.cancel();
									setImage(null);
									WorldBuilder.screen.getCollisions().remove(t);
									Runtime.getRuntime().gc();
								}
							}
						}
					}
						
					} catch(ConcurrentModificationException e){
						// Added sprite. rechecking!
					} catch (NullPointerException ee){
						setImage(null);
						WorldBuilder.screen.getCollisions().remove(t);
						Runtime.getRuntime().gc();
					}
					setFally(getFally()+1);
				}
			}, 25, 25);
			
			if(!dieOnCollide)
			kill.schedule(new TimerTask(){
				@Override
				public void run() {
					fly.cancel();
					kill.cancel();
					WorldBuilder.screen.getCollisions().remove(t);
				}
			}, 300);
		
	}
	
	public void checkCollisionWithWorldObjects(Timer fly, Timer kill, Projectile t){
		if(isByplayer()){
			//setVisible(false);
			for(WorldItem s: WorldBuilder.screen.getObjects()){
				if(((WorldItem)s).getScript() == WorldItem.Script.BREAK && !s.isUsed()){
					Rectangle r1 = new Rectangle(getX(), getY(), getWidth(), getHeight());
					Rectangle r2 = new Rectangle(s.getX(), s.getY(), s.getWidth(), s.getHeight());
					if(r1.intersects(r2)){
						if(s.getExecute() != null){
							if(!t.isConcluded())
								s.getExecute().run();
							if(!isBounceBack()){
								fly.cancel();
								kill.cancel();
								setImage(null);
								WorldBuilder.screen.getCollisions().remove(t);
							} else {
								setConcluded(true);
							}
							break;
						}
					}
				}
			}
		}
	}

	public ImageIcon getShatter() {
		return shatter;
	}

	public void setShatter(ImageIcon shatter) {
		this.shatter = shatter;
	}

	public ImageIcon getAnimatedImage() {
		return animatedImage;
	}

	public void setAnimatedImage(ImageIcon animatedImage) {
		this.animatedImage = animatedImage;
	}

	public boolean isDieOnCollide() {
		return dieOnCollide;
	}

	public void setDieOnCollide(boolean dieOnCollide) {
		this.dieOnCollide = dieOnCollide;
	}

	public int getFally() {
		return fally;
	}

	public void setFally(int fally) {
		this.fally = fally;
	}

	public boolean isVisible() {
		return visible;
	}

	public void setVisible(boolean visible) {
		this.visible = visible;
	}

	public int getDamage() {
		return damage;
	}

	public void setDamage(int damage) {
		this.damage = damage;
	}

	public boolean isByplayer() {
		return byplayer;
	}

	public void setByplayer(boolean byplayer) {
		this.byplayer = byplayer;
	}

	public boolean isConcluded() {
		return concluded;
	}

	public void setConcluded(boolean concluded) {
		this.concluded = concluded;
	}

	public boolean isBounceBack() {
		return bounceBack;
	}

	public void setBounceBack(boolean bounceBack) {
		this.bounceBack = bounceBack;
	}

	public Stimulus getStimulus() {
		return stimulus;
	}

	public void setStimulus(Stimulus stimulus) {
		this.stimulus = stimulus;
	}

	public Point getTarget() {
		return target;
	}

	public void setTarget(Point target) {
		this.target = target;
	}

	public Timer getFly() {
		return fly;
	}

	public void setFly(Timer fly) {
		this.fly = fly;
	}

	public int getModWidth() {
		return modWidth;
	}

	public void setModWidth(int modWidth) {
		this.modWidth = modWidth;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}
	
	
	
}
