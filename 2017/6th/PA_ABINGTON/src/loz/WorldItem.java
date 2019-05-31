package loz;

import java.awt.Rectangle;
import java.util.Timer;
import java.util.TimerTask;
import javax.swing.ImageIcon;

import loz.Player.EquipmentList;
import loz.Player.ItemList;

public class WorldItem extends Sprite{

	public enum Script {
		
		PICKUP,
		
		USE_OPEN,
		
		USE_DOOR,
		
		BREAK,
		
		ITEM_RUP1,
		
		ITEM_RUP5,
		
		ITEM_RUP20,
		
		ITEM_RUP50,
		
		ITEM_RUP100,
		
		Heart,
		
		chest,
		
		wispSpawn,
		
		element,
		
		USE_TELEPORT;
	
	}
	
	private Script script;
	private Runnable execute;
	private boolean used, dropped, pickupBreak;
	private String refName = "null", facing = "";
	public Player link = WorldBuilder.link;
	private int usedID;
	
	public WorldItem(ImageIcon icon, Script type){
		super(icon);
		this.setScript(type);
	}
	
	public WorldItem(int tid, String l1, String l2, boolean t, boolean c, String add){
		setTileid(tid - 1);
		setCollision(setCollisionData(tid));
		setId(getTileid() - 1);
		setWidth(16);
		setHeight(16);
		setY(Integer.parseInt(l1) * 16);
		setX(Integer.parseInt(l2) * 16);
		setImage(WorldBuilder.spritelist.get(tid).getImage());
		if(t){
			setScript(WorldItem.Script.BREAK);
			generateScript();
		}
		if(c){
			if(add!=null)
				setUsedID(Integer.parseInt(add));
			setScript(Script.chest);
			generateScript();
		}
	}
	
	public WorldItem(String n1, String n2, String tid){
		setId(-1);
		setScript(WorldItem.Script.USE_DOOR);
		generateScript();
		startCollisionWatchDelay(1500);
		setId(Integer.parseInt(tid));
		setRefName(n1);
		setFacing(n2);
	}
	
	public WorldItem(){
		usedID = -1;
	}
	
	public boolean checkChest(){
		if (getTileid() == 12) { // chest brown
			return true;
		}
		return false;
	}
	
	public void usedImageGenerate() { // change image when used based on id

		if (getTileid() == 12) { // chest brown
			changeImage(1);
		} else if (getTileid() == 78) {
			changeImage(167);
			startEmitterTexturedOneShot(8, 32, 20, 15, 1.5f, false, new ImageIcon("Images\\particles\\leaf2.png"));
		} else if (getTileid() == 90) {
			changeImage(166);
			startEmitterTexturedOneShot(8, 32, 20, 15, 1.5f, false, new ImageIcon("Images\\particles\\leaf2.png"));
		} else if (getTileid() == 66) {
			changeImage(168);
			startEmitterTexturedOneShot(8, 32, 20, 15, 1.5f, false, new ImageIcon("Images\\particles\\leaf2.png"));
		} else if (getTileid() == 359) {
			changeImage(411);
		} else {
			removeSprite();
			removeObject();
			this.removePickup();
		}
		setExecute(null);
		setScript(null);
	}
	
