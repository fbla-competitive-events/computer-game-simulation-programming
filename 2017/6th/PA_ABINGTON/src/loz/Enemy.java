package loz;

import java.awt.Point;
import java.awt.Rectangle;
import java.io.FileNotFoundException;
import java.util.ConcurrentModificationException;
import java.util.NoSuchElementException;
import java.util.Timer;
import java.util.TimerTask;
import javax.swing.ImageIcon;

import loz.Player.ItemList;

public class Enemy extends Sprite{

	private int health, speed, aiType, wanderDistance, sightDistance, damage;
	private int invTime;
	private Point origin, target;
	private Timer tick;
	private Timer waiter;
	public boolean inCombat, wandering, gotoPoint, invulnerable, invisible, primed, boss, forcedkill;
	public ImageIcon left, right, down, up, leftm, rightm, downm, upm;
	private String name;
	
	public Enemy(int x, int y){
		this.setOrigin(new Point(x,y));
		init("bird");
		sightDistance = 64; // 4 tiles away
		this.setX(x);
		this.setY(y);
	}
	
	public void initImages(String name){
		left = new ImageIcon("Images\\enemies\\"+name+"\\left.gif");
		right = new ImageIcon("Images\\enemies\\"+name+"\\right.gif");
		down = new ImageIcon("Images\\enemies\\"+name+"\\down.gif");
		up = new ImageIcon("Images\\enemies\\"+name+"\\up.gif");
		this.setWidth(left.getIconWidth());
		this.setHeight(left.getIconHeight());
	}
	
	public int checkSoulList() { // return the value of the soul that the enemy gives.
		int soulReturn = 0;
		
		if (WorldBuilder.screen.isSoulHardened()) {
			if (this.getAiType() == 0) {
				soulReturn = 1;
			}
			if (this.getAiType() == 1) {
				soulReturn = 1;
			}
			if (this.getAiType() == 2) {
				soulReturn = 50;
			}
			if (this.getAiType() == 3) {
				soulReturn = 2;
			}
		}
		System.out.println(soulReturn+" Souls Added");
		return soulReturn;
	}
	
	public void modhp(int hp){
		this.health+=hp;
		if(health<1){
			this.removeSelf();
			try{
				this.getPather().cancel();
				this.getPather().purge();
				this.setPather(null);
			} catch(NullPointerException fg){
				
			}
			
			Sprite die = new Sprite();
			die.setX(getX()+getWidth()/2 - 16);
			die.setY(getY()+getHeight()/2 - 16);
			die.setWidth(32);
			die.setHeight(32);
			die.playAnimation("die", 15, true);
			WorldBuilder.screen.getOverlays().add(die);
			
			if(!forcedkill)
				checkDeathAnimation();
			
			if (!boss && !forcedkill) {
				WorldItem drop = new WorldItem();
				drop.setExecute(null);
				drop.setX(getX());
				drop.setY(getY());
				drop.setWidth(16);
				drop.setHeight(16);
				drop.setTileid(DropTable.generateItemEnemy(drop));
				if (drop.getTileid() != -1) {
					WorldBuilder.screen.getDrops().add(drop);
				}
			}
		}
	}
	
	public void init(String name){
		super.changeImage(359);
		wandering = true;
		gotoPoint = true;
		inCombat = false;
		wanderDistance = 128;
		speed = 2;
		sightDistance = 64; // 4 tiles away
		initImages(name);
		damage = 1;
		health = 2;
		invTime = 750;
		this.setName(name);
	}

	public void buildAI(int id) {
		// build the ai object specific for each id. The id represents a new type of enemy.
		
		if(id == 1){ // test Slime AI
			this.setAiType(1);
			init("minisquish");
		}
		if(id == 2){ // test boss AI
			boss= true;
			this.setAiType(2);
			this.setName("KingSquish");
			init("kingsquish");
			this.setHealth(24);
			invTime = 550;
			speed = 1;
		}
		if(id == 3){ // test boss AI
			this.setAiType(3);
			this.setName("Wyrgle");
			init("wyrgle");
			this.setWandering(false);
			this.setHealth(6);
			invTime = 550;
			speed = 1;
		}
		
	}
	
