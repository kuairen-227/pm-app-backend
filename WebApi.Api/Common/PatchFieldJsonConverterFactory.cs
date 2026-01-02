using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi.Api.Common;

/// <summary>
/// <see cref="PatchField{T}"/> 用の JsonConverter を自動生成する Factory
/// </summary>
public sealed class PatchFieldJsonConverterFactory : JsonConverterFactory
{
    /// <summary>
    /// 対象の型が <see cref="PatchField{T}"/> かどうかを判定する
    /// </summary>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(PatchField<>);
    }

    /// <summary>
    /// 対応する <see cref="PatchFieldJsonConverter{T}"/> を生成する
    /// </summary>
    public override JsonConverter CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var valueType = typeToConvert.GetGenericArguments()[0];

        var converterType = typeof(PatchFieldJsonConverter<>)
            .MakeGenericType(valueType);

        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}
