namespace Game.Entity.Tank
{
    public class TankArmor
    {
        public int ArmorProcent { get; private set; }

        public TankArmor(int armor)
        {
            ArmorProcent = armor;
        }
    }
}