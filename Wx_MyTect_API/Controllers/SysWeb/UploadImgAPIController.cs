using System.IO;
using System.Web;
using Taurus.Core;

namespace Controllers.SysWeb
{
    public class UploadImgAPIController : Controller
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        public void UploadImg()
        {
            string id = Query<string>("id");
            string path;
            string end = "{\"code\": 1,\"msg\": \"服务器故障\",\"data\": {\"src\": \"\"}}"; //返回的json  

            HttpPostedFile file = Context.Request.Files[0]; //获取选中文件 
            path = CommonHelp.ImageHelper.ImgUpload(file, id);
            if (id == "proImg")
            {
                path = CYQ.Data.AppConfig.GetApp("DomianName") + path;
            }
            end = "{\"code\": 0,\"msg\": \"成功\",\"data\": {\"src\": \"" + path + "\"}}";

            Context.Response.Write(end);//输出结果  
            Context.Response.End();
        }
    }
}
