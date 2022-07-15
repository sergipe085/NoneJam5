using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundContainer", menuName = "ScriptableObjects/SoundContainer")]
public class SoundContainerSO : ScriptableObject
{
    public List<AudioClip> dieSounds = new();
    public List<AudioClip> hurtSounds = new();
}
