using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Camera
    {

        public float Xoffset { set; get; }
        public float Yoffset { set; get; }
        
        private readonly static int MAX_FRAME_RATE = 60;    // Frames per Second
        private const short sampleSize = 100;
        private int lastTime = 0;
        private int numRenders = 0;
        private bool _Vsync = true;

        private TextBlock fpsMeter = new TextBlock(null)
        {
            X = 1,
            Y = -1,
            Z = 128,
            height = 5,
            width = 20,
            VAlignment = TextBlock.VerticalAlignment.Bottom,
            HAlignment = TextBlock.HorizontalAlignment.Left,
            Text = "0",
            IsGUI = true
        };

        private byte[] toRenderData = new byte[3 * Engine.Render.RenderWidth * Engine.Render.RenderHeight];

        public IPage sceneReference { get { return Engine.Page; } }

        Thread Ren;
        private bool RenderAbort = false;

        public Camera()
        {
            Engine._baseCam = this;
        }

        public void Init(int Xoffset, int Yoffset)
        {
            this.Xoffset = Xoffset;
            this.Yoffset = Yoffset;

            //fpsMeter.text = "";
            Engine.ToRender.Add(fpsMeter);
            
            Ren = new Thread(() => Rendering());
            Ren.Start();
        }

        public void Abort()
        {
            if (Ren != null)
            {
                RenderAbort = true;
            }
        }

        private unsafe void Rendering()
        {

            using (Graphics g = Graphics.FromHwnd(GetConsoleWindow()))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                while (!RenderAbort)
                {
                    int beginRender = Environment.TickCount;
                    
                    Point location = new Point(0, 0);
                    Size imageSize = new Size(Console.WindowWidth, Console.WindowHeight); // desired image size in characters
                    
                    fixed (byte* ptr = toRenderData)
                    {

                        using (Bitmap outFrame = new Bitmap(Engine.Render.RenderWidth,
                                                            Engine.Render.RenderHeight,
                                                            3 * Engine.Render.RenderWidth,
                                                            System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                                            new IntPtr(ptr)))
                        {
                            Size fontSize = GetConsoleFontSize();

                            Rectangle imageRect = new Rectangle(location.X * fontSize.Width,
                                                                location.Y * fontSize.Height,
                                                                imageSize.Width * fontSize.Width,
                                                                imageSize.Height * fontSize.Height);

                            g.DrawImage(outFrame, imageRect);
                        }
                    }

                    int endRender = Environment.TickCount - beginRender;

                    if(_Vsync) Vsync(MAX_FRAME_RATE, endRender, false);
                }
            }
        }

        internal void BufferImage()
        {
            Array.Clear(Engine.Render.imageBuffer, 0, Engine.Render.imageBuffer.Length);
            Array.Clear(Engine.Render.imageBufferKey, 0, Engine.Render.imageBufferKey.Length);

            List<I3Dimensional> Temp = null;

            lock (Engine.ToRender)
            {
                Temp = Engine.ToRender.Where(item => ((I3Dimensional)item).IsInView()).ToList<I3Dimensional>(); 
            }

            List<I3Dimensional> GUI = Temp.Where(item => ((IGraphics)item).IsGUI);

            for (int i = 0; i < GUI.Count; i++)
                Temp.Remove(GUI[i]);

            while (GUI.Count > 0)
            {
                float tempHeight = GUI.FindMaxZ();
                List<I3Dimensional> toRender = GUI.Where(item => item.Z == tempHeight).ToList();

                Parallel.For(0, toRender.Count, (i) =>
                {
                    ((ICore)toRender[i]).Render();
                });

                for (int i = 0; i < toRender.Count; i++)
                    GUI.Remove(toRender[i]);
            }

            while(Temp.Count > 0)
            {
                float tempHeight = Temp.FindMaxZ();
                List<I3Dimensional> toRender = Temp.Where(item => item.Z == tempHeight).ToList();

                Parallel.For(0, toRender.Count, (i) =>
                {
                    ((ICore)toRender[i]).Render();
                });

                for (int i = 0; i < toRender.Count; i++)
                    Temp.Remove(toRender[i]);
            }
            
            Buffer.BlockCopy(Engine.Render.imageBuffer, 0, toRenderData, 0, Engine.Render.imageBuffer.Length);

            int endRender = Environment.TickCount - Engine.UpdateStartTime;
            
            if (numRenders == 0)
            {
                lastTime = Environment.TickCount;
            }

            numRenders++;

            if(_Vsync) Vsync(MAX_FRAME_RATE, endRender, true);
        }

        private void Vsync(int TargetFrameRate, int imageRenderDelay, bool renderFPS)
        {
            int targetDelay = 1000 / TargetFrameRate;

            if (imageRenderDelay < targetDelay)
            {
                //Thread.Sleep(targetDelay - imageRenderDelay);
            }

            if (renderFPS)
            {
                if (numRenders == sampleSize)
                {
                    int temp = Environment.TickCount - lastTime;

                    if (temp > 0)
                    {
                        fpsMeter.Text = string.Format("{0}", sampleSize * 1000 / temp);
#if DEBUG
                        //Debug.WriteLine(string.Format("Buff {0}", sampleSize * 1000 / temp));
#endif
                    }

                    numRenders = 0;
                }
            }
        }




        private static Size GetConsoleFontSize()
        {
            // getting the console out buffer handle
            IntPtr outHandle = CreateFile("CONOUT$",
                                           GENERIC_READ | GENERIC_WRITE,
                                           FILE_SHARE_READ | FILE_SHARE_WRITE,
                                           IntPtr.Zero,
                                           OPEN_EXISTING,
                                           0,
                                           IntPtr.Zero);

            int errorCode = Marshal.GetLastWin32Error();
            if (outHandle.ToInt32() == INVALID_HANDLE_VALUE)
            {
                throw new IOException("Unable to open CONOUT$", errorCode);
            }

            ConsoleFontInfo cfi = new ConsoleFontInfo();

            if (!GetCurrentConsoleFont(outHandle, false, cfi))
            {
                throw new InvalidOperationException("Unable to get font information.");
            }

            return new Size(cfi.dwFontSize.X, cfi.dwFontSize.Y);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            int dwDesiredAccess,
            int dwShareMode,
            IntPtr lpSecurityAttributes,
            int dwCreationDisposition,
            int dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetCurrentConsoleFont(
            IntPtr hConsoleOutput,
            bool bMaximumWindow,
            [Out][MarshalAs(UnmanagedType.LPStruct)]ConsoleFontInfo lpConsoleCurrentFont);

        [StructLayout(LayoutKind.Sequential)]
        internal class ConsoleFontInfo
        {
            internal int nFont;
            internal Coord dwFontSize;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct Coord
        {
            [FieldOffset(0)]
            internal short X;
            [FieldOffset(2)]
            internal short Y;
        }

        private const int GENERIC_READ = unchecked((int)0x80000000);
        private const int GENERIC_WRITE = 0x40000000;
        private const int FILE_SHARE_READ = 1;
        private const int FILE_SHARE_WRITE = 2;
        private const int INVALID_HANDLE_VALUE = -1;
        private const int OPEN_EXISTING = 3;
    }

}
