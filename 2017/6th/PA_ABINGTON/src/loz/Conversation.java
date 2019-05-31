package loz;

import java.awt.Font;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.Scanner;
import java.util.Timer;
import java.util.TimerTask;

import eui.Instruction;

public class Conversation {

	private String Identifier;
	private ArrayList<String> lines = new ArrayList<String>();
	private Conversation link; // can start other conversations at end
	private ArrayList<Instruction> steps = new ArrayList<Instruction>();
	private Font font;
	private int linePosition, lineNumber;
	private String currentLine;
	private Timer scroller = new Timer();
	private Runnable endScript;
	
	public Conversation(){
		currentLine = "";
		linePosition = 0;
		lineNumber = 0;
		this.setFont(WorldBuilder.font);
	}

	public boolean advance(){
		try{
			currentLine = lines.get(lineNumber).substring(0, linePosition);
			linePosition++;
			currentLine = WorldBuilder.wrappedString(getCurrentLine(), 101);
		} catch(StringIndexOutOfBoundsException oobLine){
			return true;
		} catch (IndexOutOfBoundsException oobFull){
			if(getEndScript()!=null)
				getEndScript().run();
			WorldBuilder.screen.setConversation(new Conversation());
			WorldBuilder.screen.setConv(false);
			WorldBuilder.screen.setCantWalk(false);
			return true;
		}
		return false;
	}
	
	public void nextLine() {
		try {
			if (currentLine.equals(WorldBuilder.wrappedString(lines.get(lineNumber), 101))) {
				lineNumber++;
				linePosition = 0;
				startScroll();
			} else {
				this.getScroller().cancel();
				this.getScroller().purge();
				scroller = new Timer();
				this.setCurrentLine(WorldBuilder.wrappedString(lines.get(lineNumber), 101));
			}
		} catch (IndexOutOfBoundsException a) {

		}
	}
	
	public void startScroll(){
		scroller = new Timer();
		scroller.schedule(new TimerTask(){
			@Override
			public void run() {
				if(advance()){
					scroller.cancel();
					scroller.purge();
				}
			}
		}, 50, 50);
	}
	
	public Conversation loadConversation(String fileName) {
		WorldBuilder.screen.setCantWalk(true);
		Scanner scan;
		try {
			scan = new Scanner(new File("conversations\\"+fileName+".con"));
			
			this.setIdentifier(fileName);
			
			do {
				String nxt = scan.nextLine();
				
				if(!nxt.equals("%option")){
					lines.add(nxt);
				}
				
			} while (scan.hasNextLine());
			
			scan.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		
		return this;

	}
	
	public void saveConversation(String fileName) {

		PrintWriter write;
		try {
			write = new PrintWriter(new File("conversations\\"+fileName+".con"));
			
			for(String l : lines){
				write.println(l);
			}
			for(Instruction l : steps){
				write.println(l.getId());
			}
			
			write.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}

	}

	public String getIdentifier() {
		return Identifier;
	}

	public void setIdentifier(String identifier) {
		Identifier = identifier;
	}

	public ArrayList<String> getLines() {
		return lines;
	}

	public void setLines(ArrayList<String> lines) {
		this.lines = lines;
	}

	public Conversation getLink() {
		return link;
	}

	public void setLink(Conversation link) {
		this.link = link;
	}

	public ArrayList<Instruction> getSteps() {
		return steps;
	}

	public void setSteps(ArrayList<Instruction> steps) {
		this.steps = steps;
	}

	public Font getFont() {
		return font;
	}

	public void setFont(Font font) {
		this.font = font;
	}

	public String getCurrentLine() {
		return currentLine;
	}

	public void setCurrentLine(String currentLine) {
		this.currentLine = currentLine;
	}

	public int getLinePosition() {
		return linePosition;
	}

	public void setLinePosition(int linePosition) {
		this.linePosition = linePosition;
	}

	public int getLineNumber() {
		return lineNumber;
	}

	public void setLineNumber(int lineNumber) {
		this.lineNumber = lineNumber;
	}

	public Timer getScroller() {
		return scroller;
	}

	public void setScroller(Timer scroller) {
		this.scroller = scroller;
	}

	public Conversation loadConversation(String fileName, Runnable runnable) {
		WorldBuilder.screen.setCantWalk(true);
		Scanner scan;
		try {
			scan = new Scanner(new File("conversations\\"+fileName+".con"));
			
			this.setIdentifier(fileName);
			
			do {
				String nxt = scan.nextLine();
				
				if(!nxt.equals("%option")){
					lines.add(nxt);
				}
				
			} while (scan.hasNextLine());
			
			scan.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		endScript = runnable;
		return this;
	}

	public Runnable getEndScript() {
		return endScript;
	}

	public void setEndScript(Runnable endScript) {
		this.endScript = endScript;
	}
	
}
