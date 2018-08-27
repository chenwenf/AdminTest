using CYQ.Data;
using CYQ.Data.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.SysCon
{
    public partial class RoleLogic
    {
    }


    public partial class RoleLogic
    {


        /// <summary>
        /// 添加或修改权限管理员
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool AddOrUpdRole(int Id, string name, string Remark)
        {
            using (MAction m = new MAction("C_Role"))
            {
                m.Set("Name", name);
                m.Set("Remark", Remark);
                if (Id > 0) { return m.Update(" Id=" + Id); }//修改
                else { return m.Insert(); }//添加
            }
        }
        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DelRole(string ids)
        {
            using (MAction m = new MAction("C_Role"))
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
        public MDataTable GetRoleList(string name, out int total, int page = 1, int rows = 10)
        {
            StringBuilder where = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(name))
            {
                where.AppendFormat(" AND Name='{0}'", name);
            }


            using (MAction m = new MAction("C_Role"))
            {
                return m.Select(page, rows, where.ToString(), out total);
            }
        }

        /// <summary>
        ///获取单条管理员
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MDataRow GetOneRole(int Id)
        {
            using (MAction m = new MAction("C_Role"))
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
