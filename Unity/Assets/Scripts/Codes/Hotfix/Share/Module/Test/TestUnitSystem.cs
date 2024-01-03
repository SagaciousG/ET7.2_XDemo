namespace ET
{
    public static class TestUnitSystem
    {
        public static Profession GetProfession(this TestUnit self, ProfessionNum num)
        {
            foreach (var profession in self.GetChildren<Profession>())
            {
                if (profession.Num == num)
                    return profession;
            }

            return null;
        }
        public static Profession GetProfession(this TestUnit self)
        {
            foreach (var profession in self.GetChildren<Profession>())
            {
                if (profession.Num == self.ProfessionNum)
                    return profession;
            }

            return null;
        }
    }
}