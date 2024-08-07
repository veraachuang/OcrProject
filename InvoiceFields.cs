using System.Text.Json;
using System.Text.Json.Serialization;

namespace OcrProject;

public class InvoiceFields
{
    public string? VendorName { get; set; }
    public float? VendorNameConfidence { get; set; }
    public string? CustomerName { get; set; }
    public float? CustomerNameConfidence { get; set; }
    public List<Items> Items { get; set; } = [];
    public Currency? Subtotal { get; set; }
    public float? SubtotalConfidence { get; set; }
    public Currency? TotalTax { get; set; }
    public float? TotalTaxConfidence { get; set; }
    public Currency? InvoiceTotal { get; set; }
    public float? InvoiceTotalConfidence { get; set; }
}

public class Items
{
    public string? Description { get; set; }
    public Currency? Amount { get; set; }
    public Currency? UnitPrice { get; set; }
    public Currency? TotalPrice { get; set; }
}

public class Currency
{
    public string? CurrencyValue { get; set; }

    public override string ToString()
    {
        return CurrencyValue ?? string.Empty;
    }
}

// Custom converter: serializing currency values without property name
public class CurrencyConverter : JsonConverter<Currency>
{
    public override Currency Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new Currency { CurrencyValue = reader.GetString() };
    }

    public override void Write(Utf8JsonWriter writer, Currency value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.CurrencyValue);
    }
}