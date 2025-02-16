using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class ModDef : Def
    {

        public List<StatModifier> statOffsets; // Diffrent from main

        public string extensionClass;

        public List<string> tags;

        public string bottomlabel;

        public List<string> requiredWeaponTags;

        public bool matchAllNotNeeded; // If this then only 1 tag is needed

        public List<ThingDefCountClass> costList;

        public bool not; // Not these tags

        public List<string> requiredDefs; // Match only 1 needed

        public Polarity polarity;

        public int drain;

        public IModEffect GetExtensionClassInstance()
        {
            if (string.IsNullOrEmpty(extensionClass))
                return null;

            Type type = GenTypes.GetTypeInAnyAssembly(extensionClass);
            if (type == null || !typeof(IModEffect).IsAssignableFrom(type))
            {
                ModUtils.Error($"Unable to find or assign {extensionClass} as a valid IModEffect.");
                return null;
            }

            return (IModEffect)Activator.CreateInstance(type);
        }
    }

    public enum Polarity
    {
        Godel, // Weapon basic
        Erebus, // Weapon vertical
        Azoth, // Weapon unique
        Monad // Weapon element
    }
}
