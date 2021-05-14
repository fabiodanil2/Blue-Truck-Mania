using Microsoft.Xna.Framework;

namespace RobialacPereiraGarciaOpenWorld.Models.Colliders.Extensions
{
    public static class Vector2Extensions
    {
        // a = new Vector2(10, 20)
        // a.Pos(0) == 10
        // a.Pos(1) == 20
        public static float Pos(this Vector2 vector, int i)
        {
            return i == 0 ? vector.X : vector.Y;
        }
    }
}