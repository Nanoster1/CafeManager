using System.Text.Json.Serialization;

namespace CafeManager.Contracts.Dto.Orders.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentTypeDto
{
    Cash,
    Card
}