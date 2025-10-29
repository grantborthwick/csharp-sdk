using ModelContextProtocol.Protocol;

namespace ModelContextProtocol.Server;

/// <summary>
/// Specifies the locale/language for the tool, prompt, or resource output.
/// </summary>
/// <remarks>
/// <para>
/// This attribute is a convenience wrapper around <see cref="McpMetaAttribute"/> that sets
/// locale metadata for internationalization support. It's commonly used with systems like 
/// ChatGPT's Apps SDK to indicate the language of the response content.
/// </para>
/// <para>
/// The locale should follow standard locale identifiers such as:
/// <list type="bullet">
///   <item>"en_US" - English (United States)</item>
///   <item>"fr_FR" - French (France)</item>
///   <item>"ja_JP" - Japanese (Japan)</item>
///   <item>"es_ES" - Spanish (Spain)</item>
/// </list>
/// </para>
/// <para>
/// This attribute can be combined with other metadata attributes to provide comprehensive
/// context for UI rendering and content localization.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// [McpServerTool]
/// [Locale("en_US")]
/// [Description("Returns weather information in English")]
/// public static async Task&lt;string&gt; GetWeather(string city, CancellationToken cancellationToken)
/// {
///     // Tool implementation
///     return $"Weather in {city}: Sunny, 72°F";
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class LocaleAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LocaleAttribute"/> class.
    /// </summary>
    /// <param name="locale">The locale identifier (e.g., "en_US", "fr_FR").</param>
    /// <param name="metadataKey">
    /// The metadata key to use. Defaults to "openai/locale" for ChatGPT compatibility.
    /// Can be customized for other systems.
    /// </param>
    public LocaleAttribute(string locale, string metadataKey = "openai/locale")
    {
        Locale = locale ?? throw new ArgumentNullException(nameof(locale));
        MetadataKey = metadataKey ?? throw new ArgumentNullException(nameof(metadataKey));
    }

    /// <summary>
    /// Gets the locale identifier.
    /// </summary>
    public string Locale { get; }

    /// <summary>
    /// Gets the metadata key used to store the locale.
    /// </summary>
    /// <remarks>
    /// Defaults to "openai/locale" for ChatGPT compatibility, but can be customized
    /// for other systems that support localization via metadata.
    /// </remarks>
    public string MetadataKey { get; }
}
