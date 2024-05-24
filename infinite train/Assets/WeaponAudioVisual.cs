using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudioVisual : MonoBehaviour
{
    public Animator mAnimator;
    public AudioClip hitSound; // D�wi�k ataku

    // Odtw�rz animacj� ataku
    public void PlayAttackAnimation()
    {
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("TrAttack");
            Debug.Log("Animacja ataku zosta�a odtworzona!");
        }
    }

    // Odtw�rz d�wi�k ataku
    public void PlayAttackSound()
    {
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            Debug.Log("D�wi�k ataku zosta� odtworzony!");
        }
    }
}