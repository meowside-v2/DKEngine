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
        /// Parent of this material
        /// </summary>
        public IGraphics Parent;

        /// <summary>
        /// Source image used for this Material
        /// </summary>
        public Image SourceImage;

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
        public List<byte[,]> colorMapA { get; private set; } = new List<byte[,]>();

        /// <summary>
        /// Represents Red channel of image
        /// </summary>
        public List<byte[,]> colorMapR { get; private set; } = new List<byte[,]>();

        /// <summary>
        /// Represents Green channel of image
        /// </summary>
        public List<byte[,]> colorMapG { get; private set; } = new List<byte[,]>();

        /// <summary>
        /// Represents Blue channel of image
        /// </summary>
        public List<byte[,]> colorMapB { get; private set; } = new List<byte[,]>();
        
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
                
                width = source.Width;
                height = source.Height;

                for (int frame = 0; frame < Frames; frame++)
                {
                    source.SelectActiveFrame(frameDimension, frame);

                    colorMapA.Add(new byte[source.Width, source.Height]);
                    colorMapR.Add(new byte[source.Width, source.Height]);
                    colorMapG.Add(new byte[source.Width, source.Height]);
                    colorMapB.Add(new byte[source.Width, source.Height]);

                    for (int row = 0; row < source.Height; row++)
                    {
                        for (int column = 0; column < source.Width; column++)
                        {
                            Color temp = ((Bitmap)source).GetPixel(column, row);

                            colorMapA[frame][column, row] = temp.A;
                            colorMapR[frame][column, row] = temp.R;
                            colorMapG[frame][column, row] = temp.G;
                            colorMapB[frame][column, row] = temp.B;
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
                colorMapA.Add(new byte[source.Width, source.Height]);
                colorMapR.Add(new byte[source.Width, source.Height]);
                colorMapG.Add(new byte[source.Width, source.Height]);
                colorMapB.Add(new byte[source.Width, source.Height]);

                width = source.Width;
                height = source.Height;
                
                for (int row = 0; row < source.Height; row++)
                {
                    for (int column = 0; column < source.Width; column++)
                    {
                        Color temp = ((Bitmap)source).GetPixel(column, row);

                        colorMapA[0][column, row] = temp.A;
                        colorMapR[0][column, row] = temp.R;
                        colorMapG[0][column, row] = temp.G;
                        colorMapB[0][column, row] = temp.B;
                    }
                }
            }
        }

        /*/// <summary>
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
            
            for (int i = 0; i < Frames; i++)
            {
                this.colorMapA.Add(new byte[width, height]);
                this.colorMapR.Add(new byte[width, height]);
                this.colorMapG.Add(new byte[width, height]);
                this.colorMapB.Add(new byte[width, height]);

                for (int j = 0; j < height; j++)
                {
                    for(int k = 0; k < width; k++)
                    {
                        int X = (int)(k / parent.ScaleX);
                        int Y = (int)(j / parent.ScaleY);

                        if(reColor != null)
                        {
                            Color temp = (Color)reColor;

                            colorMapA[i][k, j] = temp.A;
                            colorMapR[i][k, j] = temp.R;
                            colorMapG[i][k, j] = temp.G;
                            colorMapB[i][k, j] = temp.B;
                        }
                        else
                        {
                            colorMapA[i][k, j] = source.colorMapA[i][X, Y];
                            colorMapR[i][k, j] = source.colorMapR[i][X, Y];
                            colorMapG[i][k, j] = source.colorMapG[i][X, Y];
                            colorMapB[i][k, j] = source.colorMapB[i][X, Y];
                        }
                    }
                }
            }
        }*/

        /// <summary>
        /// Creates new material with given color and scales it by parent's given scales
        /// </summary>
        /// <param name="clr">Source color</param>
        /// <param name="parent">I3Dimensional used for material scale</param>
        public Material(Color clr, I3Dimensional parent)
        {
            this.width = (int)parent.width;
            this.height = (int)parent.height;

            colorMapA.Add(new byte[width, height]);
            colorMapR.Add(new byte[width, height]);
            colorMapG.Add(new byte[width, height]);
            colorMapB.Add(new byte[width, height]);

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    colorMapA[0][column, row] = clr.A;
                    colorMapR[0][column, row] = clr.R;
                    colorMapG[0][column, row] = clr.G;
                    colorMapB[0][column, row] = clr.B;
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
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", this.colorMapA[frame][x, y],
                                                              this.colorMapR[frame][x, y],
                                                              this.colorMapG[frame][x, y],
                                                              this.colorMapB[frame][x, y]);
        }


        /// <summary>
        /// Render material into engine image buffer
        /// </summary>
        /// <param name="Parent">I3Dimensional for coordiantions</param>
        public void Render(I3Dimensional Parent)
        {

            int AnimationState = ((IGraphics)Parent).Animator != null ? ((IGraphics)Parent).Animator.AnimationState : 0;
            bool HasShadow = ((IGraphics)Parent).HasShadow;

            float x = Parent.X;
            float y = Parent.Y;

            float RasteredHeight = this.height * Parent.ScaleY;
            float RasteredWidth = this.width * Parent.ScaleX;

            float NonRasteredWidthRatio = 1 / Parent.ScaleX;
            float NonRasteredHeightRatio = 1 / Parent.ScaleY;

            float NonRasteredHeight = 0;
            float NonRasteredWidth = 0;

            for (int row = 0; row < RasteredHeight; row++)
            {
                NonRasteredWidth = 0;

                for (int column = 0; column < RasteredWidth; column++)
                {
                    if (Extensions.IsOnScreen(x + column, y + row))
                    {
                        
                        int offset = (int)(((3 * (y + row)) * Engine.Render.RenderWidth) + (3 * (x + column)));
                        int keyOffset = (int)(((y + row) * Engine.Render.RenderWidth) + (x + column));

                        int tempColumn = (int)NonRasteredWidth;
                        int tempRow    = (int)NonRasteredHeight;

                        if (Engine.Render.imageBuffer[offset] != 255 && colorMapA[AnimationState][tempColumn, tempRow] != 0)
                        {
                            Color temp = Extensions.MixPixel(Color.FromArgb(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset]),
                                                Color.FromArgb(colorMapA[AnimationState][tempColumn, tempRow], colorMapR[AnimationState][tempColumn, tempRow], colorMapG[AnimationState][tempColumn, tempRow], colorMapB[AnimationState][tempColumn, tempRow]));

                            Engine.Render.imageBufferKey[keyOffset] = temp.A;

                            Engine.Render.imageBuffer[offset] = temp.B;
                            Engine.Render.imageBuffer[offset + 1] = temp.G;
                            Engine.Render.imageBuffer[offset + 2] = temp.R;
                        }
                    }

                    NonRasteredWidth += NonRasteredWidthRatio;
                }

                NonRasteredHeight += NonRasteredHeightRatio;
            }

            if (HasShadow)
            {
                NonRasteredHeight = 0;
                NonRasteredWidth = 0;

                for (int row = 0; row < RasteredHeight; row++)
                {
                    NonRasteredWidth = 0;

                    for (int column = 0; column < RasteredWidth; column++)
                    {
                        if (Extensions.IsOnScreen(x + column, y + row))
                        {
                            int offset = (int)(((3 * (y + row)) * Engine.Render.RenderWidth) + (3 * (x + column)));
                            int keyOffset = (int)(((y + row) * Engine.Render.RenderWidth) + (x + column));

                            int tempColumn = (int)NonRasteredWidth;
                            int tempRow = (int)NonRasteredHeight;

                            if (Engine.Render.imageBuffer[offset] != 255 && colorMapA[AnimationState][tempColumn, tempRow] != 0)
                            {
                                Color temp = Extensions.MixPixel(Color.FromArgb(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset]),
                                                                    Color.FromArgb(0xAA, 0x00, 0x00, 0x00));

                                Engine.Render.imageBufferKey[keyOffset] = temp.A;

                                Engine.Render.imageBuffer[offset] = temp.B;
                                Engine.Render.imageBuffer[offset + 1] = temp.G;
                                Engine.Render.imageBuffer[offset + 2] = temp.R;
                            }
                        }

                        NonRasteredWidth += NonRasteredWidthRatio;
                    }

                    NonRasteredHeight += NonRasteredHeightRatio;
                }
            }
        }
    }
}
