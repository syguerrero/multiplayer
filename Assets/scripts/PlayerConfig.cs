using UnityEngine;
public class PlayerConfig : MonoBehaviour
{
    public static PlayerConfig Instance;
    public Texture selectedTexture;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
