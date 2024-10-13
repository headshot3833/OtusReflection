
using Newtonsoft.Json;
using OtusReflection.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;


int iterations = 10000000;
var obj = F.Get();


Stopwatch stopwatch = Stopwatch.StartNew();
string customSerialized = "";
for (int i = 0; i < iterations; i++)
{
    customSerialized = SerializeToString(obj);
}
stopwatch.Stop();
long customSerializationTime = stopwatch.ElapsedMilliseconds;
Console.WriteLine($"Кастомная сериализация (рефлекшен): {customSerialized}");
Console.WriteLine($"Время кастомной сериализации для {iterations} итераций: {customSerializationTime} ms");


stopwatch.Restart();
Console.WriteLine(customSerialized);
stopwatch.Stop();
long consoleTime = stopwatch.ElapsedMilliseconds;
Console.WriteLine($"Время вывода в консоль: {consoleTime} ms");



stopwatch.Restart();
string jsonSerialized = "";
for (int i = 0; i < iterations; i++)
{
    jsonSerialized = JsonConvert.SerializeObject(obj);
}
stopwatch.Stop();
long jsonSerializationTime = stopwatch.ElapsedMilliseconds;
Console.WriteLine($"JSON сериализация (Newtonsoft.Json): {jsonSerialized}");
Console.WriteLine($"Время JSON сериализации для {iterations} итераций: {jsonSerializationTime} ms");


stopwatch.Restart();
for (int i = 0; i < iterations; i++)
{
    var jsonObj = JsonConvert.DeserializeObject<F>(jsonSerialized);
}
stopwatch.Stop();
long jsonDeserializationTime = stopwatch.ElapsedMilliseconds;
Console.WriteLine($"Время JSON десериализации для {iterations} итераций: {jsonDeserializationTime} ms");


static string SerializeToString(object obj)
{
    StringBuilder sb = new StringBuilder();
    Type type = obj.GetType();

    foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
    {
        object value = field.GetValue(obj);
        sb.Append($"{field.Name}={value}; ");
    }

    foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
    {
        object value = property.GetValue(obj);
        sb.Append($"{property.Name}={value}; ");
    }

    return sb.ToString();
}

