package loz;

import java.awt.AlphaComposite;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.awt.event.MouseWheelEvent;
import java.awt.event.MouseWheelListener;
import java.awt.geom.AffineTransform;
import java.awt.geom.Area;
import java.awt.geom.Ellipse2D;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.ConcurrentModificationException;
import java.util.NoSuchElementException;
import java.util.Scanner;
import java.util.Timer;
import java.util.TimerTask;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JTextArea;
import javax.swing.JTextField;

import eui.Score;
import eui.SplashString;
import loz.Player.EquipmentList;
import loz.Player.ItemList;
import loz.WorldItem.Script;
import particleForge.Particle;

@SuppressWarnings("serial")
public class Window extends JPanel implements KeyListener, MouseListener, MouseMotionListener, MouseWheelListener {

	private boolean gameUI = true, mainMenu = false, inventory = false, invOne = false, invTwo = false, drawOne = true,
			conv, loading, cantWalk, highscore, controls, dead, cheats, soulHardened, releasedUse, edisplay, directOverlay;
	private int xShift = 0, yShift = 0, dungeonSize = 0, convLine, deathSelect, element;
	private float pscale = 4, dws = 0, brightness = 0.25f, insideLight = -1, speedRunTime;
	private Player link = null;

	private Score[] Hscores;

	private Conversation conversation = new Conversation();

	private ArrayList<Integer> usedChests = new ArrayList<Integer>();

	private ArrayList<Sprite> sprites = new ArrayList<Sprite>(); // list of
																	// drawable
																	// objects
																	// sorted
																	// into
																	// several
																	// arraylists
																	// for
																	// organization
	private ArrayList<Enemy> enemies = new ArrayList<Enemy>();
	private ArrayList<Sprite> collisions = new ArrayList<Sprite>();
	private ArrayList<WorldItem> objects = new ArrayList<WorldItem>();
	private ArrayList<WorldItem> drops = new ArrayList<WorldItem>();
	private ArrayList<Integer> breakList = new ArrayList<Integer>();
	private ArrayList<Integer> animated = new ArrayList<Integer>();
	private ArrayList<SplashString> strings = new ArrayList<SplashString>();
	private ArrayList<Particle> particleSystem = new ArrayList<Particle>();
	private ArrayList<Light> Lights = new ArrayList<Light>();
	private ArrayList<Sprite> overlays = new ArrayList<Sprite>();
	private ArrayList<Sprite> sky = new ArrayList<Sprite>();
	private ArrayList<Sprite> uiSprites = new ArrayList<Sprite>();

	private Sprite watchLink = link; // default sprite for camera to follow
	private Color back = Color.black;
	private Timer particler;
	private Timer runTime;

	private Image bg = new ImageIcon("").getImage();
	private ImageIcon doverlay = new ImageIcon("");
	private JTextArea show = new JTextArea();
	private String northMap = "", southMap = "", westMap = "", eastMap = "";

	public Window() {
		WorldBuilder.loadTileset();
		this.addKeyListener(this);
		setFocusable(true);
		this.addMouseListener(this);
		this.addMouseMotionListener(this);
		this.addMouseWheelListener(this);
		this.setLayout(null);

		show.setPreferredSize(new Dimension(1750 / 4, 400 / 4));
		show.setBounds(170 / 4 - 8, 880 / 4 - 45, 1750 / 4, 400 / 4);
		show.setOpaque(false);
		show.setColumns(40);
		show.setRows(20);

		this.add(show);

	}

	public void startSpeedRun() { // start speedrun countdown
		runTime = new Timer();
		runTime.schedule(new TimerTask() {
			@Override
			public void run() {
				speedRunTime += 0.01f;
			}
		}, 10, 10);
	}

	public void stopSpeedRun() { // pause speedrun countdown
		runTime.cancel();
		runTime.purge();
		runTime = new Timer();
	}
	
	public void displayElement(int el){
		element = el;
		edisplay = true;
	}

	public void endSpeedRun() throws FileNotFoundException { // end speedrun
																// countdown and
																// save into the
																// highscore
																// list
		runTime.cancel();
		runTime.purge();
		runTime = new Timer();

		FileWriter saver;
		try {
			saver = new FileWriter("Saves\\speenrun.sav", true);
			saver.write("\n" + nameCreator() + "\n");
			saver.write("" + (speedRunTime-(int)(link.getRupees()/2)+300));
			saver.close();
			highscore = true;
			loadDungeon("null");
		} catch (IOException e) {
			e.printStackTrace();
		}

		speedRunTime = -1;

		setScores();

	}

	private String getCharForNumber(int i) {
		return i > 0 && i < 27 ? String.valueOf((char) (i + 64)) : null;
	}

	public String nameCreator() {

		Object[] options1 = new Object[27];

		JPanel panel = new JPanel();
		panel.add(new JLabel("Input 3 Initials"));

		JTextField textField = new JTextField(3);
		textField.setEditable(false);
		textField.setOpaque(false);
		textField.setFocusable(false);
		for (int i = 1; i < 27; i++) {
			JButton b = new JButton(getCharForNumber(i));
			b.addActionListener(new ActionListener() {
				@Override
				public void actionPerformed(ActionEvent arg0) {
					if (textField.getText().length() < 3) {
						textField.setText(textField.getText() + b.getText());
					}
				}
			});
			b.setText(getCharForNumber(i));
			options1[i - 1] = b;

			JButton c = new JButton(getCharForNumber(i));
			c.addActionListener(new ActionListener() {
				@Override
				public void actionPerformed(ActionEvent arg0) {
					if (textField.getText().length() > 0) {
						textField.setText(textField.getText().substring(0, textField.getText().length() - 1));
					}
				}
			});
			c.setText("DEL");
			options1[26] = c;
		}

		panel.add(textField);

		int result = JOptionPane.showOptionDialog(null, panel, "Enter 3 Initials", JOptionPane.PLAIN_MESSAGE,
				JOptionPane.PLAIN_MESSAGE, null, options1, null);
		if (result == JOptionPane.YES_OPTION) {
			JOptionPane.showMessageDialog(null, textField.getText());
		}

		return textField.getText();
	}

	public void useLogic() { // Spacebar logical code

		if (link != null && link.isLifting()) {
			int l = link.getLastLiftID();
			link.setLastLiftID(-1);
			link.setLifting(false);
			Projectile throwing = new Projectile(WorldBuilder.spritelist.get(l).getImage(),
					WorldBuilder.spritelist.get(l).getImage());
			throwing.setX(link.getX());
			throwing.setY(link.getY());
			throwing.Spawn(link.getDir());
		} else if (link != null && !link.isLifting()) {
			findPickup();
		}

	}

	public void findPickup() { // check ahead for pickable object
		boolean frob = true;
		for (WorldItem w : objects) {

			if (!w.isUsed()) {
				if (isPickable(w)) {
					Rectangle r1 = new Rectangle(w.getX() + 4, w.getY() + 3, 8, 8);

					int modx = 0, mody = 16;

					if (link.getImage() == link.getIdleD() // determine
															// direction
							|| link.getImage() == link.getAnimD()) {
						mody += 16;
					}
					if (link.getImage() == link.getIdleU() || link.getImage() == link.getAnimU()) {
						mody -= 8;
					}
					if (link.getImage() == link.getIdleR() || link.getImage() == link.getAnimR()) {
						modx += 8;
					}
					if (link.getImage() == link.getIdleL() || link.getImage() == link.getAnimL()) {
						modx -= 8;
					}
					
					Rectangle r2 = new Rectangle(link.getX() + modx, link.getY() + mody, 16, 16);
					
					if (r2.intersects(r1)) {
						w.setPickupBreak(true);
						link.stopWalking();
						if (w.getScript() != null && w.getExecute() != null && !w.isUsed()) {
							w.getExecute().run();
							w.setUsed(true);
							frob = false;
							break;
						}
					}
				}
			}
		}
		if (frob) {
			link.frob();
		}
	}

	private boolean isPickable(WorldItem w) { // arguments for worlditem frob
												// check

		int[] ids = { 66, 78, 90, 24, 25, 26, 36, 37, 38, 12 }; // ID Offset -1
																// -- LEVEL 1
																// PICKUP POWER
		for (int i = 0; i < ids.length; i++) {
			if (w.getTileid() == ids[i]) {
				return true;
			}
		}

		int[] ids2 = { 67 }; // ID Offset -1 -- LEVEL 2 PICKUP POWER
		for (int i = 0; i < ids2.length; i++) {
			if (w.getTileid() == ids2[i]) {
				if (link.getGripPower() > 1) {
					setItemPickable(w);
					return true;
				}
			}
		}

		int[] ids3 = { 79 }; // ID Offset -1 -- LEVEL 3 PICKUP POWER
		for (int i = 0; i < ids3.length; i++) {
			if (w.getTileid() == ids3[i]) {
				if (link.getGripPower() > 2) {
					setItemPickable(w);
					return true;
				}
			}
		}

		return false;
	}

	public void setItemPickable(WorldItem w) {
		w.setScript(WorldItem.Script.BREAK);
		w.setExecute(new Runnable() {
			@Override
			public void run() {
				if (!w.isUsed()) {
					if (w.isPickupBreak()) {
						link.setLifting(true);
						link.setLastLiftID(w.getTileid());
						w.setPickupBreak(false);
						// getExecute().run();
						w.setExecute(null);
						w.setUsed(true);
						w.setScript(null);
					}
					w.usedImageGenerate();
					w.setScript(null);
					w.setExecute(null);
					w.removeObject();
					w.setUsed(true);
				}
			}
		});
	}

	public void rain() {
		Sprite make3 = new Sprite();
		make3.setX(0);
		make3.setY(0);
		make3.setWidth(32);
		make3.setHeight(32);
		make3.playAnimation("rain", 30, false);
		WorldBuilder.screen.getOverlays().add(make3);
		make3.followPlayer(-256, -128);
	}

	public void fireFlies() {
		Sprite make3 = new Sprite();
		make3.setX(0);
		make3.setY(0);
		make3.setWidth(32);
		make3.setHeight(32);
		make3.playAnimation("firefly", 30, false);
		WorldBuilder.screen.getOverlays().add(make3);
		make3.followPlayer(-256, -128);
	}

	public void loadDungeon() {
		String dn = JOptionPane.showInputDialog("Input map name", JOptionPane.WANTS_INPUT_PROPERTY);
		loadDungeon(dn);
	}
	
	public void clearProjectiles(){
		for (int i = 0; i < collisions.size(); i++) {
			if (collisions.get(i) instanceof Projectile) {
				((Projectile) collisions.get(i)).getFly().cancel();
				((Projectile) collisions.get(i)).getFly().purge();
				((Projectile) collisions.get(i)).getAnimate().cancel();
				((Projectile) collisions.get(i)).getAnimate().purge();
				collisions.get(i).free();
				collisions.remove(i);
				i--;
			}
		}
	}

