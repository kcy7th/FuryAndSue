using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class SoundEmitter : MonoBehaviour
{
    public SoundData SoundData { get; set; }

    private AudioSource source;
    private bool isStop = false;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        isStop = false;
    }

    public void Initialize(SoundData data)
    {
        SoundData = data;

        source.clip = data.clip;
        source.outputAudioMixerGroup = data.outputMixerGroup;
        source.mute = data.mute;
        source.loop = data.loop;
        source.playOnAwake = data.playOnAwake;
        source.pitch = data.pitch;
        source.volume = data.volume;
        source.panStereo = data.panStereo;
        source.spatialBlend = data.spatialBlend;
        source.reverbZoneMix = data.reverbZoneMix;
    }

    public void Play()
    {
        source.Play();
        WaitForSoundToEnd().Forget();

        if (SoundManager.Instance.soundInstances.TryGetValue(SoundData, out var count))
        {
            SoundManager.Instance.soundInstances[SoundData] += 1;
        }
        else
        {
            SoundManager.Instance.soundInstances[SoundData] = 1;
        }
    }

    private async UniTask WaitForSoundToEnd()
    {
        await UniTask.WaitUntil(() => source && !source.isPlaying);
        if (!isStop)
        {
            Stop();
        }
    }

    public void Stop()
    {
        isStop = true;
        SoundManager.Instance.ReturnToPool(this);
    }
}
