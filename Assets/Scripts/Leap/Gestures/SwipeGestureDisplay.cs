using UnityEngine;
using System.Collections;
using Leap;

public class SwipeGestureDisplay : GestureDisplay {
	
	public GameObject swipePointPrefab;
	private GestureDisplay point1;
	private GestureDisplay point2;
	
	private SwipeGesture _swipeGesture;

	public SwipeGesture swipeGesture
	{
		get
		{
			return _swipeGesture;
		}
		
		set
		{
			_swipeGesture = value;
			gesture = value;
			if (point1 != null) point1.gesture = value; // needs to decay			
			if (point2 != null) point2.gesture = value; // needs to decay			
		}
	}
	
	public override void Start()
	{
		if (swipeGesture == null)
			Destroy (this);
		
		// Create initial point indicator
		GameObject go = (GameObject) GameObject.Instantiate(swipePointPrefab);
		go.transform.position = _swipeGesture.StartPosition.ToUnityTranslated();
		point1 = go.GetComponent<GestureDisplay>();
		point1.gesture = _swipeGesture;
		point1.transform.position = _swipeGesture.StartPosition.ToUnityTranslated();
	}
	
	public override void Update()
	{
		base.Update();
		
		if (_swipeGesture == null)
			Destroy(gameObject);
		
		Vector3 startPosition = _swipeGesture.StartPosition.ToUnityTranslated();
		Vector3 endPosition = _swipeGesture.Position.ToUnityTranslated();
		Vector3 offset = endPosition - startPosition;
		
		transform.localScale = new Vector3(offset.magnitude, 1, 1);
		transform.right = offset;
		transform.position = startPosition + offset/2;
		
		// Create end point on initial swipe completion
		if (_swipeGesture.State == Gesture.GestureState.STATESTOP && point2 == null) {
			GameObject go = (GameObject) GameObject.Instantiate(swipePointPrefab);
			point2 = go.GetComponent<GestureDisplay>();
			point2.gesture = _swipeGesture;
			point2.transform.position = endPosition;
		}
			
		//transform.position = circleGesture.Center.ToUnityTranslated();
		//transform.localScale = circleGesture.Radius * LeapManager.instance.LeapScaling * 2;
	}
}