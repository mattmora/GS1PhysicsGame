using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CParticleScaleVelocity : MonoBehaviour
{
	public Rigidbody rb;
	public IInputManager im;
	ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem.MainModule main = ps.main;
	main.maxParticles = im.freeze ? 0 : (int)(rb.velocity.sqrMagnitude*0.5f);
    }
}
