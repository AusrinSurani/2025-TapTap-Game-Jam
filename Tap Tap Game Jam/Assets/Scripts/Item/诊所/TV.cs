using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TV : Interactable
{
    private Animator animator;
    private bool isOpen = true;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        animator.SetBool("Open", isOpen);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        isOpen = !isOpen;
        animator.SetBool("Open", isOpen);
    }
}