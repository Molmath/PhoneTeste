using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Godot.OpenXRInterface;
namespace Com.IsartDigital.ProjectName
{
    public static partial class Utils
    {
        public const string
        SCALE = "scale",
        OFFSET = "offset",
        ROTATION = "rotation",
        ROTATION_DEGREES = "rotation_degrees";

        public static float LookPosition(this Vector2 pStartPosition, Vector2 pEndPosition)
        {
            Vector2 lDistancePoint = pEndPosition - pStartPosition;
            return Mathf.Atan2(lDistancePoint.Y, lDistancePoint.X);
        }
        public static Vector2 MoveWithRotation(float pRotation, float pSpeed)
        {
            return new Vector2(Mathf.Cos(pRotation), Mathf.Sin(pRotation)) * pSpeed;
        }
        public static T RandIndex<T>(this IEnumerable<T> pListe , RandomNumberGenerator pRand)
        {
            return pListe.ElementAt(pRand.RandiRange(0, pListe.Count() - 1));
        }
        public static bool GetContainAction<T>(this Action<T> pAction, string pMethodName)
        {
            return pAction?.GetInvocationList().Any(lAction => lAction.Method.Name == pMethodName) == true;
        }

        public static bool GetContainAction(this Action pAction, string pMethodName)
        {
            return pAction?.GetInvocationList().Any(lAction => lAction.Method.Name == pMethodName) == true;
        }
    }
}

