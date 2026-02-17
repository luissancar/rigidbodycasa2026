using UnityEngine;
using UnityEngine.InputSystem;

public class Disparar : MonoBehaviour
{ [Header("Prefab del proyectil")]
    public GameObject proyectilPrefab;

    [Header("Punto de disparo")]
    public Transform puntoDisparo;

    [Header("Fuerza del disparo")]
    public float fuerza = 20f;
    public AnimacionesPlayer  animator;
    
    private void DispararB()
    {
        if (proyectilPrefab == null || puntoDisparo == null)
        {
            Debug.LogWarning("Faltan referencias en el script Disparo.");
            return;
        }
        animator.AnimacionDisparar();
        // Instanciar el proyectil en el punto de disparo
        GameObject bala = Instantiate(proyectilPrefab, puntoDisparo.position, puntoDisparo.rotation);
        Destroy(bala,3f);

        // Dar empuje si tiene Rigidbody
        Rigidbody rb = bala.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = puntoDisparo.forward * fuerza;
        }
    }


    public void OnDisparar(InputValue value)
    {
        if (value.isPressed)
           DispararB();
    }

}
