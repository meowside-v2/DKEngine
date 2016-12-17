using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    /// <summary>
    /// Material represents ones texture with animations states
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Represents scaled length of image in pixels
        /// </summary>
        public int width { get; private set; } = 0;

        /// <summary>
        /// Represents scaled height of image in pixels
        /// </summary>
        public int height { get; private set; } = 0;

        /// <summary>
        /// Number of frames
        /// </summary>
        public int Frames { get; private set; } = 1;

        /// <summary>
        /// Total duration of animated image
        /// </summary>
        public int Duration { get; private set; } = 1;

        /// <summary>
        /// Duration between two images
        /// </summary>
        public int DurationPerFrame { get; private set; } = 1;

        /// <summary>
        /// Returns true if image is animated
        /// </summary>
        public bool IsAnimated { get; private set; } = false;

        /// <summary>
        /// Returns true if image is in loop
        /// </summary>
        public bool IsLooped { get; private set; } = false;

        /// <summary>
        /// Represents Alpha channel of image
        /// </summary>
        public byte[,,] colorMapA { get; private set; }

        /// <summary>
        /// Represents Red channel of image
        /// </summary>
        public byte[,,] colorMapR { get; private set; }

        /// <summary>
        /// Represents Green channel of image
        /// </summary>
        public byte[,,] colorMapG { get; private set; }

        /// <summary>
        /// Represents Blue channel of image
        /// </summary>
        public byte[,,] colorMapB { get; private set; }
        
        /// <summary>
        /// Loads image and creates new material
        /// </summary>
        /// <param name="source">Source image</param>
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
                    delay += (this_delay < 1 ? 33 : this_delay);
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

        /// <summary>
        /// Loads existing material and scales it by parent's given scales
        /// </summary>
        /// <param name="source">EXisting material</param>
        /// <param name="parent">I3Dimensional used for material scale</param>
        /// <param name="reColor">Color for material recoloration</param>
        public Material(Material source, I3Dimensional parent, Color? reColor = null)
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

                        if(reColor != null)
                        {
                            Color temp = (Color)reColor;

                            colorMapA[k, j, i] = temp.A;
                            colorMapR[k, j, i] = temp.R;
                            colorMapG[k, j, i] = temp.G;
                            colorMapB[k, j, i] = temp.B;
                        }
                        else
                        {
                            colorMapA[k, j, i] = source.colorMapA[X, Y, i];
                            colorMapR[k, j, i] = source.colorMapR[X, Y, i];
                            colorMapG[k, j, i] = source.colorMapG[X, Y, i];
                            colorMapB[k, j, i] = source.colorMapB[X, Y, i];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates new material with given color and scales it by parent's given scales
        /// </summary>
        /// <param name="clr">Source color</param>
        /// <param name="parent">I3Dimensional used for material scale</param>
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

        /// <summary>
        /// Returns color of pixel on coordinations
        /// </summary>
        /// <param name="x">Column coordination</param>
        /// <param name="y">Row coordination</param>
        /// <param name="frame">Layer/Frame coordination</param>
        /// <returns></returns>
        public string PixelToString(int x, int y, int frame = 0)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", this.colorMapA[x, y, frame],
                                                              this.colorMapR[x, y, frame],
                                                              this.colorMapG[x, y, frame],
                                                              this.colorMapB[x, y, frame]);
        }


        /// <summary>
        /// Render material into engine image buffer
        /// </summary>
        /// <param name="Parent">I3Dimensional for coordiantions</param>
        public void Render(I3Dimensional Parent)
        {

            int AnimationState = ((IGraphics)Parent).Animator.AnimationState;
            bool HasShadow = ((IGraphics)Parent).HasShadow;

            double x = Parent.X;
            double y = Parent.Y;
            
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
