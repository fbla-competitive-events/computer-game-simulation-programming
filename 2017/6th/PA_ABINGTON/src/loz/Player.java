package loz;

import java.awt.Point;
import java.awt.Rectangle;
import java.util.ArrayList;
import java.util.ConcurrentModificationException;
import java.util.Timer;
import java.util.TimerTask;
import javax.swing.ImageIcon;

import particleForge.Particle;

public class Player extends Sprite{

	private int health, maxhealth, rupees, bombs, arrows, arrowsMax, offsetX,
	bombmax, x, y, gripPower;
	private double armor;
	public static final int maxrupees = 9999;
	private boolean walkUp, walkDown, walkLeft, walkRight,
	lastLeft, lastRight, lastUp, lastDown, lifting, swinging,
	invulnerable, invisible, solid, hasboomerang = true, boostedSpeed;
	private int lastLiftID, invTime, shieldlevel, swordlevel;
	private Timer mover = new Timer();
	private int speed = 2, swingFrame, slow, souls;
	private float magic, maxmagic;
	
	private Light glow;
	
	private Sprite overrideImage = null;
	
	private ImageIcon IdleL = new ImageIcon("Images\\people\\linkl.png"); // Standard player animation & idle quick references
	private ImageIcon IdleR = new ImageIcon("Images\\people\\linkr.png");
	private ImageIcon IdleD = new ImageIcon("Images\\people\\linkd.png");
	private ImageIcon IdleU = new ImageIcon("Images\\people\\linku.png");
	
	private ImageIcon AnimL = new ImageIcon("Images\\people\\linkl.gif");
	private ImageIcon AnimR = new ImageIcon("Images\\people\\linkr.gif");
	private ImageIcon AnimD = new ImageIcon("Images\\people\\linkd.gif");
	private ImageIcon AnimU = new ImageIcon("Images\\people\\linku.gif");
	
	private ItemList equippedItem;
	public static int itemCount = 0, equipmentCount = 0;
	public int equippedInt = -1; 
	public static ArrayList<ItemList> itemArray = new ArrayList<ItemList>();
	public static ArrayList<EquipmentList> equipArray = new ArrayList<EquipmentList>();
	
	public enum ItemList {
		
		// All Usable Game Items & Chest Drops
		Trident(4, "projectiles\\", "Magic Trident"), // String 1 references the directory where the icon is stored 
		Boomerang(1, "projectiles\\", "Boomerang"), //   String 2 references the name of the item
		Boomerangr(2, "projectiles\\", "Red Boomerang"),
		Boomerangr2(4, "projectiles\\", "Magic Boomerang"),
		ShadowConduit(1, "Special\\", "Shadow Conduit"),
		Blink(1,"Special\\","Test Teleporter"),
		teleport(1,"Special\\","I am become death"),
		Glove(1,"Special\\","Power Glove"),
		starburst(1, "Special\\","Sonos Medallion"),
		greenburst(1, "Special\\","Erdos Medallion"),
		redburst(1, "Special\\","Flamos Medallion"),
		firework(1, "Special\\","Firework Launcher"),
		thrrup(1, "","300RupeesYUHAVETHIS?"),
		heart(1, "","FullHeartPiece"),
		branch(1, "Special\\","No, dont touch that!"),
		shield2(1, "","shield2"),
		shield3(1, "","shield3"),
		potion(1, "Special\\","Noob Potion For Those That Have Died Before"),
		epotion(1, "Special\\","So Sad"),
		BK(1, "","bk"),
		lenny(1, "","LF"),
		Bow(2, "Special\\"),
		Nothing(0, "", "You have nothing.");
		
		private Object value;
		private String url, name;
		private boolean unlocked;
		
		ItemList(Object o, String icon){
			this(o,icon,"");
			this.setName(this.toString());
		}
		
		ItemList(Object o, String icon, String name){
			this.setUrl(icon);
			this.setValue(o);
			itemCount++;
			this.setName(name);
		}
		
		public void unlock(){
			itemArray.add(this);
			this.setUnlocked(true);
		}
		
		public Object getValue(){
			return value;
		}

		public String getUrl() {
			return "Images\\"+url+this.toString()+"1.png"; // Example: "Images\\projectiles\\Trident1.png"
		}
		
		public void setValue(Object o){
			value = o;
		}

		public void setUrl(String url) {
			this.url = url;
		}

		public boolean isUnlocked() {
			return unlocked;
		}

		public void setUnlocked(boolean unlocked) {
			this.unlocked = unlocked;
			WorldBuilder.link.setEquippedItem(itemArray.get(itemArray.size()-1));
		}

		public String getName() {
			return name;
		}

		public void setName(String name) {
			this.name = name;
		}

		public void lock() {
			this.unlocked = false;
			for(int i = 0; i<itemArray.size(); i++){
				if(WorldBuilder.link.equippedItem == this){
					if(itemArray.get(i).toString() == this.toString())
						itemArray.remove(this);
					if(itemArray.size()>0 && itemArray.get(0)!=null)
						WorldBuilder.link.setEquippedItem(itemArray.get(0));
					else
						WorldBuilder.link.setEquippedItem(ItemList.Nothing);
				}
			}
		}
		
		public void replace(ItemList item){
			this.setUnlocked(true);
			for(int i = 0; i<itemArray.size(); i++){
				if(itemArray.get(i) == this){
					itemArray.set(i, item);
					WorldBuilder.link.setEquippedItem(this);
				}
			}
		}
		
	}
	
	public enum EquipmentList {
		
		Shield1(1,"Special\\","Iron Shield"),
		Tunic1(1,"Special\\","Green Tunic"),
		Sword1(1,"Special\\","Iron Sword"),
		Glove(1,"Special\\","Power Glove");
		
		private Object value;
		private String url, name;
		private boolean unlocked;
		
		EquipmentList(Object o, String icon){
			this(o,icon,"");
			this.setName(this.toString());
		}
		
		EquipmentList(Object o, String icon, String name){
			this.setUrl(icon);
			this.setValue(o);
			itemCount++;
			this.setName(name);
		}
		
