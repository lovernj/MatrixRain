using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics.CodeAnalysis;
namespace MatrixRain
{
    /// <summary>
    /// Run a screensaver inside a particular bounds until user input is detected
    /// </summary>
    public sealed partial class Screensaver : Form
    {
        const int DefaultRunners = 15;

        #region Win32 API functions

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        private static readonly Brush CursorBrush = new SolidBrush(Color.White);
        private static readonly Brush BgBrush = new SolidBrush(Color.FromArgb(6, 0, 10, 0));
        private static readonly Random rand = new();


        private readonly Font myFont;
        private readonly bool PreviewMode = false;
        private readonly int horiz_modifier;
        private readonly int vert_modifier;

        private Bitmap bitmap;


        private Rectangle NormalizedBounds;
        private Point mouseLocation;

        private int ticks = 0;
        private System.Threading.Timer TickTimer;
        delegate void tickhandler();

        private List<Runner> Runners;

        private BufferedGraphics? graphicsBuffer;
        private Graphics? gbGraphics;

        private static string GetRandChar() => Convert.ToChar(rand.Next(0xff01, 0xff9d)).ToString();
        private class Runner
        {
            public Runner(Screensaver parent)
            {
                horiz_mod = parent.horiz_modifier;
                vert_mod = parent.vert_modifier;
                horiz_mult = parent.Bounds.Width / horiz_mod;
                vert_mult = parent.Bounds.Height / vert_mod;
                Update();
            }
            public void Update()
            {
                Location.X = horiz_mod * rand.Next(horiz_mult);
                Location.Y = vert_mod * rand.Next(vert_mult);
                Velocity = rand.Next(1, 8);
            }
            public PointF GetLocInColumn()
            {
                return new PointF(Location.X,Location.Y - vert_mod * rand.Next(vert_mult / 4));
            }
            public static Runner operator++(Runner lhs)
            {
                lhs.Location.Y += lhs.vert_mod;
                lhs.LastCharacter = GetRandChar();
                return lhs;
            }

            public int Velocity {get; private set;}
            public PointF Location;
            public string LastCharacter = " ";
            private readonly int horiz_mod, horiz_mult, vert_mod, vert_mult;
        }



        #region constructors

        /// <summary>
        /// Initialize a <c>screensaver</c> to a specific bounds
        /// </summary>
        public Screensaver(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            myFont = new Font("Lucida Console", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);

            horiz_modifier = 12;
            vert_modifier = 10;

            LoadData();
        }
        /// <summary>
        /// Initialize a screensaver as a child of another window
        /// e.g. for previewing
        /// </summary>
        public Screensaver(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            int retval = SetWindowLong(Handle, -16, new IntPtr(GetWindowLong(Handle, -16) | 0x40000000));
            if (retval == 0) //SetWindowLong returns 0 on error. 
                Application.Exit();

            // Place our window inside the parent
            GetClientRect(PreviewWndHandle, out Rectangle ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);
            Bounds = ParentRect;

            // Make text smaller
            myFont = new Font("Lucida Console", 5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            horiz_modifier = 7;
            vert_modifier = 5;

            PreviewMode = true;
            LoadData();
        }
        #endregion
        /// <summary>
        /// Setup structures that can be determined before UI-time
        /// in a constructor-invariant way. 
        /// </summary>
        [MemberNotNull(nameof(bitmap),
                       nameof(Runners),
                       nameof(TickTimer))]
        private void LoadData()
        {
            NormalizedBounds = new Rectangle(0, 0, Bounds.Width, Bounds.Height);
            bitmap = new Bitmap(Bounds.Width, Bounds.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int concurrentRunners = DefaultRunners;
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\MatrixRainScreensaver");
            // Try to set concurrentRunners from the registry. Or if it doesn't exist, setup the registry
            var keyvalue = key.GetValue("ConcurrentRunners");

            if (keyvalue == null)
                key.SetValue("ConcurrentRunners", DefaultRunners, RegistryValueKind.DWord);
            else
                concurrentRunners = (int)keyvalue;

            //setup the runner structs.
            Runners = new List<Runner>();
            for (int ii = 0; ii < concurrentRunners; ii++)
                Runners.Add(new Runner(this));

            // start the tick timer. 
            TickTimer = new System.Threading.Timer(new TimerCallback(_ => Invoke((MethodInvoker)delegate () { TimerTick(); })));
        }

        /// <summary>
        /// Step the simulation along one tick.
        /// </summary>
        private void TimerTick()
        {
            ticks++;
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Once every six ticks, darken the screen a little. 
                // FillRectangle is slow, so having BgBrush be higher alpha and doing 
                // this less frequently is more performant.
                if ((ticks % 6) == 0)
                    g.FillRectangle(BgBrush, NormalizedBounds);

                for (int ii = 0; ii < Runners.Count; ii++)
                {
                    Runner runner = Runners[ii];
                    if ((ticks % runner.Velocity) == 0)
                    {
                        if (runner.Location.Y > Bounds.Bottom)
                            runner.Update();

                        using (Brush tmpGreen = new SolidBrush(Color.FromArgb(255, 0, rand.Next(90, 170), 0)))
                        {
                            if (rand.Next(15) == 1)
                                g.DrawString(GetRandChar(), myFont, tmpGreen, runner.GetLocInColumn());
                            g.DrawString(runner.LastCharacter, myFont, tmpGreen, runner.Location);
                        }
                        //Update the location and character
                        runner++;
                    }
                    g.DrawString(runner.LastCharacter, myFont, CursorBrush, runner.Location);

                    //Copy it back
                    Runners[ii] = runner;
                }
            }
            // Draw the image to the graphics buffer. 
            // Most of the CPU time is spent in this function
            // but like... I can't really change that
            gbGraphics.DrawImage(bitmap, 0, 0);
            graphicsBuffer.Render(CreateGraphics());
        }
        #region Overrided Events
        protected override void OnPaintBackground(PaintEventArgs e){}
        protected override void OnPaint(PaintEventArgs e){}

        /// <summary>
        /// Setup the remaining structures after the UI has finished loading
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //Setup the graphics contexts necessary
            using (Graphics g = CreateGraphics())
                graphicsBuffer = BufferedGraphicsManager
                                 .Current
                                 .Allocate(g, NormalizedBounds);

            gbGraphics = graphicsBuffer.Graphics;
            gbGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            gbGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Fill the screen with preformatted text
                // so when text glows it doesn't look so weird
                for (int x = 0; x < Bounds.Width / horiz_modifier; x++)
                    for (int y = 0; y < Bounds.Height / vert_modifier; y++)
                        using (Brush tmpGreen = new SolidBrush(Color.FromArgb(255, 0, rand.Next(90, 170), 0)))
                            g.DrawString(GetRandChar(), myFont, tmpGreen, new PointF(x * horiz_modifier, y * vert_modifier));
                // Apply a heavy alpha-wash so it doesn't stand out so much. 
                g.FillRectangle(new SolidBrush(Color.FromArgb(225, 0, 10, 0)), NormalizedBounds);
            }

            TickTimer.Change(0, 25);
            Cursor.Hide();
            TopMost = true;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!PreviewMode)
                Application.Exit();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!PreviewMode)
            {
                // Terminate if mouse is moved a significant distance
                if (!mouseLocation.IsEmpty &&
                    (Math.Abs(mouseLocation.X - e.X) > 5 ||
                     Math.Abs(mouseLocation.Y - e.Y) > 5))
                    Application.Exit();
                // Update current mouse location
                mouseLocation = e.Location;
            }
        }
        #endregion

    }
}
