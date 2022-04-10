using System;
using System.Collections.Generic;
using Godot;
using Timer = Godot.Timer;

public class EnemySpawner : Node2D
{
    private Random randomPosition;
    private List<Position2D> positions;
    
    private Timer spawnTimer;
    private PackedScene enemyScene;
    
    public override void _Ready()
    {
        randomPosition = new Random();
        positions = new List<Position2D>();
        
        var allChildren = GetChildren();
        foreach (var child in allChildren)
            if (child.GetType() == typeof(Position2D))
            {
                var pos = (Position2D) child;
                positions.Add(pos);
            }
        
        spawnTimer = GetNode<Timer>("SpawnTimer");
        enemyScene = (PackedScene) ResourceLoader.Load("res://Scene/Enemy/Enemy.tscn");
        spawnTimer.Connect("timeout", this, nameof(CreateEnemies));
    }
    
    private void CreateEnemies()
    {
        Position2D curPosition = positions[randomPosition.Next(0, positions.Count)];
        var createdEnemy = (Enemy) enemyScene.Instance();
        createdEnemy.GlobalPosition = curPosition.GlobalPosition;
        AddChild(createdEnemy);
    }
    
}
