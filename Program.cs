using System.Collections;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Linq;

namespace TheBestCourse;
static class Program { 
	private const float _chanceForPlayerWithSecretGun = .43f;
	private const float _chanceForPlayerIsDetective = .25f;
	private static string[] _players = {"titanovsky", "zozda", "ExtraDip", "0xacgh", "LOLIK403"};
	private static Queue<string> _murders = new();
	private static string _detective = string.Empty;
	private static string _murder = string.Empty;
	private static string _playerWithSecretGun = string.Empty;
	private static Random _rand = new();

	static void Main(string[] args) {
		Start(); 
	}

	private static void Start()
	{
		if (_players.Length < 2) { Error("The game can't start"); return; }

		_playerWithSecretGun = string.Empty;
		_detective = string.Empty;
		_murder = string.Empty;

		foreach (var ply in _players)
		{
			RefreshMurders();
		}

		var i = 0;
		foreach (var ply in _murders)
		{
			Console.WriteLine($"{i}. {ply}");
			i++;
		}

		ChooseMurder();
		ChooseDetective();
		ChoosePlayerWithGun();

		End();
	}

	private static void End()
	{
		Console.WriteLine("The game is end");
	}

	private static bool IsPlayerOnline(string ply)
	{
		return _players.Contains(ply);
	}

	private static void RefreshMurders()
	{
		int randomPlayerID = _rand.Next(0, _players.Length);
		string playerToMurdersQueue = _players[randomPlayerID];
        if (_murders.Count > 0 && playerToMurdersQueue.Equals(_murders.Last())) { RefreshMurders(); return; } // for a more variable murders

		_murders.Enqueue(playerToMurdersQueue);
	}

	private static void ChooseDetective() 
	{
		if (_players.Length < 3) { Error("The game can't choose a detecitve"); return; }

		foreach (var ply in _players)
		{
			if (_murder == ply) continue;
			if (_playerWithSecretGun == ply) continue;

			bool has = MathF.Round(_rand.NextSingle(), 2) < _chanceForPlayerIsDetective;
			if (!has) continue;

			Console.WriteLine($"The player {ply} become the detective!");

			_detective = ply;

			return;
		}

		Console.WriteLine("Without a detective");
	}

	private static void ChooseMurder()
	{
		if (_murders.Count == 0) { Error("The game can't choose a murder"); return; }

		var murder = _murders.Dequeue();
		if (!IsPlayerOnline(murder))
		{
			for (int i = 0; i < _murders.Count; i++)
			{
				murder = _murders.Dequeue();
				if (IsPlayerOnline(murder)) break;

				ChooseMurder(); // show error
			}
		}

        Console.WriteLine($"The player {murder} will become the murder");

		_murder = murder;
    }

	private static void ChoosePlayerWithGun()
	{
		if (_players.Length < 3) { Error("The game can't choose a player with gun"); return; }

		foreach (var ply in _players)
		{
			if (_murder == ply) continue;
			if (_detective == ply) continue;

			bool hasGun = MathF.Round(_rand.NextSingle(), 2) < _chanceForPlayerWithSecretGun;
			if (!hasGun) continue;

			Console.WriteLine($"The player {ply} has a secret gun");

			_playerWithSecretGun = ply;

			return;
		}

        Console.WriteLine("Without secret gun");
    }

	private static void Error(string msg)
	{
		var oldColor = Console.ForegroundColor;
		Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {msg}");
		Console.ForegroundColor = oldColor;
	}
}