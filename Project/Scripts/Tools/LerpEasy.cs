using Godot;
using System;

// Author : Mathys Moles

namespace Com.IsartDigital.ProjectName
{

    public static class LerpEasy
    {
        public static Vector2[] easeBackOutPoints = new Vector2[]
         {
             new Vector2(0.00f, 0.00f),
             new Vector2(0.05f, -0.10f), // recul léger
             new Vector2(0.25f, 0.30f),
             new Vector2(0.50f, 0.70f),
             new Vector2(0.75f, 1.10f),  // dépassement
             new Vector2(0.90f, 1.02f),
             new Vector2(1.00f, 1.00f)
         };
        public static Vector2[] easeElasticOutPoints = new Vector2[]
        {
            new Vector2(0.00f, 0.00f),
            new Vector2(0.10f, 0.55f),
            new Vector2(0.25f, 1.15f), // premier overshoot important
            new Vector2(0.40f, 0.85f), // premier retour en dessous
            new Vector2(0.55f, 1.08f), // rebond plus petit
            new Vector2(0.70f, 0.96f), // encore un petit rebond
            new Vector2(0.85f, 1.02f), // micro-rebond
            new Vector2(1.00f, 1.00f)
        };


        public static Vector2[] easeCustomPoints = new Vector2[]
        {
            new Vector2(0.00f, 0.00f),
            new Vector2(0.10f, 0.25f),
            new Vector2(0.25f, 0.55f),
            new Vector2(0.45f, 0.85f),
            new Vector2(0.60f, 1.05f),
            new Vector2(0.75f, 1.02f),
            new Vector2(0.90f, 1.01f),
            new Vector2(1.00f, 1.00f)
        };

        public static float EaseTrans(this Vector2[] pListe, float pWeigh)
        {
            // pT est entre 0 et 1
            for (int i = 0; i < pListe.Length - 1; i++)
            {
                Vector2 p0 = pListe[i];
                Vector2 p1 = pListe[i + 1];
                if (p1.Y > 1) p1 = new Vector2(p1.X, p1.Y);
                if (pWeigh >= p0.X && pWeigh <= p1.X)
                {
                    float localT = (pWeigh - p0.X) / (p1.X - p0.X);
                    return Mathf.Lerp(p0.Y, p1.Y, localT);
                }
            }
            return 1f;
        }
    }
}
