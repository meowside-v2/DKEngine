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
        /// Source image used for this Material
        /// </summary>
        public readonly Image SourceImage = null;

        /// <summary>
        /// Represents scaled length of image in pixels
        /// </summary>
        public readonly int width = 0;

        /// <summary>
        /// Represents scaled height of image in pixels
        /// </summary>
        public readonly int height = 0;

        /// <summary>
        /// Number of frames
        /// </summary>
        public readonly int Frames = 1;

        /// <summary>
        /// Total duration of animated image
        /// </summary>
        public readonly int Duration = 1;

        /// <summary>
        /// Duration between two images
        /// </summary>
        public readonly int DurationPerFrame = 1;

        /// <summary>
        /// Returns true if image is animated
        /// </summary>
        public readonly bool IsAnimated = false;

        /// <summary>
        /// Returns true if image is in loop
        /// </summary>
        public readonly bool IsLooped = false;

        /// <summary>
        /// Represents Alpha channel of image
        /// </summary>
        public readonly List<byte[,]> colorMapA = new List<byte[,]>();

        /// <summary>
        /// Represents Red channel of image
        /// </summary>
        public readonly List<byte[,]> colorMapR = new List<byte[,]>();

        /// <summary>
        /// Represents Green channel of image
        /// </summary>
        public readonly List<byte[,]> colorMapG = new List<byte[,]>();

        /// <summary>
        /// Represents Blue channel of image
        /// </summary>
        public readonly List<byte[,]> colorMapB = new List<byte[,]>();
        
        /// <summary>
        /// Loads image and creates new material
        /// </summary>
        /// <param name="source">Source image</param>
        public Material(Image source)
        {
            if(source != null)
            {
                SourceImage = source;

                width = source.Width;
                height = source.Height;

                if (ImageAnimator.CanAnimate(source))
                {
                    FrameDimension frameDimension = new FrameDimension(source.FrameDimensionsList[0]);

                    Frames = source.GetFrameCount(frameDimension);
                    
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
        }

        /// <summary>
        /// Creates new material with given color and scales it by parent's given scales
        /// </summary>
        /// <param name="clr">Source color</param>
        /// <param name="Parent">I3Dimensional used for material scale</param>
        public Material(Color clr, I3Dimensional Parent)
        {
            this.width = (int)Parent.width;
            this.height = (int)Parent.height;

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
        public void Render(I3Dimensional Parent, Color? ReColor = null)
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

            I3Dimensional ParentOfParent = ((ICore)Parent).Parent;

            float ToMaxWidth = ParentOfParent != null ? ParentOfParent.width : Engine.Render.RenderWidth;
            float ToMaxHeight = ParentOfParent != null ? ParentOfParent.height : Engine.Render.RenderHeight;

            if (ReColor == null)
            {
                for (int row = 0; row < RasteredHeight && row < ToMaxHeight; row++)
                {
                    NonRasteredWidth = 0;

                    if (y + row >= Engine.Render.RenderHeight)
                        break;

                    for (int column = 0; column < RasteredWidth && column < ToMaxWidth; column++)
                    {

                        if (x + column >= Engine.Render.RenderWidth)
                            break;

                        if (Extensions.IsOnScreen(x + column, y + row))
                        {

                            int offset = (int)(((3 * (y + row)) * Engine.Render.RenderWidth) + (3 * (x + column)));
                            int keyOffset = (int)(((y + row) * Engine.Render.RenderWidth) + (x + column));

                            int tempColumn = (int)NonRasteredWidth;
                            int tempRow = (int)NonRasteredHeight;

                            if (Engine.Render.imageBufferKey[keyOffset] != 255 && colorMapA[AnimationState][tempColumn, tempRow] != 0)
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
            }
            else
            {
                Color tempColor = (Color)ReColor;

                for (int row = 0; row < RasteredHeight && row < ToMaxHeight; row++)
                {
                    NonRasteredWidth = 0;

                    if (y + row >= Engine.Render.RenderHeight)
                        break;

                    for (int column = 0; column < RasteredWidth && column < ToMaxWidth; column++)
                    {

                        if (x + column >= Engine.Render.RenderWidth)
                            break;

                        if (Extensions.IsOnScreen(x + column, y + row))
                        {

                            int offset = (int)(((3 * (y + row)) * Engine.Render.RenderWidth) + (3 * (x + column)));
                            int keyOffset = (int)(((y + row) * Engine.Render.RenderWidth) + (x + column));

                            int tempColumn = (int)NonRasteredWidth;
                            int tempRow = (int)NonRasteredHeight;

                            if (Engine.Render.imageBufferKey[keyOffset] != 255 && colorMapA[AnimationState][tempColumn, tempRow] != 0)
                            {
                                Color temp = Extensions.MixPixel(Color.FromArgb(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset]),
                                                                 tempColor);

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

            if (HasShadow)
            {
                NonRasteredHeight = 0;
                NonRasteredWidth = 0;

                RasteredHeight++;
                RasteredWidth++;

                for (int row = 1; row < RasteredHeight && row < ToMaxHeight; row++)
                {
                    NonRasteredWidth = 0;

                    if (y + row >= Engine.Render.RenderHeight)
                        break;

                    for (int column = 1; column < RasteredWidth && column < ToMaxWidth; column++)
                    {
                        if (x + column >= Engine.Render.RenderWidth)
                            break;

                        if (Extensions.IsOnScreen(x + column, y + row))
                        {
                            int offset = (int)(((3 * (y + row)) * Engine.Render.RenderWidth) + (3 * (x + column)));
                            int keyOffset = (int)(((y + row) * Engine.Render.RenderWidth) + (x + column));

                            int tempColumn = (int)NonRasteredWidth;
                            int tempRow = (int)NonRasteredHeight;

                            if (Engine.Render.imageBufferKey[keyOffset] != 255 && colorMapA[AnimationState][tempColumn, tempRow] != 0)
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
