using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSfxHandler : MonoBehaviour
{
    public AudioSource engineAudio;
    public AudioSource tireAudio;
    public AudioSource collisionAudio;

    float enginePitch = 0.5f;
    float tirePitch = 0.5f;

    CarController carController;

    void Awake()
    {
       carController = GetComponentInParent<CarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSfx();
        UpdateTiresSfx();
    }

    void UpdateEngineSfx()
    {
        float velocityMagnitude = carController.GetVelocityMagnitude();

        float engineVolume = velocityMagnitude * 0.05f;

        engineVolume = Mathf.Clamp(engineVolume, 0.2f, 1.0f);

        engineAudio.volume = Mathf.Lerp(engineAudio.volume, engineVolume, Time.deltaTime * 10);

        enginePitch = velocityMagnitude * 0.2f;
        enginePitch = Mathf.Clamp(enginePitch, 0.5f, 2f);
        engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, enginePitch, Time.deltaTime * 1.5f);
    }

    void UpdateTiresSfx()
    {
        if (carController.IsDrifting(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
            {
                tireAudio.volume = Mathf.Lerp(tireAudio.volume, 1.0f, Time.deltaTime * 10);
                tirePitch = Mathf.Lerp(tirePitch, 0.5f, Time.deltaTime * 10);
            }

            else
            {
                tireAudio.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tirePitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }

        else
        {
            tireAudio.volume = Mathf.Lerp(tireAudio.volume, 0, Time.deltaTime * 10);
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        float relativeVelocity = collision2D.relativeVelocity.magnitude;

        float volume = relativeVelocity * 0.1f;

        collisionAudio.pitch = Random.Range(0.95f, 1.05f);
        collisionAudio.volume = volume;

        if (!collisionAudio.isPlaying) 
        {
            collisionAudio.Play();
        }
    }
}
