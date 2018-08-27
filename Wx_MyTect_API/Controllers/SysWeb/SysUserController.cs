using CommonHelp;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taurus.Core;

namespace Controllers.SysWeb
{
    public partial class SysUserController : SysBaseController
    {
        public SysUserController()
        {

        }

        #region MyRegion 视图入口控制器

        public void UserList()
        {

        }
        public void UserDetail()
        {

        }
        #endregion

    }

    public partial class SysUserController
    {

        #region MyRegion后台数据操作Api

        public void GetUserList()
        {

            MDataTable mData = new MDataTable();
            mData.Columns.Add("UserHead", System.Data.SqlDbType.VarChar);
            mData.Columns.Add("UserName", System.Data.SqlDbType.VarChar);
            mData.Columns.Add("Remark", System.Data.SqlDbType.VarChar);
            mData.Columns.Add("subNum", System.Data.SqlDbType.Int);
            mData.Columns.Add("UserId", System.Data.SqlDbType.Int);
            mData.Columns.Add("IsAmbassador", System.Data.SqlDbType.Int);
            MDataRow info = mData.NewRow();
            info["UserHead"].Value = "";
            info["UserName"].Value = "cc";
            info["Remark"].Value = "ss";
            info["subNum"].Value = 5;
            info["UserId"].Value = 1;
            info["IsAmbassador"].Value = 0;
            mData.Rows.Add(info);
            Write(LayuiHelper.GetResponseJson(JsonHelper.ToJson(mData), 1, 1));

        }



        public void GetUserOne()
        {
            var into = new
            {
                UserName = "cwf",
                UserId = 101,
                Remark = "测试",
                ParentName = "cy",
                Sex = 1,
                WxCountry = "中国",
                WxProvince = "深圳市",
                WxCity = "龙华区",
                Phone = "15773003882",
                CreateTime = DateTime.Now,
                Status = 0,

            };
            Write(JsonHelper.ToJson(into));

        }

        public void UpdUserInfo()
        {
            string userName = Query<string>("UserName");
            long UserId = Query<long>("UserId");

            Write("保存成功..", true);
        }

        #endregion

    }
}
