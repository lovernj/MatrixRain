using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
namespace MatrixRain
{
    /// <summary>
    /// Run a screensaver inside a particular bounds until user input is detected
    /// </summary>
    public sealed partial class Screensaver : Form
    {
        private const int DefaultRunners = 15;
        private const int DefaultMsPerTick = 20;

#if DEBUG
        private const int DebugDefaultRunners = 70;
        private const int DebugDefaultMsPerTick = 5;
#endif

        #region Win32 API functions
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        private class Runner
        {
            public int Velocity { get; private set; }
            public PointF Location;
            public string LastCharacter = " ";
            private readonly int horiz_mod, horiz_mult, vert_mod, vert_mult;
            public PointF GetLocInColumn() => new(Location.X, Location.Y - vert_mod * rand.Next(vert_mult / 4));
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
                Velocity = rand.Next(5, 20);
            }
            public static Runner operator ++(Runner lhs)
            {
                lhs.Location.Y += lhs.vert_mod;
                lhs.LastCharacter = GetRandChar();
                return lhs;
            }
        }

        #region Variables
        private readonly Font myFont;
        private readonly bool PreviewMode = false;
        private readonly int horiz_modifier;
        private readonly int vert_modifier;

        private int ticks = 0;
        private int MsPerTick;
        private System.Threading.Timer TickTimer;
        private System.Threading.Timer DrawTimer;

        //Graphics objects
        private BufferedGraphics? graphicsBuffer;
        private Bitmap bitmap;

        //simulation objects
        private List<Runner> Runners;

        private delegate void tickhandler();

        private Rectangle NormalizedBounds;
        private Point mouseLocation; //Track the mouse to see if it's moved more than 5px recently

        private static readonly Brush CursorBrush = new SolidBrush(Color.White);
        private static readonly Brush BgBrush = new SolidBrush(Color.FromArgb(6, 0, 20, 0));
        private static readonly Random rand = new(); //NOT SECURE! Don't use it for crypto. 
        private static string GetRandChar() => Convert.ToChar(rand.Next(0xff01, 0xff9d)).ToString();
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize a <c>screensaver</c> to a specific bounds
        /// </summary>
        public Screensaver(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            myFont = new Font("Lucida Console", 8F);
            
            horiz_modifier = 11;
            vert_modifier = 9;

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
            if (retval == 0) //SetWindowLong returns 0 on error. If it errors, we're fubar'd and should probably just fail out tbh.
            {
                Application.Exit();
            }

            // Place our window inside the parent
            GetClientRect(PreviewWndHandle, out Rectangle ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);
            Bounds = ParentRect;

            // Make text smaller
            myFont = new Font("Lucida Console", 5F);
            horiz_modifier = 7;
            vert_modifier = 5;

            PreviewMode = true;
            LoadData();
        }
        #endregion
        /// <summary>
        /// Get a value from the registry and convert it to a <typeparamref name="T"/>. 
        /// This method is extremely tolerant and will always try to return a coherent value. 
        /// Don't make it. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subkey">HKEY_CURRENT_USER\subkey</param>
        /// <param name="value">The dictionary value to retrieve from </param>
        /// <param name="defaultValue">If in any case you cannot retrieve a value, return this instead.</param>
        /// <returns>The value of the registry, or defaultValue if it doesn't exist, or the default value of T if that doesn't exist.</returns>
        private static T? GetFromRegistry<T>(string subkey, string value, T? defaultValue)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(subkey);
            if (key==null)
            {
                if (defaultValue == null)
                    return default;
                return defaultValue;
            }
            object? keyvalue = key.GetValue(value);
            if (keyvalue == null)
                return defaultValue;
            return (T)keyvalue;
        }

        /// <summary>
        /// Setup structures that can be determined before UI-time
        /// in a constructor-invariant way. 
        /// </summary>
        /// 
        [MemberNotNull(nameof(bitmap),
                       nameof(Runners),
                       nameof(TickTimer),
                       nameof(DrawTimer),
                       nameof(MsPerTick))]
        private void LoadData()
        {
            NormalizedBounds = new Rectangle(0, 0, Bounds.Width, Bounds.Height);
            bitmap = new Bitmap(Bounds.Width, Bounds.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);


#if DEBUG
            int concurrentRunners = DebugDefaultRunners;
            MsPerTick = DebugDefaultMsPerTick;
#else
            int concurrentRunners = GetFromRegistry("SOFTWARE\\MatrixRainScreensaver", "ConcurrentRunners", DefaultRunners);
            MsPerTick = GetFromRegistry("SOFTWARE\\MatrixRainScreensaver", "MsPerTick", DefaultMsPerTick);
#endif
            //setup the runner structs.
            Runners = new List<Runner>();
            for (int ii = 0; ii < concurrentRunners; ii++)
                Runners.Add(new Runner(this));

            // start the tick timer. 
            TickTimer = new System.Threading.Timer(new TimerCallback(_ => Invoke((MethodInvoker)delegate () { TimerTick(); })));
            DrawTimer = new System.Threading.Timer(new TimerCallback(_ => Invalidate()));
        }

        /// <summary>
        /// Step the simulation along one tick.
        /// </summary>
        private void TimerTick()
        {
            ticks++;
            using Graphics g = Graphics.FromImage(bitmap);
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
                    if (runner.Location.Y >= Bounds.Bottom)
                        runner.Update();

                    using (Brush tmpGreen = new SolidBrush(Color.FromArgb(255, 20, rand.Next(20, 255), 20)))
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
            // Draw the image to the graphics buffer. 
            // Most of the CPU time is spent in this function
            // but like... I can't really change that
        }
#region Overrided Events
        protected override void OnPaintBackground(PaintEventArgs e) { }
        protected override void OnPaint(PaintEventArgs e) {
            Graphics gbGraphics = graphicsBuffer.Graphics;
            gbGraphics.DrawImage(bitmap, 0, 0);
            graphicsBuffer.Render(e.Graphics);
        }

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

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Fill the screen with preformatted text
                // so when text glows it doesn't look so weird
                for (int x = 0; x < Bounds.Width / horiz_modifier; x++)
                    for (int y = 0; y < Bounds.Height / vert_modifier; y++)
                        using (Brush tmpGreen = new SolidBrush(Color.FromArgb(255, 0, rand.Next(90, 170), 0)))
                            g.DrawString(GetRandChar(), myFont, tmpGreen, new PointF(x * horiz_modifier, y * vert_modifier));
                // Apply a heavy alpha-wash so it doesn't stand out so much. 
                g.FillRectangle(new SolidBrush(Color.FromArgb(225, 0, 0, 0)), NormalizedBounds);
            }

            TickTimer.Change(0, MsPerTick); 
            DrawTimer.Change(0, 41); //24fps. (ish). Roughly the speed a movie runs at
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
