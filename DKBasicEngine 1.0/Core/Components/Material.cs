using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Material
    {
        public int width { get; private set; }
        public int height { get; private set; }

        public int Frames { get; private set; } = 1;
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

        public Material(Material source, I3Dimensional parent)
        {
            this.Frames = source.Frames;

            this.height = (int)parent.height;
            this.width = (int)parent.width;

            this.colorMapA = new byte[width, height, Frames];
            this.colorMapR = new byte[width, height, Frames];
            this.colorMapG = new byte[width, height, Frames];
            this.colorMapB = new byte[width, height, Frames];
            
            for (int i = 0; i < Frames; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    for(int k = 0; k < width; k++)
                    {
                        int X = (int)(k / parent.ScaleX);
                        int Y = (int)(j / parent.ScaleY);

                        colorMapA[k, j, i] = source.colorMapA[X, Y, i];
                        colorMapR[k, j, i] = source.colorMapR[X, Y, i];
                        colorMapG[k, j, i] = source.colorMapG[X, Y, i];
                        colorMapB[k, j, i] = source.colorMapB[X, Y, i];
                    }
                }
            }
        }

        public Material(Color clr, I3Dimensional parent)
        {
            this.width = (int)parent.width;
            this.height = (int)parent.height;

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

        public void Render(I3Dimensional Parent, Color? clr = null)
        {

            int AnimationState = ((IGraphics)Parent).AnimationState;
            bool HasShadow = ((IGraphics)Parent).HasShadow;

            double x = Parent.X;
            double y = Parent.Y;


            if (clr == null)
            {
                /*Parallel.For(0, this.width * this.height, (i) =>
                {
                    int column = i % this.width;
                    int row = i / this.width;

                    int offset = (int)(((3 * (y + row)) * Engine.Render.RenderWidth) + (3 * (x + column)));
                    int keyOffset = (int)(((y + row) * Engine.Render.RenderWidth) + (x + column));

                    if (Extensions.IsOnScreen(x + column, y + row))
                    {
                        if (Engine.Render.imageBuffer[offset] != 255)
                        {

                            if (colorMapA[column, row, AnimationState] != 0)
                            {
                                Color temp = Extensions.MixPixel(Color.FromArgb(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset]),
                                                      Color.FromArgb(colorMapA[column, row, AnimationState], colorMapR[column, row, AnimationState], colorMapG[column, row, AnimationState], colorMapB[column, row, AnimationState]));

                                Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                Engine.Render.imageBuffer[offset] = temp.B;
                                Engine.Render.imageBuffer[offset + 1] = temp.G;
                                Engine.Render.imageBuffer[offset + 2] = temp.R;
                            }
                        }
                    }
                });*/

                for (int row = 0; row < this.height; row++)
                {
                    for (int column = 0; column < this.width; column++)
                    {
                        int offset = (int)(((3 * (y + row)) * Engine.Render.RenderWidth) + (3 * (x + column)));
                        int keyOffset = (int)(((y + row) * Engine.Render.RenderWidth) + (x + column));

                        if (Extensions.IsOnScreen(x + column, y + row))
                        {
                            if (Engine.Render.imageBuffer[offset] != 255)
                            {

                                if (colorMapA[column, row, AnimationState] != 0)
                                {
                                    Color temp = Extensions.MixPixel(Color.FromArgb(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset]),
                                                          Color.FromArgb(colorMapA[column, row, AnimationState], colorMapR[column, row, AnimationState], colorMapG[column, row, AnimationState], colorMapB[column, row, AnimationState]));

                                    Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                    Engine.Render.imageBuffer[offset] = temp.B;
                                    Engine.Render.imageBuffer[offset + 1] = temp.G;
                                    Engine.Render.imageBuffer[offset + 2] = temp.R;
                                }
                            }
                        }
                    }
                }
            }

            else
            {
                for (int row = 0; row < this.height; row++)
                {
                    for (int column = 0; column < this.width; column++)
                    {
                        int offset = (int)(((3 * (y + row)) * Engine.Render.RenderWidth) + (3 * (x + column)));
                        int keyOffset = (int)(((y + row) * Engine.Render.RenderWidth) + (x + column));

                        if (Extensions.IsOnScreen(x + column, y + row))
                        {
                            if (Engine.Render.imageBufferKey[keyOffset] != 255)
                            {
                                Color color = (Color)clr;

                                if (colorMapA[column, row, AnimationState] != 0)
                                {
                                    Color temp = Extensions.MixPixel(Color.FromArgb(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset]),
                                                          Color.FromArgb(colorMapA[column, row, AnimationState], colorMapR[column, row, AnimationState], colorMapG[column, row, AnimationState], colorMapB[column, row, AnimationState]));

                                    Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                    Engine.Render.imageBuffer[offset] = temp.B;
                                    Engine.Render.imageBuffer[offset + 1] = temp.G;
                                    Engine.Render.imageBuffer[offset + 2] = temp.R;
                                }
                            }
                        }
                    }
                }
            }

            if (HasShadow)
            {
                for (int row = 0; row < this.height; row++)
                {

                    for (int column = 0; column < this.width; column++)
                    {
                        int offset = (int)(((3 * (y + row)) * Engine.Render.RenderWidth) + (3 * (x + column)));
                        int keyOffset = (int)(((y + row) * Engine.Render.RenderWidth) + (x + column));

                        if (Extensions.IsOnScreen(x + column, y + row))
                        {

                            if (Engine.Render.imageBuffer[offset] != 255)
                            {
                                Color color = (Color)clr;

                                if (colorMapA[column, row, AnimationState] != 0)
                                {
                                    Color temp = Extensions.MixPixel(Color.FromArgb(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset]),
                                                          Color.FromArgb(colorMapA[column, row, AnimationState], colorMapR[column, row, AnimationState], colorMapG[column, row, AnimationState], colorMapB[column, row, AnimationState]));

                                    Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                    Engine.Render.imageBuffer[offset] = temp.B;
                                    Engine.Render.imageBuffer[offset + 1] = temp.G;
                                    Engine.Render.imageBuffer[offset + 2] = temp.R;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
