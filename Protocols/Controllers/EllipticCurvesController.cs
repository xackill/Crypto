using System;
using System.Linq;
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

        public ActionResult Help()
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

        public string CalculateFromFile(string finiteFieldType, string inputFile)
        {
            var isPrimeField = finiteFieldType == "GF(p)";
            var data = inputFile.Split(new[] { "\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                if (data.Length < 4)
                    throw new Exception("Неверный формат файла");

                FiniteField field;
                if (isPrimeField)
                    field = EllipticParser.ParsePrimeField(data[0]);
                else
                {
                    var fieldParams = data[0].Split(' ');
                    if (fieldParams.Length != 2)
                        throw new Exception("Неверный формат файла");

                    field = EllipticParser.ParseBinaryField(f: fieldParams[1], p: fieldParams[0]);
                }

                var curveParams = data[1].Split(' ');
                if (curveParams.Length != 2)
                    throw new Exception("Неверный формат файла");
                
                var curve = isPrimeField
                    ? EllipticParser.ParsePrimeEllipticCurve(a: curveParams[0], b: curveParams[1], field: field)
                    : EllipticParser.ParseBinaryEllipticCurve(a: curveParams[0], b: curveParams[1], field: field);

                var operationsParams = data[2].Split(' ');
                if (operationsParams.Length != 2)
                    throw new Exception("Неверный формат файла");

                var t = int.Parse(operationsParams[0]);
                var s = int.Parse(operationsParams[1]);

                if (data.Length < t + s + 3)
                    throw new Exception("Неверный формат файла");

                var sumOperations = data.Skip(3).Take(t).Select(ParseSummarizeOperation);
                var mulOperations = data.Skip(3 + t).Take(s).Select(ParseMultiplyOperation);
                
                return Calculate(curve, sumOperations.Union(mulOperations).ToArray(), field);               
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

        private static EllipticCurveOperationModel ParseSummarizeOperation(string data)
        {
            var operationParams = data.Split(' ');
            if (operationParams.Length != 4)
                throw new Exception("Неверный формат файла");
            
            return new EllipticCurveOperationModel
            {
                Type = "+",
                X1 = operationParams[0],
                Y1 = operationParams[1],
                X2 = operationParams[2],
                Y2 = operationParams[3]
            };
        }
        
        private static EllipticCurveOperationModel ParseMultiplyOperation(string data)
        {
            var operationParams = data.Split(' ');
            if (operationParams.Length != 3)
                throw new Exception("Неверный формат файла");
            
            return new EllipticCurveOperationModel
            {
                Type = "x",
                X1 = operationParams[0],
                Y1 = operationParams[1],
                Factor = operationParams[2],
            };
        }
    }
}