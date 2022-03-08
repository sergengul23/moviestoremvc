using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Movie.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new();

            if (id == null || id == 0)
            {
                return View(company);
            }

            else
            {
                company = _unitOfWork.Companies.GetOne(x => x.Id == id);

                return View(company);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.Companies.Add(company);
                    TempData["success"] = "Company is created successfully";
                }

                else
                {
                    _unitOfWork.Companies.Update(company);
                    TempData["success"] = "Company updated successfully";
                }

                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(company);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Companies.GetAll();

            return Json(new { data = companyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var company = _unitOfWork.Companies.GetOne(x => x.Id == id);

            if (company == null)
            {
                return Json(new { success = false, message = "Error while deleting..." });
            }

            _unitOfWork.Companies.Remove(company);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete operation is successful." });
        }
        #endregion
    }
}
