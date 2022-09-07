using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager current;

    [Header("环境声音")]
    public AudioClip ambientCilp;//周围的声音
    public AudioClip musicClip;
    public AudioClip doorClip;

    [Header("FX音效")]
    public AudioClip deathFXClip;
    public AudioClip orbFXClip;
    public AudioClip startLevelClip;
    public AudioClip winClip;

    [Header("人物音效")]
    public AudioClip[] walkStepClips; //数组存
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip deathClip;

    public AudioClip orbVoiceClip;
    public AudioClip jumpVoiceClip;
    public AudioClip deathVoiceClip;

    AudioSource ambientSorce;
    AudioSource musicSorce;
    AudioSource fxSorce;
    AudioSource playertSorce;
    AudioSource voiceSorce;
    AudioSource doorSorce;

    public AudioMixerGroup ambientGroup, musicGroup, fxGroup, playertGroup, voiceGroup;
    private void Awake()
    {
        if(current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);

        ambientSorce = gameObject.AddComponent<AudioSource>();
        musicSorce = gameObject.AddComponent<AudioSource>();
        fxSorce = gameObject.AddComponent<AudioSource>();
        playertSorce = gameObject.AddComponent<AudioSource>();
        voiceSorce = gameObject.AddComponent<AudioSource>();
        doorSorce = gameObject.AddComponent<AudioSource>();

        ambientSorce.outputAudioMixerGroup = ambientGroup;
        musicSorce.outputAudioMixerGroup = musicGroup;
        fxSorce.outputAudioMixerGroup = fxGroup;
        playertSorce.outputAudioMixerGroup = playertGroup;
        voiceSorce.outputAudioMixerGroup = voiceGroup;

        StartLevelAudio();
    }
    void StartLevelAudio()
    {
        //环境音
        current.ambientSorce.clip = current.ambientCilp;
        current.ambientSorce.loop = true;//默认为false
        current.ambientSorce.Play();
        //背景音
        current.musicSorce.clip = current.musicClip;
        current.musicSorce.loop = true;//默认为false
        current.musicSorce.Play();
        //开始音效
        current.fxSorce.clip = current.startLevelClip;
        current.fxSorce.Play();
    }
    //脚步声
    public static void playFootstepAudio()
    {
        int index = Random.Range(0, current.walkStepClips.Length);
        current.playertSorce.clip = current.walkStepClips[index];
        current.playertSorce.Play();
    }
    //蹲下
    public static void playCrouchAudio()
    {
        int index = Random.Range(0, current.crouchStepClips.Length);
        current.playertSorce.clip = current.crouchStepClips[index];
        current.playertSorce.Play();
    }
    //跳跃
    public static void playJumpAudio()
    {
        current.playertSorce.clip = current.jumpClip;
        current.playertSorce.Play();

        current.voiceSorce.clip = current.jumpVoiceClip;
        current.voiceSorce.Play();
    }
    //死亡
    public static void playDeathAudio()
    {
        current.playertSorce.clip = current.deathClip;
        current.playertSorce.Play();

        current.voiceSorce.clip = current.deathVoiceClip;
        current.voiceSorce.Play();

        current.fxSorce.clip = current.deathFXClip;
        current.fxSorce.Play();
    }
    //宝珠
    public static void playOrbAudio()
    {
        current.playertSorce.clip = current.orbVoiceClip;
        current.playertSorce.Play();

        current.fxSorce.clip = current.orbFXClip;
        current.fxSorce.Play();
    }
    //石门
    public static void playDoorOpen()
    {
        current.doorSorce.clip = current.doorClip;
        current.doorSorce.PlayDelayed(1f);//延迟播放
    }
    //胜利
    public static void playerWin()
    {
        current.fxSorce.clip = current.winClip;
        current.fxSorce.Play();
        current.playertSorce.Stop();//停止角色的声音
    }
}