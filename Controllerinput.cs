using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllerinput : MonoBehaviour {

    public AudioClip clip;
    public AudioSource audioSource;

   

    public Transform gunBarre;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;

    }
	
	// Update is called once per frame
	void Update () {


        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            audioSource.Play();
           

           
        }
		
	}


    
}
