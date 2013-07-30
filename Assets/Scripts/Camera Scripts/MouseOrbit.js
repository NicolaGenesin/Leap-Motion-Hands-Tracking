var target : Transform;
var distance = 10.0;
var cameraFollowSpeed = .04;

var xSpeed = 250.0;
var ySpeed = 120.0;
var zSpeed = 100.0;

var yMinLimit = -20;
var yMaxLimit = 80;

private var x = 0.0;
private var y = 0.0;
private var z = 0.0;

@script AddComponentMenu("Camera-Control/Mouse Orbit")

function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;

	// Make the rigid body not change rotation
   	if (rigidbody)
		rigidbody.freezeRotation = true;		
}

function LateUpdate () {
    if (target && (Input.GetMouseButton(0) || Input.GetAxis("Mouse ScrollWheel")) ) {
        x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
        distance -= Input.GetAxis("Mouse ScrollWheel") * zSpeed ;
  		//updateTransform();
 	} 	
}
function FixedUpdate()
{
	updateTransform();
}

function updateTransform () {
	if ( target ) {
 		y = ClampAngle(y, yMinLimit, yMaxLimit);

	    var rotation = Quaternion.Euler(y, x, 0);
	    var position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
	    
	    transform.rotation = rotation;
	    transform.position = Vector3.Lerp(transform.position, position, cameraFollowSpeed);
    }
}

static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}