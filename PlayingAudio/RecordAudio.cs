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
using System.IO;

namespace PlayingAudio
{
    class RecordAudio : ANotificationReceiver
    {
        static string filePath = "C:/Users/wington-wg/Desktop/Xamarin/assets/Audio/testAudio.mp3";
        MediaRecorder recoder = null;

        public void StartRecorder()
        {
            try
            {
                if (File.Exists (filePath))
                {
                    File.Delete(filePath);
                }

                if (recoder == null)
                    recoder = new MediaRecorder();
                else
                    recoder.Reset();

                recoder.SetAudioSource(AudioSource.Mic);
                recoder.SetOutputFormat(OutputFormat.Mpeg4);
                recoder.SetAudioEncoder(AudioEncoder.AmrNb);
                recoder.SetOutputFile(filePath);
                recoder.Prepare();
                recoder.Start();
            }catch(Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
        }
        public void StopRecorder()
        {
            if(recoder != null)
            {
                recoder.Stop();
                recoder.Release();

            }
        }

        public Task StartAsync()
        {
            StartRecorder();
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }
        public void Stop()
        {
            StopRecorder();
        }

    }
}