	public void unloadRoom(Runnable runnable) {
		loading = true;
		// free lists
		for (int i = 0; i < sprites.size(); i++) {
			sprites.set(i, null);
		}

		for (int i = 0; i < overlays.size(); i++) {
			overlays.set(i, null);
		}
		do {
			for (int i = 0; i < enemies.size(); i++) {
				if (enemies.get(i).getWaiter() != null) {
					enemies.get(i).getWaiter().cancel();
					enemies.get(i).getWaiter().purge();
				}
				enemies.get(i).setForcedkill(true);
				enemies.get(i).modhp(-(enemies.get(i).getHealth()) - 1);
			}
		} while (enemies.size() > 0);
		for (int i = 0; i < drops.size(); i++) {
			drops.get(i).setX(-999);
			drops.get(i).setY(-999);
			drops.set(i, null);
		}
		for (int i = 0; i < objects.size(); i++) {
			objects.get(i).setX(-999);
			objects.get(i).setY(-999);
			objects.set(i, null);
		}

		do {
			for (int i = 0; i < collisions.size(); i++) {
				if (collisions.get(i) instanceof Projectile) {
					((Projectile) collisions.get(i)).getFly().cancel();
					((Projectile) collisions.get(i)).getFly().purge();
					((Projectile) collisions.get(i)).getAnimate().cancel();
					((Projectile) collisions.get(i)).getAnimate().purge();
					collisions.set(i, null);
					i--;
				}
				collisions.remove(0);
			}
		} while (collisions.size() > 0);

		sprites.clear();
		overlays.clear();
		collisions.clear();
		objects.clear();
		drops.clear();
		breakList.clear();
		animated.clear();
		strings.clear();
		enemies.clear();
		particleSystem.clear();
		Lights.clear();

		sprites = null;
		overlays = null;
		collisions = null;
		objects = null;
		drops = null;
		breakList = null;
		animated = null;
		strings = null;
		enemies = null;
		particleSystem = null;
		Lights = null;

		if (particler != null) {
			particler.cancel();
			particler.purge();
		}
		particler = new Timer();

		sprites = new ArrayList<Sprite>();
		overlays = new ArrayList<Sprite>();
		collisions = new ArrayList<Sprite>();
		objects = new ArrayList<WorldItem>();
		drops = new ArrayList<WorldItem>();
		breakList = new ArrayList<Integer>();
		animated = new ArrayList<Integer>();
		strings = new ArrayList<SplashString>();
		enemies = new ArrayList<Enemy>();
		particleSystem = new ArrayList<Particle>();
		Lights = new ArrayList<Light>();
		
		Timer rada = new Timer();
		rada.schedule(new TimerTask(){
			@Override
			public void run() {
				if(runnable!=null){
					runnable.run();
				}
			}
		}, 500);
	}

	public void loadDungeon(String dn) {

		unloadRoom(null);

		setDws(50f);
		Timer delayedStart = new Timer();
		
		delayedStart.schedule(new TimerTask(){

			@Override
			public void run() {
				
				if (!dead) {

					Scanner scan;
					Scanner scan2;
					Scanner scan3;
					Scanner scan4;

					// OBJECTS
					try {
						scan3 = new Scanner(new File("animated.txt"));
						do { // load animated sprite data
							animated.add((Integer.parseInt(scan3.nextLine()) - 1));
						} while (scan3.hasNextLine());

						scan3.close();

					} catch (FileNotFoundException e) {
						e.printStackTrace();
					}
					bg = new ImageIcon("Maps\\" + dn + ".png").getImage();
					System.out.println(bg + " - " + new File("Maps\\" + dn + ".png").exists());
					try {
						scan = new Scanner(new File("World\\" + dn + "COLLISIONMAP.dng")); // load
																							// collisions
						do {
							String tid = scan.nextLine();
							WorldItem s = new WorldItem();
							if (!tid.equals("null")) {
								s.setCollision(s.setCollisionData(tid));
								// System.out.println("");
								s.setCollisionData(s.getTileid() - 1);
								s.setId(s.getTileid() - 1);
								s.setWidth(16);
								s.setHeight(16);
								s.setY(Integer.parseInt(scan.nextLine()) * 16);
								s.setX(Integer.parseInt(scan.nextLine()) * 16);

								try {
									s.setImage(s.collisionImage().getImage());
								} catch (NullPointerException n) {

								}
							} else {
								scan.nextLine();
								scan.nextLine();
							}

							if (s.getCollision() != null) {
								collisions.add(s);
							}
							s = null;
						} while (scan.hasNextLine());

						scan.close();
					} catch (FileNotFoundException e) {
						// e.printStackTrace();
					}

					// OBJECTS
					int c = -1;
					try {
						scan = new Scanner(new File("World\\" + dn + "SPAWNS.dng")); // load
																						// spawns
						do {

							c++;
							scan2 = new Scanner(new File("break.txt"));
							String tid = scan.nextLine();
							if (c == 0) {
								breakList.add(Integer.parseInt(tid));
							}

							WorldItem s = new WorldItem();
							s.setTileid(Integer.parseInt(tid) - 1);
							s.setCollision(s.setCollisionData(tid));
							// s.setCollisionData(s.getTileid()-1);
							s.setId(s.getTileid() - 1);
							s.setWidth(16);
							s.setHeight(16);
							s.setY(Integer.parseInt(scan.nextLine()) * 16);
							s.setX(Integer.parseInt(scan.nextLine()) * 16);
							s.setImage(WorldBuilder.spritelist.get(s.getTileid()).getImage());

							if (!matchOverlay(s.getTileid()) && !matchTeleport(s.getTileid()) && !matchAI(s.getTileid())) {

								try {
									do {
										int g = (Integer.parseInt(scan2.nextLine()) - 1);
										if (!checkList(s.getTileid()) && g == s.getTileid()) {
											s.setScript(WorldItem.Script.BREAK);
											s.generateScript();
										}
									} while (scan2.hasNextLine());
								} catch (NoSuchElementException e) {

								}

								if (s.checkChest()) { // find chest, set id
									s.setUsedID(Integer.parseInt(scan.nextLine()));
									if (!checkUsedChest(s.getUsedID())) {
										s.setScript(Script.chest);
										s.generateScript();
									} else {
										s.usedImageGenerate();
									}
								}

								objects.add(s);
								s = null;
							} else if (!matchTeleport(s.getTileid()) && !matchAI(s.getTileid())) {
								overlays.add(s);
								s = null;
							} else if (matchAI(s.getTileid())) { // is an ai
								Enemy test = new Enemy(s.getX(), s.getY());
								test.buildAI(Integer.parseInt(scan.nextLine()));
								test.setWanderDistance(128);
								test.spawn();
								test = null;
								s = null;
							} else { // is a door
								// Dont add, set collide script
								s.setId(-1);
								s.setScript(WorldItem.Script.USE_DOOR);
								s.generateScript();
								s.startCollisionWatchDelay(350);
								s.setId(Integer.parseInt(scan.nextLine()));

								s.setRefName(scan.nextLine());
								s.setFacing(scan.nextLine());
								objects.add(s);
							}

							if (s != null && s.getCollision() != null)
								collisions.add(s);
							s = null;

						} while (scan.hasNextLine());

						scan.close();
						scan2.close();

					} catch (FileNotFoundException e) {
						// e.printStackTrace(); Wrong input name.
					} catch (NoSuchElementException noSpawns) {
						// Empty file will load no spawns
					} catch (NumberFormatException noSpawns) {
						// abrupt end. Stop loading, but run level anyway
					}

					try {
						scan4 = new Scanner(new File("World\\" + dn + "INFO.dng"));

						do {

							setDungeonSize(Integer.parseInt(scan4.nextLine()));
							northMap = scan4.nextLine();
							southMap = scan4.nextLine();
							eastMap = scan4.nextLine();
							westMap = scan4.nextLine();

							try {
								insideLight = Float.parseFloat(scan4.nextLine());
							} catch (NoSuchElementException nl) {
								insideLight = -1;
							} catch (NumberFormatException s) {
							}

							int songID = 0;
							try {
								songID = Integer.parseInt(scan4.nextLine());
							} catch (NoSuchElementException nl) {
								songID = -1;
							}

							if (songID != -1
									&& !Music.getMusicURL()[songID].equals("Music\\" + Music.activeMusic.getFile().getName())) {
								try {
									WorldBuilder.task.cancel();
									WorldBuilder.task = new Timer();
									Music.activeMusic.stop();
									Music.activeMusic.reinitialize(Music.getMusicURL()[songID]);
									WorldBuilder.playMusic(songID);
									WorldBuilder.daySong = Music.getMusicURL()[songID];
								} catch (IllegalArgumentException ag) {

								}
							}

						} while (scan4.hasNextLine());

						scan4.close();

					} catch (FileNotFoundException e) {
						// e.printStackTrace(); Wrong input name
					}

					/*
					 * // splash room name SplashString s = new SplashString(
					 * "Entering: " + dn, (int) ((int)
					 * WorldBuilder.ScreenSize.getWidth() / 2 / 4 - 80 - (dn.length() *
					 * 4.1)), (int) WorldBuilder.ScreenSize.getHeight() / 2 / 4, 32,
					 * 100, 1000, Color.black); getStrings().add(s);
					 * s.setPointerList(getStrings());
					 * 
					 * SplashString s2 = new SplashString("Entering: " + dn, (int)
					 * ((int) WorldBuilder.ScreenSize.getWidth() / 2 / 4 - 79 -
					 * (dn.length() * 4.1)), (int) WorldBuilder.ScreenSize.getHeight() /
					 * 2 / 4 + 1, 32, 100, 1000, Color.yellow); getStrings().add(s2);
					 * s2.setPointerList(getStrings());
					 */

					// free scanners and run gc
					scan = null;
					scan2 = null;
					scan3 = null;
					scan4 = null;

					Runtime.getRuntime().gc();

					Timer loader = new Timer();

					if (!dead)
						loader.schedule(new TimerTask() {
							@Override
							public void run() {
								loading = false;
								cantWalk = false;
							}
						}, 1500);
					else
						loader = null;
					if (link != null)
						link.stopWalking();
					cantWalk = true;
					setDws(0f);
					bg = new ImageIcon("Maps\\" + dn + ".png").getImage();
					
					/*Sprite menuIcon = new Sprite();
					menuIcon.setImage(new ImageIcon("Images\\barcodes\\2.png"));
					menuIcon.setWidth(206);
					menuIcon.setHeight(50);
					menuIcon.setX(link.getX());
					menuIcon.setY(link.getY());
					menuIcon.setName("bc1");
					menuIcon.setScale(0.5);
					getOverlays().add(menuIcon);*/
					
				}
				
			}
			
		}, 3500);
		
	
	}

	private boolean checkUsedChest(int usedID) {

		for (int i = 0; i < usedChests.size(); i++) {
			if (usedChests.get(i) == usedID) {
				return true;
			}
		}

		return false;
	}

	public void arriveAtDoor(int id) { // find a matching door and move to it.
		for (int i = 0; i < objects.size(); i++) {
			if (objects.get(i).getRefName().length() > 1) {
				if (objects.get(i).getId() == id) {
					link.setX((objects.get(i).getX()));
					link.setY((objects.get(i).getY()));

					if (objects.get(i).getFacing().equalsIgnoreCase("Down")) {
						link.setY(link.getY() - 40);
					}
					if (objects.get(i).getFacing().equalsIgnoreCase("Up")) {
						link.setY(link.getY() + 24);
					}
					if (objects.get(i).getFacing().equalsIgnoreCase("Left")) {
						link.setX(link.getX() + 40);
					}
					if (objects.get(i).getFacing().equalsIgnoreCase("Right")) {
						link.setX(link.getX() - 22);
					}
				}
			}
		}
	}

