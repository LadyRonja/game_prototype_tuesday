using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : NPC, IKnockbackable
{
    private Transform gawkDestination;
    private AIPath pathFinder;
    private AIDestinationSetter destinationSetter;
    [SerializeField] private float recoveryAtVelocity = 0.5f;
    [SerializeField] private float destinationRadius = 0.7f;
    private bool gawking = false;

    public void AddKnockback(Vector2 amount)
    {
        pathFinder.enabled = false;
        myRb.velocity = Vector2.zero;
        //GetComponent<Collider2D>().enabled = false;
        myRb.AddForce(amount, ForceMode2D.Impulse);
        Die();
    }

    protected override void Start()
    {
        base.Start();

        pathFinder = GetComponent<AIPath>();
        if (pathFinder == null) Debug.LogError("No Pathfinding Script found");
        destinationSetter = GetComponent<AIDestinationSetter>();
        if (destinationSetter == null) Debug.LogError("No Destination Setter Script found");
        UpdateDestination();
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            MovementManager();
        }
        else
        {
            DeathHandler();
        }
    }

    private void MovementManager()
    {
        //KnockbackRecovery();
        DestinationArivalTracker();
    }

    private void KnockbackRecovery()
    {
        if (pathFinder.enabled) return;

        if (Vector2.Distance(myRb.velocity, Vector2.zero) <= recoveryAtVelocity)
        {
            pathFinder.enabled = true;
        }
    }

    private void DestinationArivalTracker()
    {
        if (!pathFinder.enabled) return;

        if (Vector2.Distance(transform.position, gawkDestination.position) < destinationRadius && !gawking)
        {
            StartCoroutine(GawkUntilBored(2f));
        }
    }

    private IEnumerator GawkUntilBored(float duration)
    {
        gawking = true;
        yield return new WaitForSeconds(duration);
        gawking = false;
        UpdateDestination();
    }

    private void UpdateDestination()
    {
        gawkDestination = PathingAssisstant.Instance.GetRandomPoint();
        destinationSetter.target = gawkDestination;
    }

    private void Die()
    {
        alive = false;
    }

    private void DeathHandler()
    {
        mySr.color = new Color(mySr.color.r, mySr.color.g, mySr.color.b, Mathf.Clamp(mySr.color.a - Time.deltaTime * 1.5f, 0, 255));
        if (mySr.color.a <= 0f)
        {
            if (Random.Range(0, 2) == 1) EnemySpawner.Instance.SpawnEnemyAtRandomPos();
            Destroy(this.gameObject);
        }
    }
}
