using System;

namespace SharedKernel
{
    public class UPC
    {
        public UPC(string upc)
        {
            Code = upc;
        }
        
        public string Code { get; private set; }

        public int CalculateChecksum()
        {
            int sum = 0;
            var digitPositions = InitializedDigitPositions();
            for (int pos = 1; pos <= Code.Length - 1; pos++)
            {
                sum += DigitAt(pos) * WeightAt(digitPositions, pos);
            }
            return (10 - (sum % 10)) % 10;
        }

        private int[] InitializedDigitPositions()
        {
            var positions = new int[Code.Length];
            for (int i = 0; i < Code.Length; i++)
            {
                positions[i] = i + 1;
            }
            return positions;
        }

        private int WeightAt(int[] digitPositions, int pos)
        {
            return (digitPositions[pos] % 2 == 0) ? 1 : 3;
        }

        private int DigitAt(int pos)
        {
            return Convert.ToInt32(Code.Substring(pos - 1, 1));
        }
    }
}
