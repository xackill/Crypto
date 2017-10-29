using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using EllipticCurves.DataModels.EllipticCurves;
using EllipticCurves.DataModels.FiniteFields;
using EllipticCurves.Helpers;
using Newtonsoft.Json;
using Protocols.Models;
using SmartParser = Core.Helpers.BigIntegerSmartParser;

namespace Protocols.Controllers
{
    public class EllipticCurvesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Work()
        {
            return View();
        }

        public string Calculate(string finiteField, string ellipticCurve, string operations)
        {
            var finiteFieldModel = JsonConvert.DeserializeObject<FiniteFieldModel>(finiteField);
            var ellipticCurveModel = JsonConvert.DeserializeObject<EllipticCurveModel>(ellipticCurve);
            var operationsModels = JsonConvert.DeserializeObject<EllipticCurveOperationModel[]>(operations);
            
            var isPrimeField = finiteFieldModel.Type == "GF(p)";
            var field = isPrimeField
                ? EllipticParser.ParsePrimeField(finiteFieldModel.Modulus)
                : EllipticParser.ParseBinaryField(finiteFieldModel.Modulus, finiteFieldModel.ReductionPolynomial);

            var curve = isPrimeField
                ? EllipticParser.ParsePrimeEllipticCurve(ellipticCurveModel.A, ellipticCurveModel.B, field)
                : EllipticParser.ParseBinaryEllipticCurve(ellipticCurveModel.A, ellipticCurveModel.B, field);

            return Calculate(curve, operationsModels, field);
        }
        
        private static string Calculate(EllipticCurve curve, IEnumerable<EllipticCurveOperationModel> operations, FiniteField field)
        {
            var sb = new StringBuilder();

            foreach (var operation in operations)
            {
                switch (operation.Type)
                {
                    case "+":
                        var a = EllipticParser.ParsePoint(operation.X1, operation.Y1, field);
                        var b = EllipticParser.ParsePoint(operation.X1, operation.Y1, field);
                        sb.AppendLine($"{a} + {b} = {curve.Summarize(a, b)}");
                        break;
                    case "x":
                        var factor = SmartParser.Parse(operation.Factor);
                        var point = EllipticParser.ParsePoint(operation.X1, operation.Y1, field);
                        sb.AppendLine($"{factor} x {point} = {curve.Multiply(factor, point)}");
                        break;
                    default:
                        sb.AppendLine($"Неизвестная операция: {operation.Type}");
                        break;
                }
            }
            
            return sb.ToString();
        }
    }
}