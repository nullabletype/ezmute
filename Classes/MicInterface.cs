using NAudio.CoreAudioApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace ezmute
{
    internal static class MicInterface
    {
        private static readonly IEnumerable<DeviceState> IgnoredStates = new DeviceState[] { DeviceState.NotPresent };

        public static string ToggleAllMicMute()
        {
            var statusToSet = !AllMuted();
            string output = string.Empty;
            try
            {
                foreach (MMDevice dev in GetDevices())
                {
                    try
                    {
                        string muted = "undefined";
                        if (dev.AudioEndpointVolume?.Mute != null)
                        {
                            muted = dev.AudioEndpointVolume.Mute.ToString();
                        }
                        //Show us the human understandable name of the device
                        output += muted + " : " + dev.FriendlyName + Environment.NewLine;
                        //Mute it
                        dev.AudioEndpointVolume.Mute = statusToSet;
                    }
                    catch (Exception ex)
                    {
                        //Do something with exception when an audio endpoint could not be muted
                    }
                }
            }
            catch (Exception ex)
            {
                //When something happend that prevent us to iterate through the devices
            }
            return output;
        }

        private static IEnumerable<MMDevice> GetDevices()
        {
            MMDeviceEnumerator mmde = new MMDeviceEnumerator();
            //Get all the devices, no matter what condition or status
            MMDeviceCollection devCol = mmde.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All);
            return devCol.Where(d => !IgnoredStates.Contains(d.State));
        }

        public static bool AllMuted()
        {
            try
            {
                return GetDevices().All(d => d.AudioEndpointVolume.Mute);
            }
            catch (Exception ex)
            {
                //When something happend that prevent us to iterate through the devices
            }

            return false;
        }

    }
}
