using System.IO;
using System.Text;
using Godot;

namespace SpacetimeDBGodotClient
{
    public class GodotWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
        
        public override void Write(string value)
        {
            if (value == null) return;

            GD.Print(value);
        }
        
        public override void WriteLine(string value)
        {
            Write(value);
        }
    }
} 