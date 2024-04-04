using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using EZCameraShake;
using UnityEngine.UI;
using System.Threading.Tasks;

public class CauseGlitch : MonoBehaviour
{
    public bool CanBeTriggered;
    private bool IsTriggered;

    public AudioClip ClairDeLune;
    public AudioClip Violence;
    [Header("Required Objects")]
    public GameObject GlitchText;
    public TMP_Text ActualText;
    public Image GlitchEffect;
    private AudioSource GlitchSource;
    private AudioSource Terror;
    private AudioSource tok;
    public Image Flashbang;
    public PauseGame GamePauser;

    [Header("References")]
    private musicHandler moosic;
    public PlayerCam Shaker;
    public GameObject PlayerPosition;
    public GameObject GoToPos;
    public GameObject KarenChunk;
    public GameObject[] UnloadChunks;

    private IEnumerator coroutine;

    private float Intensity = 0f;
    IEnumerator ShakeText()
    {
        GlitchSource.Play();
        while (true)
        {
            
            int chance = Random.Range(1,4);
            Vector3 NewLocation = new Vector3(Screen.width * Random.Range(0.498f, 0.502f), Screen.height * Random.Range(0.495f, 0.505f), 0);
            Quaternion NewRotation = Quaternion.Euler(0, 0, Random.Range(-1, 1));
            GlitchText.GetComponent<RectTransform>().position = NewLocation;
            GlitchText.GetComponent<RectTransform>().rotation = NewRotation;
            if (chance == 1)
            {
                GlitchSource.UnPause();
            }
                
            else
            {
                GlitchSource.Pause();
            }
                
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    IEnumerator StartGlitchinTfOut()
    {
        var LeShaka = CameraShaker.Instance.StartShake(2, 100, 3);
        Terror.Play();
        yield return new WaitForSeconds(2f);
        StartCoroutine(coroutine);
        float delay = 1f;
        float Transparency = 0.1f;
        
        for (int i = 0; i < 15; i++) {
            ActualText.color = new Color(1, 1, 1, Transparency);
            GlitchEffect.color = new Color(0.45490196078431372549019607843137f, 0.45490196078431372549019607843137f, 0.45490196078431372549019607843137f, Transparency);
            float truedelay = Random.Range((delay * 0.9f),(delay*1.1f));
            truedelay = Mathf.Clamp(truedelay, 0.1f, 1.1f);
            int TextChoice = Random.Range(1, 3);
            if (TextChoice == 1)
                ActualText.text = "THE VEILS UNRAVEL";
            else if (TextChoice == 2)
                ActualText.text = "PERCEPTION SHATTERS";
            else if (TextChoice == 3)
                ActualText.text = "REALITY AWAKENS";
            yield return new WaitForSeconds(0.1f);
            ActualText.color = new Color(1, 1, 1, 0f);
            GlitchEffect.color = new Color(1, 1, 1, 0f);
            yield return new WaitForSeconds(truedelay);
            delay -= 0.09f;
            Transparency += 0.09f;
        }
        GlitchEffect.DOColor(new Color(0.45490196078431372549019607843137f, 0.45490196078431372549019607843137f, 0.45490196078431372549019607843137f, 1f), 1f);
        ActualText.color = new Color(1, 1, 1, 1f);
        for (int i = 0; i < 30;i++) {
            int TextChoice = Random.Range(1, 4);
            if (TextChoice == 1)
                ActualText.text = "THE VEILS UNRAVEL";
            else if (TextChoice == 2)
                ActualText.text = "PERCEPTION SHATTERS";
            else if (TextChoice == 3)
                ActualText.text = "REALITY AWAKENS";
            
            yield return new WaitForSeconds(0.05f);
        }
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().CanMove = false;
        Terror.Stop();
        tok.Play();
        StopCoroutine(coroutine);
        ActualText.color = new Color(0, 0, 0, 0f);
        GlitchEffect.color = new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(1.5f);
        GlitchEffect.DOColor(new Color(0,0,0, 0f), 1f);
        KarenChunk.SetActive(true);
        LeShaka.StartFadeOut(0);
        StartCoroutine(ReloadAudios());
        yield return new WaitForSeconds(0.1f);
        PlayerPosition.transform.position = GoToPos.transform.position;
        StartCoroutine(UnloadScenes());
        yield return new WaitForSeconds(.1f);
        PlayerPosition.transform.position = GoToPos.transform.position;
        yield return new WaitForSeconds(.9f);
        
        GlitchSource.Stop();
        GlitchText.SetActive(false);
        GlitchEffect.gameObject.SetActive(false);
        GamePauser.CanPressMenu = true;
        moosic.SpontaneousStart();
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().CanMove = true;
        moosic.CanGlitch = false;
    }

    IEnumerator ReloadAudios()
    {
        foreach (Transform Source in moosic.transform)
        {
            if (Source.name == "Violent")
            {
                Source.gameObject.GetComponent<AudioSource>().clip = Violence;
            }
            else
            {
                Source.gameObject.GetComponent<AudioSource>().clip = ClairDeLune;
            }

        }
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator UnloadScenes()
    {
        for (int i = 0; i < UnloadChunks.Length; i++)
        {
            UnloadChunks[i].SetActive(false);
            yield return new WaitForSeconds(.1f);
        }

    }

    private void Start()
    {
        coroutine = ShakeText();
        moosic = GameObject.FindWithTag("musichandler").GetComponent<musicHandler>();
        GlitchSource = GetComponent<AudioSource>();
        Terror = GlitchText.gameObject.GetComponent<AudioSource>();
        tok = Flashbang.gameObject.GetComponent<AudioSource>();
        ClairDeLune.LoadAudioData();
        Violence.LoadAudioData();
    }

    public void StartGlitching()
    {
        if (CanBeTriggered && !IsTriggered)
        {
            IsTriggered = true;
            StartCoroutine(StartGlitchinTfOut());
        }
    }
    bool AlreadyTriggered = false;

    private void Update()
    {
        if(!AlreadyTriggered)
        {
            if (CanBeTriggered)
            {
                if (moosic.EnemyCount <= 0 && moosic.CanGlitch)
                {
                    AlreadyTriggered = true;
                    GamePauser.CanPressMenu = false;
                    moosic.GlitchHappening = true;
                    moosic.SetGlobalVolume(0f);
                    StartGlitching();
                }
            }
        }
    }
    //0.13725490196078431372549019607843
}
