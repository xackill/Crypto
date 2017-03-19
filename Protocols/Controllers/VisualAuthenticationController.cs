using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using VisualAuthentication.DataBaseModels;
using VisualAuthentication.DataViewModels;
using VisualAuthentication.Extensions;
using VisualAuthentication.Factories;
using VisualAuthentication.Helpers;
using DataBase = Core.Workers.DataBase<VisualAuthentication.Workers.VisualAuthenticationContext>;

namespace Currency.Controllers
{
    public class VisualAuthenticationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //        public ActionResult Help()
        //        {
        //            return View();
        //        }

        public ActionResult Work()
        {
            var session = SessionFactory.CreateSession();
            var secretKey = session.SecretKey();

            DataBase.Write(session);

            ViewBag.SessionId = session.Id;
            ViewBag.KeyElementsInBase64 = secretKey.Elements.ToBase64Pics();

            return View();
        }

        public string GenerateNewField(Guid sessionId)
        {
            var session = DataBase.Read<Session>(sessionId);
            ++session.CurrentIteration;

            var field = FieldFactory.CreateField(session);
            var secretKey = session.SecretKey();
            session.CurrentCorrectNumber = PathFinder.GetCorrectAnswer(field, secretKey);

            DataBase.Update(session);

            var elementsInBase64 = field.Elements.Select(els => els.ToBase64Pics()).ToArray();
            var fieldViewModel = new FieldViewModel
            {
                ElementsInBase64 = elementsInBase64,
                ColumnAnswers = field.ColumnAnswers,
                RowAnswers = field.RowAnswers
            };

            return JsonConvert.SerializeObject(fieldViewModel);
        }
    }
}