using System.Runtime.CompilerServices;

namespace Codestellation.Pulsar.Cron
{
    internal struct IntegerIndex
    {
        public const sbyte NotFound = -1;

        private readonly sbyte[] _indexArray;

        public IntegerIndex(SimpleCronField field)
        {
            _indexArray = new sbyte[field.Settings.MaxValue + 1];

            FillDefaults();
            FillKnownValues(field);
            FillRedirectionValues();
        }

        private void FillDefaults()
        {
            for (int i = 0; i < _indexArray.Length; i++)
            {
                _indexArray[i] = NotFound;
            }
        }

        private void FillKnownValues(SimpleCronField field)
        {
            foreach (var value in field.Values)
            {
                _indexArray[value] = (sbyte)value;
            }
        }

        private void FillRedirectionValues()
        {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetValue(int value)
        {
            return _indexArray[value];
        }
    }
}