using UnityEngine;

public static class PlayAudioManager
{
    private static PlayAudioImpl _playAudioImpl;

    public static void Init(float bgmVolume, float sfxVolume)
    {
        _playAudioImpl = new PlayAudioImpl(bgmVolume, sfxVolume);
    }

    public static void PlaySFX(AudioClip audioClip)
    {
        _playAudioImpl.PlaySFX(audioClip);
    }
}

public class PlayAudioImpl
{
    private AudioSource _bgmAudioSource;
    private AudioSource _sfxAudioSource;

    public PlayAudioImpl(float bgmVolume, float sfxVolume)
    {
        GameObject bgm = new GameObject("[BGM]", typeof(AudioSource));
        _bgmAudioSource = bgm.GetComponent<AudioSource>();
        _bgmAudioSource.playOnAwake = false;
        _bgmAudioSource.loop = true;
        _bgmAudioSource.volume = bgmVolume;

        GameObject sfx = new GameObject("[SFX]", typeof(AudioSource));
        _sfxAudioSource = sfx.GetComponent<AudioSource>();
        _sfxAudioSource.playOnAwake = false;
        _sfxAudioSource.loop = false;
        _sfxAudioSource.volume = sfxVolume;

        GameObject root = new GameObject("[PlayAudioManager]");
        bgm.transform.SetParent(root.transform);
        sfx.transform.SetParent(root.transform);
        GameObject.DontDestroyOnLoad(root);
    }

    public void PlayBGM(AudioClip audioClip)
    {
        _bgmAudioSource.clip = audioClip;
        _bgmAudioSource.Play();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        _sfxAudioSource.PlayOneShot(audioClip);
    }

    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }

    public void StopSFX()
    {
        _sfxAudioSource.Stop();
    }

    public void Mute()
    {
        _bgmAudioSource.mute = true;
        _sfxAudioSource.mute = true;
    }

    public void Pause()
    {

    }

    public void UnPause()
    {

    }

    public void SetBGMVolume(float volume)
    {
        _bgmAudioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxAudioSource.volume = volume;
    }
}