	public void checkDeathAnimation(){ // hardcoded death scripts
		WorldBuilder.link.addSouls(checkSoulList());
		if(this.getName().equals("Wisp")){
			Sprite make = new Sprite();
			make.setX(getX()-256/2+8);
			make.setY(getY()-256/2+28);
			make.setWidth(32);
			make.setHeight(32);
			make.playAnimation("blueFade", 30, true);
			WorldBuilder.screen.getOverlays().add(0,make);
		}
		if(this.getAiType() == 2){
			
			Music m = new Music(Music.musicURL[11]);
			Music.setActiveMusic(m);
			Music.activeMusic.playMusic(11);
			
			SoundEffect.EnemyDie1.play(false);
			
			Sprite make = new Sprite();
			make.setX(getX()-256/2);
			make.setY(getY()-256/2);
			make.setWidth(32);
			make.setHeight(32);
			make.playAnimation("slimedie", 30, false);
			WorldBuilder.screen.getOverlays().add(0,make);
			
			Timer y = new Timer();
			y.schedule(new TimerTask(){
				@Override
				public void run() {
					Sprite make2 = new Sprite();
					make2.setX(getX()-256/2+32);
					make2.setY(getY()-256/2);
					make2.setWidth(32);
					make2.setHeight(32);
					make2.playAnimation("firework2", 30, true);
					WorldBuilder.screen.getOverlays().add(0,make2);
				}
			}, 300, 300);
			
			WorldBuilder.link.addHeart();
			ItemList getted = Player.ItemList.heart;
			WorldBuilder.screen.displayItemGet(getted);
			WorldBuilder.screen.stopSpeedRun();
			
			waiter.cancel();
			waiter.purge();
			Timer t = new Timer();
			t.schedule(new TimerTask(){
				@Override
				public void run() {
					WorldBuilder.screen.clearProjectiles();
					y.cancel();
					y.purge();
					t.cancel();
					t.purge();
				}
			}, 3000);

			Timer t2 = new Timer();
			t2.schedule(new TimerTask(){
				@Override
				public void run() {
					WorldBuilder.screen.startConversation(WorldBuilder.screen.getConversation().loadConversation("DemoEnd", new Runnable(){
						@Override
						public void run() {
							try {
								WorldBuilder.screen.endSpeedRun();
							} catch (FileNotFoundException e) {
								e.printStackTrace();
							}
						}
					}));
				}
			}, 3000);
			
		}
	}
	
	public void hurtme(int dam){ // damage/heal the enemy - knockback 
		if(!invulnerable){
			
			this.modhp(dam);
			this.setInvulnerable(true);
			Timer blink = new Timer();
			blink.schedule(new TimerTask(){
				@Override
				public void run() {
					setInvisible(!isInvisible());
					if(getImage() == up){
						move(0, 3);
					}
					if(getImage() == down){
						move(0, -3);
					}
					if(getImage() == left){
						move(3, 0);
					}
					if(getImage() == right){
						move(-3, 0);
					}
				}
			}, 30, 30);
			Timer inv = new Timer();
			inv.schedule(new TimerTask(){
				@Override
				public void run() {
					setInvisible(false);
					setInvulnerable(false);
					blink.cancel();
					blink.purge();
				}
			}, invTime);
			//System.err.println("I strike at thee! - "+health);
		}
	}
	
	public void spawn() { // add enemy to room

		WorldBuilder.screen.getEnemies().add(this);

		tick = new Timer();

		tick.schedule(new TimerTask() {

			@Override
			public void run() {
				if (health > 0) {
					// Travel AI
					if (aiType == 0)
						wanderAI();
					if (aiType == 1)
						wanderDangerous();
					if (aiType == 2){
						KingSquishAI();
					}
					if (aiType == 3){
						WyrgleAI();
					}

					// End Travel

					if (!invulnerable) {
						Rectangle r = new Rectangle(WorldBuilder.link.getX() + 4, WorldBuilder.link.getY() + 22, 10,
								10);
						Rectangle r2 = new Rectangle(getX()+6, getY()+6+getHeight()/2, getWidth()-4, getHeight()-4-getHeight()/2);

						if (r.intersects(r2)) {
							WorldBuilder.link.hurtme(-damage);
						}

						try {
							for (Sprite s : WorldBuilder.screen.getCollisions()) {
								if (s instanceof Projectile) {
									r = new Rectangle(s.getX(), s.getY(), 16, 16);
									if (r.intersects(r2) && !((Projectile) s).isConcluded() && ((Projectile) s).isByplayer()) {
										hurtme(-((Projectile) s).getDamage());
										if (!((Projectile) s).isBounceBack()) {
											WorldBuilder.screen.getCollisions().remove(s);
										} else {
											((Projectile) s).setConcluded(true);
										}
										break;
									}
								}
							}
						} catch (ConcurrentModificationException gg) {
						} catch(NoSuchElementException er){
						} catch(NullPointerException ee){ // quick killed
						}
					}
				} else {
					tick = new Timer();
					tick.cancel();
					tick.purge();
					tick = null;
				}
			}
		}, 250, 35);

	}
	
