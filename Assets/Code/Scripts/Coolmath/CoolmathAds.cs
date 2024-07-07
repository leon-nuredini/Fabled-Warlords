using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CoolMathAds : MonoBehaviour {


    public static CoolMathAds instance;
 
    void Awake(){
        if(instance == null){
            instance = this;
        } else if (instance != null && instance != this)
        {
            Destroy(this);
        }  
    }  


    void Start(){
        DontDestroyOnLoad(gameObject);    
    }
 
    public void PauseGame(){
        Debug.Log("PauseGame function called");
        Time.timeScale = 0f;
        AudioListener.volume = 0f;
//ADD LOGIC TO MUTE SOUND HERE
    }


    public void ResumeGame(){
        Debug.Log("ResumeGame function called");
        Time.timeScale = 1.0f;
//ADD LOGIC TO UNMUTE SOUND HERE
        AudioListener.volume = 1f;
    }
 
    	public void InitiateAds(){
      		triggerAdBreak();
    	}
	[DllImport("__Internal")]
private static extern void triggerAdBreak();
}
