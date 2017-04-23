using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Media;
using System.Threading;
using System.Threading.Tasks;


namespace PlayingAudio
{

    [Activity(Label = "PlayingAudio", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainAudio : Activity
    {
        AudioMediaPlayer playAudio = new AudioMediaPlayer();
        NotificationManager notificationManager = new NotificationManager();
        static Activity activity = null;
        bool isPlaying = false;
        bool haveRecording = false;
        static public bool useNotifications = false;


        static public Activity Activity
        {
            get { return (activity);  }
        }
        Button startPlayBack = null;
        Button stopPlayBack = null;
        TextView status = null;
        
        void disableButtons()
        {
            startPlayBack.Enabled = false;
            stopPlayBack.Enabled = false;
        }

        void handleButtonState()
        {
            disableButtons();
            if(isPlaying)
            {
                stopPlayBack.Enabled = true;
                return;
            }
            else
            {
                startPlayBack.Enabled = true;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            NotificationManager.audioManager = (AudioManager)GetSystemService(Context.AudioService);
            startPlayBack = FindViewById<Button>(Resource.Id.mediaPlayer);
            startPlayBack.Click += async delegate
            {
                await startOperationAsync(playAudio);
                disableButtons();
                isPlaying = true;
                handleButtonState();

            };
            stopPlayBack = FindViewById<Button>(Resource.Id.stopPlayBack);
            stopPlayBack.Click += delegate
            {
                stopOperation(playAudio);
                isPlaying = false;
                handleButtonState();

            };
        }
        async Task startOperationAsync(ANotificationReceiver nRec)
        {
            if (useNotifications)
            {
                bool haveFocus = notificationManager.RequestAudioResources(nRec);
                if (haveFocus)
                {
                    status.Text = "Granted";
                    await nRec.StartAsync();
                }
                else
                {
                    status.Text = "Denied";
                }
            }
            else
            {
                await nRec.StartAsync();
            }
        }

        void stopOperation(ANotificationReceiver nRec)
        {
            nRec.Stop();
            if (useNotifications)
            {
                notificationManager.ReleaseAudioResources();
                status.Text = "Released";
            }

        }
        public void setStatus(String message)
        {
            status.Text = message;
        }

    }
}

