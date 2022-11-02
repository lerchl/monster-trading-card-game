namespace MonsterTradingCardGame.Api {

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class Header : Attribute {

        public string? Name { get; set; }

    }
}