using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    public float jetpackForce = 75f;
    public float forwardMovementSpeed = 3.0f;
    public Transform groundCheckTransform;
    private bool grounded;
    
    // If struck by a laser the mouse becomes invunerable for a while
    private bool struck; 
    private uint struckcount = 0;
    
    public LayerMask groundCheckLayerMask;
    Animator animator;
    public ParticleSystem jetpack;
    private bool dead = false;
    private uint coins = 0;
    private uint lives = 3;
    public AudioClip coinCollectSound;
    public Texture2D coinIconTexture;
    public Texture2D livesIconTexture;

    // Outside scene
    public ParallaxScroll parallax;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }
	
    /// <summary>
    /// This is called on every frame
    /// </summary>
	void Update () {

    }

    /// <summary>
    /// This is called at a fixed rate
    /// </summary>
    void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");
        jetpackActive = jetpackActive && !dead;

        if (jetpackActive)
        {
            //rigidbody2D.AddForce(new Vector2(0, jetpackForce));
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jetpackForce));
        }

        if (!dead)
        {
            Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
            newVelocity.x = forwardMovementSpeed;
            GetComponent<Rigidbody2D>().velocity = newVelocity;
        }

        if (struck)
        {
            struckcount++;
            Debug.Log(struckcount);
            if (struckcount==100)
            {
                struckcount = 0;
                struck = false;
            }
        }

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);

        parallax.offset = transform.position.x;
    }

    void UpdateGroundedStatus()
    {
        //1
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        //2
        animator.SetBool("grounded", grounded);
    }

    /// <summary>
    /// Decrease jetpack emission rate if not in power
    /// </summary>
    /// <param name="jetpackActive"></param>
    void AdjustJetpack(bool jetpackActive) {
        ParticleSystem.EmissionModule em = jetpack.emission;
        em.enabled = true;
        em.rateOverTime = jetpackActive ? 300.0f : 75.0f;
    }

    /// <summary>
    /// We have hit something - figure out what we have hit and react
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Coins"))
            CollectCoin(collider);
        else
            HitByLaser(collider);
    }

    void HitByLaser(Collider2D laserCollider)
    {
        if (struck==false) { // Ignore if in invunerable mode
            struck = true;
            if (!dead)
                laserCollider.gameObject.GetComponent<AudioSource>().Play();
            // TODO: Need to flikr the sprite to avoid losing multiple lives
            lives--;

            if (lives == 0) {
                dead = true;
                animator.SetBool("dead", true);
            }
        }
    }

    /// <summary>
    /// We have collided with coin(s), play audio and increase coun count
    /// If we have collected enough coins we can increase life by one (you get a new life every 200 coins)
    /// </summary>
    /// <param name="coinCollider"></param>
    void CollectCoin(Collider2D coinCollider)
    {
        if (!GameObject.Find("One shot audio"))
        { // if no temporary audio source...
            AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
        }

        coins++;

        // Add a life everytime we collect 200 coinds
        if (coins % 200 == 0) {
            lives++;
        }

        Destroy(coinCollider.gameObject);
    }

    void DisplayCoinsCount() {
        Rect coinIconRect = new Rect(10, 10, 32, 32);
        GUI.DrawTexture(coinIconRect, coinIconTexture);

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;

        Rect labelRect = new Rect(coinIconRect.xMax, coinIconRect.y, 60, 32);
        GUI.Label(labelRect, coins.ToString(), style);
    }

    void DisplayLivesCount() {
        Rect livesIconRect = new Rect(470, 10, 470, 32);
        GUI.DrawTexture(livesIconRect, livesIconTexture);

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;

        Rect labelRect = new Rect(livesIconRect.xMax, livesIconRect.y, 470, 10);
        GUI.Label(labelRect, lives.ToString(), style);
    }
    void OnGUI()
    {
        DisplayCoinsCount();
        DisplayLivesCount();
    }
}
