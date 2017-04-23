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

namespace PlayingAudio
{
    class NotificationManager
    {
        static public AudioManager audioManager = null;
        static Activity mainActivity = null;

        public static Activity MainActivity
        {
            set { mainActivity = value; }
        }

        AudioManager.IOnAudioFocusChangeListener listener = null;

        private class FocusChangeListener : Java.Lang.Object, AudioManager.IOnAudioFocusChangeListener
        {
            ANotificationReceiver parent = null;

            public FocusChangeListener (ANotificationReceiver parent)
            {
                this.parent = parent;
            }
           void SetStatus (String message)
            {
                if(mainActivity is MainAudio)
                {
                    MainAudio mainAudioActivity = mainActivity as MainAudio;
                    mainAudioActivity.setStatus(message);
                }
            }
           public void OnAudioFocusChange(AudioFocus focusChange)
            {
                switch (focusChange)
                {
                    case AudioFocus.GainTransient:
                    case AudioFocus.GainTransientMayDuck:
                    case AudioFocus.Gain:
                        parent.StartAsync();
                        SetStatus("Granted");
                        break;
                    case AudioFocus.LossTransientCanDuck:
                    case AudioFocus.LossTransient:
                    case AudioFocus.Loss:
                        parent.Stop();
                        SetStatus("Removed");
                        break;
                    default:
                        break;
                }
            }
        }
        
        public Boolean RequestAudioResources (ANotificationReceiver parent)
        {
            listener = new FocusChangeListener(parent);

            var returnVal = audioManager.RequestAudioFocus(listener, Stream.Music, AudioFocus.Gain);
            if(returnVal == AudioFocusRequest.Granted)
            {
                return (true);
            }
            else if (returnVal == AudioFocusRequest.Failed)
            {
                return (false);
            }

            return (false);
        }
        public void ReleaseAudioResources()
        {
            if (listener != null)
                audioManager.AbandonAudioFocus(listener);
        }
    }
}