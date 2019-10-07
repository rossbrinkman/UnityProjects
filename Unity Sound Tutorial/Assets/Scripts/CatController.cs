using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{

    public AudioSource musicSource;

    public AudioClip musicClip1;
    public AudioClip musicClip2;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            musicSource.clip = musicClip1;
            musicSource.Play();
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            musicSource.Stop();
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            musicSource.clip = musicClip2;
            musicSource.Play();
            anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            musicSource.Stop();
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            musicSource.loop = true;
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            musicSource.loop = false;
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
