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
    public ParticleSystem psSmoke;
    bool playHopParticle;

    public MBone bone;

    public List<AudioClip> hitSounds;
    List<AudioSource> hitSources;

    public AudioClip jumpSound;
    AudioSource jumpSource;

    public Vector3 savedNormal;

    //public bool touching;
    public HashSet<GameObject> touchingObjs;
    
    // Start is called before the first frame update
    void Start()
    {
        hitSources = new List<AudioSource>();

        bone = GetComponent<MBone>();

        foreach (AudioClip clip in hitSounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            hitSources.Add(source);
        }

        jumpSource = gameObject.AddComponent<AudioSource>();
        jumpSource.volume = 0.7f;
        jumpSource.clip = jumpSound;

        touchingObjs = new HashSet<GameObject>();

        ParticleSystem.MainModule main = psRoll.main;
        main.maxParticles = 0;
        ParticleSystem.MainModule mainn = psSmoke.main;
        mainn.maxParticles = 0;

        psRoll.Play();
        psSmoke.Play();
    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem.MainModule main = psRoll.main;
	    main.maxParticles = im.freeze ? 0 : (int)(rb.velocity.sqrMagnitude*0.5f);

        ParticleSystem.MainModule mainn = psSmoke.main;
        mainn.maxParticles = im.freeze ? 0 : (int)(rb.velocity.sqrMagnitude * 0.5f);

        if (!im.jump)
        {
            playHopParticle = true;
        }
        if (im.freeze && im.jump && playHopParticle && bone.connectionPoints[0].connectedBone != null)
        {
            Debug.Log("Hop Once");
            psHop.Play();
            jumpSource.pitch = Random.Range(1f, 2f);
            jumpSource.Play();
            playHopParticle = false; 
        }
        else
        {
            psHop.Stop();
        }

    }

    private void FixedUpdate()
    {
        //if (!touching) savedNormal = Vector3.zero;
        //touching = false;
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Current V is ="+ rb.velocity.magnitude + " and collidng with" + collision.gameObject.name);
        if (touchingObjs.Count == 0) savedNormal = Vector3.zero;
        touchingObjs.Add(collision.gameObject);

        float mag = 0f;
        Vector3 tempVec = Vector3.zero;
        foreach (var contact in collision.contacts)
        {
            if (mag < rb.velocity.magnitude * -Vector3.Dot(contact.normal, rb.velocity.normalized))
            {
                tempVec = Vector3.Normalize(contact.normal);
                mag = rb.velocity.magnitude * -Vector3.Dot(contact.normal, rb.velocity.normalized);
            }
            //tempVec = contact.normal;
            //mag = Mathf.Max(mag, rb.velocity.magnitude * -Vector3.Dot(contact.normal, rb.velocity.normalized));
        }

        //Debug.Log("Norm mag " + mag);
        //Debug.Log("normal: " + tempVec + " magnitute: " + tempVec.magnitude);
        Debug.Log("dot product: " + Vector3.Dot(tempVec, savedNormal));

        if (Vector3.Dot(savedNormal, tempVec) > 0.1f) return; 

        PlayHitSounds(Mathf.Clamp01(Mathf.Log10(mag)));

        if (mag < 5.0f) return;
        if (im.freeze) return;

        savedNormal = tempVec;
        psHit.Play();
    }

    public void OnCollisionExit(Collision collision)
    {
        //Debug.Log("Exited");
        //savedNormal = Vector3.zero;
        //touching = true;
        touchingObjs.Remove(collision.gameObject);
    }


    void PlayHitSounds(float volume)
    {
        for (int i = 0; i < hitSounds.Count; ++i)
        {
            hitSources[i].volume = volume;
            hitSources[i].pitch = 1.6f - Mathf.Clamp01(Mathf.Log10(rb.velocity.magnitude)) + Random.Range(-0.1f, 0.4f);
            //hitSources[i].PlayDelayed(Random.Range(0f, 0.02f));
            hitSources[i].Play();
        }
    }
}
