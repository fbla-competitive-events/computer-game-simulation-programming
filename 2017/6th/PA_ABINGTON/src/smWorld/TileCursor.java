package smWorld;

import java.awt.MouseInfo;

import javax.swing.ImageIcon;

public class TileCursor { // Detects which tile the cursor is currently over
	
	private ImageIcon img = new ImageIcon("Images\\select.png");
	private Sprite link;
	
	public TileCursor(Sprite s){
		link = s;
	}
	
	public int getX(int tx){
		int x = MouseInfo.getPointerInfo().getLocation().x/4;
		link.setX(16*(Math.round(x/16)));
		return 16*(Math.round(x/16))- 16*tx;
	}
	
	public int getY(int ty){
		int y = MouseInfo.getPointerInfo().getLocation().y/4;
		link.setY(16*(Math.round(y/16)));
		return 16*(Math.round(y/16))- 16*ty;
	}

	public ImageIcon getImg() {
		return img;
	}

	public void setImg(ImageIcon img) {
		this.img = img;
	}

	public Sprite getLink() {
		return link;
	}

	public void setLink(Sprite link) {
		this.link = link;
	}
	
}
