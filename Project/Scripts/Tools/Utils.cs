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
        ROTATION_DEGREES = "rotation_degrees",
        SELF_MODULATE = "self_modulate";

        public const int
        ZERO = 0;

        public static bool ExitScreen(this Node2D pNode, Vector2 pScreenSize)
        {
            Vector2 NodePosition = pNode.Position;
            return pNode.Position.X < ZERO || pNode.Position.Y < ZERO || pNode.Position.X > pScreenSize.X || pNode.Position.Y > pScreenSize.Y;
        }

        /// <summary>
        /// IA
        /// </summary>
        public static T GetDeepestChildWithClasse<T>(this Node pRoot) where T : Node
        {
            T result = (T)pRoot;
            int maxDepth = -1;

            void Search(Node current, int depth)
            {
                if (current is T match && depth > maxDepth)
                {
                    result = match;
                    maxDepth = depth;
                }

                foreach (Node child in current.GetChildren())
                {
                    Search(child, depth + 1);
                }
            }

            Search(pRoot, 0);
            return result;
        }

        public static T GetFirstParentWithClasse<T>(this T pChild) where T : Node
        {
            T lPapa = pChild;
            while (true)
            {
                if (lPapa.GetParent() is T lDac) lPapa = lDac;
                else break;
            }
            return lPapa;
        }
        public static T GetFirstParentWithClasse<T>(this T pChild, out int nParent) where T : Node
        {
            T lPapa = pChild;
            int PNDepth = 0;
            while (true)
            {
                if (lPapa.GetParent() is T lDac)
                {
                    lPapa = lDac;
                    PNDepth++;
                }
                    
                else break;
            }
            nParent = PNDepth;
            return lPapa;
        }

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

