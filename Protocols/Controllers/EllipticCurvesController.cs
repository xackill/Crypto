using System;
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

            try
            {
                var field = isPrimeField
                    ? EllipticParser.ParsePrimeField(finiteFieldModel.Modulus)
                    : EllipticParser.ParseBinaryField(finiteFieldModel.Modulus, finiteFieldModel.ReductionPolynomial);

                var curve = isPrimeField
                    ? EllipticParser.ParsePrimeEllipticCurve(ellipticCurveModel.A, ellipticCurveModel.B, field)
                    : EllipticParser.ParseBinaryEllipticCurve(ellipticCurveModel.A, ellipticCurveModel.B, field);

                return Calculate(curve, operationsModels, field);
            }
            catch (Exception e)
            {
                return $"Ошибка! {e.Message}";
            }
        }
        
        private static string Calculate(EllipticCurve curve, EllipticCurveOperationModel[] operations, FiniteField field)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < operations.Length; i++)
            {
                var operation = operations[i];
                try
                {
                    switch (operation.Type)
                    {
                        case "+":
                            var a = EllipticParser.ParsePoint(operation.X1, operation.Y1, field);
                            var b = EllipticParser.ParsePoint(operation.X1, operation.Y1, field);
                            var c = curve.Summarize(a, b);
                            sb.AppendLine($"{i+1}. {a} + {b} = {c}");
                            break;
                        case "x":
                            var factor = SmartParser.Parse(operation.Factor);
                            var point = EllipticParser.ParsePoint(operation.X1, operation.Y1, field);
                            var res = curve.Multiply(factor, point);
                            sb.AppendLine($"{i+1}. {factor} x {point} = {res}");
                            break;
                        default:
                            sb.AppendLine($"{i+1}. Неизвестная операция: {operation.Type}");
                            break;
                    }
                }
                catch (Exception e)
                {
                    sb.Append($"{i+1} {e.Message}");
                }
            }
            
            return sb.ToString();
        }
    }
}