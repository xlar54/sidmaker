using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SidMaker
{
    class Song
    {
        public static List<Voice> voices = new List<Voice>();
        public static int tempo;

        public Song()
        {
            Voice v1 = new Voice();
            Voice v2 = new Voice();
            Voice v3 = new Voice();

            voices.Add(v1);
            voices.Add(v2);
            voices.Add(v3);
        }

        public void AddNote(int voice, Note note)
        {
            voices[voice].notes.Add(note);
        }

        public void Play(int startingMeasure)
        {
            Thread t0 = new Thread(new ParameterizedThreadStart(PlayVoice));
            Thread t1 = new Thread(new ParameterizedThreadStart(PlayVoice));
            Thread t2 = new Thread(new ParameterizedThreadStart(PlayVoice));

            t0.Start(0);
            t1.Start(1);
            t2.Start(2);
        }

        public static void PlaySingle(Note n)
        {
            Thread t = new Thread(new ParameterizedThreadStart(PlaySingleNote));
            t.Start(n);
        }

        static void PlayVoice(Object n)
        {
            int voice = (int)n;

            foreach (Note note in voices[voice].notes)
            {
                PlaySingleNote(note);
            }
        }

        public static void PlaySingleNote(Object n)
        {
            Note note = (Note)n;

            int msduration = getDuration(note);

            SignalGeneratorType t = SignalGeneratorType.Triangle;

            if(note.waveform == Waveforms.Triangle)
                t = SignalGeneratorType.Triangle;
            else if (note.waveform == Waveforms.Sawtooth)
                t = SignalGeneratorType.SawTooth;
            else if (note.waveform == Waveforms.Pulse)
                t = SignalGeneratorType.Square;


            var signal = new NAudio.Wave.SampleProviders.SignalGenerator()
            {
                Gain = 0.8,
                Frequency = note.frequency,
                Type = t
            }.Take(TimeSpan.FromMilliseconds(msduration));

            using (var wo = new WaveOutEvent())
            {
                wo.Init(signal);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    //Thread.Sleep(1);
                }
            }

        }

        static int getDuration(Note note)
        {
            int msduration = 60000 / tempo;

            switch (note.type)
            {
                case NoteTypes.whole: { msduration *= 4; break; }
                case NoteTypes.half: { msduration *= 2; break; }
                case NoteTypes.quarter: { break; }
                case NoteTypes.eighth: { msduration /= 2; break; }
                case NoteTypes.sixteenth: { msduration /= 4; break; }
            }

            return msduration;
        }
    }
}
