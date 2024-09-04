
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BulkyBook.Utility;


namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize(Roles =SD.Role_Admin)]
    public class CompanyController : Controller
    {
        //private readonly ApplicationDbContext _db;

        /*
         public CompanyController(ApplicationDbContext db)
         {
             _db = db;
         }
        */

        // private readonly ICompanyRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;

        
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        //Index: Action Method
        public IActionResult Index()
        {
           // List<Company> objCompanyList = _unitOfWork.Company.GetAll(includeProperties: "Category").ToList();
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();


            return View(objCompanyList);
        }


        //Create+Update : Action method.
        public IActionResult Upsert(int? id)
        {
            
            if(id==null || id == 0)
            {
                //for Creating
                return View(new Company());
            }
            else
            {
                //For Updating
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);

                return View(companyObj);
            }
           

            
        }



        [HttpPost]
        public IActionResult Upsert( Company CompanyObj)
        {
            if (ModelState.IsValid)
            {
                if(CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(CompanyObj);
            }

        }
        

        #region API CALL
        [HttpGet]
        public IActionResult GetAll()
        {
            //List<Company> objCompanyList = _unitOfWork.Company.GetAll(includeProperties: "Category").ToList()
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if(companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            
            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfully" });
        }
        #endregion


    }
}
