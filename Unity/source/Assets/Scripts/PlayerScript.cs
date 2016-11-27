using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerScript : MonoBehaviour
{
    public Vector2 speed = new Vector2(25, 25);

	public float moveForce = 5, boostMultiplier = 2;

    private Vector2 movement;
    private Rigidbody2D rigidBodyComponent;

	private WeaponScript weapon;

	Rigidbody2D myBody;

	void Start ()
	{
		myBody = this.GetComponent<Rigidbody2D>();
	}

    void Update()
    {
		float inputX = CrossPlatformInputManager.GetAxis("Horizontal");
		float inputY = CrossPlatformInputManager.GetAxis("Vertical");

        movement = new Vector2(
          speed.x * inputX,
          speed.y * inputY);

		//bool shoot = Input.GetButtonDown("Fire1") || CrossPlatformInputManager.GetButton("Shoot");
		//shoot |= Input.GetButtonDown("Fire2"); 

		if (true) {
			WeaponScript[] weapons = GetComponents<WeaponScript> ();
			if (weapons != null)// && weapon.CanAttack)
				foreach (WeaponScript weapon in weapons) {
					if(!weapon.CanAttack) continue;
					weapon.Attack (false);
					SoundEffectsHelper.Instance.MakePlayerShotSound ();
				}
		}

        var dist = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        transform.position = new Vector3(
                  Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
                  Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
                  transform.position.z
                  );
    }

    void FixedUpdate()
    {
		Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),
			CrossPlatformInputManager.GetAxis("Vertical"))
			* moveForce;
		myBody.velocity = moveVec;
		myBody.AddForce(moveVec * 5);
    }

    void OnDestroy()
    {
        HealthScript playerHealth = this.GetComponent<HealthScript>();
        if (playerHealth != null && playerHealth.hp <= 0)
        {
            var gameOver = FindObjectOfType<GameOverScript>();
            gameOver.ShowButtons();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        bool damagePlayer = false;

        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
            if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);

            damagePlayer = true;
        }

        BossScript boss = collision.gameObject.GetComponent<BossScript>();
        if (boss != null)
        {
            HealthScript bossHealth = boss.GetComponent<HealthScript>();
            if (bossHealth != null) bossHealth.Damage(5);

            damagePlayer = true;
        }

        if (damagePlayer)
        {
            HealthScript playerHealth = this.GetComponent<HealthScript>();
            if (playerHealth != null) playerHealth.Damage(1);
        }
    }
}
