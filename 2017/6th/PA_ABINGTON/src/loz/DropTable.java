package loz;

public class DropTable {

	public static int generateItem(WorldItem w){
		
		Dice d = new Dice(100);
		int roll = d.roll();
		
		if(roll < 21){
			w.setScript(WorldItem.Script.ITEM_RUP1);
			w.generateScript();
			return 183;
		} else if(roll < 25){
			w.setScript(WorldItem.Script.ITEM_RUP5);
			w.generateScript();
			return 147;
		} else if(roll < 28){
			w.setScript(WorldItem.Script.ITEM_RUP20);
			w.generateScript();
			return 195;
		} else if(roll < 29){
			w.setScript(WorldItem.Script.ITEM_RUP50);
			w.generateScript();
			return 198;
		} else if(roll < 34){
			w.setScript(WorldItem.Script.Heart);
			w.generateScript();
			return 6;
		} else if(roll < 39){
			w.setScript(WorldItem.Script.element);
			w.generateScript();
			return 1022;
		} else if(roll < 90){
			
		}
		
		w.setUsed(true);
		w.setExecute(null);

		return -1;
	}
	
	public static int generateItemEnemy(WorldItem w){
		
		Dice d = new Dice(100);
		int roll = d.roll();
		
		if(roll < 21){
			w.setScript(WorldItem.Script.ITEM_RUP1);
			w.generateScript();
			return 183;
		} else if(roll < 25){
			w.setScript(WorldItem.Script.ITEM_RUP5);
			w.generateScript();
			return 147;
		} else if(roll < 45){
			w.setScript(WorldItem.Script.Heart);
			w.generateScript();
			return 6;
		} else if(roll < 90){
			//w.setScript(WorldItem.Script.wispSpawn); bugged
			//w.generateScript();
			//return 54;
		}
		
		w.setUsed(true);
		w.setExecute(null);

		return -1;
	}
	
}
