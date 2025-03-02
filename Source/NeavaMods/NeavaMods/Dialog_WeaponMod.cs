using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace NeavaMods
{
    public class Dialog_WeaponMod : Window
    {
        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(1200f, 760f);
            }
        }

        public Dialog_WeaponMod(Thing weapon, Thing machiningTable = null, Pawn owner = null) : base(null)
        {
            this.weapon = weapon;
            this.machiningTable = machiningTable;
            this.owner = owner;
        }

        public override void PostOpen()
        {
            base.PostOpen();

            this.weaponComp = ThingCompUtility.TryGetComp<CompWeaponMods>(this.weapon);
            if (this.weaponComp == null)
            {
                ModUtils.Error("Weapon did not have Comp CompWeaponMods");
                this.Close(true);
                return;
            }

            this.slots = weaponComp.slots;
            this.slots2 = weaponComp.slots;

            this.forcePause = true;
            this.closeOnCancel = false;
            this.closeOnClickedOutside = false;
            this.closeOnAccept = false;
        }

        public override void PostClose()
        {
            base.PostClose();
        }

        public override void DoWindowContents(Rect inRect)
        {

        }

        protected Thing weapon;

        protected Thing machiningTable;

        protected Pawn owner;

        protected CompWeaponMods weaponComp;

        public Dictionary<int, ModSlot> slots;

        public Dictionary<int, ModSlot> slots2;
    }
}
