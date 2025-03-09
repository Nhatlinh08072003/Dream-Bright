
using Microsoft.AspNetCore.Mvc;

namespace Dream_Bridge.Controllers
{
    public abstract class DichvuControllerDecorator : IDichvuController
    {
        protected readonly IDichvuController _decoratedController;

        protected DichvuControllerDecorator(IDichvuController decoratedController)
        {
            _decoratedController = decoratedController;
        }

        public virtual IActionResult VisaUc() => _decoratedController.VisaUc();
        public virtual IActionResult VisaMy() => _decoratedController.VisaMy();
        public virtual IActionResult VisaSingapore() => _decoratedController.VisaSingapore();
        public virtual IActionResult VisaThuySy() => _decoratedController.VisaThuySy();
        public virtual IActionResult ChiTietTruong(int id) => _decoratedController.ChiTietTruong(id);
        public virtual IActionResult Chitiet(int id) => _decoratedController.Chitiet(id);
        public virtual IActionResult Error() => _decoratedController.Error();
    }
}
