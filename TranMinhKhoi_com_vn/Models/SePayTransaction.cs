using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TranMinhKhoi_com_vn.Models
{
    public class SePayTransaction
    {
        public int id { get; set; }
        public string? gateway { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime transactionDate { get; set; }
        public string? accountNumber { get; set; }
        public string? code { get; set; }
        public string? content { get; set; }
        public string? transferType { get; set; }
        public decimal transferAmount { get; set; }
        public decimal accumulated { get; set; }
        public string? subAccount { get; set; }
        public string? referenceCode { get; set; }
        public string? description { get; set; }
    }
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-dd HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (DateTime.TryParseExact(value, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date;
            }

            throw new JsonException($"Date '{value}' không đúng định dạng '{Format}'");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
