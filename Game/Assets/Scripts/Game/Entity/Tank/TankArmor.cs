namespace Game.Entity.Tank
{
    public class TankArmor
    {
        private Tank tank;

        public int ArmorProcent { get; private set; }

        public TankArmor(Tank tank, int armor)
        {
            this.tank = tank;
            ArmorProcent = armor;
        }
    }
}