	public void move(int x, int y){
		moveCollideLookout(x,y);
	}
	
	public void wanderAI(){ // Linear Motion, no attack
	
		if(inCombat){
			
			if(this.getX()+this.getWidth()/2<WorldBuilder.link.getX()){ // chase player
				move(speed,0);
				this.setImage(this.getRight());
			}if(this.getX()+this.getWidth()/2>WorldBuilder.link.getX()){
				move(-speed,0);
				this.setImage(this.getLeft());
			}
			if(this.getY()+this.getHeight()/2<WorldBuilder.link.getY()+16){
				move(0,speed);
				this.setImage(this.getDown());
			}if(this.getY()+this.getHeight()/2>WorldBuilder.link.getY()+16){
				move(0,-speed);
				this.setImage(this.getUp());
			}
			
			if(Math.abs(this.getX()-WorldBuilder.link.getX()) >= sightDistance*2 && Math.abs(this.getY()-WorldBuilder.link.getY()+16) >= sightDistance*2){
				inCombat = false;
				target = origin;
				wandering = true;
				gotoPoint = false;
			}
			
		} else if(wandering){
			
			if(Math.abs(this.getX()-WorldBuilder.link.getX()) <= sightDistance && Math.abs(this.getY()-WorldBuilder.link.getY()+16) <= sightDistance){
				inCombat = true;
				wandering = false;
			}
			
			if(target!=null && gotoPoint == false){
				
				if(this.getX()+this.getWidth()/2<target.x){ // goto point
					this.setX(this.getX()+this.speed);
					this.setImage(this.getRight());
				}if(this.getX()+this.getWidth()/2>target.x){
					this.setX(this.getX()-this.speed);
					this.setImage(this.getLeft());
				}
				
				if(this.getY()+this.getHeight()/2<target.y){
					this.setY(this.getY()+this.speed);
					this.setImage(this.getDown());
				}if(this.getY()+this.getHeight()/2>target.y){
					this.setY(this.getY()-this.speed);
					this.setImage(this.getUp());
				}
				
				if(Math.abs(this.getX()-target.getX()) < speed && Math.abs(this.getY()-target.getY()) < speed){
					gotoPoint = true;
				}
				
			}
				
			if(gotoPoint){

				Dice xory = new Dice(2); // find point to move to
				int roll = xory.roll();
				int mod = (int) (Math.random()*wanderDistance)+1;
				Point npoint = null;
				
				if(roll == 1)
					mod=-mod;
					
				roll = xory.roll();
				if(roll == 1)
					npoint = new Point(origin.x+mod, origin.y);
				if(roll == 2)
					npoint = new Point(origin.x, origin.y+mod);
				
				gotoPoint = false;
				target = npoint;
				
			}
			
		}
		
	}
	public void KingSquishAI(){ // Boss AI. Always follows, but can shoot a particle every interval.
		inCombat = true;
		if(inCombat){
			
			if(this.getX()+this.getWidth()/2<WorldBuilder.link.getX()+8){ // chase player
				move(speed,0);
			}if(this.getX()+this.getWidth()/2>WorldBuilder.link.getX()+8){
				move(-speed,0);
			}
			if(this.getY()+this.getHeight()/2<WorldBuilder.link.getY()+16){
				move(0,speed);
			}if(this.getY()+this.getHeight()/2>WorldBuilder.link.getY()+16){
				move(0,-speed);
			}
			
			if(this.getX()+this.getWidth()/2<WorldBuilder.link.getX()+16){ // chase player
				this.setImage(this.getRight());
			}if(this.getX()+this.getWidth()/2>WorldBuilder.link.getX()+16){
				this.setImage(this.getLeft());
			}
			
			if(this.getY()+this.getHeight()/2<WorldBuilder.link.getY()+16 && Math.abs(this.getX()+this.getWidth()/2-WorldBuilder.link.getX()) < 16){
				this.setImage(this.getDown());
			}if(this.getY()+this.getHeight()/2>WorldBuilder.link.getY()+16 && Math.abs(this.getX()+this.getWidth()/2-WorldBuilder.link.getX()) < 16){
				this.setImage(this.getUp());
			}

			if (!primed) {
				if (waiter != null) {
					waiter.cancel();
					waiter.purge();
				}
				waiter = new Timer();
				waiter.schedule(new TimerTask() {
					@Override
					public void run() {
						primed = true;
						
						try{
						waiter.schedule(new TimerTask() {
							@Override
							public void run() {
								recoil(8);
								SoundEffect.slimeShot.play(false);
								Projectile shoot = new Projectile(new ImageIcon(""), new ImageIcon(""));
								shoot.setWidth(32);
								shoot.setHeight(32);
								shoot.setX(getX() + getWidth() / 2);
								shoot.setY(getY() + getHeight() / 2);
								shoot.setByplayer(false);
								shoot.spawnPlayerTarget("squisha");
								primed = false;
							}
						}, 3000);
						} catch (IllegalStateException i){
							// enemy should already be dead then
						}
					}
				}, 25);
			}
		}
	}
	
