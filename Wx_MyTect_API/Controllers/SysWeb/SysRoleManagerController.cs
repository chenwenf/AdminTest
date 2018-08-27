using CommonHelp;
using CYQ.Data.Tool;
using Logic.SysCon;
using Taurus.Core;

namespace Controllers.SysWeb
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public partial class SysRoleManagerController : SysBaseController
    {
        private readonly RoleLinkLogic roleLinkLogic;
        private readonly RoleLogic roleLogic;
        private readonly RoleManagerLogic roleManager;

        public SysRoleManagerController()
        {
            roleLinkLogic = new RoleLinkLogic();
            roleLogic = new RoleLogic();
            roleManager = new RoleManagerLogic();
        }

        #region MyRegion 视图入口控制器

        public void RoleLinkList()
        {
        }

        public void RoleLinkDetail()
        {
        }

        public void RoleList()
        {
        }

        public void RoleDetail()
        {
        }

        #endregion MyRegion 视图入口控制器
    }

    public partial class SysRoleManagerController
    {
        #region 菜单管理

        [HttpGet]
        public void GetRoleLink()
        {
            int total = 0;
            var mData = roleLinkLogic.GetRoleLinkList(Query<string>("txtTitle"), Query<int>("Type"), out total,
                Query<int>("parentId", -1), Query<int>("page"), Query<int>("limit"));
            Write(LayuiHelper.GetResponseJson(JsonHelper.ToJson(mData), total, mData.Rows.Count));
        }

        [HttpPost]
        public void GetRoleLinkOne()
        {
            var mData = roleLinkLogic.GetRowRoleLink(Query<int>("Id"));
            Write(mData);
        }

        [HttpPost]
        public void GetParentLinkList()
        {
            var mData = roleLinkLogic.GetParentLinkList(Query<int>("parentId", 0));
            Write(JsonHelper.ToJson(mData));
        }

        [HttpPost]
        public void AddOrUpdRoleLink()
        {
            var mData = roleLinkLogic.AddOrUpdLink(Query<int>("Id"), Query<string>("txtTitle"), Query<string>("Icon"),
                Query<string>("Href"), Query<int>("Type"), Query<string>("CssId"), Query<int>("parentId"));
            Write(mData ? "保存成功!" : "保存失败!", mData);
        }

        [HttpPost]
        public void DelRoleLink()
        {
            var mData = roleLinkLogic.DelLink(Query<string>("ids"));
            Write(mData ? "删除成功!" : "删除失败!", mData);
        }
        #endregion

        #region 角色管理

        [HttpGet]
        public void GetRoleList()
        {
            int total = 0;
            var mData = roleLogic.GetRoleList(Query<string>("txtName"), out total, Query<int>("page"), Query<int>("limit"));
            Write(LayuiHelper.GetResponseJson(JsonHelper.ToJson(mData), total, mData.Rows.Count));
        }

        [HttpPost]
        public void GetRoleOne()
        {
            var mData = roleLogic.GetOneRole(Query<int>("Id"));
            if (mData.Count > 0)
            {
                var managerList = roleManager.GetRoleManagerList(Query<int>("Id"));
                var info = new
                {
                    Name = mData.Get<string>("Name"),
                    Remark = mData.Get<string>("Remark"),
                    Id = mData.Get<string>("Id"),
                    managerList = managerList
                };
                Write(info);
            }

        }

        [HttpPost]
        public void AddOrUpdRole()
        {
            int rId = Query<int>("Id");
            var mData = roleLogic.AddOrUpdRole(rId, Query<string>("txtName"), Query<string>("Remark"));
            if (rId > 0 & mData)
            {
                var limts = Query<string>("Ids");
                mData = roleManager.AddRoleManager(rId, limts);
            }
            Write(mData ? "保存成功!" : "保存失败!", mData);
        }

        [HttpPost]
        public void DelRole()
        {
            var mData = roleLogic.DelRole(Query<string>("ids"));
            Write(mData ? "删除成功!" : "删除失败!", mData);
        }
        #endregion
    }
}