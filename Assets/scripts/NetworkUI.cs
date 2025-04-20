using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class NetworkUI : MonoBehaviour
{
    string ip = "127.0.0.1";

    void OnGUI()
    {
        if (NetworkManager.Singleton == null)
        {
            GUILayout.Label("❌ No hay NetworkManager en la escena.");
            return;
        }

        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.BeginVertical();

        GUILayout.Label("IP del host:");
        ip = GUILayout.TextField(ip, 25);

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Start Host"))
                NetworkManager.Singleton.StartHost();

            if (GUILayout.Button("Start Client"))
            {
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                if (transport != null)
                {
                    transport.ConnectionData.Address = ip;
                    NetworkManager.Singleton.StartClient();
                }
                else
                {
                    Debug.LogWarning("⚠️ UnityTransport no encontrado en el NetworkManager.");
                }
            }

            if (GUILayout.Button("Start Server"))
                NetworkManager.Singleton.StartServer();
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
