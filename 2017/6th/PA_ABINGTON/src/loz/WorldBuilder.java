package loz;

import java.awt.AWTException;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.FontFormatException;
import java.awt.GraphicsEnvironment;
import java.awt.Image;
import java.awt.Point;
import java.awt.Robot;
import java.awt.Toolkit;
import java.awt.event.KeyEvent;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;
import javax.swing.ImageIcon;
import javax.swing.JFrame;
import org.lwjgl.LWJGLException;
import org.lwjgl.input.*;
import eui.SplashString;

public class WorldBuilder {
	
	public static Controller controller;
	public static GraphicsEnvironment ge;
	public static Font font;
	public static JFrame frame = new JFrame();
	public static Dimension ScreenSize = Toolkit.getDefaultToolkit().getScreenSize();
	public static ArrayList<Sprite> spritelist = new ArrayList<Sprite>();
	public static Window screen = new Window();
	public static Player link = new Player();
	public static boolean requestCheck = false, scanner = false, allowMusic;
	public static int lastPressX = -1, lastPressY = -1;
	public static boolean ra = false, rb = false, ry = false, rx = false, sel = false, start = false,
			rb1 = false, rb2 = false, wu = false, wl = false, padPress = false, nsp, dsp, hasController = false;
	
	public static ArrayList<String> pressMemory = new ArrayList<String>();
	public static Timer fatherTime = new Timer();
	public static Timer task = new Timer();
	
	public static int[] blocked = new int[0];
	
	public static int time = 1800, timeMod = 200, percentTime = 1800;
	
	public static String daySong, nightSong;
	
	public static void ChangeFont(String name, int size){
		try {
			ge.registerFont(Font.createFont(Font.TRUETYPE_FONT, new File("Fonts\\"+name+".ttf")).deriveFont(size));
			font = ge.getAllFonts()[ge.getAllFonts().length - 1].deriveFont((float)size);
			System.out.println(font);
		} catch (FontFormatException | IOException e) {
			e.printStackTrace();
		}
	}
	
	public static String wrappedString(String s, int size){ // wrap font for containers
		StringBuilder sb = new StringBuilder(s);
		int i = 0;
		while (i + size < sb.length() && (i = sb.lastIndexOf(" ", i + size)) != -1) {
		    sb.replace(i, i + 1, "\n");
		}
		return sb.toString();
	}
	
	public static String numberWithLeadingZeros(int num, int mod){ // returns a number with mod amount of leading zeros
		String g = ""+num;
		StringBuilder sb = new StringBuilder(g);
		
		for(int i = g.length(); i<mod; i++){
			sb.insert(0, "0");
		}
		
		return sb.toString();
	}
	
	public static boolean numToBool(int n){ // 0 or 1 to false or true
		String g = ""+n;
		if(g.length()>1){
			return false;
		}
		if(g.equals("0")){
			return true;
		}
		if(g.equals("1")){
			return false;
		}
		return false;
	}
	
	public static void main(String[] args) { // testing main method
		
		screen.setCantWalk(true);
		nightSong = "Music\\Dark Forces.wav";
		daySong = "Music\\Outlook.wav";
		//scanner = true;
		allowMusic = true;
		playTitleMusic();
		
		initController();
		
		ge = GraphicsEnvironment.getLocalGraphicsEnvironment();

		ChangeFont("lttp", 12);
		
		frame.add(screen);
		frame.setUndecorated(true);
		frame.setSize(ScreenSize);
		frame.setVisible(true);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.setTitle("Elemetarium");
		
		//testGame("big");

		if (controller != null) {
			enableController1();
		}
		
		showMainMenu();

		Toolkit toolkit = Toolkit.getDefaultToolkit();
		Image image = toolkit.getImage("nop");
		java.awt.Cursor c = toolkit.createCustomCursor(image , new Point(0, 0), "img");
		frame.setCursor (c);
		//screen.displayElement((int) (Math.random()*9+1));
		//MaidService();
		
	}
	
	public static void playTitleMusic(){
		Music m = new Music(Music.musicURL[10]);
		Music.setActiveMusic(m);
		Music.activeMusic.playMusic(10);
	}
	