	public void generateScript(){ // generate Runnable object based on script
		
		if(getScript() == Script.wispSpawn && !used){
			execute = new Runnable(){
				@Override
				public void run() {
						Enemy test = new Enemy(getX(), getY());
						test.invulnerable = true;
						test.setAiType(-1);
						test.setWidth(16);
						test.setHeight(16);
						test.spawn();
						test.setDamage(2);
						test.startEmitterTexturedShift(8, 32, 10, 15, 1f, false, new ImageIcon("Images\\particles\\electric.png"),-16,-16);
						test.initImages("dont");
						used = true;
						
						Timer enabler = new Timer();
						enabler.schedule(new TimerTask(){
							@Override
							public void run() {
								test.setAiType(0);
								test.setInvulnerable(false);
							}
						}, 200);
				}
			};
			execute.run();
		}
		
		if(getScript() == Script.USE_DOOR){
			this.setImage(new ImageIcon(""));
			execute = new Runnable(){
				@Override
				public void run() {
					if(getId() != -1 && !used){
						used = true;
						WorldBuilder.screen.loadDungeonDoor(refName, getId());
						setRefName("");
					}
				}
			};
			execute.run();
		}
		
		if(getScript() == Script.Heart && !used){
			startCollisionWatch();
			execute = new Runnable(){
				@Override
				public void run() {
						link.modhp(2);
						SoundEffect.HeartUp.play(false);
						removePickup();
						used = true;
				}
			};
		}
		
		if(getScript() == Script.element && !used){
			startCollisionWatch();
			execute = new Runnable(){
				@Override
				public void run() {
					removePickup();
					used = true;
					WorldBuilder.screen.displayElement((int) (Math.random()*9+1));
					Timer kill = new Timer();
					kill.schedule(new TimerTask(){
						@Override
						public void run() {
							WorldBuilder.screen.setEdisplay(false);
							kill.cancel();
						}
					}, 5000);
				}
			};
		}
		
		if(getScript() == Script.PICKUP && !used){
			execute = new Runnable(){
				@Override
				public void run() {
					if(!used){
						link.setLifting(true);
						link.setLastLiftID(getTileid());
						usedImageGenerate();
						used = true;
					}
				}
			};
		}
		
		if(getScript() == Script.chest && !used){
			execute = new Runnable(){
				@Override
				public void run() {
					used = true;
					usedImageGenerate();
					chestReward();
				}
			};
		}
		
		rupGen();
		
		if(getScript() == Script.BREAK && !used){
			execute = new Runnable(){
				@Override
				public void run() {
					if(!used){
					
					if(pickupBreak){
						link.pickupAnimate();
						SoundEffect.pickup.play(false, 2);
						link.setLifting(true);
						link.setLastLiftID(getTileid());
						pickupBreak = false;
						//getExecute().run();
						execute = null;
						used = true;
						setScript(null);
					}
					
					usedImageGenerate();
					used = true;
					WorldItem drop = new WorldItem();
					drop.setExecute(null);
					drop.setX(getX());
					drop.setY(getY());
					drop.setWidth(16);
					drop.setHeight(16);
					drop.setTileid(DropTable.generateItem(drop));
					System.out.println(drop.getTileid());
					if(drop.getTileid()!=-1){
						WorldBuilder.screen.getDrops().add(drop);
					}
					
					script = null;
					execute = null;

					removeObject();
					
					used = true;
					
					}
				}
			};
		}
		
	}

	protected void chestReward() { // Hard Coded Chest Scripts Based On ID (Very limited planned variance)
		
		ItemList getted = null;
		EquipmentList gettedE = null;
		
		if(this.getUsedID() == 1){
			Player.ItemList.Boomerang.replace(Player.ItemList.Boomerangr);
			getted = Player.ItemList.Boomerangr;
		}
		
		if(this.getUsedID() == 12){
			Player.ItemList.Trident.unlock();
			getted = Player.ItemList.Trident;
		}
		
		if(this.getUsedID() == 2){
			Player.ItemList.Bow.unlock();
			getted = Player.ItemList.Bow;
		}
		
		if(this.getUsedID() == 3){
			Player.ItemList.Bow.unlock();
			getted = Player.ItemList.Bow;
		}
		
		if(this.getUsedID() == 4){
			Player.ItemList.Boomerang.unlock();
			getted = Player.ItemList.Boomerang;
		}
		
		if(this.getUsedID() == 11){
			//Player.ItemList.greenburst.unlock();
			getted = Player.ItemList.thrrup;
			SoundEffect.rupee.play(false);
			WorldBuilder.link.addRupees(300);
		}
		
		if(this.getUsedID() == 100){
			getted = Player.ItemList.shield2;
			link.setShieldlevel(2);
		}
		
		if(this.getUsedID() == 101){
			getted = Player.ItemList.shield3;
			link.setShieldlevel(3);
		}
		
		if(this.getUsedID() == 999){
			getted = Player.ItemList.branch;
			Player.ItemList.branch.unlock();
		}
		
		if(this.getUsedID() == 9){
			getted = Player.ItemList.BK;
		}
		
		if(this.getUsedID() == 69){
			getted = Player.ItemList.lenny;
		}
		
		if(this.getUsedID() == 100){
			getted = Player.ItemList.heart;
			link.addHeart();
		}
		
		if(this.getUsedID() == 99){
			Player.EquipmentList.Glove.unlock();
			gettedE = Player.EquipmentList.Glove;
			if(link.getGripPower()<2){
				link.setGripPower(2);
			}
			WorldBuilder.screen.setCantWalk(true);
			WorldBuilder.screen.startConversation(WorldBuilder.screen.getConversation().loadConversation("Glove", new Runnable(){
				@Override
				public void run() {
					WorldBuilder.screen.setCantWalk(false);
				}
			}));
		}
		
		if(getted!=null){
			WorldBuilder.screen.displayItemGet(getted);
		} else if(gettedE!=null){
			WorldBuilder.screen.displayItemGet(gettedE);
		}
		
		WorldBuilder.screen.getUsedChests().add(this.getUsedID());
		
	}

