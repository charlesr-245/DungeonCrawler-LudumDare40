using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    private Animator anim;

    private bool animating;
    private Dictionary<string,int> animations;
    private List<string> animationQueue;

    private void Start()
    {
        animations = new Dictionary<string, int>();
        animationQueue = new List<string>();
        animations.Add("Move", 0);
        animations.Add("Hit", 1);
        animations.Add("Dead", 0);
        animations.Add("Attack", 1);
        animations.Add("Stunned", 1);
        animations.Add("Attack2", 1);
        anim = GetComponent<Animator>();
    }

    public void AddToQueue(string animation)
    {
        //Debug.Log(animation);
        animationQueue.Add(animation);
    }

    private void Update()
    {
        PlayAnimations();
    }

    private void PlayAnimations()
    {
        if (animationQueue.Count != 0)
        {
            if (anim.GetCurrentAnimatorStateInfo(animations[animationQueue[0]]).IsName("Default"))
            {
                SetAnimation();
                animationQueue.RemoveAt(0);
            }
        }
    }

    private void SetAnimation()
    {
        //Debug.Log(animationQueue[0]);
        anim.Play(animationQueue[0],animations[animationQueue[0]]);
    }

}
