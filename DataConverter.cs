using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace OcrProject;

public class DataConverter
{
    public static void Convert(AnalyzedDocument document, InvoiceFields data)
    {
        data.VendorName = SetValue(document.Fields, nameof(InvoiceFields.VendorName));
        data.CustomerName = SetValue(document.Fields, nameof(InvoiceFields.CustomerName));

        document.Fields.TryGetValue("Items", out var itemsField);

        if (itemsField is { FieldType: DocumentFieldType.List })
            foreach (var itemField in itemsField.Value.AsList())
            {
                if (itemField.FieldType != DocumentFieldType.Dictionary) continue;

                var itemFields = itemField.Value.AsDictionary();

                data.Items.Add(new Items
                {
                    Description = SetValue(itemFields, nameof(Items.Description)),
                    Amount = SetCurrency(itemFields, nameof(Items.Amount)),
                    UnitPrice = SetCurrency(itemFields, nameof(Items.UnitPrice)),
                    TotalPrice = SetCurrency(itemFields, nameof(Items.TotalPrice))
                });
            }

        data.InvoiceTotal = SetCurrency(document.Fields, nameof(InvoiceFields.InvoiceTotal));
        data.Subtotal = SetCurrency(document.Fields, nameof(InvoiceFields.Subtotal));
        data.TotalTax = SetCurrency(document.Fields, nameof(InvoiceFields.TotalTax));
        data.InvoiceTotal = SetCurrency(document.Fields, nameof(InvoiceFields.InvoiceTotal));
    }
    
    private static string SetValue(IReadOnlyDictionary<string, DocumentField> fields, string key)
    {
        fields.TryGetValue(key, out var documentField);

        return documentField is { FieldType : DocumentFieldType.String }
            ? documentField.Value.AsString()
            : string.Empty;
    }
    
    private static Currency? SetCurrency(IReadOnlyDictionary<string, DocumentField> fields, string key)
    {
        fields.TryGetValue(key, out var documentField);

        if (documentField is not { FieldType : DocumentFieldType.Currency }) return null;

        var currency = documentField.Value.AsCurrency();
        return new Currency
        {
            CurrencyValue = $"{currency.Symbol}{currency.Amount} {currency.Code}"
        };
    }
}