		public void unlock(){
			equipArray.add(this);
			this.setUnlocked(true);
		}
		
		public Object getValue(){
			return value;
		}

		public String getUrl() {
			return "Images\\"+url+this.toString()+"1.png";
		}
		
		public void setValue(Object o){
			value = o;
		}

		public void setUrl(String url) {
			this.url = url;
		}

		public boolean isUnlocked() {
			return unlocked;
		}

		public void setUnlocked(boolean unlocked) {
			this.unlocked = unlocked;
		}

		public String getName() {
			return name;
		}

		public void setName(String name) {
			this.name = name;
		}

		public void lock() {
			this.unlocked = false;
			equipArray.remove(this);
		}
		
		public void replace(EquipmentList item){
			this.setUnlocked(true);
			for(int i = 0; i<equipArray.size(); i++){
				if(equipArray.get(i) == this){
					equipArray.set(i, item);
				}
			}
		}
		
	}
	
	public void respawn(){
		health = (int) (maxhealth/1.5);
		magic = 20;
		swingFrame = 0;
		lifting = false;
		swinging = false;
		x = 248;
		y = 164;
		this.setImage(this.getIdleD());
		mover.cancel();
		mover.purge();
		mover = new Timer();
		mover.schedule(new TimerTask(){
			@Override
			public void run() {
				if(!swinging){
				if(walkUp){
					if(!isMovingExclude(1)){
						setImage(AnimU);
					}
					move(0,-speed+slow);
				}if(walkDown){
					if(!isMovingExclude(2)){
						setImage(AnimD);
					}
					move(0,speed-slow);
				}if(walkLeft){
					if(!isMovingExclude(3)){
						setImage(AnimL);
					}
					move(-speed+slow,0);
				}if(walkRight){
					if(!isMovingExclude(4)){
						setImage(AnimR);
					}
					move(speed-slow,0);
				}

			}
				WorldBuilder.frame.repaint();
			}
		}, 25,25);
		this.setImage(IdleD);
	}
	
	public void reset(){
		solid = true;
		armor = 0;
		health = 6;
		maxhealth = 6;
		rupees = 0;
		
		bombmax = 100;// test
		arrowsMax = 100;//
		arrows=100;//
		bombs=100; // test
		
		magic = 20;
		maxmagic = 20;
		swingFrame = 0;
		invTime = 1000;
		shieldlevel = 1;
		swordlevel = 1;
		lifting = false;
		swinging = false;
		gripPower = 1;
		
		this.setEquippedItem(ItemList.Nothing);
		EquipmentList.Sword1.unlock();    
		EquipmentList.Shield1.unlock();
		EquipmentList.Tunic1.unlock(); 
		
		x = 248;
		y = 164;
		this.setImage(this.getIdleD());
		mover.cancel();
		mover.purge();
		mover = new Timer();
		mover.schedule(new TimerTask(){
			@Override
			public void run() {
				if(!swinging){
				if(walkUp){
					if(!isMovingExclude(1)){
						setImage(AnimU);
					}
					move(0,-speed+slow);
				}if(walkDown){
					if(!isMovingExclude(2)){
						setImage(AnimD);
					}
					move(0,speed-slow);
				}if(walkLeft){
					if(!isMovingExclude(3)){
						setImage(AnimL);
					}
					move(-speed+slow,0);
				}if(walkRight){
					if(!isMovingExclude(4)){
						setImage(AnimR);
					}
					move(speed-slow,0);
				}

			}
				WorldBuilder.frame.repaint();
			}
		}, 20,20);
		this.setImage(IdleD);
	}
	
	public Player(){
		reset();
	}
	
	public void addHeart(){
		this.maxhealth+=2;
		this.heal(maxhealth-health);
	}
	
	public void resetOverrider(){
		setOverrideImage(null); // get hit and reset. Become invulnerable for a short time.
		hurtme(0);
		costMagic(1);
	}
	
	public void cast(Projectile p, String url, long castTime){
		
		Sprite s = new Sprite();
		s.playAnimation(url, 30, false);
		s.followPlayer(-8, -8);
		WorldBuilder.screen.getOverlays().add(s);
		WorldBuilder.screen.setCantWalk(true);
		
		Timer shoot = new Timer();
		shoot.schedule(new TimerTask(){
			@Override
			public void run() {
				s.getAnimate().cancel();
				s.getPather().cancel();
				s.setX(-999);
				s.free();
				throwProjectileAnimated(getDir(), p.getName(), p.getDamage());
				shoot.cancel();
				shoot.purge();
				WorldBuilder.screen.setCantWalk(false);
			}
		}, castTime);
		
	}
	
	public void checkOverrider(){
		
		if (this.getOverrideImage() == null) {
			Sprite s = new Sprite();
			s.playAnimation("gcirc", 30, false);
			s.setId(9001);
			setSpeed(getSpeed()+4);
			this.setOverrideImage(s);
			Timer g = new Timer();
			g.schedule(new TimerTask(){
				@Override
				public void run() {
					if(!costMagic(0.05f) || getOverrideImage() == null){
						setOverrideImage(null);
						g.cancel();
						g.purge();
						Sprite make = new Sprite();
						make.setX(getX()-512/2+8);
						make.setY(getY()-512/2+16);
						make.setWidth(32);
						make.setHeight(32);
						make.playAnimation("ostarburst", 30, true);
						WorldBuilder.screen.getOverlays().add(make);
						setSpeed(getSpeed()-4);
					}
				}
			}, 50, 50);
		} else if(this.getOverrideImage().getId() == 9001) {
			this.getOverrideImage().getAnimate().cancel();
			this.setOverrideImage(null);
			Sprite make = new Sprite();
			make.setX(getX()-512/2+8);
			make.setY(getY()-512/2+16);
			make.setWidth(32);
			make.setHeight(32);
			make.playAnimation("ostarburst", 30, true);
			WorldBuilder.screen.getOverlays().add(make);
		} else {
			this.getOverrideImage().getAnimate().cancel();
			this.setOverrideImage(null);
		}
	}
	
