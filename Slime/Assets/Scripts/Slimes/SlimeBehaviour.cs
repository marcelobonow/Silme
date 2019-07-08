using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SlimeBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private float OnWallFallAccelaration;
    [SerializeField] private float maxFallSpeed;

    public bool isAttached { private set; get; }
    public Walls currentWall { private set; get; }
    private float oldGravity;

    private void FixedUpdate()
    {
        if(isAttached)
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, Mathf.Clamp(rigidbody2d.velocity.y, -maxFallSpeed, Mathf.Infinity));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Assert.IsNotNull(rigidbody2d);
        if(collision.CompareTag("Wall"))
        {
            currentWall = collision.GetComponent<WallData>().wall;
            isAttached = true;
            oldGravity = rigidbody2d.gravityScale;
            rigidbody2d.gravityScale = OnWallFallAccelaration;
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezePositionX;
            rigidbody2d.velocity = Vector2.zero;
        }
    }
    public void LaunchSlime(Vector2 velocity)
    {
        rigidbody2d.velocity = velocity;
        rigidbody2d.constraints = RigidbodyConstraints2D.None;
        rigidbody2d.gravityScale = oldGravity;
        isAttached = false;
    }
}
