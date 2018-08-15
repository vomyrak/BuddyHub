using NAudio.Wave;

namespace AppServer
{
    public static class AudioPlayer
    {
        
        public static void Play(string url)
        {
            using (var mf = new MediaFoundationReader(url))
            using (var wo = new WaveOutEvent())
            {
                wo.Init(mf);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }


}
