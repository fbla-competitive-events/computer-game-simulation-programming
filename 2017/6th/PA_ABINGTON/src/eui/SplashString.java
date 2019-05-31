package eui;

import java.awt.Color;
import java.awt.Font;
import java.awt.FontFormatException;
import java.awt.GraphicsEnvironment;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;

public class SplashString {

	private int x, y, size;
	private String text = "";
	private boolean remove, rainbow;
	private Color color;
	private Font font;
	private GraphicsEnvironment ge;
	private Timer die = new Timer();
	private Timer appender = new Timer();
	private int appendCount = 0;
	private ArrayList<SplashString> pointerList = new ArrayList<SplashString>();
	
	public SplashString(String text, int x, int y){
		size = 16;
		font = getScaledFont(size);
		color = Color.black;
		this.x = x;
		this.y = y;
		this.setText(text);
	}

	public SplashString(String text, int x, int y, int s){
		this(text,x,y);
		size = s;
	}
	
	public SplashString(String text, int x, int y, int s, Color c){
		this(text,x,y);
		size = s;
		color = c;
	}
	
	public SplashString(String text, int x, int y, int s, long speed, long holdTime, Color c, boolean r){ // add string to screen, move according to timers, and leave screen.
		size = s;
		rainbow = r;
		SplashString self = this;
		font = getScaledFont(size);
		color = c;
		this.x = x;
		this.y = y;
		
		appender = new Timer(); // adder timer
		appender.schedule(new TimerTask(){
			@Override
			public void run() {
				try{
					setText(text.substring(0,appendCount+1));
					appendCount++;
					if(rainbow){
						color = new Color((int)Math.random()*255+1,(int)Math.random()*255+1,(int)Math.random()*255+1);
					}
				} catch (StringIndexOutOfBoundsException kk){
				}
			}
		}, speed, speed);
		
		if (rainbow) {
			appender.schedule(new TimerTask() {
				@Override
				public void run() {
					try {
						color = new Color((int) Math.random() * 255 + 1,(int) Math.random() * 255 + 1, (int) Math.random() * 255 + 1);
					} catch (StringIndexOutOfBoundsException kk) {
					}
				}
			}, 35, 35);
		}
		
		die = new Timer();
		die.schedule(new TimerTask(){ // fly away + release timer & object
			@Override
			public void run() {
				appendCount = 0;
				appender.cancel();

				appender = new Timer();
				
				appender.schedule(new TimerTask(){
					@Override
					public void run() {
						
						appendCount++;
						setX(getX()-appendCount);
						
						if(appendCount > 50){
							if(pointerList != null){
								pointerList.remove(self);
								appendCount = 0;
								appender.cancel();
							}
						}
					}
				}, 15,15);
				
				die.cancel();
				die.purge();
				appender.purge();
			}
		}, text.length()*speed+speed+holdTime);
	}
	
	// auto append generation
	public SplashString(String text, int x, int y, int s, long speed, long holdTime, Color c){
		this(text,x,y,s,speed,holdTime,c,false);
	}
	
	public void append(String s){
		this.setText(this.getText()+s);
	}
	
	public void setLife(long ms){
		SplashString s = this;
		Timer die = new Timer();
		die.schedule(new TimerTask(){
			@Override
			public void run() {
				if(pointerList != null){
					pointerList.remove(s);
				}
				die.cancel();
			}	
		}, ms);
		die.purge();
	}
	
	public Font getScaledFont(float s){
		ge = GraphicsEnvironment.getLocalGraphicsEnvironment();
		try {
			ge.registerFont(Font.createFont(Font.TRUETYPE_FONT, new File("Fonts\\lttp.ttf")).deriveFont(32));
			return (ge.getAllFonts()[ge.getAllFonts().length-1].deriveFont(s));
		} catch (FontFormatException | IOException e) {
			e.printStackTrace();
		}
		return null;
	}
	
	public void setScaledFont(float s){
		font = getScaledFont(s);
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

	public int getSize() {
		return size;
	}

	public void setSize(int size) {
		this.size = size;
	}

	public String getText() {
		return text;
	}

	public void setText(String text) {
		this.text = text;
	}

	public boolean isRemove() {
		return remove;
	}

	public void setRemove(boolean remove) {
		this.remove = remove;
	}

	public Color getColor() {
		return color;
	}

	public void setColor(Color color) {
		this.color = color;
	}

	public Font getFont() {
		return font;
	}

	public void setFont(Font font) {
		this.font = font;
	}

	public Timer getDie() {
		return die;
	}

	public void setDie(Timer die) {
		this.die = die;
	}

	public ArrayList<SplashString> getPointerList() {
		return pointerList;
	}

	public void setPointerList(ArrayList<SplashString> pointerList) {
		this.pointerList = pointerList;
	}

	public int getAppendCount() {
		return appendCount;
	}

	public void setAppendCount(int appendCount) {
		this.appendCount = appendCount;
	}

	public Timer getAppender() {
		return appender;
	}

	public void setAppender(Timer appender) {
		this.appender = appender;
	}

	public boolean isRainbow() {
		return rainbow;
	}

	public void setRainbow(boolean rainbow) {
		this.rainbow = rainbow;
	}
	
}
