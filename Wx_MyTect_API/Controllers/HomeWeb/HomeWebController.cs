using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.HomeWeb
{
    public partial class HomeWebController : BaseUIController
    {
        public HomeWebController()
        {
        }

    }


    public partial class HomeWebController {


        public void index()
        {

            View.Set("MyHello", "欢迎测试~~~~");
        }
    }
}
