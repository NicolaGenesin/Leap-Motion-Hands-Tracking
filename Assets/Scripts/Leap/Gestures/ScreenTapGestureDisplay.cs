using UnityEngine;
using System.Collections;
using Leap;

public class ScreenTapGestureDisplay : GestureDisplay {
	
	private ScreenTapGesture screenTapGesture;
	
	public override void Start()
	{
		if (gesture == null || gesture.Type != Gesture.GestureType.TYPESCREENTAP)
			Destroy (this);
		else
		{
			screenTapGesture = new ScreenTapGesture(gesture); 
			transform.position = screenTapGesture.Position.ToUnityTranslated();
		}
	}
	
}
