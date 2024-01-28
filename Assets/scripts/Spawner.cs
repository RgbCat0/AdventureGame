using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Spawner : MonoBehaviour
{
    [Header("Entity Selection")]
    public GameObject[] AvailableEntities;
    public int EntityChoice;
    [Header("References")]
    public Transform SpawnPoint;
    public GameObject ParticleEmitter;
    private GameObject MusicHandler;

    private Light FXlight;

    [Header("Cheese")]
    public float DelayTime;
    private float DelayTimer;
    private bool HasSpawned = false;
    public bool StartTimer;
    public float DecrementMultiplier;
    private void Start()
    {
        DelayTimer = DelayTime;
        HasSpawned = false;
        FXlight = ParticleEmitter.GetComponent<Light>();
        MusicHandler = GameObject.FindWithTag("musichandler");
    }
    private void FixedUpdate()
    {
        if (StartTimer && !HasSpawned)
        {
            if (DelayTimer > 0)
            {
                DelayTimer -= Time.deltaTime;
            }
            else if (DelayTimer <= 0)
            {
                HasSpawned = true;
                Spawn();
            }
        }
        if (FXlight.intensity > 0)
        {
            FXlight.intensity -= Time.deltaTime * DecrementMultiplier;
        } else
        {
            FXlight.intensity = 0;
        }
    }

    private void Spawn()
    {
        if(EntityChoice == 0) // Filth
        {
            GameObject CreatedEntity = Instantiate(AvailableEntities[EntityChoice], transform.position, transform.rotation);
            CreatedEntity.GetComponent<FilthBehaviour>().BehaviourEnabled = true;
        }
        FXlight.intensity = 1f;
        MusicHandler.GetComponent<musicHandler>().EnemyCount += 1;
        var emitParams = new ParticleSystem.EmitParams();
        ParticleEmitter.GetComponent<ParticleSystem>().Emit(emitParams,1);
    }
}
