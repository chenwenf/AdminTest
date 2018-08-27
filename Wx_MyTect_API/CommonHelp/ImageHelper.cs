using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CommonHelp
{
    public class ImageHelper
    {
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="file"></param>
        /// <param name="saveFoldersName"></param>
        /// <returns></returns>
        public static string ImgUpload(HttpPostedFile file, string saveFoldersName)
        {
            if (file == null) { return string.Empty; }
            if (file.ContentLength > 0)
            {
                if (!string.IsNullOrEmpty(file.FileName))
                {
                    string ext = Path.GetExtension(file.FileName).ToLower();//当前文件后缀名
                    //验证文件类型是否正确gif，jpeg，jpg，bmp，png
                    if (ext.Equals(".gif") || ext.Equals(".jpg") || ext.Equals(".jpeg") || ext.Equals(".png") || ext.Equals(".bmp"))
                    {
                        string uploadPath = "/imgUploads/" + saveFoldersName + "/";
                        string newFilename = GenerateFilename(ext);
                        //当前待上传的服务端路径相对
                        var imageUrl = uploadPath + newFilename;

                        string folder = AppDomain.CurrentDomain.BaseDirectory + uploadPath;
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        var imageFilePath = AppDomain.CurrentDomain.BaseDirectory + imageUrl;
                        file.SaveAs(imageFilePath);
                        return imageUrl;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 生成图片文件名称
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GenerateFilename(string extension)
        {
            return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + extension;
        }


    }
}
