using UnityEngine;
using System.Collections;

public class CameraLeap : MonoBehaviour {
	
	public Leap.Controller controller;
	public Leap.Listener listener;
	public Leap.Frame guiFrame;
	public string globalInfo;
	public string old_globalInfo;
	
	// Use this for initialization
	void Start () {
		
		print("Inizializzo controller");
		listener = new Leap.Listener();
		controller = new Leap.Controller();
		controller.AddListener(listener);
        controller.EnableGesture(Leap.Gesture.GestureType.TYPECIRCLE);
        controller.EnableGesture(Leap.Gesture.GestureType.TYPEKEYTAP);
        controller.EnableGesture(Leap.Gesture.GestureType.TYPESCREENTAP);
        controller.EnableGesture(Leap.Gesture.GestureType.TYPESWIPE);
	}
	
	// Update is called once per frame
	void Update () {
		Leap.Frame frame = controller.Frame();
		guiFrame=frame;

		if (!frame.Hands.Empty)
        {
            // Get the first hand
            Leap.Hand hand = frame.Hands[0];

            // Check if the hand has any fingers
            Leap.FingerList fingers = hand.Fingers;
			
            if (!fingers.Empty && !frame.Hands[1].IsValid)
            {
				Leap.Finger firstfinger = fingers[0];
										
				float moveInputX = firstfinger.TipVelocity.x /1000; 
		        transform.position += new Vector3(moveInputX, 0, 0);
				
				float moveInputY = firstfinger.TipVelocity.y /1000; 
		        transform.position += new Vector3(0, moveInputY, 0);
			
				float moveInputZ = firstfinger.TipVelocity.z /1000; 
		        transform.position -= new Vector3(0, 0 , moveInputZ);
					
                // Calculate the hand's average finger tip position
                Leap.Vector avgPos = Leap.Vector.Zero;
                foreach (Leap.Finger finger in fingers)
                {
                    avgPos += finger.TipPosition;
                }
                avgPos /= fingers.Count;
                print("Hand has " + fingers.Count
                            + " fingers, average finger tip position: " + avgPos);
            }
			
			if (frame.Hands[1].IsValid){
				Leap.Hand handSx = frame.Hands[1];
				
				if (handSx.Fingers.Count==4) transform.position = new Vector3(0,50,-50);
				
			}

            // Get the hand's sphere radius and palm position
            print("Hand sphere radius: " + hand.SphereRadius.ToString("n2")
                        + " mm, palm position: " + hand.PalmPosition);

            // Get the hand's normal vector and direction
            Leap.Vector normal = hand.PalmNormal;
            Leap.Vector direction = hand.Direction;

            // Calculate the hand's pitch, roll, and yaw angles
            print("Hand pitch: " + direction.Pitch * 180.0f / (float)3.14 + " degrees, "
                        + "roll: " + normal.Roll * 180.0f / (float)3.14 + " degrees, "
                        + "yaw: " + direction.Yaw * 180.0f / (float)3.14 + " degrees");
        }
		
		
		
		Leap.GestureList gestures = frame.Gestures();
        for (int i = 0; i < gestures.Count; i++)
        {
            Leap.Gesture gesture = gestures[i];

            switch (gesture.Type)
            {
                case Leap.Gesture.GestureType.TYPECIRCLE:
                    Leap.CircleGesture circle = new Leap.CircleGesture(gesture);

                    // Calculate clock direction using the angle between circle normal and pointable
                    string clockwiseness;
                    if (circle.Pointable.Direction.AngleTo(circle.Normal) <= 3.1487 / 4)
                    {
                        //Clockwise if angle is less than 90 degrees
                        clockwiseness = "clockwise";
                    }
                    else
                    {
                        clockwiseness = "counterclockwise";
                    }

                    float sweptAngle = 0;

                    // Calculate angle swept since last frame
                    if (circle.State != Leap.Gesture.GestureState.STATESTART)
                    {
                        Leap.CircleGesture previousUpdate = new Leap.CircleGesture(controller.Frame(1).Gesture(circle.Id));
                        sweptAngle = (circle.Progress - previousUpdate.Progress) * 360;
                    }
					globalInfo = ("Circle id: " + circle.Id
                                   + ", " + circle.State
                                   + ", progress: " + circle.Progress
                                   + ", radius: " + circle.Radius
                                   + ", angle: " + sweptAngle
                                   + ", " + clockwiseness);

                    break;
                case Leap.Gesture.GestureType.TYPESWIPE:
                    Leap.SwipeGesture swipe = new Leap.SwipeGesture(gesture);
                    globalInfo = ("Swipe id: " + swipe.Id
                                   + ", " + swipe.State
                                   + ", position: " + swipe.Position
                                   + ", direction: " + swipe.Direction
                                   + ", speed: " + swipe.Speed);
				
							
//							
//					float moveInputx = swipe.Direction.x * Time.deltaTime * 3; 
//			        transform.position += new Vector3(moveInputx, 0, 0);
//					
//					float moveInputy = swipe.Direction.y * Time.deltaTime * 3;
//			        transform.position += new Vector3(0, moveInputy, 0);
//					
//					if (Input.GetKey (KeyCode.R)){ 
//						float moveInputz = Time.deltaTime * 3; 
//			        	transform.position += new Vector3(0, 0, moveInputz);
//					}
//					
//					if (Input.GetKey (KeyCode.F)){ 
//						float moveInputz = Time.deltaTime * 3; 
//			        	transform.position += new Vector3(0, 0, -moveInputz);
//					}
//							
//							
//							
							
							
							
							
							
				
				
                    break;
                case Leap.Gesture.GestureType.TYPEKEYTAP:
                    Leap.KeyTapGesture keytap = new Leap.KeyTapGesture(gesture);
                    globalInfo = ("Tap id: " + keytap.Id
                                   + ", " + keytap.State
                                   + ", position: " + keytap.Position
                                   + ", direction: " + keytap.Direction);
                    break;
                case Leap.Gesture.GestureType.TYPESCREENTAP:
                    Leap.ScreenTapGesture screentap = new Leap.ScreenTapGesture(gesture);
                    globalInfo = ("Tap id: " + screentap.Id
                                   + ", " + screentap.State
                                   + ", position: " + screentap.Position
                                   + ", direction: " + screentap.Direction);
                    break;
                default:
                    print("Unknown gesture type.");
                    break;
            }
        }
	
	}
	
	void OnGUI()
    {
        //We display the game GUI from the playerscript
        //It would be nicer to have a seperate script dedicated to the GUI though...
        GUILayout.Label("Frame id: " + guiFrame.Id
                    + ", timestamp: " + guiFrame.Timestamp
                    + ", hands: " + guiFrame.Hands.Count
                    + ", fingers: " + guiFrame.Fingers.Count
                    + ", tools: " + guiFrame.Tools.Count
                    + ", gestures: " + guiFrame.Gestures().Count);
		
		GUILayout.Label(globalInfo);
    }  
	
	void Exit(){
		controller.RemoveListener(listener);
        controller.Dispose();
	}
}
