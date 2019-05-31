package smWorld;

import java.awt.Dimension;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.Toolkit;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.Timer;
import java.util.TimerTask;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JSlider;
import javax.swing.JTextField;
import javax.swing.WindowConstants;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;

public class Mapper {

	public static JFrame frame = new JFrame("Map Maker");
	public static int msize = 8;
	public static Window inner = new Window();
	public static Dimension ScreenSize = Toolkit.getDefaultToolkit().getScreenSize();
	public static String output = "test";
	
	public static void main(String[] args) {
		
		frame.setSize(500, 300);
		frame.setVisible(true);
		frame.setLayout(new GridLayout(3,2));
		frame.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
		
		final JSlider size = new JSlider(0);
		final JTextField ss = new JTextField();
		size.setPaintTicks(true);
		size.setMajorTickSpacing(10);
		size.setMinorTickSpacing(1);
		size.setValue(40);
		size.setMaximum(200);
		size.setMinimum(1);
		ss.setText("Map Size: "+size.getValue()+" squared Tiles.");
		ss.setEditable(false);
		ss.setFocusable(false);
		ss.setOpaque(false);
		ss.setFont(new Font("TimesRoman",Font.BOLD,22));
		ss.setHorizontalAlignment(JTextField.CENTER);
		frame.add(ss);
		
		size.addChangeListener(new ChangeListener(){

			public void stateChanged(ChangeEvent arg0) {
				ss.setText("Map Size: "+size.getValue()+" squared Tiles - "+(size.getValue()*64)+" x" +(size.getValue()*64)+" Pixels");
			}
			
		});
		
		JButton adds = new JButton("Initialize Map");
		adds.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent arg0) {
				msize = size.getValue();
				startEditor(msize);
			}
		});
		
		frame.add(adds);
		frame.add(size);
		frame.repaint();
		frame.revalidate();
		
		startEditor(50);
		
	}
	
	public static void startEditor(int size){
		freeFrame();
		inner.setBuilderShow(true);
		inner.setMsize(size);
	}
	
	public static void freeFrame(){
		frame.dispose();
		
		JFrame F = new JFrame();
		F.add(inner);
		F.setUndecorated(true);
	    F.setSize(ScreenSize);
		F.setVisible(true);
		F.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
		Timer r = new Timer();
		r.schedule(new TimerTask(){
			@Override
			public void run() {
				F.repaint();
			}
		}, 10,10);
		
	}

}
