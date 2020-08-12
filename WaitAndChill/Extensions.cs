using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaitAndChill
{
    public static class Extensions
    {
        public static void SetWeaponAmmo(this ReferenceHub Rh, int Amount)
        {
            Rh.inventory.items.ModifyDuration(
            Rh.inventory.items.IndexOf(Rh.inventory.GetItemInHand()),
            Amount);
        }
    }
}
