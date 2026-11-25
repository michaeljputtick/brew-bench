using System.Text.Json;
using BrewBench.Models;
using Microsoft.JSInterop;

namespace BrewBench.Services;

/// <summary>
/// Recipes live in the browser's localStorage, so each visitor has their own private library.
/// WebAssembly can call localStorage synchronously, which keeps the same API the pages already use.
/// Export / import moves recipes between browsers and devices.
/// </summary>
public class RecipeStore
{
    private const string StorageKey = "brewbench.recipes";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    private readonly IJSInProcessRuntime js;
    private readonly Dictionary<string, Recipe> recipes;

    public RecipeStore(IJSRuntime jsRuntime)
    {
        js = (IJSInProcessRuntime)jsRuntime;
        recipes = Load();
        SeedPresetsIfMissing();
    }

    private Dictionary<string, Recipe> Load()
    {
        try
        {
            var json = js.Invoke<string?>("localStorage.getItem", StorageKey);
            var list = string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<List<Recipe>>(json, JsonOptions);
            return (list ?? []).ToDictionary(r => r.Id);
        }
        catch
        {
            return []; // storage blocked or data malformed: start fresh rather than crash
        }
    }

    private void Persist()
    {
        try
        {
            js.InvokeVoid("localStorage.setItem", StorageKey, JsonSerializer.Serialize(recipes.Values.ToList(), JsonOptions));
        }
        catch
        {
            // Private browsing or full storage: keep working in memory for this session.
        }
    }

    private void SeedPresetsIfMissing()
    {
        var added = false;
        foreach (var preset in PresetRecipes.All().Where(p => !recipes.ContainsKey(p.Id)))
        {
            recipes[preset.Id] = preset;
            added = true;
        }
        if (added) Persist();
    }

    public IEnumerable<Recipe> List() => recipes.Values.ToList();

    public Recipe? Get(string id) => recipes.TryGetValue(id, out var r) ? r : null;

    public void Save(Recipe r)
    {
        r.UpdatedUtc = DateTime.UtcNow;
        recipes[r.Id] = r;
        Persist();
    }

    public void Delete(string id)
    {
        if (recipes.Remove(id)) Persist();
    }

    public Recipe Duplicate(Recipe r, string? newName = null)
    {
        var copy = r.Clone();
        copy.Name = newName ?? (r.Name + " (copy)");
        Save(copy);
        return copy;
    }

    public string ExportJson(IEnumerable<Recipe>? only = null) =>
        JsonSerializer.Serialize((only ?? recipes.Values).ToList(), JsonOptions);

    /// <summary>
    /// Accepts an exported list or a single recipe (the desktop app saved one recipe per file).
    /// A recipe with a known id replaces it; anything else is added as your own recipe.
    /// </summary>
    public int ImportJson(string json)
    {
        json = json.Trim();
        var incoming = json.StartsWith('[')
            ? JsonSerializer.Deserialize<List<Recipe>>(json, JsonOptions) ?? []
            : [JsonSerializer.Deserialize<Recipe>(json, JsonOptions)!];

        var count = 0;
        foreach (var r in incoming.Where(r => r is not null && !string.IsNullOrWhiteSpace(r.Name)))
        {
            r.IsPreset = recipes.TryGetValue(r.Id, out var existing) && existing.IsPreset;
            recipes[r.Id] = r;
            count++;
        }
        if (count > 0) Persist();
        return count;
    }
}
