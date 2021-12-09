using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCollectible : MonoBehaviour
{
    public Vector3 rotation;

    GameObject collectible;

    public AudioClip collectSound;
    AudioSource source;

    public float pitch, volume;

    // Start is called before the first frame update
    void Start()
    {
        // Get the collectible which should be the first and only child
        collectible = transform.GetChild(0).gameObject;

        source = gameObject.AddComponent<AudioSource>();
        source.clip = collectSound;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the collectible through this as the parent so 
        // we don't need to worry about where each objects center is
        transform.Rotate(rotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Skull")
        {
            // Set the hat instances on the skull inactive
            Transform collectibles = other.transform.Find("Collectibles");
            ParticleSystem psHat = other.transform.parent.Find("Player").Find("Particle System HatWord").gameObject.GetComponent<ParticleSystem>();
            ParticleSystem psHatE = other.transform.parent.Find("Player").Find("Particle System HatWord").Find("Particle System Hat").gameObject.GetComponent<ParticleSystem>();

            psHat.Play();
            psHatE.Play();
            for (int i = 0; i < collectibles.childCount; ++i)
            {
                collectibles.GetChild(i).gameObject.SetActive(false);
            }
            // Activate the hat matchiing the one just collected
            collectibles.Find(collectible.name).gameObject.SetActive(true);

            CParticleScaleVelocity particles = other.gameObject.GetComponent<CParticleScaleVelocity>();
            //particles.psCollect.Play();

            source.volume = volume;
            source.pitch = pitch;
            source.Play();
        }
    }
}