	public void WyrgleAI(){ // Never follows, but can shoot a particle every interval, and will teleport after every projectile shot.
		inCombat = true;
		if(inCombat){

			if (!wandering) {
				if (this.getX() + this.getWidth() / 2 < WorldBuilder.link.getX() + 16) { // look
																							// at
																							// player
					this.setImage(this.getRight());
				}
				if (this.getX() + this.getWidth() / 2 > WorldBuilder.link.getX() + 16) {
					this.setImage(this.getLeft());
				}

				if (this.getY() + this.getHeight() / 2 < WorldBuilder.link.getY() + 16
						&& Math.abs(this.getX() + this.getWidth() / 2 - WorldBuilder.link.getX() - 8) < 16) { // look
																											// at
																											// player
					this.setImage(this.getDown());
				}
				if (this.getY() + this.getHeight() / 2 > WorldBuilder.link.getY() + 16
						&& Math.abs(this.getX() + this.getWidth() / 2 - WorldBuilder.link.getX() - 8) < 12) {
					this.setImage(this.getUp());
				}
				
			}
			
			if (!primed) { // is ready to attack
				if (waiter != null) {
					waiter.cancel();
					waiter.purge();
					waiter = new Timer();
				}
				waiter = new Timer();
				waiter.schedule(new TimerTask() {
					@Override
					public void run() {
						primed = true;
						waiter.schedule(new TimerTask() {
							@Override
							public void run() {
								wandering = true;
								Projectile shoot = new Projectile(new ImageIcon(""), new ImageIcon("")); // spawn projectile
								shoot.setWidth(32);
								shoot.setHeight(32);
								shoot.setX(getX() + getWidth() / 2 - 32);
								shoot.setY(getY() + getHeight() / 2 - 32);
								shoot.setByplayer(false);
								shoot.setCanRotate(true);
								
								Sprite cast = new Sprite();
								cast.setX(getX()+getWidth()/2-32);
								cast.setY(getY()+getHeight()/2-32);
								cast.setWidth(32);
								cast.setHeight(32);
								cast.playAnimation("spell2", 30, false);
								setInvulnerable(true);
								SoundEffect.cast.play(false);
								
								WorldBuilder.screen.getOverlays().add(cast);
								
								waiter.schedule(new TimerTask() {
									@Override
									public void run() {
										
										Timer blink = new Timer();
										blink.schedule(new TimerTask(){
											@Override
											public void run() {
												setInvisible(!isInvisible());
											}
										}, 40, 40);
										Timer inv = new Timer();
										inv.schedule(new TimerTask(){
											@Override
											public void run() {
												setInvisible(false);
												setInvulnerable(false);
												blink.cancel();
												blink.purge();
												inv.cancel();
												inv.purge();
											}
										}, 500);
										
										cast.setImage(new ImageIcon(""));
										cast.setX(-1000);
										cast.getPather().cancel();
										cast.getPather().purge();
										cast.setPather(new Timer());
										SoundEffect.wizShot3.play(false,6);
										if(getImage() == getUp()){ // display cast image & Pick image to use. Travel 1000 pixels left, up, down, or right before dying.
											shoot.setOffSetX(20);
											setImage(new ImageIcon("Images\\Enemies\\Wyrgle\\upcast.gif"));
											shoot.spawnTarget("mpu",new Point(shoot.getX(),shoot.getY()-1000));
										}
										if(getImage() == getDown()){
											shoot.setOffSetX(20);
											shoot.spawnTarget("mpd",new Point(shoot.getX(),shoot.getY()+1000));
											setImage(new ImageIcon("Images\\Enemies\\Wyrgle\\downcast.gif"));
										}
										if(getImage() == getLeft()){
											shoot.setOffSetY(10);
											shoot.spawnTarget("mpl",new Point(shoot.getX()-1000,shoot.getY()));
											setImage(new ImageIcon("Images\\Enemies\\Wyrgle\\leftcast.gif"));
										}
										if(getImage() == getRight()){
											shoot.setOffSetX(26);
											shoot.setOffSetY(6);
											shoot.spawnTarget("mpr",new Point(shoot.getX()+1000,shoot.getY()));
											setImage(new ImageIcon("Images\\Enemies\\Wyrgle\\rightcast.gif"));
										}	
										
										waiter.schedule(new TimerTask() {
											@Override
											public void run() {
												setInvulnerable(false);
												if(getImage() == new ImageIcon("Images\\Enemies\\Wyrgle\\upcast.gif")) // display cast image
													setImage(getUp());
												if(getImage() == new ImageIcon("Images\\Enemies\\Wyrgle\\downcast.gif"))
													setImage(getDown());
												if(getImage() == new ImageIcon("Images\\Enemies\\Wyrgle\\leftcast.gif"))
													setImage(getLeft());
												if(getImage() == new ImageIcon("Images\\Enemies\\Wyrgle\\rightcast.gif"))
													setImage(getRight());
												
												wandering = false;
												setX((int) (getOrigin().x - 32 + Math.random()*32));
												setY((int) (getOrigin().y - 32 + Math.random()*32));
												
												waiter.schedule(new TimerTask() {
													@Override
													public void run() {
														primed = false;
													}
												}, 500);
												
											}
										}, 1400);
										
									}
								}, 1400);
								
							}
						}, 3000);
					}
				}, 25);
			}
		}
	}
	