	public static void playMusic(int t){
		Music m = new Music(Music.musicURL[t]);
		Music.setActiveMusic(m);
		Music.activeMusic.playMusic(t);
	}
	
	public static void showMainMenu(){ // initialize main menu animations
		screen.setMainMenu(true);
		// elementarium icon
		Sprite menuIcon = new Sprite(new ImageIcon("Images\\Special\\Elementarium.png"),(int)(ScreenSize.getWidth()/2-8),(int)(ScreenSize.getHeight()/2-8));
		menuIcon.setWidth(32);
		menuIcon.setHeight(32);
		menuIcon.setX(64);
		menuIcon.setY(64);
		menuIcon.setName("elementarium idol");
		screen.getSprites().add(menuIcon);
		Sprite focus = new Sprite();
		focus.setX((int) menuIcon.getOrigin().getX()+8);
		focus.setY((int) menuIcon.getOrigin().getY());
		screen.setWatchLink(focus);
		screen.getSprites().add(focus);
		
		Sprite make3 = new Sprite();
		make3.setX((int) menuIcon.getOrigin().getX()-244);
		make3.setY((int) menuIcon.getOrigin().getY()-176);
		make3.setWidth(32);
		make3.setHeight(32);
		make3.playAnimation("gauge", 30, false);
		WorldBuilder.screen.getSprites().add(1,make3);
		
		if (hasController) { // display start instructions
			SplashString s2 = new SplashString("Press A to start",
					(int) ((int) WorldBuilder.ScreenSize.getWidth() / 2 / 4 - 16 - (17 * 4.1)),
					(int) WorldBuilder.ScreenSize.getHeight() / 2 / 4 + 118, 32, 100, 100000000, Color.white);
			screen.getStrings().add(s2);
			s2.setPointerList(screen.getStrings());
		} else {
			SplashString s2 = new SplashString("Press SPACE to start",
					(int) ((int) 1920/8 - 40 - (17 * 4.1)),
					(int) 1080/8 + 118, 32, 100, 100000000, Color.white);
			screen.getStrings().add(s2);
			s2.setPointerList(screen.getStrings());
		}
		
	}
	
	public static void splashOverlay(ImageIcon image, long timeOn, long endTime){
		
		screen.setDoverlay(image);
		Timer flash = new Timer();
		Timer end = new Timer();
		
		flash.schedule(new TimerTask(){
			@Override
			public void run() {
				screen.setDirectOverlay(!screen.isDirectOverlay());
			}
		}, timeOn, timeOn);
		
		end.schedule(new TimerTask(){
			@Override
			public void run() {
				flash.purge();
				flash.cancel();
				end.purge();
				end.cancel();
				screen.setDirectOverlay(false);
			}
		}, endTime);
		
	}
	
	public static void zoomOutToNormal(){
		Timer zman = new Timer();
		
		screen.setDws(5f); // set scale to 5
		
		zman.schedule(new TimerTask(){
			
			@Override
			public void run() {
				if(screen.getDws()>0){
					screen.setDws(screen.getDws() - .1f); // decrease scale coefficient
				} else {
					screen.setDws(0); // free
					zman.cancel();
					zman.purge();
				}
			}
			
		}, 20,20);
	}

	public static void testGame(String b) { // start default map
		screen.setMainMenu(false);
		screen.loadDungeon(b);
		screen.setLink(link);
		screen.setWatchLink(link);
		Music.activeMusic.playMusic(10);
		Music.activeMusic.loop = true;
		playMusicLoop();
		//zoomOutToNormal();
		
		fatherTime.schedule(new TimerTask(){
			@Override
			public void run() {
				forwardTime(1);
			}
		}, 20,1000);
		
		//screen.rain();
		
		Timer BeepersnBoopers = new Timer(); // Check player health, play sound if low.
		
		BeepersnBoopers.schedule(new TimerTask(){
			@Override
			public void run() {
				if(link.getHealth()<=link.getMaxhealth()/3+1.9)
					SoundEffect.Beat.play(false,6);
				else
					SoundEffect.Beat.stop();
			}
		}, 700,700);
		
		Timer delay = new Timer();
		delay.schedule(new TimerTask(){
			@Override
			public void run() {
				screen.setCantWalk(true);
				link.stopWalking();
			}
		}, 8750);
		
		screen.startConversation(screen.getConversation().loadConversation("Demo", new Runnable(){
			@Override
			public void run() {
				screen.setCantWalk(false);
				screen.startSpeedRun();
			}
		}));
		
		Player.ItemList.potion.unlock();
		link.setEquippedItem(Player.itemArray.get(0));
		
		Sprite skyTest = new Sprite();
		//skyTest.followPlayer(16, 22);
		skyTest.playAnimation("fog", 30, false);
		//screen.getSky().add(skyTest);
		
		// CHEATS !!!!!!!!!!!!!!!!
		screen.setCheats(false);
		screen.setSoulHardened(true);
		
		Player.ItemList.firework.unlock();
		//Player.ItemList.branch.unlock();
		Player.ItemList.teleport.unlock();
		

		
	}
	
