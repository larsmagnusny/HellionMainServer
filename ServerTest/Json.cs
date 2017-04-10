namespace ServerTest
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public interface ISceneData
    {

    }

    public static class Json
    {
        public static string Serialize(object obj, Formatting format = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, (Newtonsoft.Json.Formatting)format);
        }
        public static void SerializeToFile(object obj, string filePath, Formatting format = Formatting.None)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(obj, (Newtonsoft.Json.Formatting) format));
        }

        public enum Formatting
        {
            None,
            Indented
        }
    }
}
