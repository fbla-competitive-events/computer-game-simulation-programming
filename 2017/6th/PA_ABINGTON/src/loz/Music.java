package loz;

import java.io.File;
import java.io.IOException;
import java.util.Timer;
import javax.sound.sampled.AudioInputStream;
import javax.sound.sampled.AudioSystem;
import javax.sound.sampled.Clip;
import javax.sound.sampled.FloatControl;
import javax.sound.sampled.LineUnavailableException;
import javax.sound.sampled.UnsupportedAudioFileException;

public class Music
{
  public boolean loop = true;
  public static boolean playing = false, disableSound = false;
  public static Music activeMusic = new Music("Music\\Outlook.wav");
  public static int recentMusic;
  private File file;
  private Clip clip;
  
  public static String[] musicURL = {
		  "Music\\BlueOyster.wav", 					// 0
		  "Music\\Outlook.wav", 					// 1
		  "Music\\Cusp of Light.wav", 				// 2
		  "Music\\Outlook.mid", 					// 3
		  "Music\\Soul Storm.mid", 					// 4
		  "Music\\voyage.mid", 						// 5
		  "Music\\Elementarium.mid",				// 6
		  "Music\\ElectricZoo.wav",					// 7
		  "Music\\Corrupted Cathedral.wav",			// 8
		  "Music\\DeadNight.wav",					// 9
		  "Music\\Elementarium.wav",				// 10
		  "Music\\seventhStep.wav",					// 11
		  "Music\\Home.wav",						// 12
		  "Music\\Vigil.wav",						// 13
		  "Music\\Mountain People.wav",				// 14
		  "Music\\Defiler.wav",						// 15
    };
  
  public Music(String soundFileName){
    this.file = new File(soundFileName);
    try{
      AudioInputStream audioInputStream = AudioSystem.getAudioInputStream(this.file);
      if(activeMusic!=null && activeMusic.getClip()!=null)
    	  activeMusic.getClip().close();
      this.clip = AudioSystem.getClip();
      
      this.clip.open(audioInputStream);
    }
    catch (UnsupportedAudioFileException e){
      e.printStackTrace();
    }
    catch (IOException e){
      e.printStackTrace();
    }
    catch (LineUnavailableException e){
      e.printStackTrace();
    }
    catch (IllegalArgumentException localIllegalArgumentException) {}
  }
  
  public void reinitialize(String soundFileName){
	  this.clip.close();
	  this.file = new File(soundFileName);
	    try{
	      AudioInputStream audioInputStream = AudioSystem.getAudioInputStream(this.file);
	      
	      this.clip = AudioSystem.getClip();
	      
	      this.clip.open(audioInputStream);
	    }
	    catch (UnsupportedAudioFileException | IOException | LineUnavailableException e){
	      e.printStackTrace();
	    }
  }
  
  public static enum Volume{
    MUTE,  LOW,  MEDIUM,  HIGH;
  }
  
  public static Volume volume = Volume.LOW;
  
  public void play(){
    try{
      if (volume != Volume.MUTE) {
        if (this.clip.isRunning()) {
          this.clip.stop();
        }
        
        FloatControl gainControl =(FloatControl) clip.getControl(FloatControl.Type.MASTER_GAIN);
        gainControl.setValue(-1.0f);
        
        this.clip.setFramePosition(0);
        this.clip.start();
   	 	clip.setLoopPoints(0, clip.getFrameLength()-1);
    	clip.loop(-1);
      }
    }
    catch (NullPointerException localNullPointerException) {}
  }
  
  public void playMusic(int i){
	  WorldBuilder.task.cancel();
	  WorldBuilder.task = new Timer();
	  activeMusic.stop();
      activeMusic = new Music(musicURL[i]);
      playing = true;
      activeMusic.play();
  }
  
  public void stop(){
	  if (!disableSound){
	  try{
		  if (this.clip.isActive()) {
			  this.clip.stop();
		  }
	  } catch(NullPointerException g){
		  System.err.println("No Sound");
		  disableSound = true;
	  	}
	  }
  }
  
  public boolean isPlaying(){
    try{
      if (this.clip.isActive()) {
        return true;
      }
      playing = false;
      return false;
    }
    catch (NullPointerException e) {}
    return false;
  }
  
  public File getFile(){
    return this.file;
  }
  
  public void setFile(File file){
    this.file = file;
  }

public boolean isLoop() {
	return loop;
}

public void setLoop(boolean loop) {
	this.loop = loop;
}

public static Music getActiveMusic() {
	return activeMusic;
}

public static void setActiveMusic(Music activeMusic) {
	Music.activeMusic = activeMusic;
}

public static int getRecentMusic() {
	return recentMusic;
}

public static void setRecentMusic(int recentMusic) {
	Music.recentMusic = recentMusic;
}

public Clip getClip() {
	return clip;
}

public void setClip(Clip clip) {
	this.clip = clip;
}

public static String[] getMusicURL() {
	return musicURL;
}

public static void setMusicURL(String[] musicURL) {
	Music.musicURL = musicURL;
}

public static Volume getVolume() {
	return volume;
}

public static void setVolume(Volume volume) {
	Music.volume = volume;
}

public static void setPlaying(boolean playing) {
	Music.playing = playing;
}
  
}
