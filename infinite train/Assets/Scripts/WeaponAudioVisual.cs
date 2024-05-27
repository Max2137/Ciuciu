using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudioVisual : MonoBehaviour
{
    public Animator mAnimator;
    public AudioClip hitSound; // DŸwiêk ataku

    // Odtwórz animacjê ataku
    public void PlayAttackAnimation()
    {
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("TrAttack");
            Debug.Log("Animacja ataku zosta³a odtworzona!");
        }
    }

    // Odtwórz dŸwiêk ataku
    public void PlayAttackSound()
    {
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            Debug.Log("DŸwiêk ataku zosta³ odtworzony!");
        }
    }
}