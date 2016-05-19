---
title:Chrysler Carnage
description: Car Crash Game
Programmers: Alejandro Saura Villanueva,Tin-Tin Chin,Eleni Paskali,Sokol Murturi
Artists:     Mikko Torvinen, Rob Allen
tags: master, project Unity 3D framework
created: 2016 February 28
modified:In progress
---
##Introduction

In the team my part was to create the GUI Window, the sound effects for the cars, the environment and the scripting for the sounds.

##Sounds Creation
 
 Due to my background as a musician for twelve years, i started creating music through piano to find the right melody for our main theme of the game. A first recording was at the university in  a room with piano. After being satisfied from the melody i asked a friend to import based on the sound the guitar. I took the final files editing in the beginning with logic and i merged them with audacity where i edited the most part of the soundtrack.

The idea of the team was to have a sound based on the song of Led Zeppelin-Black Dog and Rock and Roll. The outcome was very satisfied so i continued with the environment sound. The idea was to create rock music very fast to make you feel the anxiety and the rush of the game during the game. By the same way i converted part of the piano and the guitar with audacity by changing the pitch of the music organs to have a more electronic outcome. The main sound is attached in the GUI Window and the Environment sound is attached in the main camera of the body of the cars. 


The small sound effects were created in real time by recording the sound and edited in the music program called Audacity. Some of them were made and by me for example the sound bump of the car in the beginning was created by hitting the laptop with my hand and recording that sound.

The main script for the audio when the car is accelerating: 

using System;
using UnityEngine;
using Random = UnityEngine.Random;

    [RequireComponent(typeof(CarPhysicsController))]
    public class CarAudio : MonoBehaviour
    {

        public AudioClip AccelClip;
 
        public float pitchMultiplier = 1f;
        public float lowPitchMin = 1f;
        public float lowPitchMax = 6f;
        public float highPitchMultiplier = 0.25f;
        public float maxRolloffDistance = 500;
        public float dopplerLevel = 1;
        public bool useDoppler = true;

        private AudioSource m_HighAccel;

        private bool m_StartedSound;
        private CarPhysicsController m_CarController;


        private void StartSound()
        {
            m_CarController = GetComponent<CarPhysicsController>();
            m_HighAccel = SetUpEngineAudioSource(AccelClip);
            m_StartedSound = true;
        }


        private void StopSound()
        {
            foreach (var source in GetComponents<AudioSource>())
            {
                Destroy(source);
            }

            m_StartedSound = false;
        }

        private void Update()
        {
            // get the distance to main camera
            float camDist = (Camera.main.transform.position -     transform.position).sqrMagnitude;

            // stop sound if the object is beyond the maximum roll off distance
            if (m_StartedSound && camDist > maxRolloffDistance * maxRolloffDistance)
            {
                StopSound();
            }

            // start the sound if not playing and it is nearer than the maximum distance
            if (!m_StartedSound && camDist < maxRolloffDistance * maxRolloffDistance)
            {
                StartSound();
            }

            if (m_StartedSound)
            {
                // The pitch is interpolated between the min and max values
                float pitch = ULerp(lowPitchMin, lowPitchMax, 0);
                // The pitch is interpolated between the min and max values, according to the car's revs.
                //float pitch = ULerp(lowPitchMin, lowPitchMax, m_CarController.Revs);

                // clamp to minimum pitch (note, not clamped to max for high revs while burning out)
                pitch = Mathf.Min(lowPitchMax, pitch);

                    m_HighAccel.pitch = pitch * pitchMultiplier * highPitchMultiplier;
                    m_HighAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                    m_HighAccel.volume = 1;

            }
        }

        private AudioSource SetUpEngineAudioSource(AudioClip clip)
        {
            // create the new audio source component on the game object and set up its properties
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = 0;
            source.loop = true;

            // start the clip from a random point
            source.time = Random.Range(0f, clip.length);
            source.Play();
            source.minDistance = 5;
            source.maxDistance = maxRolloffDistance;
            source.dopplerLevel = 0;
            return source;
        }


        // unclamped versions of Lerp and Inverse Lerp, to allow value to exceed the from-to range
        private static float ULerp(float from, float to, float value)
        {
            return (1.0f - value) * from + value * to;
        }
    }

The main script for the car when colliding (bump sound):

using UnityEngine;
using System.Collections;

public class crashsounder : MonoBehaviour {


    public AudioClip crashSound;
    private AudioSource source;
    
    void Awake (){

        source = GetComponent<AudioSource>();

    }

