using UnityEngine;

public class MicVolumeDetector : MonoBehaviour
{
    public string micName;
    public float volumeThreshold = 0.1f;
    public Vector3 micPosition;

    public delegate void OnNoise(Vector3 pos);
    public static event OnNoise OnNoiseHeard;

    private AudioClip micClip;
    private const int sampleWindow = 128;

    public static bool isMakingNoise = false;

    public GameObject personaje;
    Animator anim;

    void Start()
    {
        anim = personaje.GetComponent<Animator>();
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            micClip = Microphone.Start(micName, true, 1, 44100);
        }
        else
        {
            Debug.LogError("No se detectó ningún micrófono.");
        }
    }

    void Update()
    {
        micPosition = transform.position;
        float volume = GetMicVolume();
        isMakingNoise = volume > volumeThreshold;
        anim.SetBool("talk", isMakingNoise);
        //Debug.Log("🎙️ MicVolumeDetector.isMakingNoise = " + isMakingNoise);

        if (isMakingNoise)
        {
            OnNoiseHeard?.Invoke(micPosition);
            
        }
    }

    float GetMicVolume()
    {
        float levelMax = 0;
        float[] waveData = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(micName) - sampleWindow + 1;
        if (micPosition < 0) return 0;
        micClip.GetData(waveData, micPosition);

        for (int i = 0; i < sampleWindow; ++i)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak) levelMax = wavePeak;
        }

        return Mathf.Sqrt(levelMax);
    }
}