	public void useWeapon(){ // determine weapon use
		if(getEquippedItem() == ItemList.Trident){
			if(costMagic(2)){
				throwProjectile(getDir(), "trident", (int)ItemList.Trident.getValue()).startEmitterTextured(5, 64, 50, 10, 0.40f, true, new ImageIcon("Images\\particles\\redcross.png"));
				SoundEffect.trident.play(false);
			}
		}
		
		if(getEquippedItem() == ItemList.branch){
				SoundEffect.branch.play(false);
				ItemList.branch.setName("LT Branch");
		}
		
		if(getEquippedItem() == ItemList.potion){
			modhp(6);
			ItemList.potion.replace(ItemList.epotion);
			ItemList.potion.replace(ItemList.epotion);
		}
		
		if(getEquippedItem() == ItemList.teleport){
			if(getMagic() > 1)
				checkOverrider();
		}
		
		if(getEquippedItem() == ItemList.Bow){
			if(costArrows(1)){
				stopWalking();
				Timer doit = new Timer();
				doit.schedule(new TimerTask(){
					@Override
					public void run() {
						SoundEffect.arrow.play(false);
						throwProjectile(getDir(), "bow", 2);
						setImage(new ImageIcon("Images\\people\\"+getImage().toString().substring(14, 19)+".png"));
						WorldBuilder.screen.setCantWalk(false);
						setOffsetX(0);
						returnToIdle();
					}
				}, 500);
				WorldBuilder.screen.setCantWalk(true);
				setImage(new ImageIcon("Images\\people\\"+getImage().toString().substring(14, 19)+"bow.png"));
				
				if(getImage().toString().equals("Images\\people\\linklbow.png")){
					this.setOffsetX(-20);
				}
				if(getImage().toString().equals("Images\\people\\linkubow.png")){
					this.setOffsetX(-5);
				}
				
			}
		}
		
		if(getEquippedItem() == ItemList.Boomerang && isHasboomerang()){
			throwBoomerang(getDir(),1,1);
			SoundEffect.boomerang1.play(true);
			this.setHasboomerang(false);
		}
		if(getEquippedItem() == ItemList.Boomerangr && isHasboomerang()){
			throwBoomerang(getDir(),2,2);
			SoundEffect.boomerang1.play(true);
			this.setHasboomerang(false);
		}
		if(getEquippedItem() == ItemList.Boomerangr2 && isHasboomerang()){
			throwBoomerang(getDir(),4,4);
			SoundEffect.boomerang1.play(true);
			this.setHasboomerang(false);
		}
		if(getEquippedItem() == ItemList.Blink){
			teleport(getDir(),true);
		}
		
		if(getEquippedItem() == ItemList.starburst){
			Sprite make = new Sprite();
			make.setX(getX()-512/2+8);
			make.setY(getY()-512/2+16);
			make.setWidth(32);
			make.setHeight(32);
			make.playAnimation("ostarburst", 30, true);
			WorldBuilder.screen.getOverlays().add(make);
		}
		if(getEquippedItem() == ItemList.greenburst){
			Sprite make = new Sprite();
			make.setX(getX()-128/2+8);
			make.setY(getY()-128/2+16);
			make.setWidth(32);
			make.setHeight(32);
			make.playAnimation("greenburst", 20, true);
			WorldBuilder.screen.getOverlays().add(make);
		}
		if(getEquippedItem() == ItemList.redburst){
			Sprite make = new Sprite();
			make.setX(getX()-256/2+8);
			make.setY(getY()-256/2+28);
			make.setWidth(32);
			make.setHeight(32);
			make.playAnimation("redstar", 30, true);
			WorldBuilder.screen.getOverlays().add(make);
		}
		
		if(getEquippedItem() == ItemList.firework){
			
			SoundEffect.firework.play(false);
			Projectile f = new Projectile(new ImageIcon("Images\\Special\\firework1.png"),new ImageIcon("Images\\Special\\firework1.png"));
			f.launch(new Runnable(){

				@Override
				public void run() {
					
					Sprite make = new Sprite();
					make.setX(f.getX()-256/2);
					make.setY(f.getY()-256/2);
					make.setWidth(32);
					make.setHeight(32);
					make.playAnimation("firework2", 30, true);
					WorldBuilder.screen.getOverlays().add(make);
					
				}
				
			});
			
		}
		
		if(getEquippedItem() == ItemList.ShadowConduit){
			if (!this.boostedSpeed && costMagic(12)) {
				boostedSpeed = true;
				SoundEffect.cast.play(false);

				setSpeed(getSpeed()+2);

				Timer buff = new Timer();
				buff.schedule(new TimerTask(){

					@Override
					public void run() {
						setSpeed(getSpeed()-2);
						boostedSpeed = false;
					}
					
				}, 8000);
			
			}
		}
	}
	
	private void teleport(int dir, boolean skip) {
		boolean fail = false;
		if (!skip) {
			for (int i = 0; i < WorldBuilder.screen.getCollisions().size(); i++) {
				if (willCollide(dir, new Rectangle(getX(), getY(), 20, 20),
						new Rectangle(WorldBuilder.screen.getCollisions().get(i).getX(),
								WorldBuilder.screen.getCollisions().get(i).getY(), 16, 16))) {
					fail = true;
				}
			}
		}
		if(!fail){
			if(dir == 1){
				this.setY(this.getY()-128);
			}
			if(dir == 2){
				this.setY(this.getY()+128);
			}
			if(dir == 3){
				this.setX(this.getX()-128);
			}
			if(dir == 4){
				this.setX(this.getX()+128);
			}
		}
	}
	
	public boolean willCollide(int dir, Rectangle one, Rectangle two){
		
		if(dir == 1){
			one.translate(0, -128);
		}
		if(dir == 2){
			one.translate(0, 128);
		}
		if(dir == 3){
			one.translate(-128, 0);
		}
		if(dir == 4){
			one.translate(128, 0);
		}
		if(one.intersects(two)){
			return true;
		}
		return false;
	}

