/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2013.                                   *
* Leap Motion proprietary and  confidential.  Not for distribution.            *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement between *
* Leap Motion and you, your company or other organization.                     *
\******************************************************************************/
using UnityEngine;
using System.Collections;

//This relatively simple classis added to fingertip objects by the LeapUnityBridge,
//which allows our LeapSelectionController to be notified when a finger collides with any
//object tagged as 'Touchable'
public class LeapFingerCollisionDispatcher : MonoBehaviour {
	
	const float kHitDistance = 20.0f;
	
	void OnTriggerEnter(Collider other)
	{		
		if( other.tag == "Touchable" )
		{
			LeapUnitySelectionController.Get().OnTouched(gameObject, other);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if( other.tag == "Touchable" )
		{
			LeapUnitySelectionController.Get().OnStoppedTouching(gameObject, other);	
		}
	}
	
	void FixedUpdate()
	{
		if( gameObject.GetComponent<Collider>().enabled )
		{
			Debug.DrawRay(transform.position, transform.forward, Color.green);
			RaycastHit hit;
			if( Physics.Raycast(transform.position, transform.forward, out hit, 20.0f) )
			{
				LeapUnitySelectionController.Get().OnRayHit(hit);	
			}
		}
	}
}