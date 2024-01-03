namespace ET
{
    public static class DefineEnumExtension
    {
        public static EquipmentHole ToHole(this EquipmentPart p, bool right = true)
        {
            switch (p)
            {
                case EquipmentPart.Arm:
                    return EquipmentHole.Arm;
                case EquipmentPart.Belt:
                    return EquipmentHole.Belt;
                case EquipmentPart.Cloth:
                    return EquipmentHole.Cloth;
                case EquipmentPart.Head:
                    return EquipmentHole.Head;
                case EquipmentPart.Pants:
                    return EquipmentHole.Pants;
                case EquipmentPart.Shield:
                    return EquipmentHole.WeaponL;
                case EquipmentPart.Shoes:
                    return EquipmentHole.Shoes;
                case EquipmentPart.Weapon:
                    return EquipmentHole.WeaponR;
                case EquipmentPart.WeaponOrShield:
                    return right ? EquipmentHole.WeaponR : EquipmentHole.WeaponL;
            }

            return (EquipmentHole)(int)(p);
        }
    }
}