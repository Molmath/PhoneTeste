using Godot;

// Author : Mathys Moles

namespace Com.IsartDigital.ProjectName {
	
	public partial class Spawn : Sprite2D
	{
        public Vector2 Velocity = Vector2.Zero;
        public float rotaSpeed = 0;
        RandomNumberGenerator rand = new RandomNumberGenerator();
		public override void _Ready()
		{
			Modulate = new Color(rand.RandfRange(0, 1), rand.RandfRange(0, 1), rand.RandfRange(0, 1));
			Rotation = rand.RandfRange(0, 2);
        }
        public override void _Process(double delta)
        {
            base._Process(delta);
            float lDelta = (float)delta;
            Position += Velocity * Vector2.One * lDelta;
            Rotation = Mathf.Pi * 0.5f + LookPosition(Position, Position + Velocity.Normalized());
        }
        public static float LookPosition(Vector2 pStartPosition, Vector2 pEndPosition)
        {
            Vector2 lDistancePoint = pEndPosition - pStartPosition;
            return Mathf.Atan2(lDistancePoint.Y, lDistancePoint.X);
        }

    }
}
