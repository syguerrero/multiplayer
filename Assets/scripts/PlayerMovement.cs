using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public GameObject personaje;
    Animator anim;

    private void Start()
    {
        anim = personaje.GetComponent<Animator>();

        if (IsOwner)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0.18f, 0.4f, 0.1f);  // Ajusta la posición según necesites
            Camera.main.transform.localRotation = Quaternion.Euler(20, 0, 0);
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Movimiento del jugador
        Vector3 direction = new Vector3(h, 0, v);
        transform.Translate(direction * Time.deltaTime * 5f);

        // Activar animación solo si hay movimiento
        bool isWalking = direction.magnitude > 0.01f;
        anim.SetBool("walk", isWalking);
    }
}
