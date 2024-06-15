namespace Butterfly.system.objects.main.information
{
    public class Tegs : Informing
    {
        private readonly information.State _stateInformation;

        public Tegs(information.State stateInformation, informing.IMain informing)
            : base("TegsInformation", informing)
                => _stateInformation = stateInformation;

        public string[] _tegsString = new string[0];
        private byte[] _tegsByte = new byte[0];

        public void Add(string teg)
        {
            if (_stateInformation.IsContruction)
            {
                for (int i = 0; i < _tegsString.Length; i++)
                {
                    if (_tegsString[i] == teg)
                    {
                        Exception($"Вы дважды добавили один и тот же тег {teg}.");
                        return;
                    }
                }

                Hellper.ExpendArray(ref _tegsString, teg);
            }
            else
                Exception($"Добавить тег {teg} можно только в методе Construction.");
        }

        public bool Contains(string teg)
        {
            if (_tegsString.Length == 0) return false;

            for (int i = 0; i < _tegsString.Length; i++)
                if (_tegsString[i] == teg)
                    return true;
            return false;
        }

        public void Add(byte teg)
        {
            if (_stateInformation.IsContruction)
            {
                for (int i = 0; i < _tegsByte.Length; i++)
                {
                    if (_tegsByte[i] == teg)
                    {
                        Exception($"Вы дважды добавили один и тот же тег {teg}.");
                        return;
                    }
                }

                Hellper.ExpendArray(ref _tegsByte, teg);
            }
            else
                Exception($"Добавить тег {teg} можно только в методе Construction.");
        }

        public bool Contains(byte teg)
        {
            if (_tegsByte.Length == 0) return false;

            for (int i = 0; i < _tegsByte.Length; i++)
                if (_tegsByte[i] == teg)
                    return true;
            return false;
        }
    }
}