	private boolean matchOverlay(int id) {
		int[] ids = { 804, 816, 698, 792, 758, 757, 756, 715, 727, 739, 742, 741, 740, 307, 309, 311, 323, 250, 274,
				262, 320, 333, 309, 440, 450, 449, 448, 447, 446, 461, 460, 459, 458, 472, 471, 470, 484, 436, 435, 434,
				864, 852, 876, 872, 884, 860, 961, 973, 949, 985 };
		
		for (int i = 0; i < ids.length; i++) {
			if (id == ids[i]) {
				return true;
			}
		}
		return false;
	}

	public boolean matchAI(int id) {
		int[] ids = { 273 }; // ID Offset -1
		for (int i = 0; i < ids.length; i++) {
			if (id == ids[i]) {
				return true;
			}
		}
		return false;
	}

	private boolean matchTeleport(int id) {
		int[] ids = { 614,626,627,639}; // id array - All should be added to no list
									// instead of objects. ID Offset -1
		for (int i = 0; i < ids.length; i++) {
			if (id == ids[i]) {
				return true;
			}
		}
		return false;
	}

	public void deathScreen() { // game over
		dead = true;
		deathSelect = 0;
		unloadRoom(new Runnable(){ // unload room and then display cool fire animation
			@Override
			public void run() {
				link.respawn();
				Sprite bg = new Sprite();
				bg.setX(+16);
				bg.setY(+50);
				WorldBuilder.screen.getOverlays().add(bg);
				bg.playAnimation("firewall", 15, false);
			}
			
		});
	}

	private boolean checkList(int id) { // temp always false. Will implement in a later update
		return false;
	}

