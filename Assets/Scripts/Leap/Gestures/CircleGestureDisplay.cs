using UnityEngine;
using System.Collections;
using Leap;

public class CircleGestureDisplay : GestureDisplay {
	
	private CircleGesture _circleGesture;

	public CircleGesture circleGesture
	{
		get
		{
			return _circleGesture;
		}
		
		set
		{
			_circleGesture = value;
			gesture = value;
		}
	}
	
	public override void Start()
	{
		if (circleGesture == null)
			Destroy (this);
	}
	
	public override void Update()
	{
		base.Update();
		
		if (circleGesture == null)
			Destroy(gameObject);
		
		transform.position = circleGesture.Center.ToUnityTranslated();
		transform.localScale = circleGesture.Radius * LeapManager.instance.LeapScaling * 2;
		
		if (circleGesture.Pointable.Direction.AngleTo(circleGesture.Normal) < Mathf.PI/4)
		{
			transform.eulerAngles = new Vector3( 0, 0, circleGesture.Progress * -360);	
		}
		else
		{
			transform.eulerAngles = new Vector3( 0, 180, circleGesture.Progress * -360);
		}
		
		
	}
		
}
