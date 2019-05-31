package loz;
import java.io.*;

import javax.sound.sampled.*;
   
/**
 * This enum encapsulates all the sound effects of a game, so as to separate the sound playing
 * codes from the game codes.
 * 1. Define all your sound effect names and the associated wave file.
 * 2. To play a specific sound, simply invoke SoundEffect.SOUND_NAME.play().
 * 3. You might optionally invoke the static method SoundEffect.init() to pre-load all the
 *    sound files, so that the play is not paused while loading the file for the first time.
 * 4. You can use the static variable SoundEffect.volume to mute the sound.
 */

public enum SoundEffect {

	trident("Sounds\\mshot.wav"),
	boomerang1("Sounds\\Boomerang.wav"),
	cast("Sounds\\HoldCast.wav"),
	hurt("Sounds\\hurt.wav"),
	rupee("Sounds\\rupee.wav"),
	rupeeRegister("Sounds\\rupeeRegister.wav"),
	arrow("Sounds\\arrow.wav"),
	cane("Sounds\\mcane.wav"),
	select("Sounds\\select.wav"),
	getHeart("Sounds\\heartGet.wav"),
	HeartUp("Sounds\\heartUp.wav"),
	Beat("Sounds\\beat.wav"),
	branch("Sounds\\earrapestick.wav"),
	pickup("Sounds\\pickup.wav"),
	attack("Sounds\\sword.wav"),
	failMagic("Sounds\\mshot2.wav"), 
	slimeShot("Sounds\\sludge.wav"), 
	EnemyDie1("Sounds\\die.wav"), 
	firework("Sounds\\fireworklaunch.wav"),
	wizShot3("Sounds\\wizShot3.wav");
   
   // Nested class for specifying volume
   public static enum Volume {
      MUTE, LOW, MEDIUM, HIGH
   }
   
   public static Volume volume = Volume.HIGH;
   
   // Each sound effect has its own clip, loaded with its own sound file.
   
   private File file;
   private Clip clip;
   
   // Constructor to construct each element of the enum with its own sound file.
   SoundEffect(String soundFileName) {
	   this.file= new File(soundFileName);
      try {
         // Use URL (instead of File) to read from disk and JAR.
         //URL url = this.getClass().getClassLoader().getResource(soundFileName);
         // Set up an audio input stream piped from the sound file.
         AudioInputStream audioInputStream = AudioSystem.getAudioInputStream(this.file);
         // Get a clip resource.
         clip = AudioSystem.getClip();
         // Open audio clip and load samples from the audio input stream.
         clip.open(audioInputStream);
         
      } catch (UnsupportedAudioFileException e) {
         e.printStackTrace();
      } catch (IOException e) {
         e.printStackTrace();
      } catch (LineUnavailableException e) {
         e.printStackTrace();
      
   	  } catch (IllegalArgumentException y) {
       
    }
   }
   
   // Play or Re-play the sound effect from the beginning, by rewinding.
   public void play(boolean loop) {
	   try{
      if (volume != Volume.MUTE) {
         if (clip.isRunning())
            clip.stop();   // Stop the player if it is still running
         clip.setFramePosition(0); // rewind to the beginning
         
         //FloatControl gainControl =(FloatControl) clip.getControl(FloatControl.Type.MASTER_GAIN);
         //gainControl.setValue(2.0f); // increase volume by 6 decibels.
         
         clip.start();     // Start playing
         if(loop){
        	 clip.setLoopPoints(0, clip.getFrameLength()-1);
         	clip.loop(-1);
         }
      }
	   } catch (NullPointerException e){
		   
	   }
	   
   }
   
   
   public void play(boolean loop, float boost) {
	   try{
      if (volume != Volume.MUTE) {
         if (clip.isRunning())
            clip.stop();   // Stop the player if it is still running
         clip.setFramePosition(0); // rewind to the beginning
         
         FloatControl gainControl =(FloatControl) clip.getControl(FloatControl.Type.MASTER_GAIN);
         gainControl.setValue(boost); // increase volume by x decibels.
         
         clip.start();     // Start playing
         if(loop){
        	 clip.setLoopPoints(0, clip.getFrameLength()-1);
         	clip.loop(-1);
         }
      }
	   } catch (NullPointerException e){
		   
	   }
	   
   }
   
   // Optional static method to pre-load all the sound files.
   static void init() {
      values(); // calls the constructor for all the elements
   }

   public void stop() {
	   if (this.clip.isActive()){
		 this.clip.stop();
	   }
	}
   
   public boolean isPlaying() {
	   try{
	   if (this.clip.isActive()){
		return true;
	   }
	   return false;
	   } catch (NullPointerException e){
		 return false;  
	   } 
	}

public File getFile() {
	return file;
}

public void setFile(File file) {
	this.file = file;
}
}
