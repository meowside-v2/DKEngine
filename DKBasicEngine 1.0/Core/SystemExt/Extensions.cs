using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public static class Extensions
    {
        public static void AddAll<T>(this List<T> list, params T[] stuff)
        {
            foreach (var item in stuff)
            {
                list.Add(item);
            }
        }

        public static double FindMaxZ(this List<I3Dimensional> list)
        {
            return list.Max(item => item.Z);
        }

        public static List<I3Dimensional> GetGameObjectsInView(this List<I3Dimensional> list)
        {
            List<I3Dimensional> retValue;

            lock (list)
            {
                retValue = list.Where(item => item.IsInView()).ToList();
            }

            return retValue;
        }

        public static bool IsInView(this I3Dimensional obj)
        {
            double X = Engine._baseCam != null ? Engine._baseCam.RenderXOffset : 0;
            double Y = Engine._baseCam != null ? Engine._baseCam.RenderYOffset : 0;

            return (obj.X + obj.width >= X && obj.X < X + Engine.Render.RenderWidth && obj.Y + obj.height >= Y && obj.Y < Y + Engine.Render.RenderHeight);
        }

        public static bool IsUnsupportedEscapeSequence(this char letter)
        {
            switch (letter)
            {
                case '\0':
                    return true;

                case '\a':
                    return true;

                case '\b':
                    return true;

                case '\f':
                    return true;

                /*case '\n':
                    return true;*/

                /*case '\r':
                    return true;*/

                case '\t':
                    return true;

                case '\v':
                    return true;

                case '\'':
                    return true;

                case '\"':
                    return true;
            }

            return false;
        }

        public static bool BufferIsFull(this byte[] buffer, byte value, int EachIndex)
        {
            for (int i = 0; i < buffer.Length; i += EachIndex)
            {
                if (buffer[i] != value)
                    return false;
            }

            return true;
        }

        public static void FillEach(this byte[] buffer, byte value, int EachIndex)
        {
            for(int i = 0; i < buffer.Length; i+= EachIndex)
            {
                buffer[i] = value;
            }
        }

        public static void Populate<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
                arr[i] = value;
        }

        public static void Populate<T>(this T[,] arr, int arrHeight, int arrWidth, T value)
        {
            for (int i = 0; i < arrHeight; i++)
                for (int j = 0; j < arrWidth; j++)
                    arr[j, i] = value;
        }

        public static void Populate<T>(this T[,,] arr, int arrHeight, int arrWidth, int arrDepth, T value)
        {
            for (int d = 0; d < arrDepth; d++)
                for (int i = 0; i < arrHeight; i++)
                    for (int j = 0; j < arrWidth; j++)
                        arr[j, i, d] = value;
        }

        public static void Render(this Color clr, I3Dimensional Parent)
        {
            int rowInBuffer = 0;
            int columnInBuffer = 0;

            double plusX = 1 / Parent.ScaleX;
            double plusY = 1 / Parent.ScaleY;

            double x = Parent.X;
            double y = Parent.Y;

            if(clr.A != 0)
            {
                for (double row = 0; row < Parent.height; row += plusY)
                {
                    if (y + row > Engine.Render.RenderHeight) return;

                    for (double column = 0; column < Parent.width; column += plusX)
                    {
                        if (x + column > Engine.Render.RenderWidth) break;

                        if (IsOnScreen(x + columnInBuffer, y + rowInBuffer))
                        {
                            int offset = (int)(((4 * (y + rowInBuffer)) * Engine.Render.RenderWidth) + (4 * (x + columnInBuffer)));

                            if (Engine.Render.imageBuffer[offset] != 255)
                            {
                                Color temp = MixPixel(Color.FromArgb(Engine.Render.imageBuffer[offset], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 2]),
                                                      clr);

                                Engine.Render.imageBuffer[offset] = temp.A;
                                Engine.Render.imageBuffer[offset + 1] = temp.B;
                                Engine.Render.imageBuffer[offset + 2] = temp.G;
                                Engine.Render.imageBuffer[offset + 3] = temp.R;
                            }
                        }

                        columnInBuffer++;
                    }

                    rowInBuffer++;
                    columnInBuffer = 0;
                }
            }
            
        }

        public static bool IsOnScreen(double x, double y)
        {
            return x >= 0 && x < Engine.Render.RenderWidth && y >= 0 && y < Engine.Render.RenderHeight;
        }

        public static Color MixPixel(Color top, Color bottom)
        {
            /*byte A = (byte)(top.A + bottom.A >= 255 ? 255 : top.A + bottom.A);

            byte bottomAlpha = (byte)(255 - top.A);

            byte R = (byte)(top.R * top.A + bottom.R * A);
            byte G = (byte)(top.G * top.G + bottom.G * A);
            byte B = (byte)(top.B * top.B + bottom.B * A);

            return Color.FromArgb(A, R, G, B);*/

            double opacityTop = (double)1 / 255 * top.A;
            
            byte newA = (byte)(top.A + bottom.A >= 255 ? 255 : top.A + bottom.A);
            byte A = (byte)(newA - top.A);

            double opacityBottom = (double)1 / 255 * A;

            byte R = (byte)(top.R * opacityTop + bottom.R * opacityBottom);
            byte G = (byte)(top.G * opacityTop + bottom.G * opacityBottom);
            byte B = (byte)(top.B * opacityTop + bottom.B * opacityBottom);

            return Color.FromArgb(newA, R, G, B);
        }
    }
}
