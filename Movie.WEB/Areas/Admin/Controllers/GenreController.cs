using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Movie.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var genres = _unitOfWork.Genres.GetAll();

            return View(genres);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Genres.Add(genre);
                _unitOfWork.Save();


                TempData["Success"] = "Genre is created successfully.";

                return RedirectToAction("Index");
            }

            return View(genre);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var genre = _unitOfWork.Genres.GetOne(x => x.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Genres.Update(genre);
                _unitOfWork.Save();

                TempData["Success"] = "Genre is updated successfully.";

                return RedirectToAction("Index");
            }

            return View(genre);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var genre = _unitOfWork.Genres.GetOne(x => x.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var genre = _unitOfWork.Genres.GetOne(x => x.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            _unitOfWork.Genres.Remove(genre);
            _unitOfWork.Save();

            TempData["Success"] = "Genre is deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
