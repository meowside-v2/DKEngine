using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
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
        public readonly int Width = 0;

        /// <summary>
        /// Represents scaled height of image in pixels
        /// </summary>
        public readonly int Height = 0;

        /// <summary>
        /// Number of frames
        /// </summary>
        public readonly int Frames = 1;

        /// <summary>
        /// Images of Animation loaded so far
        /// </summary>
        private int BufferImages = -1;

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

                Width = source.Width;
                Height = source.Height;

                if (ImageAnimator.CanAnimate(source))
                {
                    FrameDimension frameDimension = new FrameDimension(source.FrameDimensionsList[0]);
                    Frames = source.GetFrameCount(frameDimension);

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
                    
                    Bitmap[] layersOfImage = new Bitmap[Frames];

                    for(int i = 0; i < Frames; i++)
                    {
                        source.SelectActiveFrame(frameDimension, i);
                        layersOfImage[i] = new Bitmap(source);
                    }

                    Task.Factory.StartNew(() =>
                    {
                        int PixelFormat = 0;

                        switch (layersOfImage[0].PixelFormat)
                        {
                            case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                                PixelFormat = 3;
                                break;

                            case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                                PixelFormat = 4;
                                break;

                            default:
                                throw new Exception("Unsupported image pixel format");
                        }

                        for (int frame = 0; frame < Frames; frame++)
                        {
                            BitmapData ImageLayerData = layersOfImage[frame].LockBits(new Rectangle(0, 0, this.Width, this.Height), ImageLockMode.ReadOnly, layersOfImage[frame].PixelFormat);
                            int size = ImageLayerData.Stride * ImageLayerData.Height;
                            byte[] data = new byte[size];
                            Marshal.Copy(ImageLayerData.Scan0, data, 0, size);

                            colorMapA.Add(new byte[this.Width, this.Height]);
                            colorMapR.Add(new byte[this.Width, this.Height]);
                            colorMapG.Add(new byte[this.Width, this.Height]);
                            colorMapB.Add(new byte[this.Width, this.Height]);
                            
                            for (int row = 0; row < this.Height; row++)
                            {
                                for (int column = 0; column < this.Width; column++)
                                {
                                    int offset = PixelFormat * row * Width + PixelFormat * column;
                                    
                                    colorMapA[frame][column, row] = PixelFormat == 4 ? data[offset + 3] : (byte)255;
                                    colorMapR[frame][column, row] = data[offset + 2];
                                    colorMapG[frame][column, row] = data[offset + 1];
                                    colorMapB[frame][column, row] = data[offset];


                                }
                            }

                            layersOfImage[frame].UnlockBits(ImageLayerData);

                            BufferImages++;
                        }
                    });
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

                    BufferImages++;
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
            this.Width = (int)Parent.width;
            this.Height = (int)Parent.height;

            colorMapA.Add(new byte[Width, Height]);
            colorMapR.Add(new byte[Width, Height]);
            colorMapG.Add(new byte[Width, Height]);
            colorMapB.Add(new byte[Width, Height]);

            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    colorMapA[0][column, row] = clr.A;
                    colorMapR[0][column, row] = clr.R;
                    colorMapG[0][column, row] = clr.G;
                    colorMapB[0][column, row] = clr.B;
                }
            }

            BufferImages++;
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

            float RasteredHeight = this.Height * Parent.ScaleY;
            float RasteredWidth = this.Width * Parent.ScaleX;

            float NonRasteredWidthRatio = 1 / Parent.ScaleX;
            float NonRasteredHeightRatio = 1 / Parent.ScaleY;

            float NonRasteredHeight = 0;
            float NonRasteredWidth = 0;

            I3Dimensional ParentOfParent = ((ICore)Parent).Parent;

            float ToMaxWidth = ParentOfParent != null ? ParentOfParent.width : Engine.Render.RenderWidth;
            float ToMaxHeight = ParentOfParent != null ? ParentOfParent.height : Engine.Render.RenderHeight;

            if (ReColor == null)
            {
                if(AnimationState <= BufferImages)
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
                                    Color temp = MixPixel(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset],
                                                          colorMapA[AnimationState][tempColumn, tempRow], colorMapR[AnimationState][tempColumn, tempRow], colorMapG[AnimationState][tempColumn, tempRow], colorMapB[AnimationState][tempColumn, tempRow]);

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
                                Color temp = MixPixel(Color.FromArgb(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset]),
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
                                Color temp = MixPixel(Engine.Render.imageBufferKey[keyOffset], Engine.Render.imageBuffer[offset + 2], Engine.Render.imageBuffer[offset + 1], Engine.Render.imageBuffer[offset],
                                                      0xAA, 0x00, 0x00, 0x00);

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

        public Color MixPixel(byte topA, byte topR, byte topG, byte topB, byte bottomA, byte bottomR, byte bottomG, byte bottomB)
        {
            if (topA == 0)
                return Color.FromArgb(bottomA, bottomR, bottomG, bottomB);

            if (bottomA == 0)
                return Color.FromArgb(topA, topR, topG, topB);

            float opacityTop = (float)topA / 255;

            byte newA = (byte)(topA + bottomA >= 255 ? 255 : topA + bottomA);
            byte A = (byte)(newA - topA);

            float opacityBottom = (float)A / 255;

            byte R = (byte)(topR * opacityTop + bottomR * opacityBottom);
            byte G = (byte)(topG * opacityTop + bottomG * opacityBottom);
            byte B = (byte)(topB * opacityTop + bottomB * opacityBottom);

            return Color.FromArgb(newA, R, G, B);
        }

        public Color MixPixel(Color top, Color bottom)
        {
            if (top.A == 0)
                return bottom;

            if (bottom.A == 0)
                return top;

            float opacityTop = (float)top.A / 255;

            byte newA = (byte)(top.A + bottom.A >= 255 ? 255 : top.A + bottom.A);
            byte A = (byte)(newA - top.A);

            float opacityBottom = (float)A / 255;

            byte R = (byte)(top.R * opacityTop + bottom.R * opacityBottom);
            byte G = (byte)(top.G * opacityTop + bottom.G * opacityBottom);
            byte B = (byte)(top.B * opacityTop + bottom.B * opacityBottom);

            return Color.FromArgb(newA, R, G, B);
        }
    }
}
