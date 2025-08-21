using System;
using System.Collections.Generic;

namespace funkin
{
    public class TimeSignature
    {
        public float Time; // in ms
        public float Numerator;
        public float Denominator;

        public TimeSignature(float time, float num, float den)
        {
            Time = time;
            Numerator = num;
            Denominator = den;
        }
    }

    public static class Conductor
    {
        // --- Public fields ---
        public static float BPM = 120f;
        public static float SongPosition = 0f; // in ms
        public static List<TimeSignature> TimeSignatures = new List<TimeSignature>
        {
            new TimeSignature(0f, 4f, 4f)
        };

        public static float MsPerBeat = 60000f / BPM;
        public static int CurrentSignatureIndex = 0;
        public static float CurrentNumerator = 4f;
        public static float CurrentDenominator = 4f;

        private static float LastBeat = -1f;
        private static float LastMeasure = -1f;

        // --- Events ---
        public static Action<float> OnBeat;
        public static Action<float> OnMeasure;

        // --- Methods ---
        public static void SetBPM(float value)
        {
            BPM = value;
            MsPerBeat = 60000f / BPM;
        }

        private static void UpdateSignature()
        {
            while (CurrentSignatureIndex + 1 < TimeSignatures.Count &&
                   SongPosition >= TimeSignatures[CurrentSignatureIndex + 1].Time)
            {
                CurrentSignatureIndex++;
            }

            var sig = TimeSignatures[CurrentSignatureIndex];
            CurrentNumerator = sig.Numerator;
            CurrentDenominator = sig.Denominator;
        }

        public static float GetBeat()
        {
            return SongPosition / MsPerBeat;
        }

        public static float GetMeasure()
        {
            UpdateSignature();
            var sigTime = TimeSignatures[CurrentSignatureIndex].Time;
            var timeSinceSig = SongPosition - sigTime;
            var beatsSinceSig = timeSinceSig / MsPerBeat;
            return beatsSinceSig / CurrentNumerator;
        }

        public static float GetMeasureBeat()
        {
            UpdateSignature();
            var sigTime = TimeSignatures[CurrentSignatureIndex].Time;
            var timeSinceSig = SongPosition - sigTime;
            var beatsSinceSig = timeSinceSig / MsPerBeat;
            return beatsSinceSig % CurrentNumerator;
        }

        public static void UpdatePosition(float newSongPosition)
        {
            SongPosition = newSongPosition;

            float currentBeat = GetBeat();
            float currentMeasure = GetMeasure();
       

            if (Math.Floor(currentBeat) != Math.Floor(LastBeat))
            {
                LastBeat = currentBeat;
                OnBeat?.Invoke(currentBeat);
            }

            if (Math.Floor(currentMeasure) != Math.Floor(LastMeasure))
            {
                LastMeasure = currentMeasure;
                OnMeasure?.Invoke(currentMeasure);
            }
        }
    }
}
