using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class GestureController : MonoBehaviour {
	
    /*-------------------------------------------------------------------------
     * Class Properties & Variables
     * ----------------------------------------------------------------------*/
	/// <summary>
	/// Gesture prefabs
	/// </summary>
	public GameObject keyTapGesturePrefab;
	public GameObject screenTapGesturePrefab;
	public GameObject circleGesturePrefab;
	public GameObject swipeGesturePrefab;
	
	/// <summary>
	/// storage for continuous events
	/// </summary> 
	private Dictionary<int, CircleGestureDisplay> circleGestures = new Dictionary<int, CircleGestureDisplay>();	
	private Dictionary<int, SwipeGestureDisplay> swipeGestures = new Dictionary<int, SwipeGestureDisplay>();
	
	
    /*-------------------------------------------------------------------------
     * Unity Lifecycle Functions
     * ----------------------------------------------------------------------*/
	
	void Start () {
		// Key / Screen Tap Discrete Events
		LeapManager.KeyTapGestureEvent += new LeapManager.KeyTapGestureHandler(OnKeyTapGesture);
		LeapManager.ScreenTapGestureEvent += new LeapManager.ScreenTapGestureHandler(OnScreenTapGesture);
		
		// Circle Continuous Events
		LeapManager.CircleGestureStartedEvent += new LeapManager.CircleGestureStartedHandler(OnCircleGestureStart);
		LeapManager.CircleGestureUpdatedEvent += new LeapManager.CircleGestureUpdatedHandler(OnCircleGestureUpdate);
		LeapManager.CircleGestureStoppedEvent += new LeapManager.CircleGestureStoppedHandler(OnCircleGestureStop);
		
		// Swipe Continuous Events
		LeapManager.SwipeGestureStartedEvent += new LeapManager.SwipeGestureStartedHandler(OnSwipeGestureStart);
		LeapManager.SwipeGestureUpdatedEvent += new LeapManager.SwipeGestureUpdatedHandler(OnSwipeGestureUpdate);
		LeapManager.SwipeGestureStoppedEvent += new LeapManager.SwipeGestureStoppedHandler(OnSwipeGestureStop);
	}
	
	
	
    /*-------------------------------------------------------------------------
     * Leap Gesture Events
     * ----------------------------------------------------------------------*/
	
	// Key Tap Event
	public void OnKeyTapGesture(KeyTapGesture g) {
		GameObject go = (GameObject) GameObject.Instantiate(keyTapGesturePrefab);
		KeyTapGestureDisplay keyTap = go.GetComponent<KeyTapGestureDisplay>();
		keyTap.gesture = g;
		//Debug.Log("OnKeyTapGesture " + g.Id);
	}
	
	
	// Screen Tap Event
	public void OnScreenTapGesture(ScreenTapGesture g) {
		GameObject go = (GameObject) GameObject.Instantiate(screenTapGesturePrefab);
		ScreenTapGestureDisplay screenTap = go.GetComponent<ScreenTapGestureDisplay>();
		screenTap.gesture = g;
		//Debug.Log("OnScreenTapGesture " + g.Id);
	}
	
	
	// Circle Lifecycle Events
	public void OnCircleGestureStart(CircleGesture g) {
		//Debug.LogWarning("Circle Start " + g.Id);
		GameObject go = (GameObject) GameObject.Instantiate(circleGesturePrefab);
		CircleGestureDisplay circle = go.GetComponent<CircleGestureDisplay>();
		circle.circleGesture = g;
		circleGestures[g.Id] = circle;
	}
	
	public void OnCircleGestureUpdate(CircleGesture g) {
		//Debug.Log("Circle Update " + g.Id);
		CircleGestureDisplay circle = circleGestures[g.Id];
		if (circle != null)
			circle.circleGesture = g;		
	}
	
	public void OnCircleGestureStop(CircleGesture g) {
		//Debug.LogError("Circle Stop " + g.Id);
		CircleGestureDisplay circle = circleGestures[g.Id];
		if (circle != null)
			circle.circleGesture = g;
		circleGestures.Remove (g.Id);		
	}
	
	
	// Swipe Lifecycle Events
	public void OnSwipeGestureStart(SwipeGesture g) {
		//Debug.LogWarning("Circle Start " + g.Id);
		GameObject go = (GameObject) GameObject.Instantiate(swipeGesturePrefab);
		SwipeGestureDisplay swipe = go.GetComponent<SwipeGestureDisplay>();
		swipe.swipeGesture = g;
		swipeGestures[g.Id] = swipe;
		
	}
	
	public void OnSwipeGestureUpdate(SwipeGesture g) {
		//Debug.Log("Circle Update " + g.Id);
		SwipeGestureDisplay swipe = swipeGestures[g.Id];
		if (swipe != null)
			swipe.swipeGesture = g;	
	}
	
	public void OnSwipeGestureStop(SwipeGesture g) {
		//Debug.LogError("Circle Stop " + g.Id);
		SwipeGestureDisplay swipe = swipeGestures[g.Id];
		if (swipe != null)
			swipe.swipeGesture = g;
		swipeGestures.Remove (g.Id);		
	}
	
}