	public void rupGen(){ // generate rupee script
		if(getScript() == Script.ITEM_RUP1 && !used){
			startCollisionWatch();
			execute = new Runnable(){
				@Override
				public void run() {
					SoundEffect.rupee.play(false);
					WorldBuilder.link.addRupees(1);
					removePickup();
					used = true;
				}
			};
		}
		
		if(getScript() == Script.ITEM_RUP5 && !used){
			startCollisionWatch();
			execute = new Runnable(){
				@Override
				public void run() {
					SoundEffect.rupee.play(false);
					WorldBuilder.link.addRupees(5);
					removePickup();
					used = true;
				}
			};
		}
		
		if(getScript() == Script.ITEM_RUP20 && !used){
			startCollisionWatch();
			execute = new Runnable(){
				@Override
				public void run() {
					SoundEffect.rupee.play(false);
					WorldBuilder.link.addRupees(20);
					removePickup();
					used = true;
				}
			};
		}
		
		if(getScript() == Script.ITEM_RUP50 && !used){
			startCollisionWatch();
			execute = new Runnable(){
				@Override
				public void run() {
					SoundEffect.rupee.play(false);
					WorldBuilder.link.addRupees(50);
					removePickup();
					used = true;
				}
			};
		}
		
		if(getScript() == Script.ITEM_RUP100 && !used){
			startCollisionWatch();
			execute = new Runnable(){
				@Override
				public void run() {
					SoundEffect.rupee.play(false);
					WorldBuilder.link.addRupees(100);
					removePickup();
					used = true;
				}
			};
		}
	}

	public void startCollisionWatch(){ // lookout for player collision
		
		Timer get = new Timer();
		get.schedule(new TimerTask(){
			@Override
			public void run() {
				Rectangle r1 = new Rectangle(getX(),getY(),16,16);
				Rectangle r2 = new Rectangle(WorldBuilder.link.getX()+3,WorldBuilder.link.getY()+22,10,10);
				if(r2.intersects(r1)){
					if(!used){
						try{
							getExecute().run();
						} catch(NullPointerException nn){
							get.cancel();
						}
					}
					get.cancel();
				}
			}
			
		}, 50, 50);
	}
	
	public void startCollisionWatchDelay(long d){ // lookout for player collision
		
		Timer get = new Timer();
		get.schedule(new TimerTask(){
			@Override
			public void run() {
				Rectangle r1 = new Rectangle(getX(),getY(),16,16);
				Rectangle r2 = new Rectangle(WorldBuilder.link.getX()+3,WorldBuilder.link.getY()+22,10,10);
				if(r2.intersects(r1)){
					if(!used){
						try{
							getExecute().run();
						} catch(NullPointerException nn){
							get.cancel();
						}
					}
					get.cancel();
				}
			}
			
		}, d, 50);
	}
	
	public Script getScript() {
		return script;
	}

	public void setScript(Script script) {
		this.script = script;
	}

	public Runnable getExecute() {
		return execute;
	}

	public void setExecute(Runnable execute) {
		this.execute = execute;
	}

	public boolean isUsed() {
		return used;
	}

	public void setUsed(boolean used) {
		this.used = used;
	}

	public boolean isDropped() {
		return dropped;
	}

	public void setDropped(boolean dropped) {
		this.dropped = dropped;
	}

	public boolean isPickupBreak() {
		return pickupBreak;
	}

	public void setPickupBreak(boolean pickupBreak) {
		this.pickupBreak = pickupBreak;
	}

	public Player getLink() {
		return link;
	}

	public void setLink(Player link) {
		this.link = link;
	}

	public int getUsedID() {
		return usedID;
	}

	public void setUsedID(int usedID) {
		this.usedID = usedID;
	}

	public String getRefName() {
		return refName;
	}

	public void setRefName(String refName) {
		this.refName = refName;
	}

	public String getFacing() {
		return facing;
	}

	public void setFacing(String facing) {
		this.facing = facing;
	}

}