    void OnCollisionEnter(Collision collision)
    {
       
            source.PlayOneShot(crashSound);
    }
##Health bar

All the tiles were created in Photoshop and imported to Unity 3D. The idea of the tiles was to be minimal but to fit also with the environment of the game. The health bar is attached to the body of the car and the scripting is that every time the car is being damaged the health bar is going to reduce.
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarScript : MonoBehaviour {

	public RectTransform healthTransform;
	private float yPos;
	private float maxXValue; // 100% health = startPos - barLength
	private float minXValue;
	
	private int currentHealth;
	private float currentXValue;
	
	private int CurrentHealth {
		get {return currentHealth;}
		set {
			currentHealth = value;
			HandleHealth();
		}
	}
	
	public int maxHealth;
	
	public Text healthText;
	
	// access
	public Image visualHealth;
	
	public float coolDown;
	private bool onCoolDown;


	// Use this for initialization
	void Start () {
		onCoolDown = false;
		
		yPos = healthTransform.position.y;
		maxXValue = healthTransform.position.x;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void HandleHealth() {
		healthText.text = "Health: " + currentHealth;
		currentXValue = MapValues(currentHealth, 0, maxHealth, minXValue, maxXValue);
		
		// Transform health bar into correct osition
		healthTransform.position = new Vector3(currentXValue, yPos); 
		
		// If > 50% health, then transform R
		if (currentHealth > maxHealth / 2) {
			visualHealth.color = new Color32((byte)MapValues(currentHealth, maxHealth/2, maxHealth, 255, 0), 255, 0, 255);
			
		} 
		// If < 50% health, then transform G
		else {
			visualHealth.color = new Color32(255, (byte)MapValues(currentHealth, 0, maxHealth/2, 0, 255), 0, 255);
		}
	}
	
	IEnumerator CoolDownDamage() {
		onCoolDown = true;
		yield return new WaitForSeconds(coolDown);
		onCoolDown = false;
	}

	void OnTriggerStay(Collider other) {
		Debug.Log("Collided");
		if (other.gameObject.tag == "Damage") {
			if(!onCoolDown && currentHealth >= 1) {
				StartCoroutine(CoolDownDamage());
				CurrentHealth -= 1;
				Debug.Log("Damage");
			}
		}
		
		if (other.gameObject.tag == "Health") {
			if(!onCoolDown && currentHealth < maxHealth) {
				StartCoroutine(CoolDownDamage());
				CurrentHealth += 1;
				Debug.Log("Health");
			}
		}
		Debug.Log("Done colliding");
	}
	
	
	// health   0   50  100
	// posX     -10 -5  0
	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax) {
		return ((x - inMin) * (outMax - outMin)) / (inMax - inMin) + outMin;
		
	}
}

##Gears

After creating the scripting for the gears I had to put the sound that every time you change a gear you listen the sound:
void shiftGear()
    {
        float meanSlipRatio = Mathf.Abs(frontLeftWheel.slipRatio + frontRightWheel.slipRatio + backLeftWheel.slipRatio + backRightWheel.slipRatio) / 4;

        switch (currentGear)
        {
            case 0:

                break;
            case 1:
                engineRPM = Mathf.Clamp(rawEngineRPM * meanSlipRatio, minRPM, maxRPM);                
                if (rawEngineRPM > gearThresholds[1][1])
                {                    
                    currentGear = 2;
                    source.PlayOneShot(gears);
                }
                    break;
            case 2:
                engineRPM = Mathf.Clamp((rawEngineRPM - gearThresholds[2][0] + 1000) * meanSlipRatio, minRPM, maxRPM) ;
                if (rawEngineRPM > gearThresholds[2][1])
                {                    
                    currentGear = 3;
                    source.PlayOneShot(gears);
                }
                if (rawEngineRPM < gearThresholds[2][0])
                {                    
                    currentGear = 1;
                    source.PlayOneShot(gears);
                }
                break;
            case 3:
                engineRPM = Mathf.Clamp((rawEngineRPM - gearThresholds[3][0] + 1000) * meanSlipRatio, minRPM, maxRPM);
                if (rawEngineRPM > gearThresholds[3][1])
                {                    
                    currentGear = 4;
                    source.PlayOneShot(gears);
                }
                if (rawEngineRPM < gearThresholds[3][0])
                {                    
                    currentGear = 2;
                    source.PlayOneShot(gears);
                }
                break;
            case 4:
                engineRPM = Mathf.Clamp((rawEngineRPM - gearThresholds[4][0] + 1000) * meanSlipRatio, minRPM, maxRPM);
                if (rawEngineRPM > gearThresholds[4][1])
                {
                    currentGear = 5;
                    source.PlayOneShot(gears);
                }
                if (rawEngineRPM < gearThresholds[4][0])
                {
                    currentGear = 3;
                    source.PlayOneShot(gears);
                }
                break;
            case 5:
                engineRPM = Mathf.Clamp((rawEngineRPM - gearThresholds[5][0] + 1000) * meanSlipRatio, minRPM, maxRPM);
                if (rawEngineRPM > gearThresholds[5][1])
                {
                    currentGear = 6;
                    source.PlayOneShot(gears);
                }
                if (rawEngineRPM < gearThresholds[5][0])
                {
                    currentGear = 4;
                    source.PlayOneShot(gears);
                }
                break;
            case 6:
                engineRPM = Mathf.Clamp((rawEngineRPM - gearThresholds[6][0] + 1000) * meanSlipRatio, minRPM, maxRPM);
                if (rawEngineRPM < gearThresholds[6][0])
                {
                    currentGear = 5;
                    source.PlayOneShot(gears);
                }                
                break;


        }    
    }
        
}



