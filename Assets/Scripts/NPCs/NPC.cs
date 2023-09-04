using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected bool knockable = true;
    [SerializeField] protected float movementSpeed = 20f;
    [SerializeField] protected Rigidbody2D myRb;
    [SerializeField] protected SpriteRenderer mySr;

    protected bool alive = true;

    protected void FindMyRigidbody()
    {
        myRb = GetComponent<Rigidbody2D>();
        mySr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (myRb == null) Debug.LogError($"{gameObject.name} has no Rigidbody2D");
        if (mySr == null) Debug.LogError($"{gameObject.name} has no SpriteRenderer");
    }

    virtual protected void Start()
    {
        FindMyRigidbody();
    }

}
