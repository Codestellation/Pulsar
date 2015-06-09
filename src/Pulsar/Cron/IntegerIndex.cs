namespace Codestellation.Pulsar.Cron
{
    public struct IntegerIndex
    {
        public const sbyte NotFound = -1;

        private readonly sbyte[] _indexArray;
        public IntegerIndex(SimpleCronField field)
        {
            _indexArray = new sbyte[field.Settings.MaxValue + 1];

            for (int i = 0; i < _indexArray.Length; i++)
            {
                _indexArray[i] = NotFound;
            }

            foreach (var value in field.Values)
            {
                _indexArray[value] = (sbyte)value;
            }

            int next = NotFound;
            for (int i = _indexArray.Length - 1; i >= 0; i--)
            {
                var val = _indexArray[i];
                if (val != NotFound)
                {
                    next = val;
                }

                _indexArray[i] = (sbyte)next;
            }
        }

        public int GetValue(int value)
        {
            return _indexArray[value];
        }
    }
}