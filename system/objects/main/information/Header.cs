namespace Butterfly.system.objects.main.information
{
    public class Header : Informing
    {
        public Header(informing.IMain informing, byte type)
            : base("HeaderInformation", informing) 
        {
            if (type == Data.BOARD) Board = true;
        }

        public struct Data
        {
            public const byte ROOT = 0;

            public const byte BOARD = 1;
            public const byte CONTROLLER = 2;

            public const byte Node = 3;
            public const byte Branch = 4;
        }

        private bool Board;

        public global::System.Type Type { private set; get; }

        public byte ObjectType {private set;get;}

        /// <summary>
        /// Имя обьекта.
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// Хранит место положение обьекта относительно все системы.
        /// </summary>
        public string Explorer { private set; get; } = "";

        public string Directory {private set;get;} = "";

        public bool IsSystemController() 
            => Data.ROOT == ObjectType;

        public bool IsNodeObject() 
            => (ObjectType == Data.Node) || (ObjectType == Data.ROOT);

        public bool IsBranchObject() 
            => ObjectType == Data.Branch;

        public bool IsBoard() 
            => Board;

        public void NodeDefine(string directory, global::System.Type type, information.DOM parentDomInformation, string keyObject)
        {
            ObjectType = Data.Node;

            Define(directory, type, parentDomInformation, keyObject);
        }

        public void BranchDefine(string directory, global::System.Type type, information.DOM parentDomInformation, string keyObject)
        {
            ObjectType = Data.Branch;

            Define(directory, type, parentDomInformation, keyObject);
        }

        private void Define(string directory, global::System.Type type, information.DOM parentDomInformation, string keyObject)
        {
            if (type.FullName.Contains("root.Object"))
            {
                ObjectType = Data.ROOT;
                Name = "system";
            }
            else 
                Name = type.Name;
            
            string info = "";
            if (IsBoard() && IsSystemController())
                info = "[BR]";
            else if (IsBoard())
                info = "[BN]";
            else 
            {
                if (IsNodeObject())
                    info = "[CN]";
                else 
                    info = "[CB]";
            }

            Directory = $"{directory}/{Name}";
            Explorer = $"{info}{Directory}";
        }
    }
}