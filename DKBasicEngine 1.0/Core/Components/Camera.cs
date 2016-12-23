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

        public float Xoffset = 0;
        public float Yoffset = 0;
        
        private const short sampleSize = 100;
        private int lastTime = 0;
        private int numRenders = 0;
        private bool renderFPS = true;

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

        public void Destroy()
        {
            if (Ren != null)
                RenderAbort = true;

            fpsMeter.Destroy();
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
            
            if (numRenders == 0)
            {
                lastTime = Environment.TickCount;
            }

            numRenders++;

            if (renderFPS)
            {
                if (numRenders == sampleSize)
                {
                    int temp = Environment.TickCount - lastTime;

                    if (temp > 0)
                    {
                        fpsMeter.Text = string.Format("{0}", sampleSize * 1000 / temp);
#if DEBUG
                        Debug.WriteLine(string.Format("Buff {0}", sampleSize * 1000 / temp));
#endif
                    }

                    numRenders = 0;
                }
            }
        }
        

        private static Size GetConsoleFontSize()
        {
            IntPtr outHandle = GetStdHandle(-11);

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
        private static extern bool GetCurrentConsoleFont(
            IntPtr hConsoleOutput,
            bool bMaximumWindow,
            [Out][MarshalAs(UnmanagedType.LPStruct)]ConsoleFontInfo lpConsoleCurrentFont);

        [DllImport("kernel32.dll",
         EntryPoint = "GetStdHandle",
         SetLastError = true,
         CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
         private static extern IntPtr GetStdHandle(int nStdHandle);

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
    }

}
