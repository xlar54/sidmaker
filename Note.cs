using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SidMaker
{
    enum NoteTypes
    {
        whole,
        half,
        quarter,
        eighth,
        sixteenth
    }

    enum NoteName
    {
        C,
        Csh,
        D,
        Dsh,
        E,
        F,
        Fsh,
        G,
        Gsh,
        A,
        Ash,
        B
    }

    enum Waveforms
    {
        Triangle,
        Sawtooth,
        Pulse
    }
    
    class Note
    {
        Dictionary<NoteName, double>[] noteFrequencies = new Dictionary<NoteName, double>[8];

        public Note(NoteName name, NoteTypes type, Waveforms waveform, int octave)
        {
            this.name = name;
            this.type = type;
            this.waveform = waveform;
            this.octave = octave;

            loadFrequencies();
        }

        private void loadFrequencies()
        {
            for (int x=0; x < 8;x++)
                noteFrequencies[x] = new Dictionary<NoteName, double>();

            noteFrequencies[0].Add(NoteName.C, 16.35);
            noteFrequencies[0].Add(NoteName.Csh, 17.32);
            noteFrequencies[0].Add(NoteName.D, 18.35);
            noteFrequencies[0].Add(NoteName.Dsh, 19.45);
            noteFrequencies[0].Add(NoteName.E, 20.6);
            noteFrequencies[0].Add(NoteName.F, 21.83);
            noteFrequencies[0].Add(NoteName.Fsh, 23.12);
            noteFrequencies[0].Add(NoteName.G, 24.5);
            noteFrequencies[0].Add(NoteName.Gsh, 25.96);
            noteFrequencies[0].Add(NoteName.A, 27.5);
            noteFrequencies[0].Add(NoteName.Ash, 29.14);
            noteFrequencies[0].Add(NoteName.B, 30.87);
            noteFrequencies[1].Add(NoteName.C, 32.7);
            noteFrequencies[1].Add(NoteName.Csh, 34.65);
            noteFrequencies[1].Add(NoteName.D, 36.71);
            noteFrequencies[1].Add(NoteName.Dsh, 38.89);
            noteFrequencies[1].Add(NoteName.E, 41.2);
            noteFrequencies[1].Add(NoteName.F, 43.65);
            noteFrequencies[1].Add(NoteName.Fsh, 46.25);
            noteFrequencies[1].Add(NoteName.G, 49);
            noteFrequencies[1].Add(NoteName.Gsh, 51.91);
            noteFrequencies[1].Add(NoteName.A, 55);
            noteFrequencies[1].Add(NoteName.Ash, 58.27);
            noteFrequencies[1].Add(NoteName.B, 61.74);
            noteFrequencies[2].Add(NoteName.C, 65.41);
            noteFrequencies[2].Add(NoteName.Csh, 69.3);
            noteFrequencies[2].Add(NoteName.D, 73.42);
            noteFrequencies[2].Add(NoteName.Dsh, 77.78);
            noteFrequencies[2].Add(NoteName.E, 82.41);
            noteFrequencies[2].Add(NoteName.F, 87.31);
            noteFrequencies[2].Add(NoteName.Fsh, 92.5);
            noteFrequencies[2].Add(NoteName.G, 98);
            noteFrequencies[2].Add(NoteName.Gsh, 103.83);
            noteFrequencies[2].Add(NoteName.A, 110);
            noteFrequencies[2].Add(NoteName.Ash, 116.54);
            noteFrequencies[2].Add(NoteName.B, 123.47);
            noteFrequencies[3].Add(NoteName.C, 130.81);
            noteFrequencies[3].Add(NoteName.Csh, 138.59);
            noteFrequencies[3].Add(NoteName.D, 146.83);
            noteFrequencies[3].Add(NoteName.Dsh, 155.56);
            noteFrequencies[3].Add(NoteName.E, 164.81);
            noteFrequencies[3].Add(NoteName.F, 174.61);
            noteFrequencies[3].Add(NoteName.Fsh, 185);
            noteFrequencies[3].Add(NoteName.G, 196);
            noteFrequencies[3].Add(NoteName.Gsh, 207.65);
            noteFrequencies[3].Add(NoteName.A, 220);
            noteFrequencies[3].Add(NoteName.Ash, 233.08);
            noteFrequencies[3].Add(NoteName.B, 246.94);
            noteFrequencies[4].Add(NoteName.C, 261.63);
            noteFrequencies[4].Add(NoteName.Csh, 277.18);
            noteFrequencies[4].Add(NoteName.D, 293.66);
            noteFrequencies[4].Add(NoteName.Dsh, 311.13);
            noteFrequencies[4].Add(NoteName.E, 329.63);
            noteFrequencies[4].Add(NoteName.F, 349.23);
            noteFrequencies[4].Add(NoteName.Fsh, 369.99);
            noteFrequencies[4].Add(NoteName.G, 392);
            noteFrequencies[4].Add(NoteName.Gsh, 415.3);
            noteFrequencies[4].Add(NoteName.A, 440);
            noteFrequencies[4].Add(NoteName.Ash, 466.16);
            noteFrequencies[4].Add(NoteName.B, 493.88);
            noteFrequencies[5].Add(NoteName.C, 523.25);
            noteFrequencies[5].Add(NoteName.Csh, 554.37);
            noteFrequencies[5].Add(NoteName.D, 587.33);
            noteFrequencies[5].Add(NoteName.Dsh, 622.25);
            noteFrequencies[5].Add(NoteName.E, 659.25);
            noteFrequencies[5].Add(NoteName.F, 698.46);
            noteFrequencies[5].Add(NoteName.Fsh, 739.99);
            noteFrequencies[5].Add(NoteName.G, 783.99);
            noteFrequencies[5].Add(NoteName.Gsh, 830.61);
            noteFrequencies[5].Add(NoteName.A, 880);
            noteFrequencies[5].Add(NoteName.Ash, 932.33);
            noteFrequencies[5].Add(NoteName.B, 987.77);
            noteFrequencies[6].Add(NoteName.C, 1046.5);
            noteFrequencies[6].Add(NoteName.Csh, 1108.73);
            noteFrequencies[6].Add(NoteName.D, 1174.66);
            noteFrequencies[6].Add(NoteName.Dsh, 1244.51);
            noteFrequencies[6].Add(NoteName.E, 1318.51);
            noteFrequencies[6].Add(NoteName.F, 1396.91);
            noteFrequencies[6].Add(NoteName.Fsh, 1479.98);
            noteFrequencies[6].Add(NoteName.G, 1567.98);
            noteFrequencies[6].Add(NoteName.Gsh, 1661.22);
            noteFrequencies[6].Add(NoteName.A, 1760);
            noteFrequencies[6].Add(NoteName.Ash, 1864.66);
            noteFrequencies[6].Add(NoteName.B, 1975.53);
            noteFrequencies[7].Add(NoteName.C, 2093);
            noteFrequencies[7].Add(NoteName.Csh, 2217.46);
            noteFrequencies[7].Add(NoteName.D, 2349.32);
            noteFrequencies[7].Add(NoteName.Dsh, 2489.02);
            noteFrequencies[7].Add(NoteName.E, 2637.02);
            noteFrequencies[7].Add(NoteName.F, 2793.83);
            noteFrequencies[7].Add(NoteName.Fsh, 2959.96);
            noteFrequencies[7].Add(NoteName.G, 3135.96);
            noteFrequencies[7].Add(NoteName.Gsh, 3322.44);
            noteFrequencies[7].Add(NoteName.A, 3520);
            noteFrequencies[7].Add(NoteName.Ash, 3729.31);
            noteFrequencies[7].Add(NoteName.B, 3951.07);
        }

        public NoteName name;
        public NoteTypes type;
        public Waveforms waveform;
        public int octave;

        public double frequency
        {
            get {
                return noteFrequencies[octave][name];
            }

        }
    }
}
