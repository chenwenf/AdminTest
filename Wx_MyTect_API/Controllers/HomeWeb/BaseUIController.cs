using Logic;
using Taurus.Core;

namespace Controllers.HomeWeb
{
    public partial class BaseUIController: Controller
    {

        private readonly UserLogic userLogic;
        public BaseUIController()
        {

            userLogic = new UserLogic();

        }

    }
}
