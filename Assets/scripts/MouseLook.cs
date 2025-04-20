using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform target;         // El jugador al que sigue
    public Vector3 offset = new Vector3(0, 3, -6);
    public float mouseSensitivity = 3f;
    public float smoothSpeed = 5f;

    private float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        target = transform;
    }

    void LateUpdate()
    {

        RotatePlayer();
    }


    void RotatePlayer()
    {
        //mouseSensitivity: Es un multiplicador que ajusta la sensibilidad del mouse.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; //Input.GetAxis("Mouse X"): Captura el movimiento horizontal del mouse (izquierda/derecha)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; //Input.GetAxis("Mouse Y"): Captura el movimiento vertical del mouse (arriba/abajo).

        rotationX -= mouseY; //Es una variable que almacena la rotación vertical acumulada de la cámara.
        /* Resta el valor de mouseY a rotationX. Esto se hace porque en Unity, un valor positivo de mouseY
         * (movimiento hacia arriba) debe hacer que la cámara mire hacia abajo, y viceversa.*/


        rotationX = Mathf.Clamp(rotationX, -90f, 90f); //: Limita la rotación vertical entre -90 y 90 grados.
                                                       //Esto evita que la cámara gire demasiado hacia arriba o hacia abajo

        transform.Rotate(Vector3.up * mouseX); // Rota el GameObject al que está adjunto este script (el jugador) alrededor del eje Y (arriba).
                                               // Esto permite que el jugador gire hacia la izquierda o la derecha según el movimiento horizontal del mouse (mouseX).


        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f); // Aplica una rotación local a la cámara principal, solo en x
    }

}
