package loz;

import java.awt.Color;
import java.awt.MouseInfo;
import java.awt.Point;
import java.io.File;
import java.io.FileNotFoundException;
import java.util.ConcurrentModificationException;
import java.util.NoSuchElementException;
import java.util.Scanner;
import java.util.Timer;
import java.util.TimerTask;
import javax.swing.ImageIcon;
import particleForge.Particle;

public class Sprite {

	private int x, y, width, height, layer, timerRuns = 0, frameIndex;
	private Point origin;
	private ImageIcon image, lastParticle;
	private double modx, mody, rotateAngle, scale;
	private String name;
	private int id, tileid, frame, offSetX, offSetY;
	private boolean followScreen, canRotate;
	private collisionType collision;
	private Timer pather, animate = new Timer();

	public enum collisionType {
		TOP, LEFT, FULL, RIGHT, BOTTOM, TOPLEFTD, TOPRIGHTD, BOTTOMLEFTD, BOTTOMRIGHTD, FULLPAIN, NONE
	}
	
	public void keepFacing(Sprite tofollow, int offsetX, int offsetY){
		pather.schedule(new TimerTask(){
			@Override
			public void run() {
				int centerX = getX()+ width / 2 + offsetX;
				int centerY = getY()+ height / 2 + offsetY;
				setOffSetX(offsetX);
				setOffSetY(offsetY);
				double angle = Math.atan2(centerY - tofollow.getY(), centerX - tofollow.getX()) - Math.PI / 2;
				setRotateAngle(angle);
			}
		}, 60, 60);
	}

	public void free() {
		if (image != null) {
			image.getImage().flush();
			image = null;
		}
		if (lastParticle != null) {
			lastParticle.getImage().flush();
			lastParticle = null;
		}
		pather = null;
		origin = null;
		name = null;
	}

	public Sprite(String file) {
		x = 0;
		y = 0;
		image = new ImageIcon(file);
		width = image.getIconWidth();
		height = image.getIconHeight();
		canRotate = false;
		scale = 1;
	}

	public Sprite(String file, int x, int y) {
		this(file);
		this.x = x;
		this.y = y;
		origin = new Point(x, y);
		canRotate = false;
		scale = 1;
	}

	public Sprite() {
		x = 0;
		y = 0;
		modx = 0;
		mody = 0;
		canRotate = false;
		scale = 1;
	}

	public Sprite(ImageIcon i) {
		this();
		this.setImage(i);
		canRotate = false;
		scale = 1;
	}

	public Sprite(ImageIcon i, int x, int y) {
		this();
		this.setImage(i);
		origin = new Point(x, y);
		canRotate = false;
		scale = 1;
	}

	public void killParticles() { // stop particle timer
		pather.cancel();
		pather.purge();
		pather = null;
		pather = new Timer();
		scale = 1;
	}

