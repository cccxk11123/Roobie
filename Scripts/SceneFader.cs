using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    Animator anim;
    int fadeID;
    private void Start()
    {
        anim = GetComponent<Animator>();
        fadeID = Animator.StringToHash("Fade");
        Gamemanager.RegisterSceneFader(this);
    }
    public void FadeOut()
    {
        anim.SetTrigger(fadeID);
    }
}