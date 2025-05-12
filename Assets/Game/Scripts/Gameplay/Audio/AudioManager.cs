using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;

    [Header("Defaults")]
    [SerializeField] private AudioClip defaultSFX;
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private int poolSize = 10;

    private AudioSource[] sfxPool;
    private int currentIndex = 0;

    void Awake()
    {
        if (I != null && I != this) Destroy(gameObject);
        else I = this;

        DontDestroyOnLoad(gameObject);

        // Init SFX pool
        sfxPool = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(audioSourcePrefab, transform);
            sfxPool[i] = obj.GetComponent<AudioSource>();
        }
    }

    public void PlaySFX(AudioClip clip, Vector3 pos, float volume = 1f)
    {
        if (clip == null) clip = defaultSFX;

        AudioSource source = sfxPool[currentIndex];
        source.transform.position = pos;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1f;
        source.Play();

        currentIndex = (currentIndex + 1) % poolSize;
    }

    public void Play2D(AudioClip clip, float volume = 1f)
    {
        if (clip == null) clip = defaultSFX;

        AudioSource source = sfxPool[currentIndex];
        source.transform.position = Camera.main.transform.position;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0f;
        source.Play();

        currentIndex = (currentIndex + 1) % poolSize;
    }

    public void PlayAtSource(AudioClip clip, AudioSource source, float volume = 1f)
    {
        if (source == null || clip == null) return;
        source.clip = clip;
        source.volume = volume;
        source.Play();
    }
}
