using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float thrust = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpForce = 5f;

    //Cached references
    Animator myAnimator;
    Collider2D myCollider2D;
    Rigidbody2D myRigidbody;
    SpriteRenderer mySpriteRenderer;

    const string ANIMATOR_ISRUNNING_KEY = "isRunning";
    const string ANIMATOR_ISCLIMBING_KEY = "isClimbing";

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        Climb();
        FlipSprite();
    }

    private void Run()
    {
        // TO DO: sostituire con il nuovo input system
        var directionX = Input.GetAxis("Horizontal");
        var movement = new Vector2(directionX * thrust, myRigidbody.velocity.y);
        //myRigidbody.AddForce(newPos); // molto più lento che non modificare direttamente la velocity
        myRigidbody.velocity = movement;
        myAnimator.SetBool(ANIMATOR_ISRUNNING_KEY, PlayerHasHorizontalSpeed());
    }

    private void Climb()
    {
        if (!IsInLadderSpace()) { return; }

        var deltaY = Input.GetAxis("Vertical") * climbSpeed;
        var verticalMovement = new Vector2(myRigidbody.velocity.x, deltaY);
        Debug.Log(verticalMovement);
        //myRigidbody.AddForce(newPos); // molto più lento che non modificare direttamente la velocity
        myRigidbody.velocity = verticalMovement;
        myAnimator.SetBool(ANIMATOR_ISCLIMBING_KEY, PlayerHasVerticalSpeed());
    }


    private bool PlayerHasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidbody.velocity.x) >= Mathf.Epsilon;
    }

    private bool PlayerHasVerticalSpeed()
    {
        return Mathf.Abs(myRigidbody.velocity.y) >= Mathf.Epsilon;
    }


    private void Jump()
    {
        if (!IsOnGround() && !IsInLadderSpace()) { return; }

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump!");
            myRigidbody.velocity += new Vector2(0f, jumpForce);
        }
    }

    private void FlipSprite()
    {
        if (PlayerHasHorizontalSpeed())
        {
            mySpriteRenderer.flipX = myRigidbody.velocity.x < 0;
        }
    }

    private bool IsOnGround()
    {
        return myCollider2D.IsTouchingLayers(LayerMask.GetMask("Terrain"));
    }

    private bool IsInLadderSpace()
    {
        return myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladders"));
    }

}
