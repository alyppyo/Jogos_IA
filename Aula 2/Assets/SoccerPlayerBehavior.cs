using UnityEngine;

public class SoccerPlayerBehavior : MonoBehaviour {

	public Vector2 goal;
	public float   speed;
	public float   maxSteeringForce;
	public float   stopVal;

	public float   arrivalDistance;
	public float   minDist;

	private Rigidbody2D body;

	void Start () {
		body = GetComponent<Rigidbody2D>();
		AdjustSpriteRotation ();
	}

	void AdjustSpriteRotation() {
		float angle = Vector2.Angle(body.velocity, Vector2.right);
		body.rotation = body.velocity.y >= 0 ? angle : 360 - angle;
	}

	Vector2 SetMagnitude(Vector2 v, float max) {
		return v.normalized * max;
	}

	Vector2 Seek() {
		Vector2 velocity = SetMagnitude(goal - body.position, speed);
		Vector2 steering = velocity - body.velocity;
		return Vector2.ClampMagnitude(steering, maxSteeringForce);
	}

	Vector2 Arrival() {
		float distance = Vector2.Distance(body.position, goal);
		if (distance < stopVal)
			return -body.velocity;
		else if (distance < arrivalDistance) {
			Vector2 velocity = SetMagnitude(goal - body.position, speed);
			velocity *= distance / arrivalDistance;
			Vector2 steering = velocity - body.velocity;
			return Vector2.ClampMagnitude(steering, maxSteeringForce);
		}
		else {
			return Seek();
		}
	}

	void FixedUpdate () {
		Vector2 steering = Arrival();
		Vector2 newVelocity = body.velocity + steering;
		body.velocity = Vector2.ClampMagnitude(newVelocity, speed);

		if (body.velocity.magnitude > minDist)
			AdjustSpriteRotation();
	}

	void Update() {
		if (Input.GetButtonDown("Fire1")) {
			goal = (Vector2)
				Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
