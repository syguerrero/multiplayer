using UnityEngine;
using Unity.Netcode;

public class PlayerSetup : NetworkBehaviour
{
    public Renderer characterRenderer;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Solo para el jugador local
            Texture tex = PlayerConfig.Instance?.selectedTexture;
            if (tex != null)
            {
                characterRenderer.material.mainTexture = tex;

                // Puedes sincronizar esto por RPC si quieres que todos lo vean:
                ApplyTextureServerRpc(tex.name); // si lo cargas por nombre
            }
        }
    }

    [ServerRpc]
    void ApplyTextureServerRpc(string textureName)
    {
        Texture loaded = Resources.Load<Texture>("Textures/" + textureName);
        ApplyTextureClientRpc(textureName);
    }

    [ClientRpc]
    void ApplyTextureClientRpc(string textureName)
    {
        Texture loaded = Resources.Load<Texture>("Textures/" + textureName);
        characterRenderer.material.mainTexture = loaded;
    }
}