	public static void forwardTime(int val) { // loop brightness between 0f-1f & change song accordingly.
		if (val != 0) {
			
			if(time>600 && time<1600 && !nsp){
				nsp = true;
				dsp = false;
				task.cancel();
				task = new Timer();
				Music.activeMusic.stop();
				Music.activeMusic.reinitialize(nightSong);
				Music.activeMusic.loop = true;
				WorldBuilder.playMusicLoop();
			} else if((time<600 || time>1600) && !dsp){
				dsp = true;
				nsp = false;
				task.cancel();
				task = new Timer();
				Music.activeMusic.stop();
				Music.activeMusic.reinitialize(daySong);
				Music.activeMusic.loop = true;
				WorldBuilder.playMusicLoop();
			}
			
			if (val > -1) time++;
			else time--;
			
			if (val > -1) percentTime++;
			else percentTime--;
			if (timeMod == 0) { // save Flip
				timeMod = 1000;
				time = 0;
				percentTime = 0;
			}
			if (time < 1000 && time != 0) {
				screen.setBrightness(((float) time / 1000f));
			} else if (time > 999) {
				timeMod--;
				if (timeMod == 0) {
					time = 0;
				}
				screen.setBrightness(((float) timeMod / 1000f));
			} else if (time == 0) {
				timeMod = 1000;
				screen.setBrightness(0f);
			}
			if (val > -1)
				forwardTime(val - 1);
			else
				forwardTime(val + 1);
		}
	}
	
	public static ImageIcon randomImage(ImageIcon[] im){
		return im[(int) (Math.random()*im.length)];
	}
	
	public static void checkCheat() { // for snes controller use only. Disregard FBLA :P
		
		System.out.println(pressMemory);
		
		int s = pressMemory.size() - 1;
		String[] cheats = { 
				"011105", // 		ABBBAr
				"00011105", // 		AAABBBAr
				"010101010", // 	ABABABABA
				"2002105", // 		XAAXBAr
				"2220001114", // 	XXXAAABBBl
				"2220001115", // 	XXXAAABBBrw 
				"222022025",	// 	XXXAXXAXr
				"658324004008",
				"041900072933",
				"Elementarium",
				"3818968138661633"
				};
		String cstring = "";
		String whole = "";
		
		for (String gg : pressMemory) {
			whole += gg;
		}
		
		cstring += pressMemory.get(s);
		
		boolean success = false;
		boolean win = false;
		
		for (int i = 0; i < cheats.length; i++) {
			String cheat1 = cheats[i];
			try {
				if (cstring.equals(cheat1.substring(s, s + 1))) { // heal
					success = true;
					if (whole.equals(cheat1)) {

						// Cheat Code Index Executions
						// -------------------------------------------
						if (i == 0) {
							link.addRupees(250);
						}
						if (i == 1) {
							link.setSpeed(8);
						}
						if (i == 2) {
							screen.loadDungeon("town");
						}
						if (i == 3) {
							link.setSolid(!link.isSolid());
						}
						if (i == 4) {
							link.setX(link.getX() - 128);
						}
						if (i == 5) {
							link.setX(link.getX() + 128);
						}
						if (i == 6) {
							Enemy test = new Enemy(link.getX() - 64, link.getY() - 64);
							test.spawn();
						}
						if (i == 7) {
							Enemy test = new Enemy(link.getX(), link.getY());
							test.buildAI(1);
							test.setWanderDistance(128);
							test.spawn();
						}
						if (i == 8) {
							allowMusic = false;
							SoundEffect.branch.play(false); 
							Music.activeMusic.stop();
						}
						if (i == 9) {
							Enemy test = new Enemy(link.getX(), link.getY());
							test.buildAI(2);
							test.setWanderDistance(128);
							test.spawn();
						}
						// -------------------------------------------

						pressMemory.clear();
						win = true;
					}
				}
			} catch (StringIndexOutOfBoundsException e) {
				System.out.println("Cheat exclusion - " + i);
			}
		}
		
		if (!success) {
			pressMemory.clear();
		} else if (win) {
			screen.splashText("Cheat Activated!", 96, 96, 35, 250);
		}
	}
	
