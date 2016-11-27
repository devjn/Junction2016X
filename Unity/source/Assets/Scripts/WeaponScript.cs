using UnityEngine;

public class WeaponScript : MonoBehaviour
{
	public Transform shotPrefab;

	public float shootingRate = 0.25f;
	public float correctionY = 0;

	private float shootCooldown;

	Vector3 rig = new Vector3();

	void Start ()
	{
		shootCooldown = 0f;
	}

	void Update ()
	{
		if (shootCooldown > 0) {
			shootCooldown -= Time.deltaTime;
		}
	}

	public void Attack (bool isEnemy)
	{
		//if (CanAttack) {
			shootCooldown = shootingRate;

			var shotTransform = Instantiate (shotPrefab) as Transform;

			Vector3 vec = transform.position;
			rig.Set (vec.x, vec.y + correctionY, vec.z);
			shotTransform.position = rig;

			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript> ();
			if (shot != null) {
				shot.isEnemyShot = isEnemy;
			}

			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript> ();
			if (move != null) {
				move.direction = this.transform.right;;
			}
		//}
	}

	public bool CanAttack {
		get {
			return shootCooldown <= 0f;
		}
	}
}