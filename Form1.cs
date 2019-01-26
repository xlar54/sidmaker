using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SidMaker
{
    public partial class Form1 : Form
    {
        Bitmap bmp = null;
        Song song = new Song();
        int editVoice = 1;
        Note lastNote = null;
        int selectedNote = -1;

        public Form1()
        {
            InitializeComponent();

            this.MouseWheel += Form_MouseWheel;

            bmp = new Bitmap(pnlSheet.Width, pnlSheet.Height);
            pnlSheet.BackgroundImage = bmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.PreviewKeyDown += new PreviewKeyDownEventHandler(control_PreviewKeyDown);
            }
        }

        void control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }

        void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            int cv = editVoice - 1;

            if (Song.voices[cv].notes.Count > 0)
            {

                if (e.Delta > 0)
                {
                    if (Song.voices[cv].notes[selectedNote].name == NoteName.C)
                        Song.voices[cv].notes[selectedNote].name = NoteName.D;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.D)
                        Song.voices[cv].notes[selectedNote].name = NoteName.E;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.E)
                        Song.voices[cv].notes[selectedNote].name = NoteName.F;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.F)
                        Song.voices[cv].notes[selectedNote].name = NoteName.G;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.G)
                        Song.voices[cv].notes[selectedNote].name = NoteName.A;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.A)
                        Song.voices[cv].notes[selectedNote].name = NoteName.B;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.B)
                    {
                        Song.voices[cv].notes[selectedNote].octave++;
                        Song.voices[cv].notes[selectedNote].name = NoteName.C;
                    }
                }

                if (e.Delta < 0)
                {
                    if (Song.voices[cv].notes[selectedNote].name == NoteName.B)
                        Song.voices[cv].notes[selectedNote].name = NoteName.A;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.A)
                        Song.voices[cv].notes[selectedNote].name = NoteName.G;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.G)
                        Song.voices[cv].notes[selectedNote].name = NoteName.F;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.F)
                        Song.voices[cv].notes[selectedNote].name = NoteName.E;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.E)
                        Song.voices[cv].notes[selectedNote].name = NoteName.D;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.D)
                        Song.voices[cv].notes[selectedNote].name = NoteName.C;
                    else if (Song.voices[cv].notes[selectedNote].name == NoteName.C)
                    {
                        Song.voices[cv].notes[selectedNote].octave--;
                        Song.voices[cv].notes[selectedNote].name = NoteName.B;
                    }
                }

                pnlSheet.Invalidate();

                Song.tempo = Convert.ToInt32(numTempo.Value);
                Note note = new Note(Song.voices[cv].notes[selectedNote].name, NoteTypes.eighth, Song.voices[cv].notes[selectedNote].waveform, Song.voices[cv].notes[selectedNote].octave);
                Song.PlaySingle(note);

            }
        }

        private void pnlSheet_Paint(object sender, PaintEventArgs e)
        {
            Pen linePen = new Pen(Color.Black, 3f);
            int middleCOffset = 109;
            int noteStep = 8;
            int lineStep = 16;
            int spacing = 70;
            int noteSpacing = 40;
            int measureStep = noteSpacing * 4 + (noteSpacing / 2);
            int measureSpacing = measureStep;

            // draw treble and bass clefts
            e.Graphics.DrawImage(SidMaker.Properties.Resources.treble, 5, 55);
            e.Graphics.DrawLine(linePen, new Point(1, 70), new Point(1, 230));
            e.Graphics.DrawImage(SidMaker.Properties.Resources.bass, 5, 145);
            

            // draw treble staff lines
            for (int x = 0; x < 5; x++)
            {
                e.Graphics.DrawLine(linePen, new Point(0, spacing), new Point(pnlSheet.Size.Width, spacing));
                spacing += lineStep;
            }

            spacing += lineStep;

            // draw cleft staff lines
            for (int x = 0; x < 5; x++)
            {
                e.Graphics.DrawLine(linePen, new Point(0, spacing), new Point(pnlSheet.Size.Width, spacing));
                spacing += lineStep;
            }

            // draw measures
            /*for (int x = 0; x < 3; x++)
            {
                e.Graphics.DrawLine(linePen, new Point(measureSpacing, 70), new Point(measureSpacing, pnlSheet.Size.Height));
                measureSpacing += measureStep;
            }*/

            // draw the notes
            for (int currentVoice = 0; currentVoice < 3; currentVoice++)
            {
                int xloc = 60;
                int yloc = 0;

                for(int i=0; i<Song.voices[currentVoice].notes.Count; i++)
                {
                    Note n = Song.voices[currentVoice].notes[i];
                    int noteId = getNoteID(n);
                    yloc = middleCOffset - (noteStep * noteId);
                    Image img = getNoteImage(currentVoice + 1, n);

                    if (noteId > 6)
                    {
                        img.RotateFlip(RotateFlipType.Rotate90FlipX);
                        img.RotateFlip(RotateFlipType.Rotate90FlipY);
                        yloc = yloc + img.Height-lineStep;
                    }

                    e.Graphics.DrawImage(img, new Point(xloc, yloc));

                    if(editVoice-1 == currentVoice && i == selectedNote) //Song.voices[currentVoice].notes.Count-1)
                    {
                        // draw current note marker
                        e.Graphics.DrawImage(imageList1.Images[0], new Point(xloc+img.Width/2 - imageList1.Images[0].Width/2, 5));
                    }

                    if (noteId == 0)
                    {
                        e.Graphics.DrawLine(linePen, new Point(xloc + 10, yloc + 41), new Point(xloc + 37, yloc + 41));
                    }

                    if (noteId > 11)
                    {
                        while (noteId > 11)
                        {
                            if (noteId % 2 == 0)
                            {
                                yloc = middleCOffset - (noteStep * noteId);
                                e.Graphics.DrawLine(linePen, new Point(xloc + 10, yloc + 41), new Point(xloc + 37, yloc + 41));
                            }
                            noteId--;
                        }
                    }
                    else if (noteId < -11)
                    {
                        while (noteId < -11)
                        {
                            if (noteId % 2 == 0)
                            {
                                yloc = middleCOffset - (noteStep * noteId);
                                e.Graphics.DrawLine(linePen, new Point(xloc + 10, yloc + 41), new Point(xloc + 37, yloc + 41));
                            }
                            noteId++;
                        }
                    }

                    xloc += noteSpacing;
                }
            }

        }

        private int getNoteID(Note n)
        {
            
            int noteId = 0;

            if (n.octave == 1)
            {
                switch (n.name)
                {
                    case NoteName.C: noteId = -21; break;
                    case NoteName.D: noteId = -20; break;
                    case NoteName.E: noteId = -19; break;
                    case NoteName.F: noteId = -18; break;
                    case NoteName.G: noteId = -17; break;
                    case NoteName.A: noteId = -16; break;
                    case NoteName.B: noteId = -15; break;

                }
            }
            else if (n.octave == 2)
            {
                switch (n.name)
                {
                    case NoteName.C: noteId = -14; break;
                    case NoteName.D: noteId = -13; break;
                    case NoteName.E: noteId = -12; break;
                    case NoteName.F: noteId = -11; break;
                    case NoteName.G: noteId = -10; break;
                    case NoteName.A: noteId = -9; break;
                    case NoteName.B: noteId = -8; break;

                }
            }
            else if (n.octave == 3)
            {
                switch (n.name)
                {
                    case NoteName.C: noteId = -7; break;
                    case NoteName.D: noteId = -6; break;
                    case NoteName.E: noteId = -5; break;
                    case NoteName.F: noteId = -4; break;
                    case NoteName.G: noteId = -3; break;
                    case NoteName.A: noteId = -2; break;
                    case NoteName.B: noteId = -1; break;

                }
            }
            else if (n.octave == 4)
            {
                switch (n.name)
                {
                    case NoteName.C: noteId = 0; break;
                    case NoteName.D: noteId = 1; break;
                    case NoteName.E: noteId = 2; break;
                    case NoteName.F: noteId = 3; break;
                    case NoteName.G: noteId = 4; break;
                    case NoteName.A: noteId = 5; break;
                    case NoteName.B: noteId = 6; break;

                }
            }
            else if (n.octave == 5)
            {
                switch (n.name)
                {
                    case NoteName.C: noteId = 7; break;
                    case NoteName.D: noteId = 8; break;
                    case NoteName.E: noteId = 9; break;
                    case NoteName.F: noteId = 10; break;
                    case NoteName.G: noteId = 11; break;
                    case NoteName.A: noteId = 12; break;
                    case NoteName.B: noteId = 13; break;

                }
            }
            else if (n.octave == 6)
            {
                switch (n.name)
                {
                    case NoteName.C: noteId = 14; break;
                    case NoteName.D: noteId = 15; break;
                    case NoteName.E: noteId = 16; break;
                    case NoteName.F: noteId = 17; break;
                    case NoteName.G: noteId = 18; break;
                    case NoteName.A: noteId = 19; break;
                    case NoteName.B: noteId = 20; break;
                }
            }

            return noteId;
        }

        private Image getNoteImage(int voice, Note n)
        {
            Image i = null;

            if (n.type == NoteTypes.whole)
                i = recolorNote(voice, btnWholeNote.Image);
            else if (n.type == NoteTypes.half)
                i= recolorNote(voice, btnHalfNote.Image);
            else if (n.type == NoteTypes.quarter)
                i = recolorNote(voice, btnQuarterNote.Image);
            else if (n.type == NoteTypes.eighth)
                i = recolorNote(voice, btn8thNote.Image);
            else if (n.type == NoteTypes.sixteenth)
                i = recolorNote(voice, btn16thNote.Image);

            return i;
        }

        public Image recolorNote(int voice, Image img)
        {
            Bitmap scrBitmap = (Bitmap)img;
            Color newColor = Color.Black;
            Color actualColor;

            if (voice == 1) newColor = Color.Blue;
            else if (voice == 2) newColor = Color.Red;
            else if (voice == 3) newColor = Color.Green;
            else if (voice == 4) newColor = Color.DarkGray;

            //make an empty bitmap the same size as scrBitmap
            Bitmap newBitmap = new Bitmap(scrBitmap.Width, scrBitmap.Height);
            for (int i = 0; i < scrBitmap.Width; i++)
            {
                for (int j = 0; j < scrBitmap.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actualColor = scrBitmap.GetPixel(i, j);
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actualColor.A > 150)
                        newBitmap.SetPixel(i, j, newColor);
                    else
                        newBitmap.SetPixel(i, j, actualColor);
                }
            }
            return newBitmap;
        }

        private void DrawLine(PaintEventArgs e, int x1, int y1, int x2, int y2)
        {
            Pen linePen = new Pen(Color.Black, 3f);
            e.Graphics.DrawLine(linePen, new Point(x1, y1), new Point(x2, y2));
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Song.tempo = Convert.ToInt32(numTempo.Value);
            song.Play(0);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Song.voices[0].notes.Clear();
            Song.voices[1].notes.Clear();
            Song.voices[2].notes.Clear();
            //Song.voices[3].notes.Clear();
            pnlSheet.Invalidate();
        }


        #region DrawNotes

        private void RenderNote(NoteTypes type, Waveforms waveform)
        {
            NoteName newNoteName = NoteName.E;
            int octave = 4;

            if(lastNote != null)
            {
                newNoteName = lastNote.name;
                octave = lastNote.octave;
            }

            Note note = new Note(newNoteName, type, waveform, octave);
            song.AddNote(editVoice-1, note);

            lastNote = note;
            selectedNote++;

            pnlSheet.Invalidate();

            Song.tempo = Convert.ToInt32(numTempo.Value);
            note = new Note(newNoteName, NoteTypes.eighth, waveform, octave);
            Song.PlaySingleNote(note);
        }

        private void btnWholeNote_Click(object sender, EventArgs e)
        {
            RenderNote(NoteTypes.whole, Waveforms.Triangle);
        }

        private void btnHalfNote_Click(object sender, EventArgs e)
        {
            RenderNote(NoteTypes.half, Waveforms.Triangle);
        }

        private void btnQuarterNote_Click(object sender, EventArgs e)
        {
            RenderNote(NoteTypes.quarter, Waveforms.Triangle);
        }

        private void btn8thNote_Click(object sender, EventArgs e)
        {
            RenderNote(NoteTypes.eighth, Waveforms.Triangle);
        }

        private void btn16thNote_Click(object sender, EventArgs e)
        {
            RenderNote(NoteTypes.sixteenth, Waveforms.Triangle);
        }

        #endregion

        #region SelectVoice

        private void btnVoice1_Click(object sender, EventArgs e)
        {
            editVoice = 1;
            btnVoice1.BackColor = Color.Blue;
            btnVoice2.BackColor = Color.LightGray;
            btnVoice3.BackColor = Color.LightGray;
            btnVoice4.BackColor = Color.LightGray;

            btnVoice1.ForeColor = Color.White;
            btnVoice2.ForeColor = Color.Black;
            btnVoice3.ForeColor = Color.Black;
            btnVoice4.ForeColor = Color.Black;

            selectedNote = Song.voices[editVoice-1].notes.Count-1;

            pnlSheet.Invalidate();
        }

        private void btnVoice2_Click(object sender, EventArgs e)
        {
            editVoice = 2;
            btnVoice1.BackColor = Color.LightGray;
            btnVoice2.BackColor = Color.Red;
            btnVoice3.BackColor = Color.LightGray;
            btnVoice4.BackColor = Color.LightGray;

            btnVoice1.ForeColor = Color.Black;
            btnVoice2.ForeColor = Color.White;
            btnVoice3.ForeColor = Color.Black;
            btnVoice4.ForeColor = Color.Black;

            selectedNote = Song.voices[editVoice - 1].notes.Count-1;

            pnlSheet.Invalidate();
        }

        private void btnVoice3_Click(object sender, EventArgs e)
        {
            editVoice = 3;
            btnVoice1.BackColor = Color.LightGray;
            btnVoice2.BackColor = Color.LightGray;
            btnVoice3.BackColor = Color.Green;
            btnVoice4.BackColor = Color.LightGray;

            btnVoice1.ForeColor = Color.Black;
            btnVoice2.ForeColor = Color.Black;
            btnVoice3.ForeColor = Color.White;
            btnVoice4.ForeColor = Color.Black;

            selectedNote = Song.voices[editVoice - 1].notes.Count-1;

            pnlSheet.Invalidate();
        }

        private void btnVoice4_Click(object sender, EventArgs e)
        {
            editVoice = 4;
            btnVoice1.BackColor = Color.LightGray;
            btnVoice2.BackColor = Color.LightGray;
            btnVoice3.BackColor = Color.LightGray;
            btnVoice4.BackColor = Color.Yellow;

            selectedNote = Song.voices[editVoice - 1].notes.Count-1;

            pnlSheet.Invalidate();
        }
        #endregion

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            int cv = editVoice - 1;

            if (e.KeyCode == Keys.Delete)
            {
                if (selectedNote > -1)
                {
                    if (selectedNote == Song.voices[cv].notes.Count - 1)
                    {
                        Song.voices[cv].notes.RemoveAt(selectedNote);
                        selectedNote--;
                    }
                    else if(selectedNote < Song.voices[cv].notes.Count - 1)
                    {
                        Song.voices[cv].notes.RemoveAt(selectedNote);

                    }
                    pnlSheet.Invalidate();
                }
            }

            if(e.KeyCode == Keys.Left)
            {
                if (selectedNote > 0)
                {
                    selectedNote--;
                    pnlSheet.Invalidate();
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                if (selectedNote < Song.voices[cv].notes.Count-1)
                {
                    selectedNote++;
                    pnlSheet.Invalidate();
                }
            }
        }
    }
}
