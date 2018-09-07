//  BuddyHub Universal Controller
//
//  Created by Husheng Deng, 2018
//  https://github.com/vomyrak/BuddyHub

//  This library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.

//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.

//  You should have received a copy of the GNU General Public License
//  along with this library.  If not, see <http://www.gnu.org/licenses/>.
//
//  All trademarks, service marks, trade names, product names are the property of their respective owners.

using NAudio.Wave;

namespace UCUtility
{
    /// <summary>
    /// Audio player plugin sketch
    /// </summary>
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
