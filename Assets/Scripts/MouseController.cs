using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    public float jetpackForce = 75f;
    public float forwardMovementSpeed = 3.0f;
    public Transform groundCheckTransform;
    private bool grounded;
    public LayerMask groundCheckLayerMask;
    Animator animator;
    public ParticleSystem jetpack;

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

        if (jetpackActive)
        {
            //rigidbody2D.AddForce(new Vector2(0, jetpackForce));
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jetpackForce));
        }

        Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
        newVelocity.x = forwardMovementSpeed;
        GetComponent<Rigidbody2D>().velocity = newVelocity;

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);
    }

    void UpdateGroundedStatus()
    {
        //1
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        //2
        animator.SetBool("grounded", grounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        jetpack.enableEmission = !grounded;
        jetpack.emissionRate = jetpackActive ? 300.0f : 75.0f;
    }
}
