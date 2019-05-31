package particleForge;

import javax.swing.ImageIcon;
import org.lwjgl.util.Point;
import org.lwjgl.util.vector.Vector2f;
import loz.Sprite;
import loz.WorldBuilder;

public class Particle {

	private float life, angle, speed, alpha;
	private int x, y, radius;
	private int r, g, b;
	private Vector2f velocity;
	private Point position = new Point();
	private String name;
	private Sprite emitter;
	private ImageIcon image;
	private boolean behind;

	public Particle(int x, int y, int radius, float life, float angle, float speed, int red, int green, int blue,
			float alpha) { // create particle with location, lifetime, angle, velocity, color, and transparency.
		behind = false;
		this.x = x;
		this.y = y;
		this.life = life;
		this.angle = angle;
		this.speed = speed;
		this.setAlpha(alpha);
		r = red;
		g = green;
		b = blue;
		this.radius = radius;
		float angleInRadians = (float) (angle * Math.PI / 180);

		this.position.setX(x);
		this.position.setY(y);
		this.velocity = new Vector2f();
		this.velocity.setX((float) (speed * Math.cos(angleInRadians)));
		this.velocity.setY((float) (-speed * Math.sin(angleInRadians)));
		
		boolean waiting = false;
		do{
			waiting = false;
			try{
			WorldBuilder.screen.getParticleSystem().add(this);
			} catch(ArrayIndexOutOfBoundsException e){
				waiting = true;
			} catch (NullPointerException fg){
				
			}
		} while(waiting);

	}
	
	public Particle(int i, int j, int radius2, float life2, float f, float h, ImageIcon c) {
		this(i,j,radius2,life2,f,h, 0, 0, 0, 0);
		this.setImage(c);
	}

	public void update(float dt) { // update particle life and location
		life -= dt;

		if (this.life > 0) {
			this.position.setX(this.position.getX() + (int) (this.velocity.x * dt));
			this.position.setY(this.position.getY() + (int) (this.velocity.y * dt));
		}
		if (this.life < 1) {
				image = null;
				for (int i = 0; i<WorldBuilder.screen.getParticleSystem().size(); i++) {
					if (this == WorldBuilder.screen.getParticleSystem().get(i)) {
						WorldBuilder.screen.getParticleSystem().remove(i);
					}
				}
		}
	}

	public float getLife() {
		return life;
	}

	public void setLife(float life) {
		this.life = life;
	}

	public float getAngle() {
		return angle;
	}

	public void setAngle(float angle) {
		this.angle = angle;
	}

	public float getSpeed() {
		return speed;
	}

	public void setSpeed(float speed) {
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

	public Point getPosition() {
		return position;
	}

	public void setPosition(Point position) {
		this.position = position;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public float getAlpha() {
		return alpha;
	}

	public void setAlpha(float alpha) {
		if(alpha<0.1){
			alpha=(float) 0.1;
		}
		this.alpha = alpha;
	}

	public int getRadius() {
		return radius;
	}

	public void setRadius(int radius) {
		this.radius = radius;
	}

	public int getDiameter() {
		return radius * 2;
	}

	public Sprite getEmitter() {
		return emitter;
	}

	public void setEmitter(Sprite emitter) {
		this.emitter = emitter;
		this.setPosition(new Point(emitter.getX(), emitter.getY()));
	}

	public ImageIcon getImage() {
		return image;
	}

	public void setImage(ImageIcon image) {
		this.image = image;
	}

	public boolean isBehind() {
		return behind;
	}

	public void setBehind(boolean behind) {
		this.behind = behind;
	}
}
