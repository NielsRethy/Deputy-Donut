using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PushCollision : MonoBehaviour {

    void OnCollisionEnter(Collision c)
    {
        // force is how forcefully we will push the player away from the enemy.
        float force = 1000;

        // If the object we hit is the enemy
        if (c.gameObject.tag == "DonutTruck")
        {
            force = force * gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = c.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            if (c.gameObject.GetComponentInParent<Rigidbody>() != null)
            {
                if (c.relativeVelocity[0] > 2)
                {
                    c.gameObject.GetComponentInParent<Rigidbody>().AddForce(dir * (force * c.gameObject.GetComponentInParent<Rigidbody>().mass));
                }
                var test = c.relativeVelocity[0];
                var test2 = c.relativeVelocity[1];
                var test3 = c.relativeVelocity[2];
                var test4 = 0;
            }

        }
    }
}