	public void createTrailingParticleSystem(long duration, ImageIcon particle, int particleCount, long life, float disperseSpeed, boolean behind){
		Sprite a = new Sprite(new ImageIcon("Images\\icon.png"));
		a.setOrigin(new Point(getX(), getY()));
		a.startEmitterTextured(particleCount, 32, life, 0, disperseSpeed, behind, particle);
		
		Timer stayontarget = new Timer();
		stayontarget.schedule(new TimerTask(){
			@Override
			public void run() {
				a.setX((int) getX() - 24);
				a.setY((int) getY() - 12);
				a.setOrigin(new Point(getX(), getY()));
			}
		}, 35, 35);
		
		Timer buff = new Timer();
		buff.schedule(new TimerTask(){

			@Override
			public void run() {
				stayontarget.cancel();
				a.setImage(null);
				stayontarget.purge();
			}
			
		}, duration);
		
	}
	
	public boolean costMagic(float magic){
		if(this.magic>99) return true;
		if(this.getMagic()-magic>=-1){
			this.modmagic(-magic);
			return true;
		}
		SoundEffect.failMagic.play(false);
		return false;
	}
	
	public boolean costArrows(int arrows){
		if(this.arrows>99) return true;
		if(this.getArrows()-arrows>=0){
			this.modarrows(-arrows);
			return true;
		}
		//SoundEffect.failMagic.play(false);
		return false;
	}
	
	public int getDir(){ // return int representing face direction
		
		if(this.getImage() == this.AnimU || this.getImage() == this.IdleU
				|| this.getImage().toString().substring(14,19).equals("linku")){
			return 1;
		}
		if(this.getImage() == this.AnimD || this.getImage() == this.IdleD
				|| this.getImage().toString().substring(14,19).equals("linkd")){
			return 2;
		}
		if(this.getImage() == this.AnimL || this.getImage() == this.IdleL
				|| this.getImage().toString().substring(14,19).equals("linkl")){
			return 3;
		}
		if(this.getImage() == this.AnimR || this.getImage() == this.IdleR
				|| this.getImage().toString().substring(14,19).equals("linkr")){
			return 4;
		}
		return -1;
	}
	
	public Projectile throwProjectile(int dir, String pname, int damage){ // 1 = up, 2 = down, 3 = left, 4 = right
		
		Projectile pnew = new Projectile(new ImageIcon("Images\\projectiles\\"+pname+""+dir+".png"),new ImageIcon("Images\\projectiles\\"+pname+""+dir));
		pnew.setDamage(damage);
		pnew.setWidth(16);
		pnew.setHeight(16);
		pnew.setX(getX()+dirIntX());
		pnew.setY(getY()+16);
		pnew.setDieOnCollide(true);
		pnew.setByplayer(true);
		pnew.Spawn(dir);
		return pnew;
		
	}
	
	public Projectile throwProjectileAnimated(int dir, String pname, int damage){ // 1 = up, 2 = down, 3 = left, 4 = right
		
		Projectile pnew = new Projectile(new ImageIcon("Images\\projectiles\\"+pname+""+dir+".png"),new ImageIcon("Images\\projectiles\\"+pname+""+dir));
		pnew.setDamage(damage);
		pnew.setWidth(16);
		pnew.setHeight(16);
		pnew.setX(getX()+dirIntX()-4);
		pnew.setY(getY()+16);
		pnew.setDieOnCollide(true);
		pnew.setByplayer(true);
		pnew.Spawn(dir);
		pnew.playAnimation(pname, 30, false);
		return pnew;
		
	}
	
	public Projectile throwBoomerang(int dir, int level, int damage){
		if (isHasboomerang()) {
			Projectile pnew = new Projectile(new ImageIcon("Images\\projectiles\\boomerang" + level + ".gif"),new ImageIcon(""));
			pnew.setDamage(level);
			pnew.setWidth(16);
			pnew.setHeight(16);
			pnew.setX(getX() + dirIntX());
			pnew.setY(getY() + 16);
			pnew.setDieOnCollide(true);
			pnew.setByplayer(true);
			pnew.setBounceBack(true);
			pnew.spawnBoomerang(dir, ((level / 2) * 800) + 800, damage);
			return pnew;
		}
		return null;
	}
	
	public void startEmitterCircler(int particleCount, long sequenceTime, float life, float angle, float speed, int radius, ImageIcon[] ca){ // BURST EMITTER
		Sprite circ = new Sprite();
		circ.setX(getX());
		circ.setY(getY());
		circ.setOrigin(new Point(getX(), getY()));
		circ.moveAlongCircle(32,32,8);
		setPather(new Timer());
		getPather().schedule(new TimerTask(){
			@Override
			public void run() {
				for(int i = 0; i<particleCount; i++){
					setLastParticle(WorldBuilder.randomImage(ca));
					circ.setOrigin(new Point(getX()-getLastParticle().getIconWidth()/2, getY()+getLastParticle().getIconHeight()/2));
					@SuppressWarnings("unused")
					Particle pn = new Particle(circ.getX()+(Math.abs(circ.getWidth()-getLastParticle().getIconWidth()))/2, circ.getY()+(Math.abs(circ.getHeight()-getLastParticle().getIconHeight()))/2,
							radius ,life, (float) (Math.random()*360+1), (float) Math.random() * speed, getLastParticle());
				}
			}
		}, sequenceTime, sequenceTime);
	}

	
	public void hurtme(int dam) { // take damage (-val = heal)
		if (!invulnerable) {
			if (this.getOverrideImage() == null) {
				SoundEffect.hurt.play(false);

				recoil(6);

				for (int i = 0; i > dam; i--) {
					startEmitterTexturedOneShot(1, 85, 85, 80, (float) 0.75, false,new ImageIcon("Images\\hearthalf.png"));
				}

				this.modhp(dam);
				this.setInvulnerable(true);
				Timer blink = new Timer();
				blink.schedule(new TimerTask() {
					@Override
					public void run() {
						setInvisible(!getInvisible());
					}
				}, 30, 30);
				Timer inv = new Timer();
				inv.schedule(new TimerTask() {
					@Override
					public void run() {
						setInvisible(false);
						setInvulnerable(false);
						blink.cancel();
						blink.purge();
						inv.cancel();
						inv.purge();
					}
				}, invTime);
			} else {
				resetOverrider();
			}
		}
	}

