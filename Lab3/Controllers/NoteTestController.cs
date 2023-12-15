using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lab3.Models;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab3.Controllers
{
    public static class StringExtentions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }

    public class NoteTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectWithDataSet()
        {
            List<NoteDetail> NoteList = new List<NoteDetail>();
            NoteMethods nm = new NoteMethods();
            string error = "";
            NoteList = nm.GetNoteWithDataSet(out error);
            //ViewBag.numberOfNotes = HttpContext.Session.GetString(numberOfNotes);
            ViewBag.error = error;
            return View(NoteList);
        }

        public IActionResult SelectWithDataReader()
        {
            List<NoteDetail> NoteList = new List<NoteDetail>();
            NoteMethods nm = new NoteMethods();
            string error = "";
            NoteList = nm.GetNoteWithDataSet(out error);
            //ViewBag.numberOfNotes = HttpContext.Session.GetString(numberOfNotes);
            ViewBag.error = error;
            return View(NoteList);
        }

        public IActionResult SelectColaborations()
        {
            List<NoteColaborators> ColabList = new List<NoteColaborators>();
            NoteMethods nm = new NoteMethods();
            string error = "";
            ColabList = nm.SelectColaborations(out error);
            //ViewBag.numberOfNotes = HttpContext.Session.GetString(numberOfNotes);
            ViewBag.error = error;
            return View(ColabList);
        }

        [HttpGet]
        public IActionResult InsertNote()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertNote(NoteDetail nd)
        {
            NoteMethods nm = new NoteMethods();
            int i = 0;
            string error = "";
            i = nm.InsertNote(nd, out error);
            ViewBag.error = error;
            ViewBag.numberOfNotes = i;
            if (i == 1)
            {
                return RedirectToAction("");
            }
            else
            {
                return View("InsertNote");

            }
        }

        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(NoteDetail nd, string note_id)
        {
            int id = Convert.ToInt32(note_id);

            NoteMethods nm = new NoteMethods();
            string error = "";

            int i = nm.UpdateNote(nd, out string errorMessage1, id);

            ViewBag.error = error;
            ViewBag.numberOfNotes = i;
            if (i == 1)
            {
                return RedirectToAction("");
            }
            else
            {
                return View("InsertNote");
            }
        }

        [HttpGet]
        public IActionResult ViewNotes()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(NoteDetail nd, string note_id)
        {
            int id = Convert.ToInt32(note_id);

            NoteMethods nm = new NoteMethods();
            string error = "";

            int i = nm.DeleteNote(nd, out string errorMessage1, id);

            ViewBag.error = error;
            ViewBag.numberOfNotes = i;
            if (i == 1)
            {
                return RedirectToAction("");
            }
            else
            {
                return View("InsertNote");
            }
        }

        [HttpGet]
        public ActionResult Filter ()
        {
            NoteMethods nm = new NoteMethods();

            ViewModelPA myModel = new ViewModelPA
            {
                ColabList = nm.SelectColaborations(out string errorMessage1),
                NoteDetailList = nm.GetNoteWithDataSet(out string errorMessage2),
                OwnerDetailList = nm.GetUser(out string errorMessage3)
            };

            List<OwnerDetail> ownerList = new List<OwnerDetail>();
            ownerList = nm.GetUser(out string errorMessage4);
            ViewBag.error = "1: " + errorMessage1 + "2: " + errorMessage2 + "3: " + errorMessage3 + "4: " + errorMessage4;
            ViewData["ownerList"] = ownerList;

            ViewBag.ownerList = ownerList;

            return View(myModel);

        }

        [HttpPost]
        public ActionResult Filter(string User, string sortVar)
        {
            int i = Convert.ToInt32(User);

            NoteMethods nm = new NoteMethods();

            ViewModelPA myModel = new ViewModelPA
            {
                NoteDetailList = nm.GetNoteWithFilter(out string errorMessage1, i),
                OwnerDetailList = nm.GetUser(out string errorMessage3)
            };

            List<OwnerDetail> ownerList = new List<OwnerDetail>();
            ownerList = nm.GetUser(out string errorMessage4);
            ViewBag.error = "1: " + errorMessage1 + "3: " + errorMessage3 + "4: " + errorMessage4;
            ViewData["ownerList"] = ownerList;

            ViewBag.ownerList = ownerList;
            ViewData["user"] = i;

            return View("Sort", myModel);

        }

        [HttpGet]
        public ActionResult Sort(string User,string sortVar)
        {
            int i = Convert.ToInt32(User);

            NoteMethods nm = new NoteMethods();

            ViewData["title"] = sortVar == "title" ? "title_desc" : "title";
            ViewData["content"] = sortVar == "content" ? "content_desc" : "content";
            ViewData["owner"] = sortVar == "owner" ? "owner_desc" : "owner";

            string sqlquery = "SELECT * FROM Tbl_note"; ;

            switch (sortVar)
            {
                case "title":
                    sqlquery = "SELECT * FROM Tbl_note ORDER BY note_title ASC";
                    break;
                case "title_desc":
                    sqlquery = "SELECT * FROM Tbl_note ORDER BY note_title DESC";
                    break;
                case "content":
                    sqlquery = "SELECT * FROM Tbl_note ORDER BY note_content ASC";
                    break;
                case "content_desc":
                    sqlquery = "SELECT * FROM Tbl_note ORDER BY note_content DESC";
                    break;
                case "owner":
                    sqlquery = "SELECT * FROM Tbl_note ORDER BY note_owner ASC";
                    break;
                case "owner_desc":
                    sqlquery = "SELECT * FROM Tbl_note ORDER BY note_owner DESC";
                    break;
            }

            ViewModelPA myModel = new ViewModelPA
            {
                NoteDetailList = nm.GetNoteSorted(out string errorMessage2, sqlquery),
                OwnerDetailList = nm.GetUser(out string errorMessage3)
            };

            ViewBag.sort = sortVar;
            ViewData["user"] = i;

            return View(myModel);
        }

        [HttpGet]
        public ActionResult Search(string searchStr)
        {
            NoteMethods nm = new NoteMethods();
            
            ViewModelPA myModel = new ViewModelPA
            {
                NoteDetailList = nm.GetSearchNote(out string errorMessage2, searchStr),
                OwnerDetailList = nm.GetUser(out string errorMessage3)
            };

            return View(myModel);
        }
    }
}

