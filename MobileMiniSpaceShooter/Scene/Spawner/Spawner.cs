using System;
using System.Collections.Generic;
using Godot;

public class Spawner : Node2D
{
    private int minHeightZoneSpawnCoin;
    private int maxHeightZoneSpawnCoin;
    private int countCoins;
    private Random rnd;
    private List<Position2D> positions;
    
    private Timer spawnEnemyTimer;
    private Timer spawnCoinTimer;
    private PackedScene enemyScene;
    private PackedScene coinScene;

    [Signal] public delegate void SpawnCoin(Coin coin);
    [Signal] public delegate void SpawnEnemy(Enemy enemy);

    public override void _Ready()
    {
        int bottomOffset = 70;
        minHeightZoneSpawnCoin = 400;
        countCoins = 3;
        maxHeightZoneSpawnCoin = (int) GetViewport().Size.y - bottomOffset;
        rnd = new Random();
        positions = new List<Position2D>();
        
        var allChildren = GetChildren();
        foreach (var child in allChildren)
            if (child.GetType() == typeof(Position2D))
            {
                var pos = (Position2D) child;
                positions.Add(pos);
            }
        
        spawnEnemyTimer = GetNode<Timer>("SpawnEnemyTimer");
        spawnCoinTimer = GetNode<Timer>("SpawnCoinTimer");
        
        enemyScene = (PackedScene) ResourceLoader.Load("res://Scene/Enemy/Enemy.tscn");
        coinScene = (PackedScene) ResourceLoader.Load("res://Scene/Coin/Coin.tscn");
        
        spawnEnemyTimer.Connect("timeout", this, nameof(CreateEnemies));
        spawnCoinTimer.Connect("timeout", this, nameof(CreateCoinInRandomArea));
    }

    private Position2D GetRandomPosition()
    {
        Position2D curPosition = positions[rnd.Next(0, positions.Count)];
        return curPosition;
    }
    private void CreateEnemies()
    {
        var createdEnemy = (Enemy) enemyScene.Instance();
        createdEnemy.GlobalPosition = GetRandomPosition().GlobalPosition;
        createdEnemy.Connect(nameof(Enemy.Dead), this, nameof(CreateCoinInPosition));
        EmitSignal(nameof(SpawnEnemy), createdEnemy);
    }

    private void IncreaseCountCoins() { countCoins++; }
    
    private void CreateCoinInRandomArea()
    {
        if (countCoins <= 0) return;
        
        var createdCoin = (Coin) coinScene.Instance();
        var randomPos = GetRandomPosition().GlobalPosition;
        randomPos.y = rnd.Next(minHeightZoneSpawnCoin, maxHeightZoneSpawnCoin);
        createdCoin.GlobalPosition = randomPos;
        createdCoin.Connect(nameof(Coin.Collect), this, nameof(IncreaseCountCoins));
        countCoins--;
        EmitSignal(nameof(SpawnCoin), createdCoin);
    }

    private void CreateCoinInPosition(Vector2 pos)
    {
        var createdCoin = (Coin) coinScene.Instance();
        createdCoin.GlobalPosition = pos;
        AddChild(createdCoin);
    }
    
}