	public void playAnimation(String stripUrl, long speed, boolean fieAndForget) { // for
																					// image
																					// strips
		this.setName(stripUrl);

		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				if (new File("Images\\Particles\\Sets\\"
						+ (stripUrl + WorldBuilder.numberWithLeadingZeros(frameIndex, 4) + ".png")).exists()) {
					setImage(new ImageIcon("Images\\Particles\\Sets\\"
							+ (stripUrl + WorldBuilder.numberWithLeadingZeros(frameIndex, 4)) + ".png"));
					frameIndex++;
				} else {
					frameIndex = 0;
					if (fieAndForget) {
						pather.cancel();
						pather.purge();
						WorldBuilder.screen.removeByName(stripUrl);
						setImage(null);
					}
				}
			}
		}, speed, speed);
	}

	public void moveAlongCircle(int radius, long speed, long degree) {

		pather = new Timer();

		pather.schedule(new TimerTask() {

			@Override
			public void run() {

				timerRuns++;
				int x = (int) (radius * (float) Math.cos(Math.toRadians(timerRuns * degree)) + origin.x);
				int y = (int) (radius * (float) Math.sin(Math.toRadians(timerRuns * degree)) + origin.y);

				if (timerRuns * degree > 359) {
					timerRuns = 0;
				}

				setX(x);
				setY(y);

			}

		}, speed, speed);

	}

	public void moveAlongCircle(int radius, long speed) {

		pather = new Timer();

		pather.schedule(new TimerTask() {

			@Override
			public void run() {

				timerRuns++;
				int x = (int) (radius * (float) Math.cos(Math.toRadians(timerRuns * 15)) + origin.x);
				int y = (int) (radius * (float) Math.sin(Math.toRadians(timerRuns * 15)) + origin.y);

				if (timerRuns > 23) {
					timerRuns = 0;
				}

				setX(x);
				setY(y);

			}

		}, speed, speed);

	}

	public void bobVerticle(int length, long speed) { // move up to point A and
														// down to point B &
														// repeat.
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				timerRuns++;
				int x = origin.x;
				int y = (int) (length * (float) Math.sin(Math.toRadians(timerRuns * 15)) + origin.y);
				if (timerRuns > 23) {
					timerRuns = 0;
				}
				setX(x);
				setY(y);
			}
		}, speed, speed);
	}

	public void bobHorizontal(int length, long speed) { // move left & right
														// from point A to point
														// B & repeat.
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				timerRuns++;
				int x = origin.y;
				int y = (int) (length * (float) Math.sin(Math.toRadians(timerRuns * 15)) + origin.x);
				if (timerRuns > 23) {
					timerRuns = 0;
				}
				setY(x);
				setX(y);
			}
		}, speed, speed);
	}

	public void bobHorizontal(int length, long speed, int degree) { // move left
																	// & right
																	// from
																	// point A
																	// to point
																	// B &
																	// repeat.
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				timerRuns++;
				int x = origin.y;
				int y = (int) (length * (float) Math.sin(Math.toRadians(timerRuns * degree)) + origin.x);
				if (timerRuns * degree > 359) {
					timerRuns = 0;
				}
				setY(x);
				setX(y);
			}
		}, speed, speed);
	}

	public void followCursor() { // set coordinates to mouse position every 35ms
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				setX((int) (MouseInfo.getPointerInfo().getLocation().getX() / 4) + 720);
				setY((int) (MouseInfo.getPointerInfo().getLocation().getY() / 4) + 396);
			}
		}, 35, 35);
	}

	public void followPlayer(int offX, int offY) { // set coordinates to player
													// position every 35ms
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				setX(WorldBuilder.link.getX() + offX);
				setY(WorldBuilder.link.getY() + offY);
			}
		}, 35, 35);
	}

	public void startEmitter(int particleCount, long sequenceTime, float life, float angle, float speed, int radius,
			Color c) { // BURST EMITTER
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				for (int i = 0; i < particleCount; i++) {
					@SuppressWarnings("unused") // it is
					Particle pn = new Particle(getX(), getY(), radius, life, (float) (Math.random() * 360 + 1),
							(float) Math.random() * speed, c.getRed(), c.getGreen(), c.getBlue(), 255);
					if (getImage() == null) {
						pather.cancel();
						pather.purge();
					}
				}
			}
		}, sequenceTime, sequenceTime);
	}

	public void startEmitterTextured(int particleCount, long sequenceTime, float life, float angle, float speed,
			boolean behind, ImageIcon c) { // BURST EMITTER
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				for (int i = 0; i < particleCount; i++) {
					Particle pn = new Particle(getX() + (Math.abs(getWidth() - c.getIconWidth())) / 2,
							getY() + (Math.abs(getHeight() - c.getIconHeight())) / 2, 8, life,
							(float) (Math.random() * 360 + 1), (float) Math.random() * speed, c);
					pn.setBehind(behind);
					if (getImage() == null && pather != null) {
						pather.cancel();
						pather.purge();
					}
				}
			}
		}, sequenceTime, sequenceTime);
	}

	public void startEmitterTexturedRandom(int particleCount, long sequenceTime, float life, float angle, float speed,
			boolean behind, ImageIcon[] ca) { // BURST EMITTER RANDOM
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				for (int i = 0; i < particleCount; i++) {
					lastParticle = WorldBuilder.randomImage(ca);
					Particle pn = new Particle(getX() + (Math.abs(getWidth() - lastParticle.getIconWidth())) / 2,
							getY() + (Math.abs(getHeight() - lastParticle.getIconHeight())) / 2, 8, life,
							(float) (Math.random() * 360 + 1), (float) Math.random() * speed, lastParticle);
					pn.setBehind(behind);
					if (getImage() == null) {
						pather.cancel();
						pather.purge();
					}
				}
			}
		}, sequenceTime, sequenceTime);
	}

	public void startEmitterTexturedRandomOneShot(int particleCount, long sequenceTime, float life, float angle,
			float speed, boolean behind, ImageIcon[] ca) { // BURST EMITTER
															// RANDOM ONE USE
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				for (int i = 0; i < particleCount; i++) {
					lastParticle = WorldBuilder.randomImage(ca);
					Particle pn = new Particle(getX() + (Math.abs(getWidth() - lastParticle.getIconWidth())) / 2,
							getY() + (Math.abs(getHeight() - lastParticle.getIconHeight())) / 2, 8, life,
							(float) (Math.random() * 360 + 1), (float) Math.random() * speed, lastParticle);
					pn.setBehind(behind);
					if (getImage() == null) {
						pather.cancel();
						pather.purge();
					}
				}
			}
		}, sequenceTime);
	}

	public void startEmitterTexturedShift(int particleCount, long sequenceTime, float life, float angle, float speed,
			boolean behind, ImageIcon c, int x, int y) { // BURST EMITTER
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				for (int i = 0; i < particleCount; i++) {
					Particle pn = new Particle(getX() + x + (Math.abs(getWidth() - c.getIconWidth())) / 2,
							getY() + y + (Math.abs(getHeight() - c.getIconHeight())) / 2, 8, life,
							(float) (Math.random() * 360 + 1), (float) Math.random() * speed, c);
					pn.setBehind(behind);
					if (getImage() == null && pather != null) {
						pather.cancel();
						pather.purge();
					}
				}
			}
		}, sequenceTime, sequenceTime);
	}

	public void startEmitterTexturedOneShot(int particleCount, long sequenceTime, float life, float angle, float speed,
			boolean behind, ImageIcon c) { // BURST EMITTER ONE USE
		pather = new Timer();
		pather.schedule(new TimerTask() {
			@Override
			public void run() {
				for (int i = 0; i < particleCount; i++) {
					Particle pn = new Particle(getX() + (Math.abs(getWidth() - c.getIconWidth())) / 2,
							getY() + (Math.abs(getHeight() - c.getIconHeight())) / 2, 8, life,
							(float) (Math.random() * 360 + 1), (float) Math.random() * speed, c);
					pn.setBehind(behind);
					if (getImage() == null) {
						pather.cancel();
						pather.purge();
					}
				}
			}
		}, sequenceTime);
	}
		
	public int compareToFile(int i) {

		Scanner scan;
		try {
			scan = new Scanner(new File("collisions.txt"));

			do {

				if (i == Integer.parseInt(scan.nextLine())) {
					int s = Integer.parseInt(scan.nextLine());
					scan.close();
					return s;
				}

			} while (scan.hasNextLine());

			scan.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (NoSuchElementException f) {
			// NO COLLISION
		}

		return -1;
	}

	public void move(int gx, int gy) {

		this.setX(getX() + gx);
		this.setY(getY() + gy);

	}

	public collisionType setCollisionData(int i) {

		i = compareToFile(i);

		if (i == 0) {
			this.setCollision(collisionType.TOP);
		} else if (i == 1) {
			this.setCollision(collisionType.LEFT);
		} else if (i == 2) {
			this.setCollision(collisionType.FULL);
		} else if (i == 3) {
			this.setCollision(collisionType.RIGHT);
		} else if (i == 4) {
			this.setCollision(collisionType.BOTTOM);
		} else if (i == 5) {
			this.setCollision(collisionType.TOPLEFTD);
		} else if (i == 6) {
			this.setCollision(collisionType.TOPRIGHTD);
		} else if (i == 7) {
			this.setCollision(collisionType.BOTTOMLEFTD);
		} else if (i == 8) {
			this.setCollision(collisionType.BOTTOMRIGHTD);
		} else if (i == 9) {
			this.setCollision(collisionType.FULLPAIN);
		} else if (i == 10) {
			this.setCollision(collisionType.NONE);
		} else {
			return null;
		}

		return this.getCollision();

	}

	public collisionType setCollisionData(String s) {

		if (s.equalsIgnoreCase("top")) {
			this.setCollision(collisionType.TOP);
		}
		if (s.equalsIgnoreCase("left")) {
			this.setCollision(collisionType.LEFT);
		}
		if (s.equalsIgnoreCase("full")) {
			this.setCollision(collisionType.FULL);
		}
		if (s.equalsIgnoreCase("right")) {
			this.setCollision(collisionType.RIGHT);
		}
		if (s.equalsIgnoreCase("bottom")) {
			this.setCollision(collisionType.BOTTOM);
		}
		if (s.equalsIgnoreCase("topleftd")) {
			this.setCollision(collisionType.TOPLEFTD);
		}
		if (s.equalsIgnoreCase("toprightd")) {
			this.setCollision(collisionType.TOPRIGHTD);
		}
		if (s.equalsIgnoreCase("bottomleftd")) {
			this.setCollision(collisionType.BOTTOMLEFTD);
		}
		if (s.equalsIgnoreCase("bottomrightd")) {
			this.setCollision(collisionType.BOTTOMRIGHTD);
		}
		if (s.equalsIgnoreCase("fullpain")) {
			this.setCollision(collisionType.FULLPAIN);
		}
		if (s.equalsIgnoreCase("none")) {
			this.setCollision(collisionType.NONE);
		}
		return this.getCollision();

	}

	public void setTile(int t) {
		this.setImage(new ImageIcon("tiles\\" + t + ".png"));
		this.setTileid(t);
	}

	public boolean isBreakable() {
		for (int i : WorldBuilder.screen.getBreakList()) {
			if (this.getTileid() == i) {
				return true;
			}
		}
		return false;
	}

	public boolean isAnimated() {

		frame = 1;
		for (int i : WorldBuilder.screen.getAnimated()) {
			if (this.getTileid() == i) {
				animate.schedule(new TimerTask() {
					@Override
					public void run() {
						if (frame > 3) // 3 frames max for background sprites
							frame = 1;
						setImage(new ImageIcon("tiles\\" + (tileid + frame) + ".png"));
						frame++;
					}
				}, 120, 120);
				return true;
			}
		}
		return false;
	}

	public void removeSprite() {
		WorldBuilder.screen.getSprites().remove(this);
		WorldBuilder.screen.getCollisions().remove(this);
		WorldBuilder.screen.getObjects().remove(this);
		// Better collision remove Search
		removeCollision();
		removeObject();
	}

	public void changeImage(int t) {
		setTile(t);
		for (Sprite s : WorldBuilder.screen.getObjects()) {
			if (s.getX() == this.getX() && s.getY() == this.getY()) {
				s.setImage(getImage());
				s.setTileid(tileid);
			}
		}
	}

	public void removeCollision() {
		try {
			for (Sprite s : WorldBuilder.screen.getCollisions()) {
				if (s.getX() == this.getX() && s.getY() == this.getY()) {
					WorldBuilder.screen.getCollisions().remove(s);
					break;
				}
			}
		} catch (ConcurrentModificationException gf) {
			// wait
		}
	}
		
	public void removeObject() {
		removeCollision();
		try {
			for (WorldItem s : WorldBuilder.screen.getObjects()) {
				if (s.getX() == this.getX() && s.getY() == this.getY() && !s.isUsed()) {
					WorldBuilder.screen.getObjects().remove(s);
					break;
				}
			}
		} catch (ConcurrentModificationException gg) {
		}
	}
		
	public void removePickup() {
		removeObject();
		for (WorldItem s : WorldBuilder.screen.getDrops()) {
			if (s.getX() == this.getX() && s.getY() == this.getY() && !s.isUsed()) {
				WorldBuilder.screen.getDrops().remove(s);
				break;
			}
		}
	}

		public Sprite collisionImage(){ // collision type image setter
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
			}if(getCollision() == collisionType.FULLPAIN){
				return new Sprite(new ImageIcon("Tiles\\full.png"),x,y);
			}
			return null;
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

	public int getWidth() {
		return width;
	}

	public void setWidth(int width) {
		this.width = width;
	}

	public int getHeight() {
		return height;
	}

	public void setHeight(int height) {
		this.height = height;
	}

	public ImageIcon getImage() {
		return image;
	}

	public void setImage(ImageIcon image) {
		this.image = image;
	}

	public collisionType getCollision() {
		return collision;
	}

	public void setCollision(collisionType collision) {
		this.collision = collision;
	}

	public boolean isFollowScreen() {
		return followScreen;
	}

	public void setFollowScreen(boolean followScreen) {
		this.followScreen = followScreen;
	}

	public int getTileid() {
		return tileid;
	}

	public void setTileid(int tileid) {
		this.tileid = tileid;
		isAnimated();
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public double getMody() {
		return mody;
	}

	public void setMody(double mody) {
		this.mody = mody;
	}

	public double getModx() {
		return modx;
	}

	public void setModx(double modx) {
		this.modx = modx;
	}

	public int getLayer() {
		return layer;
	}

	public void setLayer(int layer) {
		this.layer = layer;
	}
	
	public int getFrame() {
		return frame;
	}

	public void setFrame(int frame) {
		this.frame = frame;
	}

	public Timer getPather() {
		return pather;
	}

	public void setPather(Timer pather) {
		this.pather = pather;
	}

	public int getTimerRuns() {
		return timerRuns;
	}

	public void setTimerRuns(int timerRuns) {
		this.timerRuns = timerRuns;
	}

	public Point getOrigin() {
		return origin;
	}

	public void setOrigin(Point origin) {
		this.origin = origin;
	}

	public ImageIcon getLastParticle() {
		return lastParticle;
	}

	public void setLastParticle(ImageIcon lastParticle) {
		this.lastParticle = lastParticle;
	}

	public int getFrameIndex() {
		return frameIndex;
	}

	public void setFrameIndex(int frameIndex) {
		this.frameIndex = frameIndex;
	}

	public Timer getAnimate() {
		return animate;
	}

	public void setAnimate(Timer animate) {
		this.animate = animate;
	}

	public boolean isCanRotate() {
		return canRotate;
	}

	public void setCanRotate(boolean canRotate) {
		this.canRotate = canRotate;
	}

	public double getRotateAngle() {
		return rotateAngle;
	}

	public void setRotateAngle(double rotateAngle) {
		this.rotateAngle = rotateAngle;
	}

	public int getOffSetY() {
		return offSetY;
	}

	public void setOffSetY(int offSetY) {
		this.offSetY = offSetY;
	}

	public int getOffSetX() {
		return offSetX;
	}

	public void setOffSetX(int offSetX) {
		this.offSetX = offSetX;
	}

	public double getScale() {
		return scale;
	}

	public void setScale(double scale) {
		this.scale = scale;
	}
	
}
