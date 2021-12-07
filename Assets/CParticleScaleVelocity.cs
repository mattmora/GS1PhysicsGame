using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CParticleScaleVelocity : MonoBehaviour
{
	public Rigidbody rb;
	public IInputManager im;
    public ParticleSystem psRoll;
    public ParticleSystem psHop;
    public ParticleSystem psHit;
    bool playHopParticle;

    public MBone bone;

    
    // Start is called before the first frame update
    void Start()
    {
        bone = GetComponent<MBone>();

    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem.MainModule main = psRoll.main;
	    main.maxParticles = im.freeze ? 0 : (int)(rb.velocity.sqrMagnitude*0.5f);

        if (!im.jump)
        {
            playHopParticle = true;
        }
        if (im.freeze && im.jump && playHopParticle && bone.connectionPoints[0].connectedBone != null)
        {
            Debug.Log("Hop Once");
            psHop.Play();
            playHopParticle = false; 
        }
        else
        {
            psHop.Stop();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Current V is ="+ rb.velocity.magnitude + " and collidng with" + collision.gameObject.name);
        if (rb.velocity.magnitude < 5.0f)
            return;
        if (im.freeze)
            return;

        psHit.Play();
    }

 
}
