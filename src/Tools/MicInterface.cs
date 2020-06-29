using NAudio.CoreAudioApi;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ezmute.Tools
{
    internal static class MicInterface
    {
        private static ILogger log;

        static MicInterface() {
            log = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("error.log").CreateLogger();
        }

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

                        output += muted + " : " + dev.State + " : " + dev.FriendlyName + Environment.NewLine;

                        //Mute it
                        dev.AudioEndpointVolume.Mute = statusToSet;
                        dev.Dispose();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex, $"Couldn't set mic status to {statusToSet}. Status log so far: {output}");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Couldn't set mic status to {statusToSet}. Status log so far: {output}");
            }
            return output;
        }

        private static IEnumerable<MMDevice> GetDevices()
        {
            MMDeviceEnumerator mmde = new MMDeviceEnumerator();
            //Get all the devices, no matter what condition or status
            MMDeviceCollection devCol = mmde.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All);
            return devCol.Where(d => d.State == DeviceState.Active);
        }

        public static bool AllMuted()
        {
            try
            {
                var devices = GetDevices();
                bool state = devices.All(d => d.AudioEndpointVolume.Mute);
                foreach (MMDevice device in devices)
                {
                    device.Dispose();
                }
                return state;
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Couldn't check if mics were muted");
            }

            return false;
        }

    }
}