	public void wanderDangerous(){ // no attack, threat on touch
		
		if(wandering){
			
			if(target!=null && gotoPoint == false){
				
				if(this.getX()<target.x+8){ // goto point
					move(this.speed,0);
					this.setImage(this.getRight());
				}if(this.getX()>target.x-8){
					move(-this.speed,0);
					this.setImage(this.getLeft());
				}
				
				if(this.getY()<target.y+8){
					move(0,this.speed);
					this.setImage(this.getDown());
				}if(this.getY()>target.y-8){
					move(0,-this.speed);
					this.setImage(this.getUp());
				}
				
				if(Math.abs(this.getX()-target.getX()) < speed && Math.abs(this.getY()-target.getY()) < speed){
					waiter = new Timer(); // wait to continue
					final long wait = (long) (Math.random()*1500+250);
					waiter.schedule(new TimerTask(){
						@Override
						public void run() {
							try{
								gotoPoint = true;
								waiter.cancel();
								waiter.purge();
							} catch (NullPointerException n){
								
							}
						}
					}, wait);
				}
				
			}
				
			if(gotoPoint){
				
				Dice xory = new Dice(2); // find point to move to
				int roll = xory.roll();
				int mod = (int) (Math.random()*wanderDistance)+1;
				Point npoint = null;
				
				if(roll == 1)
					mod=-mod;
					
				roll = xory.roll();
				if(roll == 1)
					npoint = new Point(origin.x+mod, origin.y);
				if(roll == 2)
					npoint = new Point(origin.x, origin.y+mod);
				
				gotoPoint = false;
				target = npoint;
			
				waiter = new Timer();
				waiter.schedule(new TimerTask(){
					@Override
					public void run() {
						gotoPoint = true;
						waiter.cancel();
						waiter.purge();
					}
				}, 2000);
				
			}
			
		}
		
	}
	
