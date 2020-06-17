using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;

namespace ReportInterface
{
    public class RdlcEmfUtils
    {
        List<EMFFile> EMFFiles = new List<EMFFile>();
        public Stream CreateStreamCallback(string name, string extension, Encoding encoding, string mimeType, bool willSeek)
        {
            int i = Convert.ToInt32(name.Substring(name.IndexOf('_') + 1)) + 1;
            Stream stream = new MemoryStream();
            EMFFiles.Add(new EMFFile(string.Format("{0}.{1}", i, extension.ToLower()), stream));
            return stream;
        }

        public RdlcEmfUtils(LocalReport lr, string savePath)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            Microsoft.Reporting.WinForms.Warning[] Warnings;

            string deviceInfo = @"<DeviceInfo>
                         <SimplePageHeaders>True</SimplePageHeaders>
                         <OutputFormat>PNG</OutputFormat>
                         <PageHeight>11in</PageHeight>
                         <PageWidth>9in</PageWidth>
                         <MarginTop>0.0in</MarginTop>
                         <MarginLeft>1.2in</MarginLeft> 
                         <MarginRight>0.0in</MarginRight>
                         <MarginBottom>0.0in</MarginBottom>
                         </DeviceInfo>         ";

            EMFFiles.Clear();
            lr.Render("Image", deviceInfo, CreateStreamCallback, out Warnings);

            foreach (EMFFile emffile in EMFFiles)
            {
                using (FileStream fs = new FileStream(string.Format(savePath + "\\{0}", emffile.Filename), FileMode.Create))
                {
                    emffile.Stream.Position = 0;
                    emffile.Stream.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                }

                string path = string.Format(savePath + "\\{0}", emffile.Filename);
                zzsTextWater(path, "苏州询测信息科技有限公司", path, 0.8f, 30);
            }
        }

        public void DeleteFolder(string strDirPath)
        {
            if (Directory.Exists(strDirPath))           //如果存在这个文件夹，执行删除操作
            {
                foreach (string d in Directory.GetFileSystemEntries(strDirPath))
                {
                    if (File.Exists(d))
                        File.Delete(d);                 //直接删除其中的文件
                    else
                        DeleteFolder(d);                //递归删除子文件夹
                }
                Directory.Delete(strDirPath, true);     //删除已空文件夹
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //OtherManagedObject.Dispose();
            }
            try
            {
                EMFFiles.Clear();
            }
            catch { }
            if (disposing)
                GC.SuppressFinalize(this);
        }

        ~RdlcEmfUtils()
        {
            Dispose(false);
        }

        public static bool zzsTextWater(string ImgFile, string TextFont, string sImgPath, float widthFont, int Alpha)
        {
            try
            {
                FileStream fs = new FileStream(ImgFile, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                byte[] bytes = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
                MemoryStream ms = new MemoryStream(bytes);
                System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(ms);
                int imgPhotoWidth = imgPhoto.Width;
                int imgPhotoHeight = imgPhoto.Height;
                Bitmap bmPhoto = new Bitmap(imgPhotoWidth, imgPhotoHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(72, 72);
                Graphics gbmPhoto = Graphics.FromImage(bmPhoto);
                gbmPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                gbmPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gbmPhoto.DrawImage(
                  imgPhoto
                 , new Rectangle(0, 0, imgPhotoWidth, imgPhotoHeight)
                 , 0
                 , 0
                 , imgPhotoWidth
                 , imgPhotoHeight
                 , GraphicsUnit.Pixel
                 );
                //建立字体大小的数组,循环找出适合图片的水印字体
                //int[] sizes = new int[] { 1000, 800, 700, 650, 600, 560, 540, 500, 450, 400, 380, 360, 340, 320, 300, 280, 260, 240, 220, 200, 180, 160, 140, 120, 100, 80, 72, 64, 48, 32, 28, 26, 24, 20, 28, 16, 14, 12, 10, 8, 6, 4, 2 };
                int[] sizes = new int[] { 28, 26, 24, 20, 16, 14, 12 };
                System.Drawing.Font crFont = null;
                System.Drawing.SizeF crSize = new SizeF();
                for (int i = 0; i < 7; i++)
                {
                    crFont = new Font("微软雅黑", sizes[i], FontStyle.Bold);
                    crSize = gbmPhoto.MeasureString(TextFont, crFont);
                    if ((ushort)crSize.Width < (ushort)imgPhotoWidth * widthFont)
                        break;
                }
                //设置水印字体的位置

                float yPosFromBottom = imgPhotoHeight * 0.1f;
                float xCenterOfImg = imgPhotoWidth * 0.1f;

                System.Drawing.StringFormat StrFormat = new System.Drawing.StringFormat();
                StrFormat.Alignment = System.Drawing.StringAlignment.Near;
                //
                Matrix mtxSave = gbmPhoto.Transform;
                Matrix mtxRotate = gbmPhoto.Transform;
                PointF p = new System.Drawing.PointF(xCenterOfImg + 1, yPosFromBottom + 1);
                mtxRotate.RotateAt(-45f, p);

                gbmPhoto.Transform = mtxRotate;
                System.Drawing.SolidBrush semiTransBrush2 = new System.Drawing.SolidBrush(Color.FromArgb(Alpha, 0, 0, 0));
                for (int i = 1; i <= 13; i++)
                {
                    gbmPhoto.DrawString(
                     TextFont
                    , crFont
                    , semiTransBrush2
                    , new System.Drawing.PointF(xCenterOfImg - (i * 40), yPosFromBottom + (i * 100))
                    , StrFormat
                    );
                }

                gbmPhoto.Transform = mtxSave;

                bmPhoto.Save(sImgPath, System.Drawing.Imaging.ImageFormat.Png);
                gbmPhoto.Dispose();
                imgPhoto.Dispose();
                bmPhoto.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

    class EMFFile
    {
        private string filename;

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }
        private Stream stream;

        public Stream Stream
        {
            get { return stream; }
            set { stream = value; }
        }
        public EMFFile(string filename, Stream stream)
        {
            this.filename = filename;
            this.stream = stream;
        }
    }
}
