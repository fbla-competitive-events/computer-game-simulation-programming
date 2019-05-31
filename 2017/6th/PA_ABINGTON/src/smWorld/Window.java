package smWorld;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.awt.event.MouseWheelEvent;
import java.awt.event.MouseWheelListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.Scanner;
import javax.imageio.ImageIO;
import javax.swing.ImageIcon;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;
import loz.WorldBuilder;

@SuppressWarnings("serial")
public class Window extends JPanel implements KeyListener, MouseListener, MouseMotionListener, MouseWheelListener{
	
	private boolean paused, builderShow, mouseDown, mL, mR, shiftDown, controlDown, preview, tileset, collision;
	private Point c1,c2;
	
	private ArrayList<Sprite> sprites = new ArrayList<Sprite>();
	private ArrayList<Sprite> collisions = new ArrayList<Sprite>();
	private ArrayList<Sprite> objects = new ArrayList<Sprite>();
	
	private ArrayList<Integer> chestIDs = new ArrayList<Integer>();
	private ArrayList<Integer> doorIDs = new ArrayList<Integer>();
	private ArrayList<String> doorRefs = new ArrayList<String>();
	private ArrayList<Integer> aiIDs = new ArrayList<Integer>();
	
	private Sprite cs = new Sprite(new ImageIcon("Images\\select.png"));
	private TileCursor builder = new TileCursor(cs);
	
	private ArrayList<Sprite> TileList = new ArrayList<Sprite>();
	private String dn = "test";
	
	private int stile, xTrans, yTrans, msize, tileshift = 0, addedTiles = 0;
	
	public Window(){
		this.addKeyListener(this);
		setFocusable(true);
		paused = false;
		this.addMouseListener(this);
		this.addMouseMotionListener(this);
		this.addMouseWheelListener(this);
		getTiles();
	}
	
	public void createCollection(int item){ // Creates multiple tiles at an origin point (the cursor) with an integer id.
		
		if(item == 0){ // Tree 1
			
		}
		
	}
	
	public void getTiles(){ // load tiles, draw on screen
		
		int c = 0;
		for (int i = 0; i<99; i++){
			for (int f = 0; f<12; f++){
				c++;
				BufferedImage tile = new BufferedImage(1600, 1600, BufferedImage.TYPE_INT_ARGB);

				Graphics g = tile.getGraphics();
				
				g.drawImage(new ImageIcon("Tiles\\tileset.png").getImage(), 0, 0, null);
				
				//try {

					//ImageIO.write(tile.getSubimage(i*16, f*16, 16, 16), "PNG", new File("tiles\\"+(c)+".png"));
					Sprite xs = new Sprite(new ImageIcon("tiles\\"+(c)+".png"));
					xs.setCollisionData(c);
					xs.setTileid(c);
					TileList.add(xs);
					File fi = new File("tiles\\temptile.png");
					fi.delete();
				//} catch (IOException e) {
				//	e.printStackTrace();
				//}
			}
		}
		
	}
	
	public void removeSprite(int x, int y){
		for (int i = sprites.size()-1; i>=0; i--){
			if (sprites.get(i).getX() == x && sprites.get(i).getY() == y){
				sprites.remove(i);
				break;
			}
		}
	}
	
	public boolean matchTeleport(int id) {
		int[] ids = {614}; // id array - All should be added to no list instead of objects. ID Offset -1
		for(int i = 0; i<ids.length; i++){
			if(id == ids[i]){
				return true;
			}
		}
		return false;
	}
	
	public boolean matchAI(int id) {
		int[] ids = {273}; // ID Offset -1
		for(int i = 0; i<ids.length; i++){
			if(id == ids[i]){
				return true;
			}
		}
		return false;
	}
	
	public boolean matchChest(int id) {
		int[] ids = {12}; // ID Offset -1
		for(int i = 0; i<ids.length; i++){
			if(id == ids[i]){
				return true;
			}
		}
		return false;
	}
	
	public void save(){

		BufferedImage combined = new BufferedImage(16*msize, 16*msize, BufferedImage.TYPE_INT_ARGB);

		Graphics g = combined.getGraphics();
		
		for (int i = 0; i<sprites.size(); i++){
				
				if (!isObject(sprites.get(i))){
					g.drawImage(sprites.get(i).getImage().getImage(), (int)sprites.get(i).getX(), (int)sprites.get(i).getY(), null);	
				} else {
					objects.add(sprites.get(i));
				}
		}
		
		try {
			ImageIO.write(combined, "PNG", new File("Maps\\"+dn+".png"));
		} catch (IOException e) {
			e.printStackTrace();
		}
		
	}
	