	public void recoil(int i) {
		
		if(i>15){ // 16 or greater will jump over grid locations and avoid collision.
			i=15;
		}
		
		if(getImage() == up){
			move(0,i);
		}
		if(getImage() == down){
			move(0,-i);
		}
		if(getImage() == left){
			move(i,0);
		}
		if(getImage() == right){
			move(-i,0);
		}
		
		Timer recursive = new Timer();
		
		i=i-1;
		final int r = i;
		
		recursive.schedule(new TimerTask(){
			@Override
			public void run() {
				if(r>0){
					recoil(r);
				}
			}
		}, 30);
		
	}

	public void moveCollideLookout(int gx, int gy) { // move player, watch for
														// collisions of
		// all types.
		try {
			boolean fail = true;

			Rectangle r = new Rectangle(getX() + gx + 2, getY() +getHeight()/2 + gy, this.getLeft().getIconWidth(), this.getLeft().getIconHeight()/2); 
			// all images should be the same width & height.
			
			try {
				for (Sprite s : WorldBuilder.screen.getCollisions()) {
					Sprite col = s;
					if (col != null) {

						Rectangle r2 = new Rectangle(0, 0, 0, 0);
						if (col.getCollision() == collisionType.FULL)
							r2 = new Rectangle(col.getX(), col.getY(), col.getWidth(), col.getHeight());
						if (col.getCollision() == collisionType.TOP)
							r2 = new Rectangle(col.getX(), col.getY(), col.getWidth(), col.getHeight() / 2);
						if (col.getCollision() == collisionType.BOTTOM)
							r2 = new Rectangle(col.getX(), col.getY() + 8, col.getWidth(), col.getHeight() / 2);
						if (col.getCollision() == collisionType.LEFT)
							r2 = new Rectangle(col.getX(), col.getY(), col.getWidth() / 2 + 1, col.getHeight());
						if (col.getCollision() == collisionType.RIGHT)
							r2 = new Rectangle(col.getX() + 6, col.getY(), col.getWidth() / 2 + 1, col.getHeight());
						if (col.getCollision() == collisionType.BOTTOMLEFTD) {

							r2 = new Rectangle(col.getX(), col.getY(), col.getWidth(), col.getHeight());

							int xo = r.x - col.getX();
							if (xo < 0) {
								xo = 0;
							}

							r2 = new Rectangle(col.getX(), col.getY() + xo, col.getWidth(), col.getHeight() - xo);

						}
						if (col.getCollision() == collisionType.BOTTOMRIGHTD) {

							r2 = new Rectangle(col.getX(), col.getY(), col.getWidth(), col.getHeight());

							int xo = col.getX() - r.x;
							if (xo < 0) {
								xo = 0;
							}

							r2 = new Rectangle(col.getX() - 2, col.getY() + xo, col.getWidth(), col.getHeight() - xo);

						}
						if (col.getCollision() == collisionType.TOPLEFTD) {

							r2 = new Rectangle(col.getX(), col.getY(), col.getWidth(), col.getHeight());

							int xo = col.getX() - r.x;
							if (xo < 0) {
								xo = 0;
							}

							r2 = new Rectangle(col.getX(), col.getY() - xo, col.getWidth(), col.getHeight() + xo);

						}
						if (col.getCollision() == collisionType.TOPRIGHTD) {

							r2 = new Rectangle(col.getX() - 1, col.getY(), col.getWidth(), col.getHeight());

							int xo = col.getX() - r.x;
							if (xo < 0) {
								xo = 0;
							}

							r2 = new Rectangle(col.getX(), col.getY() + xo, col.getWidth(), col.getHeight() - xo);

						}
						if (!(r.intersects(r2))) {
							fail = false;
						} else if (r.intersects(r2)) {
							fail = true;
							break;
						}
					}
				}
			} catch (ConcurrentModificationException e) {
				// Adding Sprite
			}
			if (!fail) {
				setX(getX() + gx);
				setY(getY() + gy);
			}

		} catch (NullPointerException npe) {

		}
	}

