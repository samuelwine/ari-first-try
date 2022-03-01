using AsterNET.ARI;
using AsterNET.ARI.Models;

namespace ari_first_try
{
    public static class SyncHelper
    {
        public static void MyWait(
            this Playback playback,
            IAriEventClient client)
        {
            AutoResetEvent playbackFinished = new AutoResetEvent(false);

            client.OnPlaybackFinishedEvent += ((s, e) =>
            {
                if (playback.Id == e.Playback.Id)
                    playbackFinished.Set();
            });

            playbackFinished.WaitOne();

        }
    }
}