	public void paintComponent(Graphics g) { // paint screen override method. Includes all draws of screen from the organized sprite lists.
											 // and handles all screen draw effects & menus.
		super.paintComponent(g);
		try {
			Graphics2D g2 = (Graphics2D) g;
			int modx = 0;
			int mody = 0;

			try {
				modx = watchLink.getX() + 8;
				mody = watchLink.getY() + 8;
			} catch (NullPointerException nm) {
			}

			int modfx = 0, modfy = 0;
			
			// set ratio between screen resolution and 1080p so that it can scale up/down.
			double dw = (double) WorldBuilder.screen.getWidth() / (double) 1920;
			double dh = (double) WorldBuilder.screen.getHeight() / (double) 1080;

			dw += dws;
			dh += dws;

			// scale screen to 1080p ratio, and multiply by zoom.
			g2.scale(pscale * dw, pscale * dh);

			if (-modx + (WorldBuilder.ScreenSize.width / 8) / dw < 0) {
				modfx = (int) (-modx + (WorldBuilder.ScreenSize.width / 8) / dw);
			}

			if (-mody + (WorldBuilder.ScreenSize.height / 8) / dh < 0) {
				modfy = (int) (-mody + (WorldBuilder.ScreenSize.height / 8) / dh);
			}

			if (watchLink == link) { // if the watch link is the player, follow him, and stay within the confines of the map.
				if (modx + (WorldBuilder.ScreenSize.width / 8) / dw > dungeonSize * 16) {
					modfx = (int) (-dungeonSize * 16 + (WorldBuilder.ScreenSize.width / 4) / dw);
				}

				if (mody + (WorldBuilder.ScreenSize.height / 8) / dh > dungeonSize * 16) {
					modfy = (int) ((-dungeonSize * 16) + (WorldBuilder.ScreenSize.height / 4) / dh);
				}
			}

			// offset graphics
			g2.translate(modfx, modfy);
			g2.setColor(Color.black);
			// fill bg
			g2.fillRect(-50, -50, 5000, 5000);
			
			// If loading, skip
			if (!loading) {

				g2.drawImage(bg, 0, 0, null);

				try {
					// draw all sprites, etc.
					for (Sprite s : sprites) {
						g2.drawImage(s.getImage().getImage(), s.getX(), s.getY(), null);
					}
					for (WorldItem s : objects) {
						g2.drawImage(s.getImage().getImage(), s.getX(), s.getY(), null);
					}
					try {
						for (WorldItem s : drops) {
							if(!checkAnimated((s.getTileid()+1)))
								s.setImage(new ImageIcon("Tiles\\"+(s.getTileid()+1)+".png"));
							g2.drawImage(s.getImage().getImage(), s.getX(), s.getY(), null);
						}
					} catch (NoSuchElementException ff) {

					}
					for (Sprite s : collisions) {
						if (s instanceof Projectile && ((Projectile) s).isVisible())
							g2.drawImage(s.getImage().getImage(), s.getX(), s.getY(), null);
					}
					for (Enemy s : enemies) { // draw enemies that are above player
						if (!s.isInvisible()){
							if(s.getY()+s.getHeight()/2 < link.getY()+22)
								g2.drawImage(s.getImage().getImage(), s.getX(), s.getY(), null);
						}
					}
					for (int i = 0; i < particleSystem.size(); i++) {
						try {
							Particle p = particleSystem.get(i);
							if (p.isBehind()) {
								particleSystem.get(i).update((float) (Math.random() * 5f));
								p.setAlpha(p.getAlpha() - 15.25f);
								g2.setColor(new Color(p.getR(), p.getG(), p.getB(), (int) p.getAlpha()));
								if (p.getImage() == null) {
									g2.fillOval(p.getPosition().getX(), p.getPosition().getY(), p.getRadius(),
											p.getRadius());
								} else {
									g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER, p.getAlpha()));
									g2.drawImage(p.getImage().getImage(), p.getPosition().getX(),
											p.getPosition().getY(), p.getImage().getIconWidth(),
											p.getImage().getIconHeight(), null);
								}
								g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER, 1));
							}
						} catch (NullPointerException nn) {

						}
					}
				} catch (NullPointerException e) {

				} catch (ConcurrentModificationException t) {
					// Waiting
				}
				
				if (!dead && link != null && link.getOverrideImage() == null) {
					// player specific draws
					if (link != null && !link.getInvisible())
						g2.drawImage(link.getImage().getImage(), link.getX() + link.getOffsetX(), link.getY(), null); // draw shield over player
																													 // so that the shield may
																													// be changed.
					if (link != null && link.isLifting()) {
						Sprite lift = WorldBuilder.spritelist.get(link.getLastLiftID());
						g2.drawImage(lift.getImage().getImage(), link.getX() + link.getOffsetX(), link.getY(), null);
					}
					if (link != null && link.getImage() == link.getAnimD() && !link.getInvisible()) {
						g2.drawImage(
								new ImageIcon("Images\\People\\shield" + link.getShieldlevel() + ".gif").getImage(),
								link.getX() + link.getOffsetX(), link.getY(), null);
					}
					if (link != null && link.getImage() == link.getAnimR() && !link.getInvisible()) {
						g2.drawImage(
								new ImageIcon("Images\\People\\shield" + link.getShieldlevel() + "r.gif").getImage(),
								link.getX() + link.getOffsetX(), link.getY(), null);
					}
					if (link != null && link.getImage() == link.getAnimL() && !link.getInvisible()) {
						g2.drawImage(
								new ImageIcon("Images\\People\\shield" + link.getShieldlevel() + "l.gif").getImage(),
								link.getX() + link.getOffsetX(), link.getY(), null);
					}
					if (link != null && link.getImage() == link.getAnimU() && !link.getInvisible()) {
						g2.drawImage(
								new ImageIcon("Images\\People\\shield" + link.getShieldlevel() + "u.gif").getImage(),
								link.getX() + link.getOffsetX(), link.getY(), null);
					}

					if (link != null && link.getImage() == link.getIdleD() && !link.getInvisible()) {
						g2.drawImage(
								new ImageIcon("Images\\People\\shield" + link.getShieldlevel() + ".png").getImage(),
								link.getX() + link.getOffsetX(), link.getY(), null);
					}
					if (link != null && link.getImage() == link.getIdleR() && !link.getInvisible()) {
						g2.drawImage(
								new ImageIcon("Images\\People\\shield" + link.getShieldlevel() + "r.png").getImage(),
								link.getX() + link.getOffsetX(), link.getY(), null);
					}
					if (link != null && link.getImage() == link.getIdleL() && !link.getInvisible()) {
						g2.drawImage(
								new ImageIcon("Images\\People\\shield" + link.getShieldlevel() + "l.png").getImage(),
								link.getX() + link.getOffsetX(), link.getY(), null);
					}
					if (link != null && link.getImage() == link.getIdleU() && !link.getInvisible()) {
						g2.drawImage(
								new ImageIcon("Images\\People\\shield" + link.getShieldlevel() + "u.png").getImage(),
								link.getX() + link.getOffsetX(), link.getY(), null);
					}
				}
				
				for (Enemy s : enemies) { // draw enemies that are below player
					if (!s.isInvisible()){
						if(s.getY()+s.getHeight()/2 >= link.getY()+22)
							g2.drawImage(s.getImage().getImage(), s.getX(), s.getY(), null);
					}
				}
				// Update particles
				for (int i = 0; i < particleSystem.size(); i++) {
					try {
						Particle p = particleSystem.get(i);
						if (!p.isBehind()) {
							particleSystem.get(i).update((float) (Math.random() * 5f));
							p.setAlpha(p.getAlpha() - 15.25f);
							g2.setColor(new Color(p.getR(), p.getG(), p.getB(), (int) p.getAlpha()));
							if (p.getImage() == null) {
								g2.fillOval(p.getPosition().getX(), p.getPosition().getY(), p.getRadius(),
										p.getRadius());
							} else {
								g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER, p.getAlpha()));
								g2.drawImage(p.getImage().getImage(), p.getPosition().getX(), p.getPosition().getY(),
										p.getImage().getIconWidth(), p.getImage().getIconHeight(), null);
							}
							g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER, 1));
						}
					} catch (NullPointerException nn) {

					}
				}

			}
			g2.translate(-modfx, -modfy);

			g2.scale(1 / (pscale * dw), 1 / (pscale * dh));
			dw -= dws;
			dh -= dws;
			g2.scale(pscale * dw, pscale * dh);
			if (loading)
				drawLoadingScreen(g2);
			if (!loading) {
				
				try {
					for (SplashString s : strings) {
						g2.setFont(s.getFont());
						g2.setColor(s.getColor());
						g2.drawString(s.getText(), s.getX(), s.getY());
						if (s.isRemove()) {
							strings.remove(s);
						}
					}
				} catch (ConcurrentModificationException cm) {
				}

				g2.scale(1 / (pscale * dw), 1 / (pscale * dh));
				dw += dws;
				dh += dws;
				g2.scale(pscale * dw, pscale * dh);

				g2.translate(modfx, modfy);
				try {
					for (Sprite s : overlays) {
						if (s.getImage() != null) {
							if (s.isCanRotate()) {
								((Graphics2D)g).rotate(s.getRotateAngle(), s.getX()+s.getWidth()/2 + s.getOffSetX(), s.getY()+s.getHeight()/2 + s.getOffSetY());
								g2.drawImage(s.getImage().getImage(), s.getX() + s.getOffSetX(), s.getY() + s.getOffSetY(), (int)(s.getWidth()*s.getScale()), (int)(s.getHeight()*s.getScale()), null);
								((Graphics2D)g).rotate(-s.getRotateAngle(), s.getX()+s.getWidth()/2 + s.getOffSetX(), s.getY()+s.getHeight()/2 + s.getOffSetY());
							} else {
								g2.drawImage(s.getImage().getImage(), s.getX(), s.getY(), null);
							}
					           
						} else {
							s = null;
						}
					}
				} catch (ConcurrentModificationException c) {
				}

				if (link != null) {
					if (insideLight < 0) {
						g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER, brightness));
						g2.drawImage(new ImageIcon("Images\\Sight" + link.getDir() + ".png").getImage(),
								link.getX() - 1920 / 2 + 8, link.getY() - 1080 / 2 + 8, null);
					} else if (new File("Images\\Sight" + link.getDir() + ".png").exists()) {
						g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER, insideLight));
						g2.drawImage(new ImageIcon("Images\\Sight" + link.getDir() + ".png").getImage(),
								link.getX() - 1920 / 2 + 8, link.getY() - 1080 / 2 + 8, null);
					} else {
						g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER, insideLight));
						g2.drawImage(new ImageIcon("Images\\Sight1.png").getImage(), link.getX() - 1920 / 2 + 8,
								link.getY() - 1080 / 2 + 8, null);
					}
					g2.setComposite(AlphaComposite.getInstance(AlphaComposite.SRC_OVER, 1f));
				}

				g2.translate(-modfx, -modfy);

				g2.scale(1 / (pscale * dw), 1 / (pscale * dh));
				dw -= dws;
				dh -= dws;
				g2.scale(pscale * dw, pscale * dh);

				if (link != null && !dead) {
					shade(g2);
				}
				g2.translate(modfx, modfy);
				try {
					for (Sprite s : sky) {
						if (s.getImage() != null) {
							g2.drawImage(s.getImage().getImage(), s.getX(), s.getY(), null);
						} else {
							s = null;
						}
					}
				} catch (ConcurrentModificationException c) {
				}
				
				if(link.getOverrideImage() != null && link.getOverrideImage().getImage() != null){
					g2.drawImage(link.getOverrideImage().getImage().getImage(), link.getX()-link.getOverrideImage().getImage().getIconWidth()/2 + 9, link.getY() - link.getOverrideImage().getHeight()/2 - 9, null);
				}
				
				g2.translate(-modfx, -modfy);
				if (link != null && !dead) {
					drawUI(g2);
				}

				if (inventory && !dead) {
					drawInventory(g2);
				}

				if (controls && !dead) {
					displayControls(g2);
				}

				if (highscore && !dead) {
					displayHighscores(g2);
				}

				if (conv) {
					show.setVisible(true);
					drawConversation(g2);
				} else {
					show.setVisible(false);
				}

				if (link == null && !dead) {
					g2.setColor(new Color((int) (Math.random() * 255 + 1), (int) (Math.random() * 255 + 1),
							(int) (Math.random() * 255 + 1)));
					// WorldBuilder.ChangeFont("lttp", 14);
					g2.setFont(WorldBuilder.font);
					g2.drawString("FBLA Build", 2, 12);
					drawUnicode(g2, "272F", 40, 12);
					g2.setColor(Color.white);
					drawUnicode(g2, "2735", 1366/4, 13);
					g2.drawString("Hold Shift Anytime For Keymap", 1432/4, 12);
				}
			}
			if (dead) {
				drawDeathScreenUI(g2);
			}
			
			if(directOverlay){ // display an image in the center of the screen. (Reserved for debugging / easter eggs)
				g2.drawImage(doverlay.getImage() , 16 , 16, null);
			}

		} catch (NullPointerException e) {

		}

	}

	private boolean checkAnimated(int id) { // is an animated sprite
		
		for(int i : animated){
			if((i+1) == id){
				return true;
			}
		}
		
		return false;
	}

	private void drawLoadingScreen(Graphics2D g2) {
		if (!dead) {
			g2.setFont(WorldBuilder.font);
			g2.setColor(Color.darkGray);
			g2.setFont(WorldBuilder.getScaledFont(32));
			g2.drawString("LOADING", 1920 / 8 + 1 - 40, 64);
			g2.setColor(Color.white);
			g2.setFont(WorldBuilder.getScaledFont(32));
			g2.drawString("LOADING", 1920 / 8 - 40, 64);
		}

	}

	public void drawDeathScreenUI(Graphics2D g2) {
		g2.setFont(WorldBuilder.font);
		g2.setColor(Color.darkGray);

		g2.drawString("Continue", 1920 / 8 + 1 - 18, 97);
		g2.drawString("Save And Continue", 1920 / 8 + 1 - 36, 161);
		g2.drawString("Save And Quit", 1920 / 8 + 1 - 28, 225);
		g2.setColor(Color.white);
		if (deathSelect == 0) {
			g2.setColor(Color.green);
			g2.drawImage(new ImageIcon("Images\\ss2.png").getImage(), 1920 / 8 + 1 - 22 - 45, 97 - 14, null);
			g2.drawImage(new ImageIcon("Images\\ss1.png").getImage(), 1920 / 8 + 1 - 22 + 39, 97 - 14, null);
		}
		g2.drawString("Continue", 1920 / 8 - 18, 96);
		g2.setColor(Color.white);
		if (deathSelect == 1) {
			g2.setColor(Color.green);
			g2.drawImage(new ImageIcon("Images\\ss2.png").getImage(), 1920 / 8 + 1 - 22 - 63, 161 - 14, null);
			g2.drawImage(new ImageIcon("Images\\ss1.png").getImage(), 1920 / 8 + 1 - 22 + 59, 161 - 14, null);
		}
		g2.drawString("Save And Continue", 1920 / 8 - 36, 160);
		g2.setColor(Color.white);
		if (deathSelect == 2) {
			g2.setColor(Color.green);
			g2.drawImage(new ImageIcon("Images\\ss2.png").getImage(), 1920 / 8 + 1 - 22 - 55, 225 - 14, null);
			g2.drawImage(new ImageIcon("Images\\ss1.png").getImage(), 1920 / 8 + 1 - 22 + 50, 225 - 14, null);
		}
		g2.drawString("Save And Quit", 1920 / 8 - 28, 224);

		g2.setColor(Color.darkGray);
		g2.setFont(WorldBuilder.getScaledFont(32));
		g2.drawString("GAME OVER", 1920 / 8 + 1 - 54, 32);
		g2.setColor(Color.white);
		g2.setFont(WorldBuilder.getScaledFont(32));
		g2.drawString("GAME OVER", 1920 / 8 - 54, 32);

		g2.setColor(Color.darkGray);
		g2.drawRect(1920 / 8 - 56 - 63, 33, 242, 2);
		g2.drawRect(1920 / 8 - 56 - 63, 1, 242, 2);
		g2.drawRect(1920 / 8 - 58 - 63, 0, 2, 800);
		g2.drawRect(1920 / 8 - 58 + 179, 0, 2, 800);

		g2.setColor(Color.white);
		g2.drawRect(1920 / 8 - 56 - 64, 32, 242, 2);
		g2.drawRect(1920 / 8 - 56 - 64, 0, 242, 2);
		g2.drawRect(1920 / 8 - 58 - 64, 0, 2, 800);
		g2.drawRect(1920 / 8 - 58 + 180, 0, 2, 800);

	}

	public void setScores() {
		try {
			Scanner scan = new Scanner(new File("Saves\\speenrun.sav"));

			int lineCount = 0;
			do {
				if (scan.hasNextLine()) {
					scan.nextLine();
				}
				lineCount++;
			} while (scan.hasNextLine());

			scan = new Scanner(new File("Saves\\speenrun.sav"));

			Score[] scores = new Score[lineCount / 2];

			int c = 0;
			do {
				if (scan.hasNextLine()) {
					String name = scan.nextLine();
					try{
					scores[c] = new Score(Float.parseFloat(scan.nextLine()), name);
					} catch (NumberFormatException e){
						
					}
				}
				c++;
			} while (scan.hasNextLine());

			for (Score f : scores) {
				System.out.println(f.getName() + ": " + f.getScore());
			}

			scores = scores[0].sortScoreList(Arrays.asList(scores));
			Hscores = scores;
			scan.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
	}

	public void displayHighscores(Graphics2D g) {
		g.setColor(Color.BLACK);
		g.fillRect(0, 0, 1920, 1080);
		g.setColor(Color.WHITE);
		g.setFont(WorldBuilder.getScaledFont(28));
		g.drawString("TOP 5 HIGHSCORES", 1920 / 8 - 76, 32);
		g.setFont(WorldBuilder.getScaledFont(14));
		for (int i = 0; i < 5; i++) {
			if (i < Hscores.length)
				g.drawString(Hscores[i].getName() + ": " + truncateDecimal(Hscores[i].getScore(), 2), 1920 / 8 - 24,
						32 * i + 96);
		}
	}

	public void displayControls(Graphics2D g) { // draw control image
		g.setColor(Color.BLACK);
		if (link == null)
			g.fillRect(0, 0, 1920, 1080);
		g.setColor(Color.WHITE);
		g.drawImage(new ImageIcon("Images\\controls.png").getImage(), -5, -32, null);
	}

	public void displayItemGet(ItemList l) { // disable controls and make player
										     // hold aquired item for 1200ms
		cantWalk = true;
		link.stopWalking();
		link.setImage(new ImageIcon("Images\\people\\linkdgetItem.png"));
		Sprite display = new Sprite();
		display.setX(link.getX());
		display.setY(link.getY() - 4);
		display.setWidth(16);
		display.setHeight(16);
		display.setImage(new ImageIcon(l.getUrl()));

		this.getOverlays().add(display);
		Timer zooms = new Timer();
		zooms.schedule(new TimerTask() {
			@Override
			public void run() {
				zooms.cancel();
				zooms.purge();

				Timer look = new Timer();

				look.schedule(new TimerTask() {
					@Override
					public void run() {
						cantWalk = false;
						link.setImage(link.getIdleD());
						look.cancel();
						look.purge();
						display.setImage(null);
						if (l == ItemList.heart) {
							link.addHeart();
						}
					}
				}, 1200);
			}
		}, 30);
	}

	public void displayItemGet(EquipmentList l) { // disable controls and make
												 // player hold aquired item
												// for 1200ms
		cantWalk = true;
		link.setImage(new ImageIcon("Images\\people\\linkdgetItem.png"));
		Sprite display = new Sprite();
		display.setX(link.getX());
		display.setY(link.getY() - 4);
		display.setWidth(16);
		display.setHeight(16);
		display.setImage(new ImageIcon(l.getUrl()));

		this.getOverlays().add(display);
		Timer zooms = new Timer();
		zooms.schedule(new TimerTask() {
			@Override
			public void run() {
				zooms.cancel();
				zooms.purge();
				Timer look = new Timer();
				look.schedule(new TimerTask() {
					@Override
					public void run() {
						cantWalk = false;
						link.setImage(link.getIdleD());
						look.cancel();
						look.purge();
						display.setImage(null);
					}
				}, 1200);
			}
		}, 30);
	}

	public void drawUnicode(Graphics2D g, String id, int x, int y) { // draw
																		// unicode
																		// image
																		// at
																		// x,y
		Font old = g.getFont();
		g.setFont(new Font("TimesRoman", Font.PLAIN, 16));
		String myString = "\\u" + id;
		String str = myString.split(" ")[0];
		str = str.replace("\\", "");
		String[] arr = str.split("u");
		String text = "";
		for (int i = 1; i < arr.length; i++) {
			int hexVal = Integer.parseInt(arr[i], 16);
			text += (char) hexVal;
		}
		g.drawString(text, x, y);

		g.setFont(old);
	}

	public void startConversation(Conversation c) { // set conversation object and start
		conversation = c;
		conv = true;
		conversation.startScroll();
	}

	public void drawConversation(Graphics2D g2) { // draw conversation ui
		show.setForeground(Color.white);
		g2.setClip(170 / 4 - 22, 880 / 4 - 54, 1750 / 4 + 8, 400 / 4);
		g2.drawImage(new ImageIcon("Images\\cbox.png").getImage(), 170 / 4 - 22, 880 / 4 - 54, null);
		show.setFont(g2.getFont());
		show.setText(conversation.getCurrentLine());
	}

	public void shade(Graphics2D g2) { // Area lighting

		Color ol = g2.getColor();
		Area outside = new Area(new Rectangle(-64, -64, dungeonSize * 16 + 64, dungeonSize * 16 + 64));

		for (Light l : Lights) { // add lights by removing alpha from circles
									// around light origin (temporarily
									// disabled)
			Area na = new Area(new Ellipse2D.Double(l.getX() - l.getRadiusDifference(),
					l.getY() - l.getRadiusDifference(), l.getApparentRadius() * 2, l.getApparentRadius() * 2));
			g2.setClip(na);
			g2.setColor(new Color(l.getRed(), l.getGreen(), l.getBlue(), 0.02f));
			int rr = 0;
			for (int i = 0; i < l.getApparentRadius(); i++) {
				rr++;
				g2.drawOval((int) (l.getX() - l.getRadiusDifference()) + rr - 4,
						(int) (l.getY() - l.getRadiusDifference()) + rr - 4,
						(int) (l.getApparentRadius() * 2) - rr * 2 + 4, (int) (l.getApparentRadius() * 2) - rr * 2 + 4);
			}
			outside.subtract(na);
		}

		if (insideLight < 0)
			g2.setColor(new Color(0f, 0f, 0f, (float) brightness));
		else
			g2.setColor(new Color(0f, 0f, 0f, (float) insideLight));
		g2.setClip(outside);
		g2.fill(g2.getClip());
		g2.setColor(ol);

		g2.setClip(null);

	}

	public void splashText(String t, int x, int y, long timeAppend, long timeStay) { // Splash
																						// moving
																						// text
																						// on
																						// sceen
																						// (Entered
																						// level
																						// text)

		SplashString s = new SplashString("" + t, x, y, 32, timeAppend, timeStay, Color.black);
		getStrings().add(s);
		s.setPointerList(getStrings());

		SplashString s2 = new SplashString("" + t, x - 1, y - 1, 32, timeAppend, timeStay, Color.yellow);
		getStrings().add(s2);
		s2.setPointerList(getStrings());

	}

	public void drawInventory(Graphics2D g2) { // draw inventory ui

		g2.setColor(Color.black);
		invOne = true;
		invTwo = false;
		
		if (drawOne)
			g2.setColor(Color.red);
		if (invOne) {
			g2.setColor(Color.green);
		}
		g2.drawRoundRect(16, 16, 1920 / 4 - 32, 128, 8, 8);
		g2.setColor(new Color(0.15f, 0.15f, 0.15f, 0.75f));
		g2.fillRoundRect(16, 16, 1920 / 4 - 32, 128, 8, 8);
		
		g2.setColor(Color.darkGray);
		g2.drawString("ITEMS", 33, 128);
		g2.setColor(Color.white);
		g2.drawString("ITEMS", 32, 128);
		
		g2.setColor(Color.black);
		if (!drawOne)
			g2.setColor(Color.red);
		if (invTwo) {
			g2.setColor(Color.green);
		}
		g2.drawRoundRect(16, 146, (int) WorldBuilder.ScreenSize.getWidth() / 4 - 32, 96, 8, 8);
		g2.setColor(new Color(0.15f, 0.15f, 0.15f, 0.75f));
		g2.fillRoundRect(16, 146, (int) WorldBuilder.ScreenSize.getWidth() / 4 - 32, 96, 8, 8);

		g2.setColor(Color.darkGray);
		g2.drawString("EQUIPMENT", 33, 224);
		g2.setColor(Color.white);
		g2.drawString("EQUIPMENT", 32, 224);
		
		int h = -1;
		if (link != null) {
			for (int i = 0; i < link.getItemArray().size(); i++) {
				g2.setColor(Color.black);
				g2.drawRect(18 * i + 32, 31, 16, 18);
				g2.drawImage(new ImageIcon(link.getItemArray().get(i).getUrl()).getImage(), 18 * i + 32, 32, null);
				if (link.getItemArray().get(i) == link.getEquippedItem()) {
					h = i;
				}
			}
			
			for (int i = 0; i < link.getEquipArray().size(); i++) {
				g2.setColor(Color.black);
				g2.drawRect(18 * i + 32, 160, 16, 18);
				g2.drawImage(new ImageIcon(link.getEquipArray().get(i).getUrl()).getImage(), 18 * i + 32, 161, null);
			}
			
		}
		g2.setColor(Color.yellow);
		if (h != -1) {
			g2.drawRoundRect(18 * h + 31, 30, 18, 19, 8, 8);
		}
		String d = link.getEquippedItem().getName();
		g2.drawString(d, 32, 96);
		g2.setColor(Color.BLACK);
		g2.fillRect(32, 98, d.length() * g2.getFont().getSize() / 3 + 1, 1);

	}

	public void drawUI(Graphics2D g2) { // draw graphical ui
		if (gameUI) {
			g2.drawImage(new ImageIcon("Images\\life.png").getImage(), 376, 16, null);
			int full = WorldBuilder.link.getHealth() / 2;
			boolean half = (WorldBuilder.link.getHealth() % 2 == 1);
			int empty = WorldBuilder.link.getMaxhealth() - full;

			int j = -1;

			for (int i = 0; i < WorldBuilder.link.getMaxhealth() / 2; i++) {
				if (i % 10 == 0) {
					j++;
				}
				int mod2 = 8 * j;
				if (i < full) {
					g2.drawImage(new ImageIcon("Images\\heartfull.png").getImage(),
							(int) (358 + (i * 8) - ((mod2) * 10)), 26 + (mod2), null);
				} else if (half && i == full) {
					g2.drawImage(new ImageIcon("Images\\hearthalf.png").getImage(),
							(int) (358 + (i * 8) - ((mod2) * 10)), 26 + (mod2), null);
				} else if (i >= full && empty > 0) {
					g2.drawImage(new ImageIcon("Images\\heartempty.png").getImage(),
							(int) (358 + (i * 8) - ((mod2) * 10)), 26 + (mod2), null);
				}
			}

			g2.setFont(WorldBuilder.font);

			if (link.getRupees() < 101) {
				g2.drawImage(new ImageIcon("Images\\rupd.png").getImage(), 84, 12, null);
			} else if (link.getRupees() < 501) {
				g2.drawImage(new ImageIcon("Images\\rupd2.png").getImage(), 84, 12, null);
			} else if (link.getRupees() < 1001) {
				g2.drawImage(new ImageIcon("Images\\rupd2.png").getImage(), 84, 12, null);
			} else if (link.getRupees() < 2001) {
				g2.drawImage(new ImageIcon("Images\\rupd3.png").getImage(), 84, 12, null);
			} else if (link.getRupees() < 5001) {
				g2.drawImage(new ImageIcon("Images\\rupd4.png").getImage(), 84, 12, null);
			} else if (link.getRupees() < 9000) {
				g2.drawImage(new ImageIcon("Images\\rupd5.png").getImage(), 84, 12, null);
			} else if (link.getRupees() > 9000) {
				g2.drawImage(new ImageIcon("Images\\rupd6.png").getImage(), 84, 12, null);
			}
			
			drawNumber(g2, WorldBuilder.link.getRupees(), 84, 36);

			g2.drawImage(new ImageIcon("Images\\bomd.png").getImage(), 119, 12, null);

			if (WorldBuilder.link.getBombs() < 100) { // greater than 99 is way
														// beyond max value.
														// Will use for infinite
														// amount.
				drawNumber(g2, WorldBuilder.link.getBombs(), 119, 28, false);
			} else {
				g2.setColor(Color.black);
				drawUnicode(g2, "221e", 117, 37);
				g2.setColor(Color.white);
				drawUnicode(g2, "221e", 116, 36);
			}

			g2.drawImage(new ImageIcon("Images\\arr.png").getImage(), 148, 12, null);
			if (WorldBuilder.link.getArrows() < 100) { // greater than 99 is way
														// beyond max value.
														// Will use for infinite
														// amount.
				drawNumber(g2, WorldBuilder.link.getArrows(), 148, 32, false);
			} else {
				g2.setColor(Color.black);
				drawUnicode(g2, "221e", 147, 37);
				g2.setColor(Color.white);
				drawUnicode(g2, "221e", 146, 36);
			}
			
			if(soulHardened && uiSprites.get(0)!=null){
				g2.drawImage(uiSprites.get(0).getImage().getImage(), 173, 8, null);
				drawNumber(g2, WorldBuilder.link.getSouls(), 177, 36, false);
			}

			g2.drawImage(new ImageIcon("Images\\itemd.png").getImage(), 50, 22, null);

			if (link != null && link.getEquippedItem() != null) {
				g2.drawImage(new ImageIcon(link.getEquippedItem().getUrl()).getImage(), 52, 24, null);
			}

			g2.drawImage(new ImageIcon("Images\\mm" + WorldBuilder.link.modmagic(0) + ".png").getImage(), 32, 22, null);

			AffineTransform identity = new AffineTransform();
			AffineTransform trans = new AffineTransform();

			trans.setTransform(identity);
			trans.translate(240 - 8, 270 - 16);
			trans.scale(0.5, 0.5);
			trans.rotate(Math.toRadians((float) WorldBuilder.percentTime / 5.59f), 16, 16);
			g2.drawImage(new ImageIcon("Images\\clock.png").getImage(), trans, this);

			if (speedRunTime != -1) {
				g2.drawString(truncateDecimal(speedRunTime, 2) + " Seconds", 1920 / 8 - 28, 16);
			}
			
			if(edisplay){ // display the elemental pickup graphics
				
				g2.drawRect(128, 64, 216, 128);
				g2.drawLine(129, 90, 343, 90);
				g2.setColor(new Color(1f,1f,1f,0.5f));
				g2.fillRect(128, 64, 216, 128);
				
				String[] d = new String[10];
				String[] d2 = new String[10];
				try {
				Scanner f = new Scanner(new File("Elements\\list.txt"));
					for( int i = 0; i<=element; i++){
						d[i] = f.nextLine();
						d2[i] = f.nextLine();
					}
					f.close();
				} catch (FileNotFoundException e) {
					e.printStackTrace();
				}
				g2.drawString(d[element], 139, 84);
				g2.setColor(Color.black);
				g2.drawString(d[element], 140, 84);
				
				g2.drawImage(new ImageIcon("Images\\balls"+element+".png").getImage(), 140, 102, null);
				
				g2.setColor(Color.white);
				g2.drawString(d2[element], 139, 164);
				g2.setColor(Color.black);
				g2.drawString(d2[element], 140, 164);
				
			}
			
		}
	}

	public BigDecimal truncateDecimal(double x, int numberofDecimals) {
		if (x > 0) {
			return new BigDecimal(String.valueOf(x)).setScale(numberofDecimals, BigDecimal.ROUND_FLOOR);
		} else {
			return new BigDecimal(String.valueOf(x)).setScale(numberofDecimals, BigDecimal.ROUND_CEILING);
		}
	}

	public void drawString(Graphics2D g2, String s, int x, int y) {
		g2.setColor(Color.BLACK);
		g2.drawString(s, x + 1, y + 1);
		g2.setColor(Color.white);
		g2.drawString(s, x, y);
	}

	public void drawNumber(Graphics2D g2, int s, int x, int y) {
		drawNumber(g2, s, x, y, true);
	}

	public void drawNumber(Graphics2D g2, int s, int x, int y, boolean hundreds) { // draw
																					// number
																					// on
																					// screen
																					// but
																					// only
																					// allow
																					// hundreds
																					// &
																					// thousands
																					// places
																					// (money)
		g2.setColor(Color.BLACK);
		String bo = "";

		if (hundreds && s < 10) {
			bo = "00";
		} else if (hundreds && s < 100) {
			bo = "0";
		} else if (hundreds && s > 99 && s < 1000) {
			bo = "0";
		}

		if (!hundreds && s < 10) {
			bo = "0";
		}

		g2.drawString(bo + s, x + 1, y + 1);
		g2.setColor(Color.white);
		g2.drawString(bo + s, x, y);
	}

	@Override
	public void mouseWheelMoved(MouseWheelEvent e) {
		// TODO Auto-generated method stub
		displayElement((int) (Math.random()*9+1));
	}

	@Override
	public void mouseDragged(MouseEvent e) {
		// TODO Auto-generated method stub

	}

	@Override
	public void mouseMoved(MouseEvent e) {
		// TODO Auto-generated method stub
	}

	@Override
	public void mouseClicked(MouseEvent e) {
		// TODO Auto-generated method stub

	}

	@Override
	public void mouseEntered(MouseEvent e) {
		// TODO Auto-generated method stub

	}

	@Override
	public void mouseExited(MouseEvent e) {
		// TODO Auto-generated method stub

	}

	@Override
	public void mousePressed(MouseEvent e) {
	}

	@Override
	public void mouseReleased(MouseEvent e) {
		// TODO Auto-generated method stub

	}

	@Override
	public void keyPressed(KeyEvent e) { // Key bindings
		//edisplay = false;
		if(WorldBuilder.scanner){
			WorldBuilder.pressMemory.add(""+e.getKeyChar());
			WorldBuilder.checkCheat();
		}
		
		if (e.getKeyCode() == KeyEvent.VK_ESCAPE)
			System.exit(0);

		if (!dead) {
			if (e.getKeyCode() == KeyEvent.VK_SHIFT) {
				controls = true;
			}
			if (link != null)
				if (e.getKeyCode() == KeyEvent.VK_SPACE) {
					if (getConversation() != null) {
						getConversation().nextLine();
					}
				}
			if (link == null)
				if (e.getKeyCode() == KeyEvent.VK_SPACE) {
					if (mainMenu) {
						WorldBuilder.testGame("House");
					}
				}

			if (!cantWalk) {
				if (cheats && e.getKeyCode() == KeyEvent.VK_EQUALS)
					WorldBuilder.link.modhp(+1);

				if (cheats && e.getKeyCode() == KeyEvent.VK_MINUS && !WorldBuilder.scanner)
					WorldBuilder.link.modhp(-1);
				if (cheats && e.getKeyCode() == KeyEvent.VK_1 && !WorldBuilder.scanner) {
					WorldBuilder.link.setShieldlevel(1);
				}
				if (cheats && e.getKeyCode() == KeyEvent.VK_N && !WorldBuilder.scanner)
					Music.activeMusic.getClip().setFramePosition(0);
				if (cheats && e.getKeyCode() == KeyEvent.VK_2 && !WorldBuilder.scanner)
					link.setShieldlevel(2);
				if (cheats && e.getKeyCode() == KeyEvent.VK_3 && !WorldBuilder.scanner)
					link.setShieldlevel(3);
				if (cheats && e.getKeyCode() == KeyEvent.VK_4 && !WorldBuilder.scanner){
					Projectile l = new Projectile(new ImageIcon(""),new ImageIcon(""));
					l.setName("soul");
					link.cast(l, "gball2", 1250);
				}
				if (cheats && e.getKeyCode() == KeyEvent.VK_6 && !WorldBuilder.scanner) { // test animated
														// particles
					Sprite make = new Sprite();
					make.setX(link.getX());
					make.setY(link.getY());
					make.setWidth(32);
					make.setHeight(32);
					make.playAnimation(JOptionPane.showInputDialog("Animation Name", JOptionPane.WANTS_INPUT_PROPERTY),
							Integer.parseInt(JOptionPane.showInputDialog("Speed", JOptionPane.WANTS_INPUT_PROPERTY)),
							WorldBuilder.numToBool(JOptionPane.showOptionDialog(null, "Oneshot?", eastMap,
									JOptionPane.YES_NO_OPTION, convLine, null, null, null)));
					WorldBuilder.screen.getOverlays().add(make);
				}
				if (cheats && e.getKeyCode() == KeyEvent.VK_7 && !WorldBuilder.scanner) {
					WorldBuilder.forwardTime(100);
				}
				if (cheats && e.getKeyCode() == KeyEvent.VK_B && !WorldBuilder.scanner) {
					link.setMaxhealth(link.getMaxhealth() + 2);
					link.addSouls(10);
				}

				if (e.getKeyCode() == KeyEvent.VK_R) { // bring up inventory
														// menu

					if (!invTwo && !invOne && link != null) {

						inventory = !inventory;

						link.setWalkDown(false);
						link.setWalkUp(false);
						link.setWalkLeft(false);
						link.setWalkRight(false);

						if (link.getImage() == link.getAnimL()) {
							link.setImage(link.getIdleL());
						}
						if (link.getImage() == link.getAnimR()) {
							link.setImage(link.getIdleR());
						}
						if (link.getImage() == link.getAnimD()) {
							link.setImage(link.getIdleD());
						}
						if (link.getImage() == link.getAnimU()) {
							link.setImage(link.getIdleU());
						}
					} else if (invTwo || invOne) { // return one screen
						inventory = !inventory;
						invTwo = false;
						invOne = false;
						SoundEffect.rupee.play(false);
					}

				}

				if (cheats && e.getKeyCode() == KeyEvent.VK_0 && !WorldBuilder.scanner)
					WorldBuilder.link.modmagic(+5);

				if (cheats && e.getKeyCode() == KeyEvent.VK_9 && !WorldBuilder.scanner)
					WorldBuilder.link.modmagic(-1);

				if (e.getKeyCode() == KeyEvent.VK_Q) {
					if (!inventory && releasedUse){
						link.useWeapon();
						releasedUse = false;
					}
					if (!invTwo && !invOne && inventory) {
						inventory = false;
					}
					if (invTwo || invOne) {
						invTwo = false;
						invOne = false;
						SoundEffect.rupeeRegister.play(false);
					}
				}

				if (cheats && e.getKeyCode() == KeyEvent.VK_P){
					WorldBuilder.link.heal(6);
					WorldBuilder.splashOverlay(new ImageIcon("Images\\Special\\WhyLookAtTheseFiles.png"), 100, 5000);
				}

				if (cheats && e.getKeyCode() == KeyEvent.VK_L) {
					WorldBuilder.link.addRupees(500);
					Sprite invf = new Sprite();
					invf.setOrigin(new Point(link.getX(), link.getY()));
					invf.setX((int) link.getX() - 22);
					invf.setY((int) link.getY() - 16);
					invf.startEmitterTexturedRandomOneShot(500, 64, 150, 0, 10.00f, false,
							new ImageIcon[] { new ImageIcon("Images\\particles\\starlight.png"),
									new ImageIcon("Images\\particles\\blackGradient.png"),
									new ImageIcon("Images\\particles\\redGradient.png"),
									new ImageIcon("Images\\particles\\electric.png") });
				}

				if (e.getKeyCode() == KeyEvent.VK_SPACE) {
					useLogic();
				}

				if (cheats && e.getKeyCode() == KeyEvent.VK_SLASH)
					loadDungeon();

				if (cheats && e.getKeyCode() == KeyEvent.VK_O) {
					WorldBuilder.link.addBombs(1);
					WorldBuilder.link.modarrows(1);
				}

				if (cheats && e.getKeyCode() == KeyEvent.VK_M)
					gameUI = !gameUI;

				if (cheats && e.getKeyCode() == KeyEvent.VK_U) {
					setDws((float) (getDws() - 0.25));
				}
				if (cheats && e.getKeyCode() == KeyEvent.VK_I) {
					setDws((float) (getDws() + 0.25));
				}
				if (link != null && !link.isSwinging()) { // player movement +
															// attack
					if (e.getKeyCode() == KeyEvent.VK_E) {
						if (!inventory) {
							link.swing();
							link.setWalkUp(false);
							link.setWalkDown(false);
							link.setWalkLeft(false);
							link.setWalkRight(false);
						} else if (!invTwo && !invOne) {
							SoundEffect.cane.play(false);
							if (drawOne) {
								invOne = true;
								invTwo = false;
							} else {
								invOne = false;
								invTwo = true;
							}
						}
					}
					if (cheats && e.getKeyCode() == KeyEvent.VK_J)
						link.hurtme(-2);

					if (e.getKeyCode() == KeyEvent.VK_W) {
						if (!inventory)
							WorldBuilder.link.setWalkUp(true);
						else if (!invTwo && !invOne) {
							drawOne = true;
							SoundEffect.select.play(false);
						}
					}
					if (e.getKeyCode() == KeyEvent.VK_S) {
						if (!inventory)
							WorldBuilder.link.setWalkDown(true);
						else if (!invTwo && !invOne) {
							drawOne = false;
							SoundEffect.select.play(false);
						}
					}
					if (e.getKeyCode() == KeyEvent.VK_A) {
						if (!inventory)
							WorldBuilder.link.setWalkLeft(true);
						else if (invOne) {
							SoundEffect.select.play(false);
							if (link.getEquippedInt() > 0)
								link.setEquippedItem(link.getItemArray().get(link.getEquippedInt() - 1));
							else {
								try {
									link.setEquippedItem(link.getItemArray().get(link.getItemArray().size() - 1));
								} catch (ArrayIndexOutOfBoundsException aioob) {
									link.setEquippedItem(Player.ItemList.Nothing);
								}
							}
						}
					}
					if (e.getKeyCode() == KeyEvent.VK_D) {
						if (!inventory)
							WorldBuilder.link.setWalkRight(true);
						else if (invOne) {
							SoundEffect.select.play(false);
							if (link.getEquippedInt() + 1 < link.getItemArray().size())
								link.setEquippedItem(link.getItemArray().get(link.getEquippedInt() + 1));
							else {
								try {
									link.setEquippedItem(link.getItemArray().get(0));
								} catch (IndexOutOfBoundsException a) {
									link.setEquippedItem(Player.ItemList.Nothing);
								}
							}
						}
					}

				}
			} else {

			}

			repaint();
		} else {

			// Dead menu

			if (e.getKeyCode() == KeyEvent.VK_W) {
				deathSelect--;
				if (deathSelect < 0) {
					deathSelect = 2;
				}
				SoundEffect.select.play(false);
			}
			if (e.getKeyCode() == KeyEvent.VK_S) {
				deathSelect++;
				if (deathSelect > 2) {
					deathSelect = 0;
				}
				SoundEffect.select.play(false);
			}

			if (e.getKeyCode() == KeyEvent.VK_SPACE || e.getKeyCode() == KeyEvent.VK_E) {
				if (deathSelect == 0) {
					SoundEffect.getHeart.play(false, 6);
					loadDungeon("House");
					particler.cancel();
					particler.purge();
					particler = new Timer();
				}
				if (deathSelect == 1) {
					SoundEffect.getHeart.play(false, 6);
					loadDungeon("House");
					particler.cancel();
					particler.purge();
					particler = new Timer();
				}
				if (deathSelect == 2) {
					Music.activeMusic.stop();
					Music.activeMusic.getClip().close();
					SoundEffect.wizShot3.play(false, 6);
					Timer gtfo = new Timer();
					gtfo.schedule(new TimerTask(){
						@Override
						public void run() {
							System.exit(0);
						}
					}, 150);
					
				}
				dead = false;
				cantWalk = false;
			}
		}

		if (cantWalk) {
			if (e.getKeyCode() == KeyEvent.VK_A && link.getImage().toString().equals("Images\\people\\linkrgrab.png")) {
				link.setImage(new ImageIcon("Images\\people\\linkrgrabbing.gif"));
			}
			if (e.getKeyCode() == KeyEvent.VK_D && link.getImage().toString().equals("Images\\people\\linklgrab.png")) {
				link.setImage(new ImageIcon("Images\\people\\linklgrabbing.gif"));
			}
			if (e.getKeyCode() == KeyEvent.VK_S && link.getImage().toString().equals("Images\\people\\linkugrab.png")) {
				link.setImage(new ImageIcon("Images\\people\\linkugrabbing.gif"));
			}
			if (e.getKeyCode() == KeyEvent.VK_W && link.getImage().toString().equals("Images\\people\\linkdgrab.png")) {
				link.setImage(new ImageIcon("Images\\people\\linkdgrabbing.gif"));
			}
		}

	}

	@Override
	public void keyReleased(KeyEvent e) {

		if (e.getKeyCode() == KeyEvent.VK_SHIFT) {
			controls = false;
		}
		if (e.getKeyCode() == KeyEvent.VK_Q) {
			releasedUse = true;
		}
		if (cantWalk) {
			if (e.getKeyCode() == KeyEvent.VK_SPACE && ((link.getImage().toString()
					.equals("Images\\people\\" + link.getImage().toString().substring(14, 19) + "grab.png"))
					|| link.getImage().toString().equals(
							"Images\\people\\" + link.getImage().toString().substring(14, 19) + "grabbing.gif"))) {
				cantWalk = false;
				link.returnToIdle();
			}
			if (e.getKeyCode() == KeyEvent.VK_A
					&& link.getImage().toString().equals("Images\\people\\linkrgrabbing.gif")) {
				link.setImage(new ImageIcon("Images\\people\\linkrgrab.png"));
			}
			if (e.getKeyCode() == KeyEvent.VK_D
					&& link.getImage().toString().equals("Images\\people\\linklgrabbing.gif")) {
				link.setImage(new ImageIcon("Images\\people\\linklgrab.png"));
			}
			if (e.getKeyCode() == KeyEvent.VK_S
					&& link.getImage().toString().equals("Images\\people\\linkugrabbing.gif")) {
				link.setImage(new ImageIcon("Images\\people\\linkugrab.png"));
			}
			if (e.getKeyCode() == KeyEvent.VK_W
					&& link.getImage().toString().equals("Images\\people\\linkdgrabbing.gif")) {
				link.setImage(new ImageIcon("Images\\people\\linkdgrab.png"));
			}
		}
		if (!cantWalk) {
			if (link != null && !link.isSwinging() && !inventory) {
				if (e.getKeyCode() == KeyEvent.VK_W) {
					WorldBuilder.link.setWalkUp(false);
					link.setLastUp(true);
					link.setLastDown(false);
					link.setLastLeft(false);
					link.setLastRight(false);
					if (!link.isMovingExclude(1)) {
						link.setImage(link.getIdleU());
					}
				}
				if (e.getKeyCode() == KeyEvent.VK_S) {
					WorldBuilder.link.setWalkDown(false);
					link.setLastUp(false);
					link.setLastDown(true);
					link.setLastLeft(false);
					link.setLastRight(false);
					if (!link.isMovingExclude(2)) {
						link.setImage(link.getIdleD());
					}
				}
				if (e.getKeyCode() == KeyEvent.VK_A) {
					WorldBuilder.link.setWalkLeft(false);
					link.setLastUp(false);
					link.setLastDown(false);
					link.setLastLeft(true);
					link.setLastRight(false);
					if (!link.isMovingExclude(3)) {
						link.setImage(link.getIdleL());
					}
				}
				if (e.getKeyCode() == KeyEvent.VK_D) {
					WorldBuilder.link.setWalkRight(false);
					link.setLastUp(false);
					link.setLastDown(false);
					link.setLastLeft(false);
					link.setLastRight(true);
					if (!link.isMovingExclude(4)) {
						link.setImage(link.getIdleR());
					}
				}
			}
		}
	}

	@Override
	public void keyTyped(KeyEvent e) {
		// TODO Auto-generated method stub

	}

	public boolean isGameUI() {
		return gameUI;
	}

	public void setGameUI(boolean gameUI) {
		this.gameUI = gameUI;
	}

	public int getyShift() {
		return yShift;
	}

	public void setyShift(int yShift) {
		this.yShift = yShift;
	}

	public int getxShift() {
		return xShift;
	}

	public void setxShift(int xShift) {
		this.xShift = xShift;
	}

	public ArrayList<Sprite> getSprites() {
		return sprites;
	}

	public void setSprites(ArrayList<Sprite> sprites) {
		this.sprites = sprites;
	}

	public ArrayList<Sprite> getCollisions() {
		return collisions;
	}

	public void setCollisions(ArrayList<Sprite> collisions) {
		this.collisions = collisions;
	}

	public Image getBg() {
		return bg;
	}

	public void setBg(Image bg) {
		this.bg = bg;
	}

	public Player getLink() {
		return link;
	}

	public void setLink(Player link) {
		this.link = link;
	}

	public ArrayList<WorldItem> getObjects() {
		return objects;
	}

	public void setObjects(ArrayList<WorldItem> objects) {
		this.objects = objects;
	}

	public ArrayList<Integer> getBreakList() {
		return breakList;
	}

	public void setBreakList(ArrayList<Integer> breakList) {
		this.breakList = breakList;
	}

	public ArrayList<Integer> getAnimated() {
		return animated;
	}

	public void setAnimated(ArrayList<Integer> animated) {
		this.animated = animated;
	}

	public ArrayList<SplashString> getStrings() {
		return strings;
	}

	public void setStrings(ArrayList<SplashString> strings) {
		this.strings = strings;
	}

	public ArrayList<WorldItem> getDrops() {
		return drops;
	}

	public void setDrops(ArrayList<WorldItem> drops) {
		this.drops = drops;
	}

	public ArrayList<Enemy> getEnemies() {
		return enemies;
	}

	public void setEnemies(ArrayList<Enemy> enemies) {
		this.enemies = enemies;
	}

	public Sprite getWatchLink() {
		return watchLink;
	}

	public void setWatchLink(Sprite watchLink) {
		this.watchLink = watchLink;
	}

	public ArrayList<Particle> getParticleSystem() {
		return particleSystem;
	}

	public void setParticleSystem(ArrayList<Particle> particleSystem) {
		this.particleSystem = particleSystem;
	}

	public Color getBack() {
		return back;
	}

	public void setBack(Color back) {
		this.back = back;
	}

	public boolean isMainMenu() {
		return mainMenu;
	}

	public void setMainMenu(boolean mainMenu) {
		this.mainMenu = mainMenu;
	}

	public float getPscale() {
		return pscale;
	}

	public void setPscale(float pscale) {
		this.pscale = pscale;
	}

	public Timer getParticler() {
		return particler;
	}

	public void setParticler(Timer particler) {
		this.particler = particler;
	}

	public int getDungeonSize() {
		return dungeonSize;
	}

	public void setDungeonSize(int dungeonSize) {
		this.dungeonSize = dungeonSize;
	}

	public boolean isInventory() {
		return inventory;
	}

	public void setInventory(boolean inventory) {
		this.inventory = inventory;
	}

	public String getNorthMap() {
		return northMap;
	}

	public void setNorthMap(String northMap) {
		this.northMap = northMap;
	}

	public String getSouthMap() {
		return southMap;
	}

	public void setSouthMap(String southMap) {
		this.southMap = southMap;
	}

	public String getWestMap() {
		return westMap;
	}

	public void setWestMap(String westMap) {
		this.westMap = westMap;
	}

	public String getEastMap() {
		return eastMap;
	}

	public void setEastMap(String eastMap) {
		this.eastMap = eastMap;
	}

	public boolean isDrawOne() {
		return drawOne;
	}

	public void setDrawOne(boolean drawOne) {
		this.drawOne = drawOne;
	}

	public float getDws() {
		return dws;
	}

	public void setDws(float dws) {
		this.dws = dws;
	}

	public float getBrightness() {
		return brightness;
	}

	public void setBrightness(float brightness) {
		this.brightness = brightness;
	}

	public boolean isInvOne() {
		return invOne;
	}

	public void setInvOne(boolean invOne) {
		this.invOne = invOne;
	}

	public boolean isInvTwo() {
		return invTwo;
	}

	public void setInvTwo(boolean invTwo) {
		this.invTwo = invTwo;
	}

	public ArrayList<Light> getLights() {
		return Lights;
	}

	public void setLights(ArrayList<Light> lights) {
		Lights = lights;
	}

	public float getInsideLight() {
		return insideLight;
	}

	public void setInsideLight(float insideLight) {
		this.insideLight = insideLight;
	}

	public boolean isConv() {
		return conv;
	}

	public void setConv(boolean conv) {
		this.conv = conv;
	}

	public Conversation getConversation() {
		return conversation;
	}

	public void setConversation(Conversation conversation) {
		this.conversation = conversation;
	}

	public int getConvLine() {
		return convLine;
	}

	public void setConvLine(int convLine) {
		this.convLine = convLine;
	}

	public void removeByName(String string) {
		for (int i = 0; i < this.getOverlays().size(); i++) {
			if (this.getOverlays().get(i).equals(string)) {
				this.getOverlays().remove(i);
			}
		}
	}

	public void removeByNameSprites(String string) {
		for (int i = 0; i < this.getSprites().size(); i++) {
			if (this.getSprites().get(i).equals(string)) {
				this.getSprites().remove(i);
			}
		}
	}

	public ArrayList<Sprite> getOverlays() {
		return overlays;
	}

	public void setOverlays(ArrayList<Sprite> overlays) {
		this.overlays = overlays;
	}

	public boolean isCantWalk() {
		return cantWalk;
	}

	public void setCantWalk(boolean cantWalk) {
		this.cantWalk = cantWalk;
	}

	public Timer getRunTime() {
		return runTime;
	}

	public void setRunTime(Timer runTime) {
		this.runTime = runTime;
	}

	public float getSpeedRunTime() {
		return speedRunTime;
	}

	public void setSpeedRunTime(float speedRunTime) {
		this.speedRunTime = speedRunTime;
	}

	public Score[] getHScores() {
		return Hscores;
	}

	public void setHScores(Score[] scores) {
		this.Hscores = scores;
	}

	public ArrayList<Integer> getUsedChests() {
		return usedChests;
	}

	public void setUsedChests(ArrayList<Integer> usedChests) {
		this.usedChests = usedChests;
	}

	public int getDeathSelect() {
		return deathSelect;
	}

	public void setDeathSelect(int deathSelect) {
		this.deathSelect = deathSelect;
	}

	public boolean isDead() {
		return dead;
	}

	public void setDead(boolean dead) {
		this.dead = dead;
	}

	public void loadDungeonDoor(String refName, int id) {
		
		unloadRoom(null);
		
		Timer delayedStart = new Timer();
		
		delayedStart.schedule(new TimerTask(){

			@Override
			public void run() {
				
				if (!dead) {

					Scanner scan;
					Scanner scan2;
					Scanner scan3;
					Scanner scan4;

					// OBJECTS
					try {
						scan3 = new Scanner(new File("animated.txt"));
						do { // load animated sprite data
							animated.add((Integer.parseInt(scan3.nextLine()) - 1));
						} while (scan3.hasNextLine());

						scan3.close();

					} catch (FileNotFoundException e) {
						e.printStackTrace();
					}
					try {
						scan = new Scanner(new File("World\\" + refName + "COLLISIONMAP.dng")); // load
																							// collisions
						do {
							String tid = scan.nextLine();
							WorldItem s = new WorldItem();
							if (!tid.equals("null")) {
								s.setCollision(s.setCollisionData(tid));
								// System.out.println("");
								s.setCollisionData(s.getTileid() - 1);
								s.setId(s.getTileid() - 1);
								s.setWidth(16);
								s.setHeight(16);
								s.setY(Integer.parseInt(scan.nextLine()) * 16);
								s.setX(Integer.parseInt(scan.nextLine()) * 16);

								try {
									s.setImage(s.collisionImage().getImage());
								} catch (NullPointerException n) {

								}
							} else {
								scan.nextLine();
								scan.nextLine();
							}

							if (s.getCollision() != null) {
								collisions.add(s);
							}
							s = null;
						} while (scan.hasNextLine());

						scan.close();
					} catch (FileNotFoundException e) {
						// e.printStackTrace();
					}

					// OBJECTS
					int c = -1;
					try {
						scan = new Scanner(new File("World\\" + refName + "SPAWNS.dng")); // load
																						// spawns
						do {

							c++;
							scan2 = new Scanner(new File("break.txt"));
							String tid = scan.nextLine();
							if (c == 0) {
								breakList.add(Integer.parseInt(tid));
							}

							WorldItem s = new WorldItem();
							s.setTileid(Integer.parseInt(tid) - 1);
							s.setCollision(s.setCollisionData(tid));
							// s.setCollisionData(s.getTileid()-1);
							s.setId(s.getTileid() - 1);
							s.setWidth(16);
							s.setHeight(16);
							s.setY(Integer.parseInt(scan.nextLine()) * 16);
							s.setX(Integer.parseInt(scan.nextLine()) * 16);
							s.setImage(WorldBuilder.spritelist.get(s.getTileid()).getImage());

							if (!matchOverlay(s.getTileid()) && !matchTeleport(s.getTileid()) && !matchAI(s.getTileid())) {

								try {
									do {
										int g = (Integer.parseInt(scan2.nextLine()) - 1);
										if (!checkList(s.getTileid()) && g == s.getTileid()) {
											s.setScript(WorldItem.Script.BREAK);
											s.generateScript();
										}
									} while (scan2.hasNextLine());
								} catch (NoSuchElementException e) {

								}

								if (s.checkChest()) { // find chest, set id
									s.setUsedID(Integer.parseInt(scan.nextLine()));
									if (!checkUsedChest(s.getUsedID())) {
										s.setScript(Script.chest);
										s.generateScript();
									} else {
										s.usedImageGenerate();
									}
								}

								objects.add(s);
								s = null;
							} else if (!matchTeleport(s.getTileid()) && !matchAI(s.getTileid())) {
								overlays.add(s);
								s = null;
							} else if (matchAI(s.getTileid())) { // is an ai
								Enemy test = new Enemy(s.getX(), s.getY());
								test.buildAI(Integer.parseInt(scan.nextLine()));
								test.setWanderDistance(128);
								test.spawn();
								test = null;
								s = null;
							} else { // is a door
								// Dont add, set collide script
								s.setId(-1);
								s.setScript(WorldItem.Script.USE_DOOR);
								s.generateScript();
								s.startCollisionWatchDelay(350);
								s.setId(Integer.parseInt(scan.nextLine()));

								s.setRefName(scan.nextLine());
								s.setFacing(scan.nextLine());
								objects.add(s);
							}

							if (s != null && s.getCollision() != null)
								collisions.add(s);
							s = null;

						} while (scan.hasNextLine());

						scan.close();
						scan2.close();

					} catch (FileNotFoundException e) {
						// e.printStackTrace(); Wrong input name.
					} catch (NoSuchElementException noSpawns) {
						// Empty file will load no spawns
					} catch (NumberFormatException noSpawns) {
						// abrupt end. Stop loading, but run level anyway
					}

					try {
						scan4 = new Scanner(new File("World\\" + refName + "INFO.dng"));

						do {

							setDungeonSize(Integer.parseInt(scan4.nextLine()));
							northMap = scan4.nextLine();
							southMap = scan4.nextLine();
							eastMap = scan4.nextLine();
							westMap = scan4.nextLine();

							try {
								insideLight = Float.parseFloat(scan4.nextLine());
							} catch (NoSuchElementException nl) {
								insideLight = -1;
							} catch (NumberFormatException s) {
							}

							int songID = 0;
							try {
								songID = Integer.parseInt(scan4.nextLine());
							} catch (NoSuchElementException nl) {
								songID = -1;
							}

							if (songID != -1
									&& !Music.getMusicURL()[songID].equals("Music\\" + Music.activeMusic.getFile().getName())) {
								try {
									WorldBuilder.task.cancel();
									WorldBuilder.task = new Timer();
									Music.activeMusic.stop();
									Music.activeMusic.reinitialize(Music.getMusicURL()[songID]);
									WorldBuilder.playMusic(songID);
									WorldBuilder.daySong = Music.getMusicURL()[songID];
								} catch (IllegalArgumentException ag) {

								}
							}

						} while (scan4.hasNextLine());

						scan4.close();

					} catch (FileNotFoundException e) {
						// e.printStackTrace(); Wrong input name
					}

					// free scanners and run gc
					scan = null;
					scan2 = null;
					scan3 = null;
					scan4 = null;
					
					Timer waiter = new Timer();
					waiter.schedule(new TimerTask(){
						@Override
						public void run() {
							WorldBuilder.screen.arriveAtDoor(id);
							
							Timer loader = new Timer();

							if (!dead)
								loader.schedule(new TimerTask() {
									@Override
									public void run() {
										loading = false;
										cantWalk = false;
									}
								}, 30);
							else
								loader = null;
							if (link != null)
								link.stopWalking();
							cantWalk = true;
							
							bg = new ImageIcon("Maps\\" + refName + ".png").getImage();
						}
					}, 1550);
				}
			}
		}, 3000);
	}

	public boolean isLoading() {
		return loading;
	}

	public void setLoading(boolean loading) {
		this.loading = loading;
	}

	public boolean isHighscore() {
		return highscore;
	}

	public void setHighscore(boolean highscore) {
		this.highscore = highscore;
	}

	public boolean isControls() {
		return controls;
	}

	public void setControls(boolean controls) {
		this.controls = controls;
	}

	public Score[] getHscores() {
		return Hscores;
	}

	public void setHscores(Score[] hscores) {
		Hscores = hscores;
	}

	public JTextArea getShow() {
		return show;
	}

	public void setShow(JTextArea show) {
		this.show = show;
	}

	public boolean isCheats() {
		return cheats;
	}

	public void setCheats(boolean cheats) {
		this.cheats = cheats;
	}

	public ArrayList<Sprite> getSky() {
		return sky;
	}

	public void setSky(ArrayList<Sprite> sky) {
		this.sky = sky;
	}

	public boolean isSoulHardened() {
		return soulHardened;
	}

	public void setSoulHardened(boolean soulHardened) {
		this.soulHardened = soulHardened;
		if(soulHardened){
			Sprite souls = new Sprite();
			souls.setX(177);
			souls.setY(16);
			souls.playAnimation("soul", 30, false);
			uiSprites.add(0,souls);
		} else {
			if(uiSprites.get(0)!=null){
				uiSprites.remove(0);
			}
		}
	}

	public ArrayList<Sprite> getUiSprites() {
		return uiSprites;
	}

	public void setUiSprites(ArrayList<Sprite> uiSprites) {
		this.uiSprites = uiSprites;
	}
	
	public boolean isReleasedUse() {
		return releasedUse;
	}

	public void setReleasedUse(boolean releasedUse) {
		this.releasedUse = releasedUse;
	}

	public boolean isEdisplay() {
		return edisplay;
	}

	public void setEdisplay(boolean edisplay) {
		this.edisplay = edisplay;
	}

	public int getElement() {
		return element;
	}

	public void setElement(int element) {
		this.element = element;
	}

	public boolean isDirectOverlay() {
		return directOverlay;
	}

	public void setDirectOverlay(boolean directOverlay) {
		this.directOverlay = directOverlay;
	}

	public ImageIcon getDoverlay() {
		return doverlay;
	}

	public void setDoverlay(ImageIcon doverlay) {
		this.doverlay = doverlay;
	}
	
}
