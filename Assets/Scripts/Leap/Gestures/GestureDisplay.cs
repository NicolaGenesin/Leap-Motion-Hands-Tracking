using UnityEngine;
using System.Collections;

public class GestureDisplay : MonoBehaviour {
	
	public float decayTime = 2.0f;
	public Color color = Color.white;
	
	public Leap.Gesture gesture;
	
	private bool stopped;
	private float stopTime;
	
	
    /*-------------------------------------------------------------------------
     * Unity Lifecycle Functions
     * ----------------------------------------------------------------------*/
	
	public virtual void Start() {
		if (gesture == null) Destroy(gameObject);
	}
	
	public virtual void Update() {
		if (gesture == null) Destroy(gameObject);
		
		// Fade out and destroy gesture display after decay time
		if (gesture.State == Leap.Gesture.GestureState.STATESTOP)
		{
			if (!stopped)
			{
				stopped = true;
				stopTime = Time.time;
			}
			
			float decayLevel = Mathf.Lerp(1,0, (Time.time - stopTime)/decayTime);
			gameObject.renderer.material.SetColor(
				"_Color",
				new Color(color.r, color.g, color.b, decayLevel));
			
			if (Time.time > stopTime + decayTime)
			{
				Destroy(gameObject);
			}
		}
		else {
			gameObject.renderer.material.SetColor(
				"_Color",
				new Color(color.r, color.g, color.b, 1));
		}
		
	}
}
