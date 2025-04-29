using System;
using System.Collections.Generic;
using Godot;
using SpacetimeDB;

namespace SpacetimeDBGodotClient;

public partial class SpacetimeDbNetworkManager : Node
{
    public static SpacetimeDbNetworkManager Instance { get; private set; }

    private readonly List<IDbConnection> activeConnections = new();

    public override void _Ready()
    {
       Instance = this;
	   Console.SetOut(new GodotWriter());
    }

    public bool AddConnection(IDbConnection conn)
    {
        if (activeConnections.Contains(conn))
        {
            return false;
        }
        activeConnections.Add(conn);
        return true;

    }

    public bool RemoveConnection(IDbConnection conn)
    {
        return activeConnections.Remove(conn);
    }
        
    private void ForEachConnection(Action<IDbConnection> action)
    {
        // It's common to call disconnect from Update, which will then modify the ActiveConnections collection,
        // therefore we must reverse-iterate the list of connections.
        for (var x = activeConnections.Count - 1; x >= 0; x--)
        {
            action(activeConnections[x]);
        }
    }

    public override void _PhysicsProcess(double delta) => ForEachConnection(conn => conn.FrameTick());
    public override void _ExitTree() => ForEachConnection(conn => conn.Disconnect());
}
