using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtilities.DesignPatterns;

public class AudioManager : Singleton<AudioManager>
{
    // Between 0 and 100
    public int s_SFXVolume = 100;
    public int s_MusicVolume = 100;
}
