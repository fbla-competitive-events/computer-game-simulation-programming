package eui;

import java.awt.Dimension;
import java.awt.GridLayout;
import java.awt.Rectangle;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JTextField;

import loz.Conversation;

public class ConversationTool {

	public static JFrame builder = new JFrame();
	public static ArrayList<JTextField> lineInputs = new ArrayList<JTextField>();
	public static ArrayList<JButton> buttons = new ArrayList<JButton>();
	public static int lineNumber = 0;
	
	public static Conversation build = new Conversation();
	
	public static void main(String[] args) { // make conversations to save as .con files (NOT IN USE IN FBLA BUILD)
		
		init();
		
		initButtons();
		
		lineInputs.add(new JTextField("INITIAL LINE - 0"));

		refresh();
		
	}
	
	public static void init(){
		
		builder.setSize(new Dimension(800,800));
		builder.setVisible(true);
		builder.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		builder.setTitle("Converstion Builder - 0");
		builder.setLayout(new GridLayout(2,2));
		
	}

	public static void initButtons(){
		JButton ls0 = new JButton("-1");
		ls0.addActionListener(new ActionListener(){
			@Override
			public void actionPerformed(ActionEvent arg0) {
				if(lineNumber>0){
					lineNumber--;
					refresh();
				}
			}
		});
		
		JButton ls1 = new JButton("+1");
		ls1.addActionListener(new ActionListener(){
			@Override
			public void actionPerformed(ActionEvent arg0) {
				if(lineNumber<lineInputs.size()-1){
					lineNumber++;
					refresh();
				}
			}
		});
		
		JButton save = new JButton("SAVE");
		save.addActionListener(new ActionListener(){
			@Override
			public void actionPerformed(ActionEvent arg0) {
				build.saveConversation(build.getIdentifier());
			}
		});
		
		JButton add = new JButton("Add Line");
		add.addActionListener(new ActionListener(){
			@Override
			public void actionPerformed(ActionEvent arg0) {
				System.out.println(lineInputs.size());
				lineInputs.add(new JTextField("Auto generated - Line: "+(lineInputs.size())));
				refresh();
			}
		});
		
		buttons.add(save);
		buttons.add(add);
		buttons.add(ls0);
		buttons.add(ls1);

		
	}
	
	public static ArrayList<String> getLines(){
		
		ArrayList<String> ret = new ArrayList<String>();
		
		for(JTextField j : lineInputs){
			ret.add(j.getText());
		}
		
		return ret;
	}
	
	public static void refresh(){
		
		if(getLines() != null && build != null){
			build.setLines(getLines());
			build.setIdentifier("TEMP");
		}
		
		Rectangle old = builder.getBounds();
		builder.dispose();
		builder = new JFrame();
		builder.setBounds(old);
		init();
		
		for(int i = 0; i<builder.getComponentCount(); i++){
			if(builder.getComponent(i) instanceof JTextField){
				builder.remove(builder.getComponent(i));
			}
			if(builder.getComponent(i) instanceof JButton){
				builder.remove(builder.getComponent(i));
			}
		}
		int ic = -1;
		for(int i = 0; i<lineInputs.size(); i++){
			ic++;
			System.out.println(lineInputs.size()+" - "+ic+" -- "+lineNumber);
			if(ic == lineNumber){
				lineInputs.get(i).setHorizontalAlignment(JTextField.CENTER);
				builder.add(lineInputs.get(i));
				System.out.println("ADDED - "+" --> At: "+ic);
				break;
			}
		}
		for(JButton f : buttons){
			builder.add(f);
		}
		
		builder.setTitle("Converstion Builder - "+lineNumber);
		builder.repaint();
		builder.revalidate();
	}
	
}
