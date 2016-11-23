﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Material
    {
        public int width { get; private set; }
        public int height { get; private set; }

        public int Frames { get; private set; } = 0;
        public int Duration { get; private set; } = 0;
        public int DurationPerFrame { get; private set; } = 0;

        public bool IsAnimated { get; private set; } = false;
        public bool IsLooped { get; private set; } = false;

        public byte[,,] colorMapA { get; private set; }
        public byte[,,] colorMapR { get; private set; }
        public byte[,,] colorMapG { get; private set; }
        public byte[,,] colorMapB { get; private set; }

        public Material(Image source)
        {
            if (ImageAnimator.CanAnimate(source))
            {
                FrameDimension frameDimension = new FrameDimension(source.FrameDimensionsList[0]);

                Frames = source.GetFrameCount(frameDimension);

                colorMapA = new byte[source.Width, source.Height, Frames];
                colorMapR = new byte[source.Width, source.Height, Frames];
                colorMapG = new byte[source.Width, source.Height, Frames];
                colorMapB = new byte[source.Width, source.Height, Frames];

                width = source.Width;
                height = source.Height;

                for (int frame = 0; frame < Frames; frame++)
                {
                    source.SelectActiveFrame(frameDimension, frame);

                    for (int row = 0; row < source.Height; row++)
                    {
                        for (int column = 0; column < source.Width; column++)
                        {
                            Color temp = ((Bitmap)source).GetPixel(column, row);

                            colorMapA[column, row, frame] = temp.A;
                            colorMapR[column, row, frame] = temp.R;
                            colorMapG[column, row, frame] = temp.G;
                            colorMapB[column, row, frame] = temp.B;
                        }
                    }
                }

                int delay = 0;
                int this_delay = 0;
                int index = 0;

                for (int i = 0; i < Frames; i++)
                {
                    this_delay = BitConverter.ToInt32(source.GetPropertyItem(20736).Value, index) * 10;
                    delay += (this_delay < 1 ? 1 : this_delay);  // Minimum delay is 100 ms
                    index += 4;
                }

                Duration = delay;
                DurationPerFrame = Duration / Frames;
                IsAnimated = true;
                IsLooped = BitConverter.ToInt16(source.GetPropertyItem(20737).Value, 0) != 1;
            }

            else
            {
                colorMapA = new byte[source.Width, source.Height, 1];
                colorMapR = new byte[source.Width, source.Height, 1];
                colorMapG = new byte[source.Width, source.Height, 1];
                colorMapB = new byte[source.Width, source.Height, 1];

                width = source.Width;
                height = source.Height;
                
                for (int row = 0; row < source.Height; row++)
                {
                    for (int column = 0; column < source.Width; column++)
                    {
                        Color temp = ((Bitmap)source).GetPixel(column, row);

                        colorMapA[column, row, 0] = temp.A;
                        colorMapR[column, row, 0] = temp.R;
                        colorMapG[column, row, 0] = temp.G;
                        colorMapB[column, row, 0] = temp.B;
                    }
                }
            }
        }

        public Material(Color clr, int width, int height)
        {
            this.width = width;
            this.height = height;

            colorMapA = new byte[width, height, 1];
            colorMapR = new byte[width, height, 1];
            colorMapG = new byte[width, height, 1];
            colorMapB = new byte[width, height, 1];

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    colorMapA[column, row, 0] = clr.A;
                    colorMapR[column, row, 0] = clr.R;
                    colorMapG[column, row, 0] = clr.G;
                    colorMapB[column, row, 0] = clr.B;
                }
            }
        }

        public string PixelToString(int x, int y, int frame = 0)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", this.colorMapA[x, y, frame],
                                                              this.colorMapR[x, y, frame],
                                                              this.colorMapG[x, y, frame],
                                                              this.colorMapB[x, y, frame]);
        }

        public object DeepCopy()
        {
            return this.MemberwiseClone();
        }

        public void Render(int x, int y, int animationState, byte[] imageBuffer, bool[] imageBufferKey, double scaleX, double scaleY, Color? clr = null)
        {
            int rowInBuffer = 0;
            int columnInBuffer = 0;

            double plusX = 1 / scaleX;
            double plusY = 1 / scaleY;

            if (clr == null)
            {
                for (double row = 0; row < this.height; row += plusY)
                {
                    if (y + row > Engine.Render.RenderHeight) return;

                    for (double column = 0; column < this.width; column += plusX)
                    {
                        if (x + column > Engine.Render.RenderWidth) break;

                        if (IsOnScreen(x + columnInBuffer, y + rowInBuffer))
                        {
                            int offset = (((3 * (y + rowInBuffer)) * Engine.Render.RenderWidth) + (3 * (x + columnInBuffer)));
                            int keyOffset = ((y + rowInBuffer) * Engine.Render.RenderWidth + x + columnInBuffer);

                            if (!imageBufferKey[keyOffset])
                            {
                                //Color temp = colorMap[(int)column, (int)row, animationState];

                                int offColumn = (int)column;
                                int offRow = (int)row;

                                if (colorMapA[offColumn, offRow, animationState] != 0)
                                {
                                    imageBuffer[offset] = colorMapB[offColumn, offRow, animationState];
                                    imageBuffer[offset + 1] = colorMapG[offColumn, offRow, animationState];
                                    imageBuffer[offset + 2] = colorMapR[offColumn, offRow, animationState];

                                    imageBufferKey[keyOffset] = true;
                                }
                            }
                        }

                        columnInBuffer++;
                    }

                    rowInBuffer++;
                    columnInBuffer = 0;
                }
            }

            else
            {
                for (double row = 0; row < this.height; row += (1 / scaleY))
                {
                    if (y + row > Engine.Render.RenderHeight) return;

                    for (double column = 0; column < this.width; column += (1 / scaleX))
                    {
                        if (x + column > Engine.Render.RenderWidth) break;

                        if (IsOnScreen(x + columnInBuffer, y + rowInBuffer))
                        {
                            int offset = (((3 * (y + rowInBuffer)) * Engine.Render.RenderWidth) + (3 * (x + columnInBuffer)));
                            int keyOffset = ((y + rowInBuffer) * Engine.Render.RenderWidth + x + columnInBuffer);

                            if (!imageBufferKey[keyOffset])
                            {
                                Color color = (Color)clr;

                                int offColumn = (int)column;
                                int offRow = (int)row;

                                if (colorMapA[offColumn, offRow, animationState] != 0)
                                {
                                    imageBuffer[offset] = color.B;
                                    imageBuffer[offset + 1] = color.G;
                                    imageBuffer[offset + 2] = color.R;

                                    imageBufferKey[keyOffset] = true;
                                }
                            }
                        }

                        columnInBuffer++;
                    }

                    rowInBuffer++;
                    columnInBuffer = 0;
                }
            }
        }

        private bool IsOnScreen(int x, int y)
        {
            return x >= 0 && x < Engine.Render.RenderWidth && y >= 0 && y < Engine.Render.RenderHeight;
        }
    }
}
