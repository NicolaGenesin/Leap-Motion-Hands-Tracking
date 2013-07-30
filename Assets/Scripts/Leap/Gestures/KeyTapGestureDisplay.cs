using UnityEngine;
using System.Collections;
using Leap;

public class KeyTapGestureDisplay : GestureDisplay {
	
	//TODO: take into account key tap object in Gesture Controller
	
	private KeyTapGesture keyTapGesture;
	
	public override void Start()
	{
		if (gesture == null || gesture.Type != Gesture.GestureType.TYPEKEYTAP)
			Destroy (this);
		else
		{
			keyTapGesture = new KeyTapGesture(gesture); 
			transform.position = keyTapGesture.Position.ToUnityTranslated();
		}
	}
	
}