##GUI  Window

First I created the Canvas by importing the Main menu tiles by title , animated the tiles so every time you want to click a tile they either hover or moving in the space. The main scripting for the Panel: 
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour {

	public Animator initiallyOpen;

	private int m_OpenParameterId;
	private Animator m_Open;
	private GameObject m_PreviouslySelected;

	const string k_OpenTransitionName = "Open";
	const string k_ClosedStateName = "Closed";

	public void OnEnable()
	{
		m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);

		if (initiallyOpen == null)
			return;

		OpenPanel(initiallyOpen);
	}

	public void OpenPanel (Animator anim)
	{
		if (m_Open == anim)
			return;

		anim.gameObject.SetActive(true);
		var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;

		anim.transform.SetAsLastSibling();

		CloseCurrent();

		m_PreviouslySelected = newPreviouslySelected;

		m_Open = anim;
		m_Open.SetBool(m_OpenParameterId, true);

		GameObject go = FindFirstEnabledSelectable(anim.gameObject);

		SetSelected(go);
	}

	static GameObject FindFirstEnabledSelectable (GameObject gameObject)
	{
		GameObject go = null;
		var selectables = gameObject.GetComponentsInChildren<Selectable> (true);
		foreach (var selectable in selectables) {
			if (selectable.IsActive () && selectable.IsInteractable ()) {
				go = selectable.gameObject;
				break;
			}
		}
		return go;
	}

	public void CloseCurrent()
	{
		if (m_Open == null)
			return;

		m_Open.SetBool(m_OpenParameterId, false);
		SetSelected(m_PreviouslySelected);
		StartCoroutine(DisablePanelDeleyed(m_Open));
		m_Open = null;
	}

	IEnumerator DisablePanelDeleyed(Animator anim)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		while (!closedStateReached && wantToClose)
		{
			if (!anim.IsInTransition(0))
				closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

			wantToClose = !anim.GetBool(m_OpenParameterId);

			yield return new WaitForEndOfFrame();
		}

		if (wantToClose)
			anim.gameObject.SetActive(false);
	}

	private void SetSelected(GameObject go)
	{
		EventSystem.current.SetSelectedGameObject(go);
	}
}


The tiles are connecting with a test_button script where on click you are being send to the wanted action :

using UnityEngine;
using System.Collections;

public class TestButton : MonoBehaviour {
  

	public void Play () 
    {
        Application.LoadLevel(1);
    }

    public void Continue()
    {
        Application.LoadLevel(1);
    }
	
	public void Quit () 
    {
        Application.Quit();
	}

    public void NewGame()
    {
        Application.LoadLevel(1);
    }

    public void Credits()
    {
        // show credits
        Application.LoadLevel(2);
        //var credits = GameObject.Find("Cred").transform;
       // credits.position = new Vector3(400, credits.position.y, 0);
    }
}

I created the animation of one of the models of the car in the Main Menu. You can see the car rotating in the window. The idea was to show to the players the cars that they will control during the game.

The credits picture was edited in Photoshop and imported in Unity 3D.


##Problems I faced
In the beginning I found problems to make the gears when shifting to work properly when changing from scale one to six and playing the sound correctly. This was overcome by asking the team about their work and what  I am doing wrong. The outcome was that in the Unity 3D the script  was attached in a different position to the body of the car.

Another problem faced was when the car is colliding. Every time the car is colliding with the environment there was the sound of car crash playing although it was not colliding with the car. After noticing I changed the sound when colliding with the environment and when colliding with the cars.
One more was to import the mesh of the cars in order to put them in the rotator window.  In the drop box files were the artist keep their files I was trying to look for the mesh of the cars but it was not working properly. This I will have to discuss with the artist and show them the problem I am facing in order to make it work by the end of the month.
Last the health bar is not attached correctly as the scripting is not working during the damage of the car. This problem I will try to solve it when we will have merged all the files and ask help from the rest of the programmers.

##Future changes
In the month we have I would like to import both of the updated model cars in the Gui Window and when selecting to read the possibilities they have. Moreover the background of the Gui Window I want to import all the pictures of the cities that were created from our artists and displaying like a movie.
In the main body I want to import more Gui features like a speed meter while racing.

##Conclusion
I would like to thank all my team as it has been a great corporation by communicating and trying to solve all the problems. Bigger thanks to Alejandro who did a great job by running the team and helping us with his knowledge.


##Car Crash Game







