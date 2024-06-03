using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestory : MonoBehaviour
{ 
    public static DontDestory Instance;
    AudioSource bgm;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void BgmStop() {
        if (Instance == null)
            bgm = gameObject.GetComponent<AudioSource>();
        else
            bgm = Instance.GetComponent<AudioSource>();
        bgm.enabled = false;
    }

    public void PlayBgm() {
        if (Instance == null)
            bgm = gameObject.GetComponent<AudioSource>();
        else
            bgm = Instance.GetComponent<AudioSource>();
        bgm.enabled = true;
    }
}