	public static void initController() {
		try {
			try {
				Controllers.create();
				hasController = true;
			} catch (LWJGLException e1) {  
				e1.printStackTrace();
			}
			
			Controllers.poll();
			
			for (int i = 0; i < Controllers.getControllerCount(); i++) {
				String n = Controllers.getController(i).getName();
				System.out.println(n);
				if (n.equals("USB,2-axis 8-button gamepad  ")) {
					controller = Controllers.getController(i);
					break;
				}
			}
			
			System.out.println(controller.getName() + " - Final");
			System.out.println(controller.getButtonCount() + " Buttons");
		} catch (NullPointerException e) {
			System.err.println("No buffalo snes controller detected.");
			hasController = false;
		}
	}
	
	public static void playMusicLoop() { // loop music
		task.schedule(new TimerTask() {
			@Override
			public void run() {
				if (!Music.activeMusic.isPlaying() && !allowMusic) {
					Music.activeMusic.play();
				}
			}
		}, 30, 30);
	}
	
	public static void MaidService() { // request the garbage collector
		Timer clean = new Timer();
		clean.schedule(new TimerTask() {
			@Override
			public void run() {
				Runtime.getRuntime().gc();
				//System.err.println("Free RAM: "+Runtime.getRuntime().freeMemory());
			}
		}, 3000, 3000);
	}
	
	public static void enableController1() {
		Timer task = new Timer();
		task.schedule(new TimerTask() {
			@Override
			public void run() { // watch controller input
				
				controller.poll();
				
				// printControllerInfo();
				
				robotControl();
				
			}
		}, 60, 60);
	}
	
