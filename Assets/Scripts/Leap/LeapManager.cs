using UnityEngine;
using System.Collections;
using Leap;

/// <summary>
/// This singleton manager exposes Leap events (new frame, found hand, 
/// gestures, etc.) to those classes that register for the information using
/// delegates
/// </summary>
public class LeapManager : MonoBehaviour
{

	// TODO: enable on-the-fly gesture enable / disable (based on registration?)
	
    /*-------------------------------------------------------------------------
     * Singleton Implementation
     * ----------------------------------------------------------------------*/
    private static LeapManager _instance;
    public static LeapManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("LeapManager").AddComponent<LeapManager>();
            }
            return _instance;
        }
    }


    /*-------------------------------------------------------------------------
     * Class Properties & Variables
     * ----------------------------------------------------------------------*/
	
    // Private Variables
    private static Controller _controller = new Leap.Controller();
    private static Frame _frame = null;

    // Class properitess
    public static Leap.Frame frame
    {
        get
        {
            return _frame;
        }
    }

    // Public properties
    public Vector3 LeapScaling = new Vector3(0.02f, 0.02f, 0.02f);
    public Vector3 LeapOffset = new Vector3(0, 0, 0);

    public bool UseFixedUpdate = false;

    // Leap Event Delegates

    /// <summary>
    /// Delegates for the Leap events to be dispatched.  
    /// </summary>
    public delegate void ObjectLostHandler(int id);
    public delegate void HandFoundHandler(Hand h);
    public delegate void PointableFoundHandler(Pointable p);
    public delegate void HandUpdatedHandler(Hand h);
    public delegate void PointableUpdatedHandler(Pointable p);
	
	public delegate void KeyTapGestureHandler(KeyTapGesture g);
	public delegate void ScreenTapGestureHandler(ScreenTapGesture g);
	
    public delegate void CircleGestureStartedHandler(CircleGesture g);
    public delegate void CircleGestureUpdatedHandler(CircleGesture g);
    public delegate void CircleGestureStoppedHandler(CircleGesture g);
	
    public delegate void SwipeGestureStartedHandler(SwipeGesture g);
    public delegate void SwipeGestureUpdatedHandler(SwipeGesture g);
    public delegate void SwipeGestureStoppedHandler(SwipeGesture g);

    /// <summary>
    /// Events dispatched in the following order:
    ///   HandLost, PointableLost
    ///   HandFound, PointableFound
    ///   HandUpdated, PointableUpdated
    ///   KeyTapGesture, ScreenTapGesture
    ///   CircleGestureStarted / SwipeGestureStarted
    ///   CircleGestureUpdated / SwipeGestureUpdated
    ///   CircleGestureStopped / SwipeGestureStopped
    /// </summary>
    public static event ObjectLostHandler HandLost;
    public static event ObjectLostHandler PointableLost;
    public static event HandFoundHandler HandFound;
    public static event PointableFoundHandler PointableFound;
    public static event HandUpdatedHandler HandUpdated;
    public static event PointableUpdatedHandler PointableUpdated;
	
    public static event KeyTapGestureHandler KeyTapGestureEvent;
    public static event ScreenTapGestureHandler ScreenTapGestureEvent;
	
    public static event CircleGestureStartedHandler CircleGestureStartedEvent;
    public static event CircleGestureUpdatedHandler CircleGestureUpdatedEvent;
    public static event CircleGestureStoppedHandler CircleGestureStoppedEvent;
	
    public static event SwipeGestureStartedHandler SwipeGestureStartedEvent;
    public static event SwipeGestureUpdatedHandler SwipeGestureUpdatedEvent;
    public static event SwipeGestureStoppedHandler SwipeGestureStoppedEvent;

	private Leap.Frame firstFrame = null;
	
    /*-------------------------------------------------------------------------
     * Unity Lifecycle Functions
     * ----------------------------------------------------------------------*/
    public void Awake()
    {
        // Singleton implementation
        if (_instance != null)
        {
            Debug.LogError(this.ToString() + ": Singleton already exists. Destroying.");
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

        // Set Leap Unity Extension Properties
        Leap.UnityVectorExtension.InputScale = LeapScaling;
        Leap.UnityVectorExtension.InputOffset = LeapOffset;

        // Set up Leap Controller
        _controller = new Controller();
		_controller.EnableGesture(Leap.Gesture.GestureType.TYPECIRCLE);
		_controller.EnableGesture(Leap.Gesture.GestureType.TYPEKEYTAP);
		_controller.EnableGesture(Leap.Gesture.GestureType.TYPESCREENTAP);
		_controller.EnableGesture(Leap.Gesture.GestureType.TYPESWIPE);
    }

    public void Update()
    {
        if (!UseFixedUpdate)
            UpdateLeap();
    }

    public void FixedUpdate()
    {
        if (UseFixedUpdate)
            UpdateLeap();
    }

    void OnDestroy()
    {
        // Singleton implementation
        _instance = null;
    }


    /*-------------------------------------------------------------------------
     * Leap Event Management Functions
     * ----------------------------------------------------------------------*/
    
	private void UpdateLeap()
    {
        if (_controller != null)
        {

            Frame lastFrame = _frame == null ? Frame.Invalid : _frame;
            _frame = _controller.Frame();
			
            // Only rocess new frames
            if (lastFrame.Id != _frame.Id)
            {
                DispatchLostEvents(frame, lastFrame);
                DispatchFoundEvents(frame, lastFrame);
                DispatchUpdatedEvents(frame, lastFrame);
				DispatchGestureEvents(frame, lastFrame);
            }
        }
    }
	
	
    private static void DispatchLostEvents(Frame newFrame, Frame oldFrame)
    {
		// Hands
        foreach (Hand h in oldFrame.Hands)
        {
            if (!h.IsValid)
                continue;
            if (!newFrame.Hand(h.Id).IsValid && HandLost != null)
                HandLost(h.Id);
        }
		
		// Pointables
        foreach (Pointable p in oldFrame.Pointables)
        {
            if (!p.IsValid)
                continue;
            if (!newFrame.Pointable(p.Id).IsValid && PointableLost != null)
                PointableLost(p.Id);
        }
    }
	
	
    private static void DispatchFoundEvents(Frame newFrame, Frame oldFrame)
    {
		// Hands
        foreach (Hand h in newFrame.Hands)
        {
            if (!h.IsValid)
                continue;
            if (!oldFrame.Hand(h.Id).IsValid && HandFound != null)
                HandFound(h);
        }
		
		// Pointables
        foreach (Pointable p in newFrame.Pointables)
        {
            if (!p.IsValid)
                continue;
            if (!oldFrame.Pointable(p.Id).IsValid && PointableFound != null)
                PointableFound(p);
        }
    }

    private static void DispatchUpdatedEvents(Frame newFrame, Frame oldFrame)
    {
		// Hands
        foreach (Hand h in newFrame.Hands)
        {
            if (!h.IsValid)
                continue;
            if (oldFrame.Hand(h.Id).IsValid && HandUpdated != null)
                HandUpdated(h);
        }
		
		// Pointables
        foreach (Pointable p in newFrame.Pointables)
        {
            if (!p.IsValid)
                continue;
			
            if (oldFrame.Pointable(p.Id).IsValid && PointableUpdated != null)
                PointableUpdated(p);
        }
    }
	
	
	private static void DispatchGestureEvents(Frame newFrame, Frame oldFrame) 
	{
		foreach (Gesture g in newFrame.Gestures(oldFrame))
		{
			//Debug.Log ("**" + oldFrame.Id + "->" + newFrame.Id + ": " + g.Type.ToString() + " " + g.State.ToString() + " (" + g.Id + ") **");
			
			// filter invalid events
			if (!g.IsValid || (g.Type == Gesture.GestureType.TYPEINVALID))
				continue;

			// process valid events based on types
			switch (g.Type)
			{
				// process key taps (discrete events)
				case Gesture.GestureType.TYPEKEYTAP:
					if (KeyTapGestureEvent != null)
						KeyTapGestureEvent(new KeyTapGesture(g));
					break;
				
				// process screen taps (discrete events)
				case Gesture.GestureType.TYPESCREENTAP:
					if (ScreenTapGestureEvent != null)
						ScreenTapGestureEvent(new ScreenTapGesture(g));
					break;
				
				// process circles (continuous events)
				case Gesture.GestureType.TYPECIRCLE:
					switch (g.State)
					{
						case Gesture.GestureState.STATESTART:
							if (CircleGestureStartedEvent != null)
								CircleGestureStartedEvent(new CircleGesture(g));
							break;
					
						case Gesture.GestureState.STATEUPDATE:
							if (CircleGestureUpdatedEvent != null)
								CircleGestureUpdatedEvent(new CircleGesture(g));
							break;
					
						case Gesture.GestureState.STATESTOP:
							if (CircleGestureStoppedEvent != null)
								CircleGestureStoppedEvent(new CircleGesture(g));
							break;
					}
				
					break;
				
				// process swipes (continuous events)
				case Gesture.GestureType.TYPESWIPE:
					switch (g.State)
					{
						case Gesture.GestureState.STATESTART:
							if (SwipeGestureStartedEvent != null)
								SwipeGestureStartedEvent(new SwipeGesture(g));
							break;
					
						case Gesture.GestureState.STATEUPDATE:
							if (SwipeGestureUpdatedEvent != null)
								SwipeGestureUpdatedEvent(new SwipeGesture(g));
							break;
					
						case Gesture.GestureState.STATESTOP:
							if (SwipeGestureStoppedEvent != null)
								SwipeGestureStoppedEvent(new SwipeGesture(g));
							break;
					}
					break;
				
				
				// invalid gesture (do nothing)
				default:
					break;
			}
		}
	}

}