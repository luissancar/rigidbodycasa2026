using System;
using UnityEngine;

public class Golpeado : MonoBehaviour
{
    private Animator animator;
    public AnimacionesPlayer animacionesPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Mano" && animacionesPlayer.puedeGolpear) || other.tag == "Bala")
        {
            animator.SetTrigger("Golpeado");
        }
    }
}