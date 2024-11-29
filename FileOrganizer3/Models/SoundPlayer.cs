using System;
using System.IO;
using NAudio.Vorbis;
using NAudio.Wave;

namespace FileOrganizer3.Models
{
    public class SoundPlayer : IDisposable
    {
        private readonly WaveOutEvent waveOut = new ();
        private WaveStream reader;

        public SoundPlayer()
        {
            waveOut.PlaybackStopped += SwitchPlayingStatus;
        }

        private FileInfoWrapper PlayingFileInfoWrapper { get; set; }

        /// <summary>
        /// 指定したファイルパスの音声ファイルを再生します。
        /// </summary>
        /// <param name="fileInfoWrapper">再生する音声ファイルのパス。</param>
        public void PlayAudio(FileInfoWrapper fileInfoWrapper)
        {
            waveOut.Stop();

            fileInfoWrapper.PlayCount++;
            fileInfoWrapper.Playing = true;

            if (PlayingFileInfoWrapper != null)
            {
                PlayingFileInfoWrapper.Playing = false;
            }

            PlayingFileInfoWrapper = fileInfoWrapper;

            if (!File.Exists(fileInfoWrapper.FullPath))
            {
                // ファイルが存在しない場合でも、再生に関する情報は更新する。
                return;
            }

            reader = fileInfoWrapper.Extension switch
            {
                ".ogg" => new VorbisWaveReader(fileInfoWrapper.FullPath),
                ".mp3" => new Mp3FileReader(fileInfoWrapper.FullPath),
                ".wav" => new WaveFileReader(fileInfoWrapper.FullPath),
                _ => null,
            };

            if (reader == null)
            {
                return;
            }

            waveOut.Init(reader);
            waveOut.Play();
        }

        public void Stop()
        {
            waveOut.Stop();
            if (PlayingFileInfoWrapper != null)
            {
                PlayingFileInfoWrapper.Playing = false;
                PlayingFileInfoWrapper = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            reader.Dispose();
            waveOut.Dispose();
        }

        private void SwitchPlayingStatus(object sender, StoppedEventArgs e)
        {
            if (sender is not WaveOutEvent { PlaybackState: PlaybackState.Stopped, })
            {
                return;
            }

            if (PlayingFileInfoWrapper != null)
            {
                PlayingFileInfoWrapper.Playing = false;
            }
        }
    }
}