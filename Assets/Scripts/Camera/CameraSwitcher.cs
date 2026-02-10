using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{   [Header("Cámaras disponibles")]
    public List<Camera> camaras;

    private int indiceActual = 0;



    private void Start()
    {
        ActivarSoloEsta(indiceActual);
    }

    private void CambiarCamara()
    {
        indiceActual++;

        if (indiceActual >= camaras.Count)
            indiceActual = 0;

        ActivarSoloEsta(indiceActual);
    }

    private void ActivarSoloEsta(int index)
    {
        for (int i = 0; i < camaras.Count; i++)
        {
            camaras[i].gameObject.SetActive(i == index);
        }
    }

    public void OnCambioCamara(InputValue value)
    {
        if (value.isPressed)
            CambiarCamara();
    }
}