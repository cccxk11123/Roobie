using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager current;

    [Header("��������")]
    public AudioClip ambientCilp;//��Χ������
    public AudioClip musicClip;
    public AudioClip doorClip;

    [Header("FX��Ч")]
    public AudioClip deathFXClip;
    public AudioClip orbFXClip;
    public AudioClip startLevelClip;
    public AudioClip winClip;

    [Header("������Ч")]
    public AudioClip[] walkStepClips; //�����
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
        //������
        current.ambientSorce.clip = current.ambientCilp;
        current.ambientSorce.loop = true;//Ĭ��Ϊfalse
        current.ambientSorce.Play();
        //������
        current.musicSorce.clip = current.musicClip;
        current.musicSorce.loop = true;//Ĭ��Ϊfalse
        current.musicSorce.Play();
        //��ʼ��Ч
        current.fxSorce.clip = current.startLevelClip;
        current.fxSorce.Play();
    }
    //�Ų���
    public static void playFootstepAudio()
    {
        int index = Random.Range(0, current.walkStepClips.Length);
        current.playertSorce.clip = current.walkStepClips[index];
        current.playertSorce.Play();
    }
    //����
    public static void playCrouchAudio()
    {
        int index = Random.Range(0, current.crouchStepClips.Length);
        current.playertSorce.clip = current.crouchStepClips[index];
        current.playertSorce.Play();
    }
    //��Ծ
    public static void playJumpAudio()
    {
        current.playertSorce.clip = current.jumpClip;
        current.playertSorce.Play();

        current.voiceSorce.clip = current.jumpVoiceClip;
        current.voiceSorce.Play();
    }
    //����
    public static void playDeathAudio()
    {
        current.playertSorce.clip = current.deathClip;
        current.playertSorce.Play();

        current.voiceSorce.clip = current.deathVoiceClip;
        current.voiceSorce.Play();

        current.fxSorce.clip = current.deathFXClip;
        current.fxSorce.Play();
    }
    //����
    public static void playOrbAudio()
    {
        current.playertSorce.clip = current.orbVoiceClip;
        current.playertSorce.Play();

        current.fxSorce.clip = current.orbFXClip;
        current.fxSorce.Play();
    }
    //ʯ��
    public static void playDoorOpen()
    {
        current.doorSorce.clip = current.doorClip;
        current.doorSorce.PlayDelayed(1f);//�ӳٲ���
    }
    //ʤ��
    public static void playerWin()
    {
        current.fxSorce.clip = current.winClip;
        current.fxSorce.Play();
        current.playertSorce.Stop();//ֹͣ��ɫ������
    }
}