	public void resize(){
		int dn = Integer.parseInt(JOptionPane.showInputDialog("Input new size",JOptionPane.WANTS_INPUT_PROPERTY));
		setMsize(dn);
	}
	
	public void loadDungeon(){
		
		sprites = new ArrayList<Sprite>();
		objects = new ArrayList<Sprite>();
		collisions = new ArrayList<Sprite>();
		
		dn = JOptionPane.showInputDialog("Input dungeon name",JOptionPane.WANTS_INPUT_PROPERTY);
		
		Scanner scan;

		try {
			scan = new Scanner(new File("world\\"+dn+".dng"));
			
			do{
				
				String tid = scan.nextLine();
				Sprite s = new Sprite(new ImageIcon("Tiles\\"+tid+".png"));
				s.setTileid(Integer.parseInt(tid));
				
				s.setId(Integer.parseInt(tid));
				s.setCollisionData(s.getTileid());
				int cl = Integer.parseInt(scan.nextLine())*16;
				s.setY(cl);
				cl = Integer.parseInt(scan.nextLine())*16;
				s.setX(cl);
				if(matchTeleport(Integer.parseInt(tid)-1)){
					doorIDs.add(Integer.parseInt(scan.nextLine()));
					doorRefs.add(scan.nextLine());
				} else if(matchAI(Integer.parseInt(tid)-1)){
					aiIDs.add(Integer.parseInt(scan.nextLine()));
				} else if(matchChest(Integer.parseInt(tid)-1)){
					chestIDs.add(Integer.parseInt(scan.nextLine()));
				}
				sprites.add(s);
				
			} while(scan.hasNextLine());
			
			scan.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
	
	}
	
	public boolean isObject(Sprite s){
		
		ArrayList<Integer> list = new ArrayList<Integer>();
		
		Scanner scan;
		
		try {
			scan = new Scanner(new File("objects.txt"));
			
			do{
				
				int l = Integer.parseInt(scan.nextLine());
				list.add(l);
				
			} while(scan.hasNextLine());
			
			scan.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		
		for (int i : list){
			if(s.getTileid() == i){
				return true;
			}
		}
		
		return false;
	}
	
	public void saveDungeon(){
		
		save();
		
		dn = JOptionPane.showInputDialog("Input dungeon name",JOptionPane.WANTS_INPUT_PROPERTY);

		try {
			PrintWriter saver = new PrintWriter("world\\"+dn+".dng");
			int c1 = 0, c2 = 0, c3 = 0, c4 = 0; // keep track of how many doors, chests, and ai marker we've gone through.
			for(int i = 0; i<sprites.size(); i++){
					saver.println(sprites.get(i).getTileid());
					saver.println((int)(sprites.get(i).getY()/16));
					saver.println((int)(sprites.get(i).getX()/16));
					
					if(matchTeleport(sprites.get(i).getId())){ // check doors, chests, and ais. Save them.
						int tid = sprites.get(i).getId();
						if (tid == 615) {
							saver.println(doorIDs.get(c1));
							c1++;
							saver.println(doorRefs.get(c2));
							saver.println("up");
							c2++;
						}
						if (tid == 627) {
							saver.println(doorIDs.get(c1));
							c1++;
							saver.println(doorRefs.get(c2));
							c2++;
							saver.println("down");
						}
						if (tid == 628) {
							saver.println(doorIDs.get(c1));
							c1++;
							saver.println(doorRefs.get(c2));
							c2++;
							saver.println("right");
						}
						if (tid == 640) {
							saver.println(doorIDs.get(c1));
							c1++;
							saver.println(doorRefs.get(c2));
							c2++;
							saver.println("left");
						}
					}
					if(matchAI(sprites.get(i).getId())){
						saver.println(aiIDs.get(c3));
						c3++;
					}
					if(matchChest(sprites.get(i).getId())){
						saver.println(chestIDs.get(c4));
						c4++;
					}
					
			}
			
			saver.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		
		
		try {
			PrintWriter saver = new PrintWriter("world\\"+dn+"COLLISIONMAP"+".dng");
			
			for(int i = 0; i<sprites.size(); i++){
					saver.println(sprites.get(i).setCollisionData(sprites.get(i).getTileid()));
					saver.println((int)(sprites.get(i).getY()/16));
					saver.println((int)(sprites.get(i).getX()/16));
			}
			
			saver.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		
		try {
			PrintWriter saver = new PrintWriter("world\\" + dn + "SPAWNS" + ".dng");

			for (int i = 0; i < objects.size(); i++) {

				int tid = objects.get(i).getTileid();
				saver.println(tid);
				saver.println((int) (objects.get(i).getY() / 16));
				saver.println((int) (objects.get(i).getX() / 16));
				if (tid == 13 || tid == 14 || tid == 15) {
					System.err.println("CHEST ADDED - "+chestIDs.get(0));
					saver.println(chestIDs.get(0));
					chestIDs.remove(0);
				}
				if (tid == 615) {
					saver.println(doorIDs.get(0));
					doorIDs.remove(0);
					saver.println(doorRefs.get(0));
					doorRefs.remove(0);
					saver.println("up");
				}
				if (tid == 627) {
					saver.println(doorIDs.get(0));
					doorIDs.remove(0);
					saver.println(doorRefs.get(0));
					doorRefs.remove(0);
					saver.println("down");
				}
				if (tid == 628) {
					saver.println(doorIDs.get(0));
					doorIDs.remove(0);
					saver.println(doorRefs.get(0));
					doorRefs.remove(0);
					saver.println("right");
				}
				if (tid == 640) {
					saver.println(doorIDs.get(0));
					doorIDs.remove(0);
					saver.println(doorRefs.get(0));
					doorRefs.remove(0);
					saver.println("left");
				}
				if (tid == 274) {
					saver.println(aiIDs.get(0));
					aiIDs.remove(0);
				}
			}
			
			saver.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		
		try {
			PrintWriter saver = new PrintWriter("world\\"+dn+"INFO"+".dng");
			
			saver.println(msize); // DONT MESS THIS UP
			saver.println(JOptionPane.showInputDialog("Input NORTH room name",JOptionPane.WANTS_INPUT_PROPERTY));
			saver.println(JOptionPane.showInputDialog("Input SOUTH room name",JOptionPane.WANTS_INPUT_PROPERTY));
			saver.println(JOptionPane.showInputDialog("Input EAST room name",JOptionPane.WANTS_INPUT_PROPERTY));
			saver.println(JOptionPane.showInputDialog("Input WEST room name",JOptionPane.WANTS_INPUT_PROPERTY));
			
			String p = "nop";
			do{
				p = (JOptionPane.showInputDialog("Input light level of room (-1 = outside)", -1));
			} while(!WorldBuilder.isParsableFloat(p));
			
			saver.println(p);
			
			saver.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		
		save(); // save image
		
	}
	
	public void toggleTileSet(){
		int c = tileshift*12;
		boolean stop = false;
		for (int i = tileshift; i<30+tileshift; i++){
			for (int f = 0; f<12; f++){
				if(tileset){
					try {
						Sprite ss = new Sprite(TileList.get(c).getImage());
						ss.setId(c);
						ss.setFollowScreen(true);
						ss.setY(f * 16);
						ss.setX(i * 16 + (-16 * tileshift + -16) + 16);
						sprites.add(ss);
						addedTiles++;
					} catch (IndexOutOfBoundsException e) {
						// no more
					}
				} else {
					try{
						if (!stop) {
							sprites.remove(sprites.size() - 1);
							addedTiles--;
							if (addedTiles == 0) {
								stop = true;
								//System.err.println("BREAK: " + addedTiles);
								break;
							}
						}
					} catch (ArrayIndexOutOfBoundsException a){
					}
				}
				c++;
			}
		}
		//System.err.println(addedTiles);
	}
	
	public void paintComponent(Graphics g2){
		
		super.paintComponent(g2);
		
		Graphics2D g = (Graphics2D)g2;
		g.scale(4, 4);
		// MAIN DRAW - DRAW WORLD IF NOT PAUSED
		// UI & MENUS ARE TO BE PAINTED WHEN PAUSED

		if (!paused){
			
			if (builderShow){
				g.translate(16*xTrans, 16*yTrans);
			}

			g.setColor(new Color(30,30,100));
			g.fillRect(0, 0, 16*msize, 16*msize);
			g.setColor(null);
			
			for(Sprite s: sprites){
				if(s.isFollowScreen()){
					s.setModx(-xTrans*16);
					s.setMody(-yTrans*16);
				}
				g.drawImage(s.getImage().getImage(), (int) s.getX(), (int) s.getY(), null);
			}
		
		}
		
		if (builderShow){
			g.translate(-16*xTrans, -16*yTrans);
		}
		
		if (builderShow){
			if(!tileset){
				g.setColor(Color.gray);
				g.setFont(new Font("TimesRoman",Font.BOLD,32));
				g.drawString("Tile ID: "+(stile+1), 32, 40);
				g.drawString("Shift: "+tileshift, 256, 40);
				g.setFont(new Font("TimesRoman",Font.BOLD,16));
				if(c1!=null && c2!=null){
					g.drawString("QuickAdd: "+c1.getX()+","+c1.getY()+" -- "+c2.getX()+","+c2.getY(), 226, 40);
					g.fillRect((int)c1.getX(), (int)c1.getY(), (int)c2.getX()-(int)c1.getX()+16, (int)c2.getY()-(int)c1.getY());
				}
			}
			g.drawImage(builder.getImg().getImage(), (int) builder.getLink().getX(), (int) builder.getLink().getY(), null);
			if(preview && !tileset){
				g.drawImage(TileList.get(stile).getImage().getImage(), 32, 72, null);
				
			}
			
			if(shiftDown){
				g.drawImage(TileList.get(stile).getImage().getImage(), (int) builder.getLink().getX(), (int) builder.getLink().getY(), null);
			}
			
		}
		
	}
	
	public void fillQuick(){
		
		int lx = (int)(c2.getX()-c1.getX())/16+1;
		int ly = (int)(c2.getY()-c1.getY())/16+1;
		
		for (int i = 0; i<lx; i++){
			for (int g = 0; g<ly; g++){
				Sprite s = new Sprite(TileList.get(stile).getImage());
				s.setTileid(TileList.get(stile).getTileid());
				s.setX(c1.getX()+i*16);
				s.setY(c1.getY()+g*16);
				s.setId(stile);
				sprites.add(s);
			}
		}
		
		c1 = null;
		c2 = null;

	}
	
	@Override
	public void mouseWheelMoved(MouseWheelEvent arg0) {

		if (builderShow){
			if(arg0.getWheelRotation() > 0){
				if (stile>0)
					stile--;
			} else {
				if(stile < TileList.size()-1)
					stile++;
			}
		}
		
	}

	@Override
	public void mouseDragged(MouseEvent arg0) {
		// TODO Auto-generated method stub
	}

	@Override
	public void mouseMoved(MouseEvent arg0) {
		
		builder.getX(xTrans);
		builder.getY(yTrans);
		
		//for (int i = 0; i<activatorsX.size(); i++){
		//	if (activatorsX.get(i) == mx/64 && activatorsY.get(i) == my/64){
		//		
		//	}
		//}
		
	}

	@Override
	public void mouseClicked(MouseEvent arg0) {
		// TODO Auto-generated method stub
	}

	@Override
	public void mouseEntered(MouseEvent arg0) {
		
	}

	@Override
	public void mouseExited(MouseEvent arg0) {
		// TODO Auto-generated method stub
	}

	@Override
	public void mousePressed(MouseEvent arg0) {
		if (builderShow && !tileset && !controlDown){
		mouseDown = true;
		if (SwingUtilities.isLeftMouseButton(arg0)){
			mL = true;
			
			Sprite s = new Sprite(TileList.get(stile).getImage());
			s.setTileid(TileList.get(stile).getTileid());
			s.setX(builder.getX(xTrans));
			s.setY(builder.getY(yTrans));
			
			s.setId(stile);
			
			if(stile == 12){
				chestIDs.add(Integer.parseInt(JOptionPane.showInputDialog("Chest Id",JOptionPane.WANTS_INPUT_PROPERTY)));
				System.out.println("TID: "+s.getTileid());
			}
			
			if(stile == 614 || stile == 626 || stile == 627 || stile == 639){ // offset -1
				doorIDs.add(Integer.parseInt(JOptionPane.showInputDialog("Door Id",JOptionPane.WANTS_INPUT_PROPERTY)));
				doorRefs.add(JOptionPane.showInputDialog("Map Link",JOptionPane.WANTS_INPUT_PROPERTY));
			}
			
			if(stile == 273){ // offset -1
				aiIDs.add(Integer.parseInt(JOptionPane.showInputDialog("AI ID",JOptionPane.WANTS_INPUT_PROPERTY)));
			}
			
			sprites.add(s);
			
		} else if (SwingUtilities.isRightMouseButton(arg0)){
			mR = true;
			removeSprite(builder.getX(xTrans),builder.getY(yTrans));
		}
		} else if (builderShow && !tileset && controlDown){
			
			if(c1 == null){
				c1 = new Point(builder.getX(xTrans),builder.getY(yTrans));
			} else if(c2 == null){
				c2 = new Point(builder.getX(xTrans),builder.getY(yTrans));
			} else {
				c2=null;
				c1=null;
			}
			
		}
	}

	@Override
	public void mouseReleased(MouseEvent arg0) {
		mouseDown = false;
		if (SwingUtilities.isLeftMouseButton(arg0)){
			mL = true;
		} else if (SwingUtilities.isRightMouseButton(arg0)){
			mR = true;
		}else if (SwingUtilities.isMiddleMouseButton(arg0)){
			for (Sprite s: sprites){

				if(s.getX() == builder.getX(xTrans) && s.getY() == builder.getY(yTrans)){
					stile = s.getId();
				}
			}
		}
	}

	@Override
	public void keyPressed(KeyEvent e) {
		
		if(e.getKeyCode() == KeyEvent.VK_ESCAPE)
			System.exit(0);
		
		if(e.getKeyCode() == KeyEvent.VK_Q){
			builderShow=!builderShow;
		}
		
		if(e.getKeyCode() == KeyEvent.VK_P){
			aiIDs.clear();
			doorIDs.clear();
			doorRefs.clear();
			chestIDs.clear();
			aiIDs = new ArrayList<Integer>();
			doorIDs = new ArrayList<Integer>();
			doorRefs = new ArrayList<String>();
			chestIDs = new ArrayList<Integer>();
			System.out.println("Cleared all id lists");
		}
		
		if (builderShow){
			if(e.getKeyCode() == KeyEvent.VK_A){
				xTrans++;
			}
			if(e.getKeyCode() == KeyEvent.VK_RIGHT){
				tileshift++;
				tileset = !tileset;
				toggleTileSet();
				tileset = !tileset;
				toggleTileSet();
			}
			if(e.getKeyCode() == KeyEvent.VK_LEFT){
				if(tileshift-1>-1){
					tileshift--;
					tileset = !tileset;
					toggleTileSet();
					tileset = !tileset;
					toggleTileSet();
				}
			}
			if(e.getKeyCode() == KeyEvent.VK_D){
				xTrans--;
			}
			if(e.getKeyCode() == KeyEvent.VK_W){
				yTrans++;
			}
			if(e.getKeyCode() == KeyEvent.VK_Y){
				resize();
			}
			if(e.getKeyCode() == KeyEvent.VK_S){
				yTrans--;
			}
			if(e.getKeyCode() == KeyEvent.VK_K){
				save();
			}
			if(e.getKeyCode() == KeyEvent.VK_N){
				saveDungeon();
			}
			if(e.getKeyCode() == KeyEvent.VK_O){
				getTiles();
			}
			if(e.getKeyCode() == KeyEvent.VK_E){
				preview = !preview;
			}
			if(e.getKeyCode() == KeyEvent.VK_R){
				collision = !collision;
			}
			if(e.getKeyCode() == KeyEvent.VK_T){
				tileset = !tileset;
				toggleTileSet();
			}
			if(e.getKeyCode() == KeyEvent.VK_SLASH){
				fillQuick();
			}
			if(e.getKeyCode() == KeyEvent.VK_L){
				loadDungeon();
			}
			if(e.getKeyCode() == KeyEvent.VK_SHIFT){
				shiftDown = true;
			}
			if(e.getKeyCode() == KeyEvent.VK_CONTROL){
				controlDown = true;
			}
		}
		
	}

	@Override
	public void keyReleased(KeyEvent e) {
		if(e.getKeyCode() == KeyEvent.VK_SHIFT){
			shiftDown = false;
		}
		if(e.getKeyCode() == KeyEvent.VK_CONTROL){
			controlDown = false;
		}
	}

	@Override
	public void keyTyped(KeyEvent e) {
		// TODO Auto-generated method stub
	}

	public boolean isPaused() {
		return paused;
	}

	public void setPaused(boolean paused) {
		this.paused = paused;
	}

	public ArrayList<Sprite> getSprites() {
		return sprites;
	}

	public void setSprites(ArrayList<Sprite> sprites) {
		this.sprites = sprites;
	}

	public TileCursor getBuilder() {
		return builder;
	}

	public void setBuilder(TileCursor builder) {
		this.builder = builder;
	}

	public boolean isBuilderShow() {
		return builderShow;
	}

	public void setBuilderShow(boolean builderShow) {
		this.builderShow = builderShow;
	}

	public Sprite getCs() {
		return cs;
	}

	public void setCs(Sprite cs) {
		this.cs = cs;
	}

	public int getStile() {
		return stile;
	}

	public void setStile(int stile) {
		this.stile = stile;
	}

	public ArrayList<Sprite> getTileList() {
		return TileList;
	}

	public void setTileList(ArrayList<Sprite> tileList) {
		TileList = tileList;
	}

	public boolean isMouseDown() {
		return mouseDown;
	}

	public void setMouseDown(boolean mouseDown) {
		this.mouseDown = mouseDown;
	}

	public int getxTrans() {
		return xTrans;
	}

	public void setxTrans(int xTrans) {
		this.xTrans = xTrans;
	}

	public int getyTrans() {
		return yTrans;
	}

	public void setyTrans(int yTrans) {
		this.yTrans = yTrans;
	}

	public int getMsize() {
		return msize;
	}

	public void setMsize(int msize) {
		this.msize = msize;
	}

	public boolean ismL() {
		return mL;
	}

	public void setmL(boolean mL) {
		this.mL = mL;
	}

	public boolean ismR() {
		return mR;
	}

	public void setmR(boolean mR) {
		this.mR = mR;
	}

	public boolean isShiftDown() {
		return shiftDown;
	}

	public void setShiftDown(boolean shiftDown) {
		this.shiftDown = shiftDown;
	}

	public boolean isControlDown() {
		return controlDown;
	}

	public void setControlDown(boolean controlDown) {
		this.controlDown = controlDown;
	}

	public boolean isPreview() {
		return preview;
	}

	public void setPreview(boolean preview) {
		this.preview = preview;
	}

	public boolean isTileset() {
		return tileset;
	}

	public void setTileset(boolean tileset) {
		this.tileset = tileset;
	}

	public ArrayList<Sprite> getCollisions() {
		return collisions;
	}

	public void setCollisions(ArrayList<Sprite> collisions) {
		this.collisions = collisions;
	}

	public boolean isCollision() {
		return collision;
	}

	public void setCollision(boolean collision) {
		this.collision = collision;
	}

	public ArrayList<Sprite> getObjects() {
		return objects;
	}

	public void setObjects(ArrayList<Sprite> objects) {
		this.objects = objects;
	}

	public Point getC2() {
		return c2;
	}

	public void setC2(Point c2) {
		this.c2 = c2;
	}

	public Point getC1() {
		return c1;
	}

	public void setC1(Point c1) {
		this.c1 = c1;
	}

	public int getAddedTiles() {
		return addedTiles;
	}

	public void setAddedTiles(int addedTiles) {
		this.addedTiles = addedTiles;
	}

	public ArrayList<Integer> getChestIDs() {
		return chestIDs;
	}

	public void setChestIDs(ArrayList<Integer> chestIDs) {
		this.chestIDs = chestIDs;
	}

	public ArrayList<Integer> getDoorIDs() {
		return doorIDs;
	}

	public void setDoorIDs(ArrayList<Integer> doorIDs) {
		this.doorIDs = doorIDs;
	}

	public ArrayList<String> getDoorRefs() {
		return doorRefs;
	}

	public void setDoorRefs(ArrayList<String> doorRefs) {
		this.doorRefs = doorRefs;
	}

	public ArrayList<Integer> getAiIDs() {
		return aiIDs;
	}

	public void setAiIDs(ArrayList<Integer> aiIDs) {
		this.aiIDs = aiIDs;
	}

	public String getDn() {
		return dn;
	}

	public void setDn(String dn) {
		this.dn = dn;
	}

	public int getTileshift() {
		return tileshift;
	}

	public void setTileshift(int tileshift) {
		this.tileshift = tileshift;
	}

}
