using framework.Types;
using AngleSharp.Common;
using AngleSharp.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace framework.Helper;

public static class TestDataManager
{
    public static ConcurrentDictionary<string, string?> TestData = new();

    public static void AddToContext(ref ScenarioContext scenarioContext)
    {
        if (TestData == null)
            return;
        foreach (var data in TestData)
        {
            scenarioContext.Add(data.Key, data.Value);
        }
    }

    public static void Configure()
    {
        var environmentName = ConfigManager.GetConfiguration("environment");
        List<ConcurrentDictionary<string, string?>>? testDataCollection = null;

        using (StreamReader r = new StreamReader("testdata.json"))
        {
            string json = r.ReadToEnd();
            if (json != null)
            {
                testDataCollection = JsonConvert.DeserializeObject<List<ConcurrentDictionary<string, string?>>>(json);
            }
        }

        if (testDataCollection != null)
        {
            foreach (var environment in testDataCollection)
            {
                if (environment != null)
                {
                    if (environment.TryGetValue("EnvironmentName", out string? actualEnvironmentName) && actualEnvironmentName == environmentName)
                    {
                        TestData = environment;
                        break;
                    }
                }
            }
        }
    }

    public static string GetTestData(string testDataName)
    {
        TestData.TryGetValue(testDataName, out var value);
        return value ?? string.Empty;
    }

    public static string Transform(ScenarioContext keyValuePairs, string valueToTransform)
    {
        if (valueToTransform == null || valueToTransform == string.Empty)
        {
            return valueToTransform;
        }
        string key = valueToTransform;
        string operation = string.Empty;
        string operand = string.Empty;
        string value = valueToTransform;
        dynamic result;
        if (key.StartsWith('['))
        {
            if (key.Contains('+') || key.Contains('-'))
            {
                operation = "+";

                if (key.Contains('-'))
                    operation = "-";
                var temp = key.Split(operation)[0];
                operand = key.Split(operation)[1];
                key = temp;
            }
            key = key.Replace("[", "");
            key = key.Replace("]", "");
            key = key.Trim();
        }

        if (key != string.Empty && keyValuePairs.ContainsKey(key))
        {
            keyValuePairs.TryGetValue<string>(key, out value);
        }

        if (operation != string.Empty && operand != string.Empty)
        {
            var actualOperand = Int32.Parse(operand);
            if (value.Contains('-')) // Check if the operand one is a date
            {
                var convertedValue = Convert.ToDateTime(value);
                DateTime temp;
                switch (operation)
                {
                    case "+":
                        temp = convertedValue.AddDays(actualOperand);
                        break;

                    case "-":
                        temp = convertedValue.AddDays(-actualOperand);
                        break;

                    default:
                        throw new Exception("Operation not implemented in TestDataManager");
                }
                result = $"{temp:yyyy-MM-dd}";
            }
            else // If not date checking if its a float or decimal
            {
                dynamic convertedValue;
                if (value.Contains('.'))
                {
                    convertedValue = Double.Parse(value);
                }
                else
                {
                    convertedValue = Int32.Parse(value);
                }

                switch (operation)
                {
                    case "+":
                        result = (convertedValue + actualOperand);
                        break;

                    case "-":
                        result = (convertedValue - actualOperand);
                        break;

                    default:
                        throw new Exception("Operation not implemented in TestDataManager");
                }
                if (result != null && result?.GetType() == typeof(double))
                {
                    result = string.Format($"{result:0.00}");
                }
            }
            value = result ?? value;
        }

        return value.ToString();
    }

    public static string TransformToValidNumber(string number)
    {
        string result = string.Empty;
        result = number.Trim();
        var lenght = number.Length;
        if (number.IndexOf(".") == lenght - 3)
        {
            result = String.Concat(number.Where(c => c != ','));
        }
        result = result.Replace(",", ".");
        result = String.Concat(result.Where(c => !c.IsWhiteSpaceCharacter()));
        return result;
    }
}