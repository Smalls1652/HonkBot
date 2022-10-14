using System.Text.Json.Serialization;

namespace HonkBot.Models.Config;

/// <summary>
/// Configuration for the random react feature in a specific server.
/// </summary>
public class RandomReactConfig : IRandomReactConfig
{
    /// <summary>
    /// Default constructor for the <see cref="RandomReactConfig"/> class.
    /// </summary>
    public RandomReactConfig()
    {
        Enabled = false;
        PercentChanceToHappen = 10;
    }

    /// <inheritdoc cref="IRandomReactConfig.Enabled"/>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    /// <inheritdoc cref="IRandomReactConfig.PercentChanceToHappen"/>
    [JsonPropertyName("percentChanceToHappen")]
    public int PercentChanceToHappen
    {
        get => _percentChanceToHappen;
        set => SetPercentChanceToHappen(value);
    }

    private int _percentChanceToHappen;

    /// <summary>
    /// Set the PercentChanceToHappen property.
    /// </summary>
    /// <param name="value">The percentage to set to.</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs because the provided value is not between 0 and 100</exception>
    private void SetPercentChanceToHappen(int value)
    {
        // Ensure that the provided value is between 0 and 100.
        if (value > 100 || value < 0)
        {
            // If the value not between 0 and 100, throw an ArgumentOutOfRangeException.
            throw new ArgumentOutOfRangeException(nameof(value), "'PercentChanceToHappen' must be between 0 and 100.");
        }

        // Set _percentChanceToHappen to the provided value.
        _percentChanceToHappen = value;
    }
}