	public void removeSelf(){
		WorldBuilder.screen.getEnemies().remove(this);
		tick.cancel();
		tick.purge();
		tick = null;
	}
	
	public int getHealth() {
		return health;
	}
	
	public void setHealth(int health) {
		this.health = health;
	}
	
	public int getSpeed() {
		return speed;
	}
	
	public void setSpeed(int speed) {
		this.speed = speed;
	}
	
	public int getAiType() {
		return aiType;
	}

	public void setAiType(int aiType) {
		this.aiType = aiType;
	}

	public Point getOrigin() {
		return origin;
	}

	public void setOrigin(Point origin) {
		this.origin = origin;
	}

	public int getWanderDistance() {
		return wanderDistance;
	}

	public void setWanderDistance(int wanderDistance) {
		this.wanderDistance = wanderDistance;
	}

	public Timer getTick() {
		return tick;
	}

	public void setTick(Timer tick) {
		this.tick = tick;
	}

	public boolean isInCombat() {
		return inCombat;
	}

	public void setInCombat(boolean inCombat) {
		this.inCombat = inCombat;
	}

	public boolean isWandering() {
		return wandering;
	}

	public void setWandering(boolean wandering) {
		this.wandering = wandering;
	}

	public boolean isGotoPoint() {
		return gotoPoint;
	}

	public void setGotoPoint(boolean gotoPoint) {
		this.gotoPoint = gotoPoint;
	}

	public Point getTarget() {
		return target;
	}

	public void setTarget(Point target) {
		this.target = target;
	}

	public int getSightDistance() {
		return sightDistance;
	}

	public void setSightDistance(int sightDistance) {
		this.sightDistance = sightDistance;
	}

	public ImageIcon getLeft() {
		return left;
	}

	public void setLeft(ImageIcon left) {
		this.left = left;
	}

	public ImageIcon getRight() {
		return right;
	}

	public void setRight(ImageIcon right) {
		this.right = right;
	}

	public ImageIcon getDown() {
		return down;
	}

	public void setDown(ImageIcon down) {
		this.down = down;
	}

	public ImageIcon getUp() {
		return up;
	}

	public void setUp(ImageIcon up) {
		this.up = up;
	}

	public int getDamage() {
		return damage;
	}

	public void setDamage(int damage) {
		this.damage = damage;
	}

	public int getInvTime() {
		return invTime;
	}

	public void setInvTime(int invTime) {
		this.invTime = invTime;
	}

	public boolean isInvulnerable() {
		return invulnerable;
	}

	public void setInvulnerable(boolean invulnerable) {
		this.invulnerable = invulnerable;
	}

	public boolean isInvisible() {
		return invisible;
	}

	public void setInvisible(boolean invisible) {
		this.invisible = invisible;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public ImageIcon getLeftm() {
		return leftm;
	}

	public void setLeftm(ImageIcon leftm) {
		this.leftm = leftm;
	}

	public ImageIcon getRightm() {
		return rightm;
	}

	public void setRightm(ImageIcon rightm) {
		this.rightm = rightm;
	}

	public ImageIcon getDownm() {
		return downm;
	}

	public void setDownm(ImageIcon downm) {
		this.downm = downm;
	}

	public ImageIcon getUpm() {
		return upm;
	}

	public void setUpm(ImageIcon upm) {
		this.upm = upm;
	}

	public Timer getWaiter() {
		return waiter;
	}

	public void setWaiter(Timer waiter) {
		this.waiter = waiter;
	}

	public boolean isPrimed() {
		return primed;
	}

	public void setPrimed(boolean primed) {
		this.primed = primed;
	}

	public boolean isBoss() {
		return boss;
	}

	public void setBoss(boolean boss) {
		this.boss = boss;
	}

	public boolean isForcedkill() {
		return forcedkill;
	}

	public void setForcedkill(boolean forcedkill) {
		this.forcedkill = forcedkill;
	}
	
}
