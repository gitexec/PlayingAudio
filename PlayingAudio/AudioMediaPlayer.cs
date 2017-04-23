using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;

namespace PlayingAudio
{
    class AudioMediaPlayer : ANotificationReceiver
    {
        MediaPlayer player = null;
        static string filePath = "http://techslides.com/demos/samples/sample.mp4";

        public async Task StartPlayerAsync()
        {
            try
            {
                if (player == null)
                {
                    player = new MediaPlayer();
                }
                else
                {
                    player.Reset();
                }


                await player.SetDataSourceAsync(filePath);
                player.Prepare();
                player.Start();

            }catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }
        public void StopPlayer()
        {
            if((player != null))
            {
                if(player.IsPlaying)
                {
                    player.Stop();
                }
                player.Release();
                player = null;
            }
        }
        public async Task StartAsync()
        {
            await StartPlayerAsync();
        }
        public void Stop()
        {
            this.StopPlayer();
        }

    }
}