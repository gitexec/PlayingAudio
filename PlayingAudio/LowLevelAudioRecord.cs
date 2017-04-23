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
using Android.Media;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace PlayingAudio
{
    class LowLevelAudioRecord : ANotificationReceiver
    {
        public Action<bool> RecordingStateChanged;
        static string filePath = "C:/Users/wington-wg/Desktop/Xamarin/assets/Audio/testAudio.mp3";
        byte[] audioBuffer = null;
        AudioTrack audioTrack = null;
        AudioRecord audioRecord = null;
        bool endRecording = false;
        bool isRecording = false;

        public Boolean IsRecording
        {
            get { return (isRecording); }
        }

        async Task ReadAudioSync ()
        {
            using (var fileStream = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                while (true)
                {
                    if (endRecording)
                    {
                        endRecording = false;
                        break;
                    }
                    try
                    {
                        int numBytes = await audioRecord.ReadAsync(audioBuffer, 0, audioBuffer.Length);
                        await fileStream.WriteAsync(audioBuffer, 0, numBytes);

                    }catch (Exception ex)
                    {
                        Console.Out.WriteLine(ex.Message);
                        break;
                    }
                }
                fileStream.Close();

            }
            audioRecord.Stop();
            audioRecord.Release();
            isRecording = false;

            RaiseRecordingStateChangedEvent();
        }


        private void RaiseRecordingStateChangedEvent()
        {
            if(RecordingStateChanged != null)
            {
                RecordingStateChanged(isRecording);
            }
        }

        protected async Task StartRecorderAsync()
        {
            endRecording = false;
            isRecording = true;

            audioBuffer = new Byte[1000000];
            audioRecord = new AudioRecord(
                AudioSource.Mic,
                11025,
                ChannelIn.Mono,
                Android.Media.Encoding.Pcm16bit,
                audioBuffer.Length);
            audioRecord.StartRecording();
            await ReadAudioSync();
        }

        public async Task StartAsync()
        {
            await StartRecorderAsync();
        }
        public void Stop()
        {
            endRecording = true;
            Thread.Sleep(500);
        }
    }
}