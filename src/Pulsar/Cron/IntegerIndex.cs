namespace Codestellation.Pulsar.Cron
{
    public struct IntegerIndex
    {
        public const int NotFound = -1;

        private readonly int[] _indexArray;
        public IntegerIndex(SimpleCronField field)
        {
            _indexArray = new int[field.Settings.MaxValue + 1];

            for (int i = 0; i < _indexArray.Length; i++)
            {
                _indexArray[i] = NotFound;
            }

            foreach (var value in field.Values)
            {
                _indexArray[value] = value;
            }

            int next = NotFound;
            for (int i = _indexArray.Length - 1; i >= 0; i--)
            {
                var val = _indexArray[i];
                if (val != NotFound)
                {
                    next = val;
                }

                _indexArray[i] = next;
            }
        }

        public int GetValue(int value)
        {
            return _indexArray[value];
        }
    }
}