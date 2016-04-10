//	Created by CanPayU on 08/12/2015.
//	Copyright © 2016 CanPayU. All rights reserved.
//	-------------------------------------------------------------------------------------
//	|									 ::::::                                   		|
//	|								`.-:::::::::::-.`                              		|
//	|							  .-:::::::::::::::::::-.                           	|
//	|							.:::::::::::::::::::::::::.                         	|
//	|						  .:::::::::::::::::::::::::::::`                       	|
//	|						.::::::::::::::::::::::::::::::::.                      	|
//	|					  .::::::::::::::/+o+osso/::::::::::::`                     	|
//	|					.:::::::::::::::.`     ./yy/:::::::::::   `                 	|
//	|				  .:::::::::::::::.           +h+::::::::::.-+:-`               	|
//	|				.:::::::::::::::.              ss::::::::::.o+:::-`             	|
//	|			  .+yo::::::::::::::-`             +o::::::::::-y::::::-`           	|
//	|			.::::oyo::::::::::::::-`          `o:::::::::::/s::::::::.          	|
//	|		  .::::::::oyo::::::::::::::-`       .::::::::::::-o+::::::::::.        	|
//	|		.::::::::::::oyo::::::::::::::-`   .:::::::::::::-`y:::::::::::::.      	|
//	|	   ./::::::::::::::-syo::::::::::::::..-:::::::::::::- :yo:::::::::::::-`    	|
//	|	  :+:::::::::::::.   -syo::::::::::::::::::::::::::-`   -yy+:::::::::::::    	|
//	|	 -o::::::::::::.       -syo::::::::::::::::::::::-`       :yy+:::::::::::-   	|
//	|	 y/::::::::::.           -syo:::::::::::::::::::.          `/ys:::::::::::`  	|
//	|	-y::::::::::-              -syo:::::::::::::::.              `yo::::::::::.  	|
//	|	:y::::::::::.               `/+yo::::::::::::::.              +o::::::::::.  	|
//	|	:y:::::::::::             .-::::+yo::::::::::::::.            o/::::::::::`  	|
//	|	.h+:::::::::::`         .:::::::::+yo::::::::::::::.         -/:::::::::::   	|
//	|	 oy:::::::::::::-.````.:::::::::::::+yo::::::::::::::-.```..:::::::::::::`   	|
//	|	 `ys::::::::::::::::::::::::::::::::.-syo:::::::::::::::::::::::::::::::`    	|
//	|	  .yy/::::::::::::::::::::::::::::.    -sy+::::::::::::::::::::::::::::`     	|
//	|	   `+yo/::::::::::::::::::::::::-        -sy+::::::::::::::::::::::::.       	|
//	|		  .oys+::::::::::::::::::::.            -syo/::::::::::::::::::/-         	|
//	|			`:oyso+/:::::::::/++:.                -+yyo+/:::::::::/+/:.           	|
//	|				.-/+oooso+++/-.                      `-:+ooooo++/:. 				|
//	-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Standing
{
    public class MainController : MonoBehaviour
    {
		[SerializeField] private GameObject Press_To_Start;
		[SerializeField] private GameObject Authentication;

		private bool authenticate;

        // Use this for initialization
        void Start()
        {
			//SaveLoad.save_user ();
			//SaveLoad.save_setting ();
			SaveLoad.load_settings ();
			this.authenticate = SaveLoad.load_user ();

			Debug.Log (User.Instance);
			Debug.Log (User.Instance.Friends);
        }

        // Update is called once per frame
        void Update()
        {
			if (Input.anyKey) {
				//return;
				if (authenticate)
					FadingManager.I.Fade ();
				else {
					Press_To_Start.SetActive (false);
					Authentication.SetActive (true);
				}
			}
        }
    }
}

