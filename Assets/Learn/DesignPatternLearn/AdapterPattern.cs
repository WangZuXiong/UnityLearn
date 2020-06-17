using UnityEngine;
/// <summary>
/// 适配器模式
/// </summary>
public class AdapterPattern : MonoBehaviour
{
    public interface IMediaPlayer
    {
        void Play(string audioType, string fileName);
    }

    public interface IAdvancedMediaPlayer
    {
        void PlayVlc(string fileName);
        void PlayMp4(string fileName);
    }

    public class VlcPlayer : IAdvancedMediaPlayer
    {
        public void PlayMp4(string fileName)
        {
            //什么也不做
        }

        public void PlayVlc(string fileName)
        {
            Debug.Log("Playing  Vlc file name:" + fileName);
        }
    }

    public class Mp4Player : IAdvancedMediaPlayer
    {
        public void PlayMp4(string fileName)
        {
            Debug.Log("Playing  Mp4 file name:" + fileName);
        }

        public void PlayVlc(string fileName)
        {
            //什么也不做
        }
    }

    public class MediaAdapter : IMediaPlayer
    {
        private IAdvancedMediaPlayer _advancedMediaPlayer;

        public MediaAdapter(string audioType)
        {
            if (audioType == "Mp4")
            {
                _advancedMediaPlayer = new Mp4Player();
            }
            else if (audioType == "Vlc")
            {
                _advancedMediaPlayer = new VlcPlayer();
            }
        }
        public void Play(string audioType, string fileName)
        {
            if (audioType == "Mp4")
            {
                _advancedMediaPlayer.PlayMp4(fileName);
            }
            else if (audioType == "Vlc")
            {
                _advancedMediaPlayer.PlayVlc(fileName);
            }
        }
    }

    public class AudioPlayer : IMediaPlayer
    {
        private IMediaPlayer _mediaPlayer;
        public void Play(string audioType, string fileName)
        {
            if (audioType == "Mp3")
            {
                Debug.Log("playing mp3 file name:" + fileName);
            }
            else if (audioType == "Vlc" || audioType == "Mp4")
            {
                _mediaPlayer = new MediaAdapter(audioType);
                _mediaPlayer.Play(audioType, fileName);
            }
            else
            {
                Debug.Log("Invalid media" + audioType + "format not supported");
            }
        }
    }

    public void Main()
    {
        AudioPlayer audioPlayer = new AudioPlayer();
        audioPlayer.Play("Mp3", "A.Mp3");
        audioPlayer.Play("Mp4", "B.Mp3");
        audioPlayer.Play("Vlc", "C.Mp3");
        audioPlayer.Play("Avi", "D.Mp3");
    }
}
