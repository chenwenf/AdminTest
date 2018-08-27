using CYQ.Data;
using CYQ.Data.Table;
using System.Text;

namespace Logic.SysCon
{
    public partial class RoleLinkLogic //: BaseLogic
    {
        //public RoleLinkLogic(IController controller) : base(controller)
        //{
        //}
    }

    public partial class RoleLinkLogic
    {
        /// <summary>
        /// 添加或修改菜单路径
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="title"></param>
        /// <param name="icon"></param>
        /// <param name="href"></param>
        /// <param name="type"></param>
        /// <param name="cssId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool AddOrUpdLink(int Id, string title, string icon,
            string href, int type, string cssId, int parentId = 0)
        {
            using (MAction m = new MAction("C_RoleLink"))
            {
                m.Set("Title", title);
                m.Set("Icon", icon);
                m.Set("Href", href);
                m.Set("Type", type);
                m.Set("CssId", cssId);
                m.Set("ParentId", parentId);
                if (Id > 0) { return m.Update(" Id=" + Id); }//修改
                else { return m.Insert(); }//添加
            }
        }

        /// <summary>
        /// 删除路径
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DelLink(string ids)
        {
            using (MAction m = new MAction("C_RoleLink"))
            {
                return m.Delete(" Id in (" + ids + ")");
            }
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public MDataTable GetRoleLinkList(string title, int type,
            out int total, int parentId = -1, int page = 1, int rows = 10)
        {
            StringBuilder where = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(title))
            {
                where.AppendFormat(" AND Title='{0}'", title);
            }
            else if (type > 0)
            {
                where.AppendFormat(" AND Type='{0}'", type);
            }
            else if (parentId > -1)
            {
                where.AppendFormat(" AND ParentId={0}", parentId);
            }

            using (MAction m = new MAction("C_RoleLink"))
            {
                return m.Select(page, rows, where.ToString(), out total);
            }
        }

        /// <summary>
        /// 获取所有的顶级菜单 0:顶级 -1:所有
        /// </summary>
        /// <returns></returns>
        public MDataTable GetParentLinkList(int ParentId = -1)
        {
            string where = "";
            if (ParentId >= 0)
            {
                where = " ParentId=" + ParentId;
            }
            using (MAction m = new MAction("C_RoleLink"))
            {
                return m.Select(where);
            }
        }

        /// <summary>
        ///获取单条数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MDataRow GetRowRoleLink(int Id)
        {
            using (MAction m = new MAction("C_RoleLink"))
            {
                if (m.Fill("Id=" + Id))
                {
                    return m.Data;
                }
                return null;
            }
        }
    }
}