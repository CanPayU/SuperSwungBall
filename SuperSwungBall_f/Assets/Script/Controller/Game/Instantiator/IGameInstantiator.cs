using UnityEngine;
using System;


namespace GameScene.Instantiator {


	public interface IGameInstantiator
	{
		Type Solo { get; set;}
		Type Multi { get; set;}
		Type Replay { get; set;}
	}


}