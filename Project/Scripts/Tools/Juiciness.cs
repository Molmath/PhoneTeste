using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Author : Mathys Moles

namespace Com.IsartDigital.ProjectName
{

    public static class Juiciness
    {
        public static List<string> ListAnimationWithName(this AnimatedSprite2D pAnimatedSprite, params string[] pName)
        {
            if(pAnimatedSprite == null) return null;
            List<string> lResult = new List<string>();
            List<string> lAnimations = new List<string>(pAnimatedSprite.SpriteFrames.GetAnimationNames());
            foreach (string lAnimationName in lAnimations)
            {
                if(pName.All(name => lAnimationName.Contains(name)))
                {
                    lResult.Add(lAnimationName);
                }
            }
            return (lResult.Count == 0) ? null : lResult;
        }
        public static List<string> ListAnimationWithName(this AnimatedSprite2D pAnimatedSprite, string pName)
        {
            if (pAnimatedSprite == null) return null;
            List<string> lResult = new List<string>();
            List<string> lAnimations = new List<string>(pAnimatedSprite.SpriteFrames.GetAnimationNames());
            foreach (string lAnimationName in lAnimations)
            {
                if (lAnimationName.Contains(pName))
                {
                    lResult.Add(lAnimationName);
                }
            }
            return (lResult.Count == 0) ? null : lResult;
        }

    
    }
}