	private void recoil(int i) { // recoil backwards by strength i
		
		if(i>15){ // 16 or greater can jump over grid locations and avoid collision.
			i=15;
		}
		
		//stopWalking();
		if(getImage() == getIdleU()){  // move according to facing
			move(0,i);
		}
		if(getImage() == getIdleD()){
			move(0,-i);
		}
		if(getImage() == getIdleL()){
			move(i,0);
		}
		if(getImage() == getIdleR()){
			move(-i,0);
		}
		
		Timer recursive = new Timer();
		
		i=i-1;
		final int r = i;
		
		recursive.schedule(new TimerTask(){ // recoil recursively
			@Override
			public void run() {
				if(r>0){
					recoil(r);
				}
			}
		}, 20);
		
	}

	public void frob(){
		this.stopWalking();
		if(getImage() == getIdleU()){
			this.setImage(new ImageIcon("Images\\people\\linkugrab.png"));
		}
		if(getImage() == getIdleD()){
			this.setImage(new ImageIcon("Images\\people\\linkdgrab.png"));
		}
		if(getImage() == getIdleL()){
			this.setImage(new ImageIcon("Images\\people\\linklgrab.png"));
		}
		if(getImage() == getIdleR()){
			this.setImage(new ImageIcon("Images\\people\\linkrgrab.png"));
		}
		WorldBuilder.screen.setCantWalk(true);
	}
	
	public void returnToIdle(){
		
		setImage(new ImageIcon("Images\\people\\"+getImage().toString().substring(14, 19)+".png"));
		
		if(getImage().toString().equals("Images\\people\\linkd.png")){
			lastDown = true;
			lastUp = false;
			lastRight = false;
			lastLeft = false;
			setImage(getIdleD());
		}
		if(getImage().toString().equals("Images\\people\\linku.png")){
			lastDown = false;
			lastUp = true;
			lastRight = false;
			lastLeft = false;
			setImage(getIdleU());
		}
		if(getImage().toString().equals("Images\\people\\linkr.png")){
			lastDown = false;
			lastUp = false;
			lastRight = true;
			lastLeft = false;
			setImage(getIdleR());
		}
		if(getImage().toString().equals("Images\\people\\linkl.png")){
			lastDown = false;
			lastUp = false;
			lastRight = false;
			lastLeft = true;
			setImage(getIdleL());
		}
	}
	
	public void pickupAnimate(){
		swingFrame = 0;
		Timer blink = new Timer();
		blink.schedule(new TimerTask(){
			@Override
			public void run() {
				swingFrame++;
				setImage(new ImageIcon("Images\\people\\"+getImage().toString().substring(14, 19)+"p"+swingFrame+".png"));
				if(swingFrame>4){
					 
					returnToIdle();
					WorldBuilder.screen.setCantWalk(false);
					blink.cancel();
					blink.purge();
				}
			}
		}, 60, 60);
		
	}
	
	public int dirIntY(){
		if (getImage() == IdleU || getImage() == AnimU){
			return 8; 
		}
		if (getImage() == IdleL || getImage() == AnimL || getImage() == IdleR || getImage() == AnimR){
			return 16; 
		}
		if (getImage() == IdleD || getImage() == AnimD){
			return 28; 
		}
		return 0;
	}
	
	public int dirIntX(){
		if (getImage() == IdleL || getImage() == AnimL){
			return -8; 
		}
		if (getImage() == IdleR || getImage() == AnimR){
			return 10; 
		}
		return 0;
	}
	
	public void stopWalking(){
		
		if(getImage() == this.getAnimD()){
			this.setImage(this.getIdleD());
		}
		if(getImage() == this.getAnimU()){
			this.setImage(this.getIdleU());
		}
		if(getImage() == this.getAnimR()){
			this.setImage(this.getIdleR());
		}
		if(getImage() == this.getAnimL()){
			this.setImage(this.getIdleL());
		}
		
		setWalkDown(false);
		setWalkLeft(false);
		setWalkUp(false);
		setWalkRight(false);
	}

	public void swing() { // determine sword swing and spawn a damage collider.
		if (!WorldBuilder.screen.isCantWalk()) {
			swinging = true;
			SoundEffect.attack.play(false,2);
			Timer framer = new Timer();

			Projectile damArea = new Projectile(new ImageIcon("Tiles\\full.png"), new ImageIcon("Tiles\\full.png"));
			damArea.setWidth(8);
			damArea.setHeight(8);
			damArea.setX(getX() + dirIntX());
			damArea.setY(getY() + dirIntY());
			damArea.setByplayer(true);
			damArea.setDamage(this.getSwordlevel());
			damArea.Spawn(-1);
			damArea.setImage(new ImageIcon(""));

			if (getImage() == IdleD || getImage() == AnimD) {

				framer.schedule(new TimerTask() {
					@Override
					public void run() {
						swingFrame++;
						setImage(new ImageIcon("Images\\people\\linkdsw" + swordlevel + swingFrame + ".png"));
						WorldBuilder.frame.repaint();
						if(swingFrame == 7){
							framer.cancel();
							swinging = false;
							swingFrame = 0;
							returnToIdle();
							offsetX = 0;
						}
					}
				}, 35, 35);
			}

			if (getImage() == IdleU || getImage() == AnimU) {
				framer.schedule(new TimerTask() {
					@Override
					public void run() {
						swingFrame++;
						setImage(new ImageIcon("Images\\people\\linkusw" + swordlevel + swingFrame + ".png"));
						WorldBuilder.frame.repaint();
						if(swingFrame == 7){
							framer.cancel();
							swinging = false;
							swingFrame = 0;
							returnToIdle();
							offsetX = 0;
						}
					}
				}, 35, 35);
			}

			if (getImage() == IdleR || getImage() == AnimR) {
				framer.schedule(new TimerTask() {
					@Override
					public void run() {
						swingFrame++;
						setImage(new ImageIcon("Images\\people\\linkrsw" + swordlevel + swingFrame + ".png"));
						WorldBuilder.frame.repaint();
						if(swingFrame == 7){
							framer.cancel();
							swinging = false;
							swingFrame = 0;
							returnToIdle();
							offsetX = 0;
						}
					}
				}, 35, 35);
			}

			if (getImage() == IdleL || getImage() == AnimL) {
				framer.schedule(new TimerTask() {
					@Override
					public void run() {
						swingFrame++;
						setImage(new ImageIcon("Images\\people\\linklsw" + swordlevel + swingFrame + ".png"));
						offsetX = -20;
						WorldBuilder.frame.repaint();
						if(swingFrame == 7){
							framer.cancel();
							swinging = false;
							swingFrame = 0;
							returnToIdle();
							offsetX = 0;
						}
					}
				}, 35, 35);
			}
		}
	}

