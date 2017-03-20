using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Newtonsoft.Json;
using Core.Extensions;
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
            var session = SessionFactory.CreateSession();
            var secretKey = session.SecretKey();

            DataBase.Write(session);

            ViewBag.SessionId = session.Id;
            ViewBag.KeyElementsInBase64 = secretKey
                                            .Elements
                                            .Select(PictureDrawer.Draw)
                                            .Select(BmpConvert.ToBase64)
                                            .ToList();

            return View();
        }

        public string GenerateNewField(Guid sessionId)
        {
            var session = DataBase.Read<Session>(sessionId);
            return GenerateNewField(session);
        }
        
        public string SendAnswer(Guid sessionId, int answer)
        {
            var session = DataBase.Read<Session>(sessionId);
            if (answer != session.CurrentCorrectNumber && session.FirstErrorIteration == -1)
                session.FirstErrorIteration = session.CurrentIteration;

            if (session.IsClose())
                return JsonConvert.SerializeObject(session.GetFinResult());
            return GenerateNewField(session);
        }

        private string GenerateNewField(Session session)
        {
            Thread.Sleep(300);
            ++session.CurrentIteration;

            var field = FieldFactory.CreateField(session);
            var secretKey = session.SecretKey();
            session.CurrentCorrectNumber = PathFinder.GetCorrectAnswer(field, secretKey);

            DataBase.Update(session);

            var picsInBase64 = new List<List<string>>();
            for (var i = 0; i < field.Elements.Length; ++i)
            {
                var lastImg = field.RowAnswers[i];
                var row = field
                            .Elements[i]
                            .Select(PictureDrawer.Draw)
                            .Concat(PictureDrawer.Draw(lastImg))
                            .Select(BmpConvert.ToBase64)
                            .ToList();

                picsInBase64.Add(row);
            }

            var lastList = field
                            .ColumnAnswers
                            .Select(PictureDrawer.Draw)
                            .Concat(PictureDrawer.DrawEmptyImg())
                            .Select(BmpConvert.ToBase64)
                            .ToList();

            picsInBase64.Add(lastList);

            var fieldViewModel = new FieldViewModel
            {
                Pics = picsInBase64,
                ColumnAnswers = field.ColumnAnswers,
                RowAnswers = field.RowAnswers
            };

            return JsonConvert.SerializeObject(fieldViewModel);
        }
    }
}