using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An extension of object pooling that pools and plays audio clips
public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    private List<List<AudioSource>> pools;
    // For the sake of clear organization, put each pool as a parent object of the pool of objects
    private GameObject[] poolObjects;
    [SerializeField] private AudioSource[] soundList;
    [SerializeField] private List<Sound> soundIndexList; // Fragile workaround; use Sound enum to retrieve index
    [SerializeField] private int[] numberOfSounds;
    private int[] nextAvailable;

    void Awake() {
        if (instance == null) {
            instance = FindObjectOfType<AudioManager>();
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                DontDestroyOnLoad(instance);
                if (instance != this) Destroy(this.gameObject);
            }
        } else {
            Destroy(gameObject);
        }
    }

    public static AudioManager GetInstance() {
        if (instance != null) {
            return instance;
        } else {
            AudioManager am = FindObjectOfType<AudioManager>();
            if (am != null) instance = am;
            if (instance != null) {
                DontDestroyOnLoad(instance);
                return instance;
            } else {
                Debug.LogError("Returning null AudioManager");
                return null;
            }
        }
    }

    void Start () {
        ReinstantiatePools();
    }

    public void ReinstantiatePools() {
        poolObjects = new GameObject[soundList.Length];
        nextAvailable = new int[soundList.Length];
        for (int y = 0; y < soundList.Length; y++) {
            GameObject poolObject = new GameObject();
            poolObject.name = soundList[y].name + " Pool";
            poolObjects[y] = poolObject;
        }
        pools = new List<List<AudioSource>>();
        for (int y = 0; y < soundList.Length; y++) {
            List<AudioSource> pool = new List<AudioSource>();
            for (int x = 0; x < numberOfSounds[y]; x++) {
                AudioSource singleSound = (AudioSource) Instantiate(soundList[y]);
                singleSound.playOnAwake = false;
                pool.Add(singleSound);
                singleSound.transform.SetParent(poolObjects[y].transform);
            }
            poolObjects[y].transform.SetParent(gameObject.transform);
            pools.Add(pool);
        }
        for (int x = 0; x < nextAvailable.Length; x++) {
            nextAvailable[x] = 0;
        }
    }

    public void PlaySound(Sound sound) {
        int soundIndex = soundIndexList.IndexOf(sound);
        int startingIndex = nextAvailable[soundIndex];
        do {
            if (!pools[soundIndex][nextAvailable[soundIndex]].isPlaying) {
                int readyIndex = nextAvailable[soundIndex];
                nextAvailable[soundIndex]++;
                nextAvailable[soundIndex] %= numberOfSounds[soundIndex];
                pools[soundIndex][readyIndex].Play();
                return;
            } else {
                nextAvailable[soundIndex]++;
                nextAvailable[soundIndex] %= numberOfSounds[soundIndex];
            }
        } while (nextAvailable[soundIndex] != startingIndex);
        Debug.LogWarning("Couldn't play sound: " + sound);
    }

}