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

    public List<AudioClip> hitSounds;
    List<AudioSource> hitSources;

    public AudioClip jumpSound;
    AudioSource jumpSource;

    
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
            jumpSource.pitch = Random.Range(1f, 2f);
            jumpSource.Play();
            playHopParticle = false; 
        }
        else
        {
            psHop.Stop();
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Current V is ="+ rb.velocity.magnitude + " and collidng with" + collision.gameObject.name);

        float mag = 0f;
        foreach (var contact in collision.contacts)
        {
            mag = Mathf.Max(mag, rb.velocity.magnitude * -Vector3.Dot(contact.normal, rb.velocity.normalized));
        }

        Debug.Log("Norm mag " + mag);

        PlayHitSounds(Mathf.Clamp01(Mathf.Log10(mag)));

        if (mag < 5.0f) return;
        if (im.freeze) return;

        psHit.Play();
    }

 
    void PlayHitSounds(float volume)
    {
        for (int i = 0; i < hitSounds.Count; ++i)
        {
            hitSources[i].volume = volume;
            hitSources[i].pitch = 1.6f - Mathf.Clamp01(Mathf.Log10(rb.velocity.magnitude)) + Random.Range(-0.1f, 0.4f);
            hitSources[i].PlayDelayed(Random.Range(0f, 0.02f));
        }
    }
}