	public static void robotControl() { // map controller input to keyboard input mimicing,
		try {
			if (screen.hasFocus()) {
				Robot robot = new Robot();

				if (controller.getAxisValue(0) == 0 && !wu) { 
																
					wu = true;
				}
				if (controller.getAxisValue(1) == 0 && !wl) {
					wl = true;
				}
				
				int mmx = -1, mmy = -1;
				
				if (!wu) {
					mmx = 0;
				}
				if (!wl) {
					mmy = 0;
				}
				
				
				if ((!screen.isInventory() || (screen.isInventory() && padPress == false))) {
					if (controller.getAxisValue(0) == mmx) {
						robot.keyPress(KeyEvent.VK_W);
						padPress = true;
						lastPressY = 0;
						pressMemory.clear();
					}
					if (controller.getAxisValue(0) == 1) {
						robot.keyPress(KeyEvent.VK_S);
						lastPressY = 1;
						padPress = true;
						pressMemory.clear();
					}
					if (controller.getAxisValue(1) == mmy) {
						robot.keyPress(KeyEvent.VK_A);
						lastPressX = 0;
						padPress = true;
						pressMemory.clear();
					}
					if (controller.getAxisValue(1) == 1) {
						robot.keyPress(KeyEvent.VK_D);
						lastPressX = 1;
						padPress = true;
						pressMemory.clear();
					}
				}
				
				boolean skip = false;
				
				if (controller.getAxisValue(0) == 0 && lastPressY == 0) {
					padPress = false;
					robot.keyRelease(KeyEvent.VK_W);
					lastPressY = -1;
				} else if (controller.getAxisValue(0) == 0 && lastPressY == 1) {
					padPress = false;
					robot.keyRelease(KeyEvent.VK_S);
					lastPressY = -1;
				}
				
				if (controller.getAxisValue(1) == 0 && lastPressX == 0 && !skip) {
					padPress = false;
					robot.keyRelease(KeyEvent.VK_A);
					lastPressX = -1;
				} else if (controller.getAxisValue(1) == 0 && lastPressX == 1 && !skip) {
					padPress = false;
					robot.keyRelease(KeyEvent.VK_D);
					lastPressX = -1;
				}
				
				if (controller.isButtonPressed(0) && !ra) {
					robot.keyPress(KeyEvent.VK_SPACE);
					ra = true;
					pressMemory.add("0");
					checkCheat();
				} else if (!controller.isButtonPressed(0) && ra) {
					ra = false;
				}
				if (controller.isButtonPressed(1) && !rb) {
					robot.keyPress(KeyEvent.VK_E);
					rb = true;
					pressMemory.add("1");
					checkCheat();
				} else if (!controller.isButtonPressed(1) && rb) {
					rb = false;
				}
				
				if (controller.isButtonPressed(2) && !rx) {
					rx = true;
					robot.keyPress(KeyEvent.VK_Q);
					pressMemory.add("2");
					checkCheat();
				} else if (!controller.isButtonPressed(2) && rx) {
					rx = false;
				}
				
				if (controller.isButtonPressed(5) && !rb2) {
					rb2 = true;
					robot.keyPress(KeyEvent.VK_R);
					pressMemory.add("5");
					checkCheat();
				} else if (!controller.isButtonPressed(5) && rb2) {
					rb2 = false;
				}
				
				if (controller.isButtonPressed(6) && sel) {
					sel = false;
					pressMemory.add("6");
					checkCheat();
				} else if(!controller.isButtonPressed(6) && !sel){
					sel = true;
				}
				if (controller.isButtonPressed(4)) {
					// robot.keyPress(KeyEvent.VK_MINUS);
					pressMemory.add("4");
					checkCheat();
				} else {

				}
				if (controller.isButtonPressed(7) && start) {
					pressMemory.add("7");
					checkCheat();
					start = false;
				} else if(!controller.isButtonPressed(7) && start){

				}
			}
		} catch (AWTException e) {
			e.printStackTrace();
		}
	}
	
	public static boolean isAnyButtonPressed() {
		
		for (int i = 0; i < controller.getAxisCount(); i++)
			if (controller.getAxisValue(i) != 0) {
				return true;
			}
		
		for (int i = 0; i < controller.getButtonCount(); i++)
			if (controller.isButtonPressed(i)) {
				return true;
			}
		
		return false;
	}
	
	public static void checkController() {
		for (int i = 0; i < controller.getAxisCount(); i++)
			if (controller.getAxisValue(i) != 0) {
				requestCheck = true;
			}
	}
	
	public static void printControllerInfo() {
		for (int i = 0; i < controller.getButtonCount(); i++)
			if (controller.isButtonPressed(i))
				System.out.println(controller.getButtonName(i) + " WAS PRESSED.");
		for (int i = 0; i < controller.getAxisCount(); i++)
			System.err.println(controller.getAxisValue(i) + " --- " + i);
	}
	
	public static Font getScaledFont(float s) { // return font of the scaled default font.
		ge = GraphicsEnvironment.getLocalGraphicsEnvironment();
		try {
			ge.registerFont(Font.createFont(Font.TRUETYPE_FONT, new File("Fonts\\lttp.ttf")).deriveFont(32));
			return (ge.getAllFonts()[ge.getAllFonts().length - 1].deriveFont(s));
		} catch (FontFormatException | IOException e) {
			e.printStackTrace();
		}
		return null;
	}
	
	public static boolean isParsableFloat(String input){
	    boolean parsable = true;
	    try{
	        Float.parseFloat(input);
	    }catch(NumberFormatException e){
	        parsable = false;
	    }
	    return parsable;
	}
	
	public static boolean isParsableInt(String input){
	    boolean parsable = true;
	    try{
	        Integer.parseInt(input);
	    }catch(NumberFormatException e){
	        parsable = false;
	    }
	    return parsable;
	}
	
	public static void loadTileset() { // load sprites into RAM
		
		int c = 0;
		for (int i = 0; i < 99; i++) {
			for (int f = 0; f < 12; f++) {
				c++;
				spritelist.add(new Sprite("tiles\\" + c + ".png"));
			}
		}
	}
	
}