	public boolean isMoving(){
		return (walkUp || walkDown || walkLeft || walkRight);
	}
	
	public boolean isMovingExclude(int dir){
		if(dir == 1)
			return (walkDown || walkLeft || walkRight);
		if(dir == 2)
			return (walkUp || walkLeft || walkRight);
		if(dir == 3)
			return (walkUp || walkDown || walkRight);
		if(dir == 4)
			return (walkUp || walkDown || walkLeft);
		return false;
	}
	
	public int getDirection(){
		if(getImage() == IdleU || getImage() == AnimU)
			return 1;
		if(getImage() == IdleD || getImage() == AnimD)
			return 2;
		if(getImage() == IdleL || getImage() == AnimL)
			return 3;
		if(getImage() == IdleR || getImage() == AnimR)
			return 4;
		return -1;
	}
	
	public void move(int gx, int gy) { // move player, watch for collisions of
										// all types.
		try {
			boolean fail = true;
			boolean shiftxp = false;
			boolean shiftxn = false;

			if (!solid) {
				if (this.isWalkUp()) {
					this.setY(getY() - speed);
				}
				if (this.isWalkDown()) {
					this.setY(getY() + speed);
				}
				if (this.isWalkLeft()) {
					this.setX(getX() - speed);
				}
				if (this.isWalkRight()) {
					this.setX(getX() + speed);
				}
			} else {

				Rectangle r = new Rectangle(getX() + gx + 4, getY() + 22 + gy, 10, 10);
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

								if (walkLeft && !walkDown) {
									shiftxn = true;
								}
								if (!walkLeft && walkDown) {
									shiftxp = true;
								}

							}
							if (col.getCollision() == collisionType.BOTTOMRIGHTD) {

								r2 = new Rectangle(col.getX(), col.getY(), col.getWidth(), col.getHeight());

								int xo = col.getX() - r.x;
								if (xo < 0) {
									xo = 0;
								}

								r2 = new Rectangle(col.getX() - 2, col.getY() + xo, col.getWidth(),
										col.getHeight() - xo);

								if (walkRight && !walkDown) {
									shiftxn = true;
								}
								if (!walkRight && walkDown) {
									shiftxp = true;
								}

							}
							if (col.getCollision() == collisionType.TOPLEFTD) {

								r2 = new Rectangle(col.getX(), col.getY(), col.getWidth(), col.getHeight());

								int xo = col.getX() - r.x;
								if (xo < 0) {
									xo = 0;
								}

								r2 = new Rectangle(col.getX(), col.getY() - xo, col.getWidth(), col.getHeight() + xo);

								if (walkLeft && !walkUp) {
									shiftxn = true;
								}
								if (!walkLeft && walkUp) {
									shiftxp = true;
								}

							}
							if (col.getCollision() == collisionType.TOPRIGHTD) {

								r2 = new Rectangle(col.getX()-1, col.getY(), col.getWidth(), col.getHeight());

								int xo = col.getX() - r.x;
								if (xo < 0) {
									xo = 0;
								}

								r2 = new Rectangle(col.getX(), col.getY() + xo, col.getWidth(), col.getHeight() - xo);

								if (walkRight && !walkUp) {
									shiftxn = true;
								}
								if (!walkRight && walkUp) {
									shiftxp = true;
								}

							}
							if (!(r.intersects(r2))) {
								fail = false;
							} else if (r.intersects(r2)) {
								fail = true;
								String cm = "" + s.getImage();
								if (cm.equals("Tiles\\bottomleft.png")) {
									if (shiftxn) {
										x -= 2;
										y -= 2;
									}
									if (shiftxp) {
										x += 2;
										y += 2;
									}
								} else if (cm.equals("Tiles\\bottomright.png")) {
									if((!walkRight && !walkDown) || (walkRight && !walkDown) || (!walkRight && walkDown)){
									if (shiftxn) {
										x += 2;
										y -= 2;
									}
									if (shiftxp) {
										x -= 2;
										y += 2;
									}
									}
								} else if (cm.equals("Tiles\\topright.png")) {
									if(!walkDown && !walkLeft){
									if ((walkRight && !walkDown) || walkUp) {
										if (shiftxn && !walkUp) {
											x += 2;
											y += 2;
										} else if (shiftxp && !walkLeft) {
											x -= 2;
											y -= 2;
										} else if (shiftxp && walkLeft) {
											fail = false;
										}
									} else if (walkRight && walkDown) {
										fail = false;
									}
									} else {
										
									}
								} else if (cm.equals("Tiles\\topleft.png")) {
									if((!walkLeft && !walkDown) || (walkLeft && !walkDown)){
									if (shiftxn && !walkUp) {
										x -= 2;
										y += 2;
									}
									if (shiftxp && !walkRight) {
										if (shiftxp && walkLeft) {
											x -= 2;
											y += 2;
										}
										x += 2;
										y -= 2;
									} else if (shiftxp && walkRight) {
										fail = false;
									}
									}
								}
								break;
							}
						}
					}
				} catch (ConcurrentModificationException e) {
					// Adding Sprite
				}
				if (!fail) {
					x += gx;
					y += gy;

					if (x < -8) {
						x = WorldBuilder.screen.getDungeonSize() * 16 - 16;
						WorldBuilder.screen.loadDungeon(WorldBuilder.screen.getWestMap());
					}
					if (y < -28) {
						y = WorldBuilder.screen.getDungeonSize() * 16 - 38;
						System.out.println(WorldBuilder.screen.getNorthMap());
						WorldBuilder.screen.loadDungeon(WorldBuilder.screen.getNorthMap());
					}
					if (x > WorldBuilder.screen.getDungeonSize() * 16 - 16) {
						x = 16;
						WorldBuilder.screen.loadDungeon(WorldBuilder.screen.getEastMap());
					}
					if (y > WorldBuilder.screen.getDungeonSize() * 16 - 28) {
						y = -8;
						WorldBuilder.screen.loadDungeon(WorldBuilder.screen.getSouthMap());
					}

				}
			}

			glow.setX(getX() - (int) glow.getRadius() + 8);
			glow.setY(getY() - (int) glow.getRadius() / 2 - 8);
		} catch (NullPointerException npe) {

		}
	}
	
	public void addRupees(int r){
		SoundEffect.rupeeRegister.play(false);
		Timer rup = new Timer();
		rup.schedule(new TimerTask(){
			@Override
			public void run() {
				rupees++;
				if(rupees>maxrupees)
					rupees=maxrupees;
				if(rupees<0)
					rupees=0;
				WorldBuilder.frame.repaint();
				if (r>=2){
					addRupees(r-1);
				}
			}
		}, 35);
		
	}
	
	public void addBombs(int r){
		Timer rup = new Timer();
		rup.schedule(new TimerTask(){
			@Override
			public void run() {
				bombs++;
				if(bombs>bombmax)
					bombs=bombmax;
				if(bombs<0)
					bombs=0;
				WorldBuilder.frame.repaint();
				if (r>=2)
					addBombs(r-1);
			}
		}, 35);
	}
	
	public void heal(int power){
		Timer heals = new Timer();
		heals.schedule(new TimerTask(){
			@Override
			public void run() {
				SoundEffect.HeartUp.play(false);
				modhp(1);
				WorldBuilder.frame.repaint();
				if (power>=2)
					heal(power-1);
			}
		}, 30);
	}
	
	public void modhp(int hp){
		this.health+=hp;
		if(health>maxhealth){
			health = maxhealth;
		}
		if(health<0){
			health=0;
		}
		if(health == 0){
			WorldBuilder.task.cancel();
			WorldBuilder.task = new Timer();
			Music.activeMusic.stop();
			Music.activeMusic.reinitialize(Music.getMusicURL()[13]);
			WorldBuilder.playMusic(13);
			WorldBuilder.daySong = Music.getMusicURL()[13];
			WorldBuilder.screen.deathScreen();
		}
	}
	
	public int modmagic(float m){
		this.magic+=m;
		if(magic>maxmagic){
			magic = maxmagic;
		}
		if(magic<-1){
			magic=-1;
		}
		return (int) (magic+1);
	}

	public int modarrows(int m){
		this.arrows+=m;
		if(arrows>arrowsMax){
			arrows = arrowsMax;
		}
		if(arrows<0){
			arrows=0;
		}
		return arrows;
	}
	
	public int getHealth() {
		return health;
	}

	public void setHealth(int health) {
		this.health = health;
	}

	public int getMaxhealth() {
		return maxhealth;
	}

	public void setMaxhealth(int maxhealth) {
		this.maxhealth = maxhealth;
		if(this.maxhealth>80){this.maxhealth=80;}
	}

	public double getArmor() {
		return armor;
	}

	public void setArmor(double armor) {
		this.armor = armor;
	}

	public int getRupees() {
		return rupees;
	}

	public void setRupees(int rupees) {
		this.rupees = rupees;
	}

	public int getMaxrupees() {
		return maxrupees;
	}

	public int getBombs() {
		return bombs;
	}

	public void setBombs(int bombs) {
		this.bombs = bombs;
	}

	public int getBombmax() {
		return bombmax;
	}

	public void setBombmax(int bombmax) {
		this.bombmax = bombmax;
	}

	public int getMagic() {
		return (int) magic;
	}

	public void setMagic(int magic) {
		this.magic = magic;
	}

	public int getMaxmagic() {
		return (int) maxmagic;
	}

	public void setMaxmagic(int maxmagic) {
		this.maxmagic = maxmagic;
	}

	public boolean isWalkUp() {
		return walkUp;
	}

	public void setWalkUp(boolean walkUp) {
		this.walkUp = walkUp;
	}

	public boolean isWalkDown() {
		return walkDown;
	}

	public void setWalkDown(boolean walkDown) {
		this.walkDown = walkDown;
	}

	public boolean isWalkLeft() {
		return walkLeft;
	}

	public void setWalkLeft(boolean walkLeft) {
		this.walkLeft = walkLeft;
	}

	public boolean isWalkRight() {
		return walkRight;
	}

	public void setWalkRight(boolean walkRight) {
		this.walkRight = walkRight;
	}

	public Timer getMover() {
		return mover;
	}

	public void setMover(Timer mover) {
		this.mover = mover;
	}

	public int getSpeed() {
		return speed;
	}

	public void setSpeed(int speed) {
		this.speed = speed;
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

	public ImageIcon getIdleL() {
		return IdleL;
	}

	public void setIdleL(ImageIcon idleL) {
		IdleL = idleL;
	}

	public ImageIcon getIdleR() {
		return IdleR;
	}

	public void setIdleR(ImageIcon idleR) {
		IdleR = idleR;
	}

	public ImageIcon getIdleD() {
		return IdleD;
	}

	public void setIdleD(ImageIcon idleD) {
		IdleD = idleD;
	}

	public ImageIcon getIdleU() {
		return IdleU;
	}

	public void setIdleU(ImageIcon idleU) {
		IdleU = idleU;
	}

	public boolean isLifting() {
		return lifting;
	}

	public void setLifting(boolean lifting) {
		this.lifting = lifting;
		if(this.lifting){
			slow++;
		} else {
			slow--;
		}
	}

	public int getLastLiftID() {
		return lastLiftID;
	}

	public void setLastLiftID(int lastLiftID) {
		this.lastLiftID = lastLiftID;
	}

	public boolean isLastLeft() {
		return lastLeft;
	}

	public void setLastLeft(boolean lastLeft) {
		this.lastLeft = lastLeft;
	}

	public boolean isLastRight() {
		return lastRight;
	}

	public void setLastRight(boolean lastRight) {
		this.lastRight = lastRight;
	}

	public boolean isLastUp() {
		return lastUp;
	}

	public void setLastUp(boolean lastUp) {
		this.lastUp = lastUp;
	}

	public boolean isLastDown() {
		return lastDown;
	}

	public void setLastDown(boolean lastDown) {
		this.lastDown = lastDown;
	}

	public ImageIcon getAnimL() {
		return AnimL;
	}

	public void setAnimL(ImageIcon animL) {
		AnimL = animL;
	}

	public ImageIcon getAnimR() {
		return AnimR;
	}

	public void setAnimR(ImageIcon animR) {
		AnimR = animR;
	}

	public ImageIcon getAnimD() {
		return AnimD;
	}

	public void setAnimD(ImageIcon animD) {
		AnimD = animD;
	}

	public ImageIcon getAnimU() {
		return AnimU;
	}

	public void setAnimU(ImageIcon animU) {
		AnimU = animU;
	}

	public boolean isSwinging() {
		return swinging;
	}

	public void setSwinging(boolean swinging) {
		this.swinging = swinging;
	}

	public int getSwingFrame() {
		return swingFrame;
	}

	public void setSwingFrame(int swingFrame) {
		this.swingFrame = swingFrame;
	}

	public boolean isInvulnerable() {
		return invulnerable;
	}

	public void setInvulnerable(boolean invulnerable) {
		this.invulnerable = invulnerable;
	}

	public int getInvTime() {
		return invTime;
	}

	public void setInvTime(int invTime) {
		this.invTime = invTime;
	}

	public boolean getInvisible() {
		return invisible;
	}

	public void setInvisible(boolean invisible) {
		this.invisible = invisible;
	}

	public int getShieldlevel() {
		return shieldlevel;
	}

	public void setShieldlevel(int shieldlevel) {
		this.shieldlevel = shieldlevel;
	}

	public int getSwordlevel() {
		return swordlevel;
	}

	public void setSwordlevel(int swordlevel) {
		this.swordlevel = swordlevel;
	}

	public boolean isSolid() {
		return solid;
	}

	public void setSolid(boolean solid) {
		this.solid = solid;
	}

	public ItemList getEquippedItem() {
		return equippedItem;
	}

	public void setEquippedItem(ItemList equippedItem) {
		this.equippedItem = equippedItem;
		this.equippedInt = intOfItem();
	}
	
	public int intOfItem(){
		for(int i = 0; i<itemArray.size(); i++){
			if(this.getEquippedItem() == itemArray.get(i)){
				return i;	
			}
		}
		return -1;
	}

	public boolean isHasboomerang() {
		return hasboomerang;
	}

	public void setHasboomerang(boolean hasboomerang) {
		this.hasboomerang = hasboomerang;
	}

	public ItemList randomItemRoll(){
		Dice r = new Dice(itemCount-2);
		int rr = r.roll()-1;
		if(this.getEquippedItem().toString().equals(itemArray.get(rr).toString())){
			return randomItemRoll();
		}
		return itemArray.get(rr);
	}

	public boolean isBoostedSpeed() {
		return boostedSpeed;
	}

	public void setBoostedSpeed(boolean boostedSpeed) {
		this.boostedSpeed = boostedSpeed;
	}

	public static int getItemCount() {
		return itemCount;
	}

	public static void setItemCount(int itemCount) {
		Player.itemCount = itemCount;
	}

	public ArrayList<ItemList> getItemArray() {
		return itemArray;
	}

	public static void setItemArray(ArrayList<ItemList> itemArray) {
		Player.itemArray = itemArray;
	}

	public int getSlow() {
		return slow;
	}

	public void setSlow(int slow) {
		this.slow = slow;
	}

	public int getEquippedInt() {
		return equippedInt;
	}

	public void setEquippedInt(int equippedInt) {
		this.equippedInt = equippedInt;
	}

	public int getArrows() {
		return arrows;
	}

	public void setArrows(int arrows) {
		this.arrows = arrows;
	}

	public Light getGlow() {
		return glow;
	}

	public void setGlow(Light glow) {
		this.glow = glow;
	}

	public int getArrowsMax() {
		return arrowsMax;
	}

	public void setArrowsMax(int arrowsMax) {
		this.arrowsMax = arrowsMax;
	}

	public int getOffsetX() {
		return offsetX;
	}

	public void setOffsetX(int offsetX) {
		this.offsetX = offsetX;
	}

	public int getGripPower() {
		return gripPower;
	}

	public void setGripPower(int gripPower) {
		this.gripPower = gripPower;
	}

	public static int getEquipmentCount() {
		return equipmentCount;
	}

	public static void setEquipmentCount(int equipmentCount) {
		Player.equipmentCount = equipmentCount;
	}

	public ArrayList<EquipmentList> getEquipArray() {
		return equipArray;
	}

	public void setEquipArray(ArrayList<EquipmentList> equipArray) {
		Player.equipArray = equipArray;
	}

	public int getSouls() {
		return souls;
	}

	public void setSouls(int souls) {
		this.souls = souls;
	}

	public void addSouls(int checkSoulList) {
		souls+=checkSoulList;
	}

	public Sprite getOverrideImage() {
		return overrideImage;
	}

	public void setOverrideImage(Sprite overrideImage) {
		this.overrideImage = overrideImage;
	}
	
	
}
