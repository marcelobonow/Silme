using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SlimeBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private float OnWallFallAccelaration;
    [SerializeField] private float maxFallSpeed;

    private bool isAttached;
    private float oldGravity;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Assert.IsNotNull(rigidbody2d);
        if(collision.CompareTag("Wall"))
        {
            isAttached = true;
            oldGravity = rigidbody2d.gravityScale;
            rigidbody2d.gravityScale = OnWallFallAccelaration;
        }
    }

    private void FixedUpdate() =>
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, Mathf.Clamp(rigidbody2d.velocity.y, -maxFallSpeed, Mathf.Infinity));
}
