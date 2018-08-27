using CYQ.Data;
using CYQ.Data.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.SysCon
{
    public partial class RoleManagerLogic
    {
    }


    public partial class RoleManagerLogic
    {


        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool AddRoleManager(int roleId, string idStrs)
        {
            try
            {
                string[] ids = idStrs.Split(',');
                if (ids.Length <= 0)
                {
                    return false;
                }
                #region 数据处理
                MDataTable dtRecord = new MDataTable();
                dtRecord.Columns.Add("RId", SqlDbType.Int);
                dtRecord.Columns.Add("LId", SqlDbType.Int);

                foreach (string item in ids)
                {
                    var row = dtRecord.NewRow();
                    row["RId"].Value = roleId;
                    row["LId"].Value = item;
                    dtRecord.Rows.Add(row);
                }
                #endregion

                using (MAction m = new MAction("C_RoleManager"))
                {
                    // 先删除数据然后批量插入记录
                    m.Delete(" RId=" + roleId);
                }
                dtRecord.TableName = "C_RoleManager";
                dtRecord.AcceptChanges(AcceptOp.Insert);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public MDataTable GetRoleManagerList(int Id)
        {

            string strSql = "SELECT l.*,m.rid,case when m.rid is null then 0 else 1 end IsCheck   FROM C_RoleLink l left join  C_RoleManager m  on l.Id=m.LId";
            StringBuilder where = new StringBuilder("");
            if (Id > 0)
            {
                where.AppendFormat(" AND RId={0}", Id);
            }

            strSql += where.ToString();
            using (MProc m = new MProc(strSql))
            {
                return m.ExeMDataTable();
            }
        }

    }
}
