using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi.Api.Common;

/// <summary>
/// <see cref="PatchField{T}"/> 用の JSON Converter
/// </summary>
/// <remarks>
/// JSON における undefined / null / value を適切に判別する
/// </remarks>
public sealed class PatchFieldJsonConverter<T> : JsonConverter<PatchField<T>>
{
    /// <summary>
    /// JSON から <see cref="PatchField{T}"/> を読み取る
    /// </summary>
    public override PatchField<T> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        // "field": null
        if (reader.TokenType == JsonTokenType.Null)
        {
            return PatchField<T>.From(default);
        }

        // "field": <value>
        var value = JsonSerializer.Deserialize<T>(ref reader, options);
        return PatchField<T>.From(value);
    }

    /// <summary>
    /// PATCH リクエスト専用のため書き込みはサポートしない
    /// </summary>
    public override void Write(
        Utf8JsonWriter writer,
        PatchField<T> value,
        JsonSerializerOptions options)
    {
        throw new NotSupportedException("PatchField<T> は PATCH リクエスト専用です");
    }
}
