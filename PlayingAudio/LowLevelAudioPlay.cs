using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using Android.Media;
using System.IO;

namespace PlayingAudio
{
    class LowLevelAudioPlay : ANotificationReceiver
    {
        static string filePath = "C:/Users/wington-wg/Desktop/Xamarin/assets/Audio/testAudio.mp3";
        byte[] buffer = null;
        AudioTrack audioTrack = null;

        public async Task PlaybackAsync()
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            long totalBytes = new System.IO.FileInfo (filePath).Length;
            buffer = binaryReader.ReadBytes((Int32)totalBytes);
            fileStream.Close();
            fileStream.Dispose();
            binaryReader.Close();
            await PlayAudioTrackAsync();
        }
        protected async Task PlayAudioTrackAsync()
        {
            audioTrack = new AudioTrack(
                Android.Media.Stream.Music,
                //Frequency
                11025,
                ChannelOut.Mono,
                Android.Media.Encoding.Pcm16bit,
                buffer.Length,
                AudioTrackMode.Stream);
            audioTrack.Play();
            await audioTrack.WriteAsync(buffer, 0, buffer.Length);
        }

        public async Task StartAsync()
        {
            await PlaybackAsync();
        }
        public void Stop()
        {
            if(audioTrack != null)
            {
                audioTrack.Stop();
                audioTrack.Release();
                audioTrack = null;
            }
        }
    }
}