using Godot;
using DW.Demo;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Linq;

public partial class Main : Node
{
	private Game[] games;

	public override void _Ready()
	{
		this.LoadGames();

		// prep ItemList 
		var gameList = GetNode<ItemList>("VBoxContainer/HSplitContainer/ItemList");
		gameList.ItemSelected += this.OnItemSelectedEventHandler;
		foreach (var game in this.games)
		{
			gameList.AddItem(game.Name);
		}
	}

	public void OnItemSelectedEventHandler(long index)
	{
		this.DisplayGame(this.games[index]);
	}

	void LoadGames()
	{
		// load the file
		var resourcePath = "res://Data/games.yaml";
		using var file = FileAccess.Open(resourcePath, FileAccess.ModeFlags.Read);
		var fileContent = file.GetAsText();
		GD.Print(fileContent);
		
		// use the Library
		var deserializer = new DeserializerBuilder()
			.WithNamingConvention(PascalCaseNamingConvention.Instance)
			.Build();

		this.games = deserializer.Deserialize<Game[]>(fileContent);
	}

	void DisplayGame(Game g)
	{
		// build the title string
		var textTitle = $"[font_size={24}]{g.Name}[/font_size] (also known as ";
		for(var index = 0; index < g.Aliases.Count(); index++)
		{
			if(index + 1 == g.Aliases.Count())
			{
				// last one
				textTitle += $"{g.Aliases.GetValue(index)})";
			}
			else
			{
				textTitle += $"{g.Aliases.GetValue(index)}, ";
			}
		}
		var textDisplay = GetNode<RichTextLabel>("VBoxContainer/HSplitContainer/MarginContainer/RichTextLabel");
		textDisplay.Text = textTitle;
	}
}
